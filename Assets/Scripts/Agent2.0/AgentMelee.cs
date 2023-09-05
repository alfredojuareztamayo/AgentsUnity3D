using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This class represents a melee agent that makes decisions based on perception and performs actions accordingly.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class AgentMelee : MonoBehaviour {
    [SerializeField] private Transform m_eyePercivedAgent;
    private AgentBasic2 agenteMelee;
    private StatsBasicAgent statsBA;
    private SteeringBehaviour2 m_stb;
    private Rigidbody m_rigidbody;
    private int layerMask;
    private ActionAgent actionState;
    private MovementAgent moveState;
    private PhysicalState physicalState;
    public Transform target;
    float radEyes = 10f;
    Collider[] eyePercived;
    [SerializeField] ParticleSystem[] m_particle;

    [Header("Set to true for team blue")]
    public bool checkLayerTeam;
    public bool damageTaken;

    /// <summary>
    /// Initialization method called when the object is created.
    /// </summary>
    private void Start() {
        // Retrieve and initialize components
        statsBA = GetComponent<StatsBasicAgent>();
        agenteMelee = new AgentBasic2(TypeAgent2.Melee);
        m_stb = new SteeringBehaviour2();
        m_rigidbody = GetComponent<Rigidbody>();

        // Initialize agent's stats
        statsBA.Vida = agenteMelee.Vida;
        statsBA.Velocidad = agenteMelee.Velocidad;
        statsBA.Armadura = agenteMelee.Armadura;
        statsBA.Fuerza = agenteMelee.Fuerza;
        statsBA.MaxVel = agenteMelee.MaxVel;
        statsBA.SteeringForce = agenteMelee.SteeringForce;
        statsBA.TypeAgent = TypeAgent2.Melee;
        statsBA.EyesPerceptionRad = 10f;
        statsBA.ProtectedAttack = false;

        if (checkLayerTeam) {
            layerMask = 1 << 6;
        } else {
            layerMask = 1 << 7;
        }
    }

    /// <summary>
    /// Update method called once per frame to make decisions.
    /// </summary>
    private void Update() {
        DecisionManager();
    }

    /// <summary>
    /// FixedUpdate method called at a fixed interval for physics updates.
    /// </summary>
    private void FixedUpdate() {
        PerceptionManager();
    }

    /// <summary>
    /// Manages perception by detecting nearby objects.
    /// </summary>
    private void PerceptionManager() {
        eyePercived = Physics.OverlapSphere(statsBA.EyePerception.position, statsBA.EyesPerceptionRad, layerMask);
    }

    /// <summary>
    /// Manages decision-making based on perception.
    /// </summary>
    private void DecisionManager() {
        if (eyePercived == null) {
            return;
        }

        float dist;
        TypeAgent2 tempType;
        foreach (Collider obj in eyePercived) {
            dist = Vector3.Distance(this.transform.position, obj.transform.position);
            if (dist > 20) {
                moveState = MovementAgent.None;
                actionState = ActionAgent.None;
                continue;
            }

            tempType = obj.GetComponent<StatsBasicAgent>().TypeAgent;
            if (tempType == TypeAgent2.Shooter && dist > 5f) {
                target = obj.transform;
                moveState = MovementAgent.Seek;
                actionState = ActionAgent.None;
                break;
            }
            if (tempType == TypeAgent2.Shooter && dist < 5f) {
                target = obj.transform;
                moveState = MovementAgent.None;
                actionState = ActionAgent.Attack;
                break;
            }
            if (tempType == TypeAgent2.Tank && dist < 5f) {
                target = obj.transform;
                moveState = MovementAgent.Flee;
                actionState = ActionAgent.Running;
                break;
            }
        }

        MovementManager(moveState);
        ActionManager(actionState);

        eyePercived = null;
    }

    /// <summary>
    /// Manages movement behavior based on the specified movement agent.
    /// </summary>
    private void MovementManager(MovementAgent movementAgent) {
        switch (movementAgent) {
            case MovementAgent.None:
                break;
            case MovementAgent.Seek:
                StartCoroutine(StartChasing());
                break;
            case MovementAgent.Flee:
                StartCoroutine(StopFleeing());
                break;
        }
    }

    /// <summary>
    /// Manages action behavior based on the specified action agent.
    /// </summary>
    private void ActionManager(ActionAgent actionAgent) {
        switch (actionAgent) {
            case ActionAgent.None:
                //m_particle[0].Stop();
                break;
            case ActionAgent.Attack:
                Debug.Log("I'm attacking");
                break;
            case ActionAgent.Heal:
                break;
            case ActionAgent.Running:
                StartCoroutine(ChangeColorFlee());
                break;
        }
    }

    /// <summary>
    /// Coroutine to change color when fleeing.
    /// </summary>
    private IEnumerator ChangeColorFlee() {
        GetComponent<Renderer>().material.color = Color.red;
        physicalState = PhysicalState.Afraid;
        yield return new WaitForSeconds(4f);
        GetComponent<Renderer>().material.color = Color.white;
    }

    /// <summary>
    /// Coroutine to stop fleeing after a certain time.
    /// </summary>
    private IEnumerator StopFleeing() {
        m_rigidbody.velocity = m_stb.Flee(this.transform, target);
        m_stb.lookAt(transform);
        yield return new WaitForSeconds(4f);
        moveState = MovementAgent.None;
        actionState = ActionAgent.None;
        physicalState = PhysicalState.Tired;
    }

    /// <summary>
    /// Coroutine to start chasing the target.
    /// </summary>
    private IEnumerator StartChasing() {
        GetComponent<Renderer>().material.color = Color.black;
        m_rigidbody.velocity = m_stb.Seek(this.transform, target);
        m_stb.lookAt(transform);
        yield return new WaitForSeconds(3f);
    }

    /// <summary>
    /// Gizmos method to draw the perception radius.
    /// </summary>
    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(m_eyePercivedAgent.position, radEyes);
    }

    /// <summary>
    /// Method to handle being attacked.
    /// </summary>
    public void BeingAttack() {
        switch (physicalState) {
            case PhysicalState.Tired:
                statsBA.Armadura -= 5f;
                break;
            case PhysicalState.Afraid:
                statsBA.Vida -= 5f;
                break;
            case PhysicalState.Happy:
                break;
        }
    }
}

