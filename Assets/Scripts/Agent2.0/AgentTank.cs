using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



/// <summary>
/// This class represents a tank agent that makes decisions based on perception and performs actions accordingly.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class AgentTank : MonoBehaviour {
    private AgentBasic2 agenteTank;
    private StatsBasicAgent statsBA;
    private SteeringBehaviour2 m_stb;
    public Transform target;
    private Rigidbody m_rigidbody;
    Collider[] eyePercived;
    [Header("Set to true for team blue")]
    public bool checkLayerTeam;
    private int layerMask;
    private ActionAgent actionState;
    private MovementAgent moveState;

    /// <summary>
    /// Initialization method called when the object is created.
    /// </summary>
    private void Start() {
        // Retrieve and initialize components
        statsBA = GetComponent<StatsBasicAgent>();
        agenteTank = new AgentBasic2(TypeAgent2.Tank);
        m_stb = new SteeringBehaviour2();
        m_rigidbody = GetComponent<Rigidbody>();

        // Initialize agent's stats
        statsBA.Vida = agenteTank.Vida;
        statsBA.Velocidad = agenteTank.Velocidad;
        statsBA.Armadura = agenteTank.Armadura;
        statsBA.Fuerza = agenteTank.Fuerza;
        statsBA.MaxVel = agenteTank.MaxVel;
        statsBA.SteeringForce = agenteTank.SteeringForce;
        statsBA.TypeAgent = TypeAgent2.Tank;
        statsBA.EyesPerceptionRad = 45f;

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
            if (dist > 50) {
                moveState = MovementAgent.None;
                actionState = ActionAgent.Heal;
                continue;
            }

            tempType = obj.GetComponent<StatsBasicAgent>().TypeAgent;
            if (tempType == TypeAgent2.Melee && dist > 30f) {
                target = obj.transform;
                moveState = MovementAgent.Seek;
                actionState = ActionAgent.None;
                break;
            }
            if (tempType == TypeAgent2.Melee && dist < 5f) {
                target = obj.transform;
                moveState = MovementAgent.None;
                actionState = ActionAgent.Attack;
                break;
            }
            if (tempType == TypeAgent2.Tank && dist < 5f) {
                target = obj.transform;
                moveState = MovementAgent.None;
                actionState = ActionAgent.Heal;
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
                StartCoroutine(MentalAttack());
                break;
            case ActionAgent.Heal:
                StartCoroutine(HealingProcess());
                if (statsBA.Vida > 1000) {
                    break;
                }
                break;
            case ActionAgent.Running:
                break;
        }
    }

    /// <summary>
    /// Coroutine to start chasing the target.
    /// </summary>
    private IEnumerator StartChasing() {
        GetComponent<Renderer>().material.color = Color.cyan;
        m_rigidbody.velocity = m_stb.Seek(this.transform, target);
       m_stb.lookAt(transform);
        yield return new WaitForSeconds(3f);
    }

    /// <summary>
    /// Coroutine to perform the healing process.
    /// </summary>
    private IEnumerator HealingProcess() {
        GetComponent<Renderer>().material.color = Color.yellow;
        yield return new WaitForSeconds(4f);
        moveState = MovementAgent.None;
        statsBA.Vida += 10;
    }

    /// <summary>
    /// Coroutine to perform a mental attack on the target.
    /// </summary>
    private IEnumerator MentalAttack() {
        if (!target.GetComponent<StatsBasicAgent>().ProtectedAttack) {
            target.GetComponent<StatsBasicAgent>().Vida -= 10;
            target.GetComponent<StatsBasicAgent>().ProtectedAttack = true;
        }
        GetComponent<Renderer>().material.color = Color.green;
        yield return new WaitForSeconds(4f);
        target.GetComponent<StatsBasicAgent>().ProtectedAttack = false;
        moveState = MovementAgent.None;
        actionState = ActionAgent.None;
    }
}
