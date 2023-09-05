using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor;
using UnityEngine;


/// <summary>
/// This class represents an agent that can shoot projectiles and make decisions based on its perception.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class AgentShooter : MonoBehaviour {
    private AgentBasic2 agenteShooter;
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
    [SerializeField] GameObject bullet;

    /// <summary>
    /// Initialization method called when the object is created.
    /// </summary>
    private void Start() {
        // Retrieve and initialize components
        statsBA = GetComponent<StatsBasicAgent>();
        agenteShooter = new AgentBasic2(TypeAgent2.Shooter);
        m_stb = new SteeringBehaviour2();
        m_rigidbody = GetComponent<Rigidbody>();

        // Initialize agent's stats
        statsBA.Vida = agenteShooter.Vida;
        statsBA.Velocidad = agenteShooter.Velocidad;
        statsBA.Armadura = agenteShooter.Armadura;
        statsBA.Fuerza = agenteShooter.Fuerza;
        statsBA.MaxVel = agenteShooter.MaxVel;
        statsBA.SteeringForce = agenteShooter.SteeringForce;
        statsBA.TypeAgent = TypeAgent2.Shooter;
        statsBA.EyesPerceptionRad = 30f;

        // Set the layer mask based on team
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
                actionState = ActionAgent.None;
                continue;
            }

            tempType = obj.GetComponent<StatsBasicAgent>().TypeAgent;
            if (tempType == TypeAgent2.Melee && dist > 30f) {
                target = obj.transform;
                moveState = MovementAgent.None;
                actionState = ActionAgent.Attack;
                break;
            }
            if (tempType == TypeAgent2.Melee && dist < 5f) {
                target = obj.transform;
                moveState = MovementAgent.Flee;
                actionState = ActionAgent.None;
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
                break;
            case ActionAgent.Attack:
                StartCoroutine(StartAttack());
                break;
            case ActionAgent.Heal:
                break;
            case ActionAgent.Running:
                break;
        }
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
    }

    /// <summary>
    /// Coroutine to start attacking and instantiate bullets.
    /// </summary>
    private IEnumerator StartAttack() {
        Instantiate(bullet, transform.position, transform.rotation);
        GetComponent<Renderer>().material.color = Color.green;
        Color c = Color.gray;
        target.GetComponent<StatsBasicAgent>().MeAtacan(c);
        yield return new WaitForSeconds(2f);
        GetComponent<Renderer>().material.color = Color.white;
    }
}

