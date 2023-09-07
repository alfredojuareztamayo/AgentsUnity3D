using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This class represents a melee agent that makes decisions based on perception and performs actions accordingly.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class AgentMelee : MonoBehaviour {
    private StatsBasicAgent statsBA;
    //private SteeringBehaviour2 m_stb;
    private Rigidbody m_rigidbody;
    private Collider[] eyePercived;

    private Transform target; // Cambiamos esto para que sea una variable de clase

    // private enum MovementState { None, Seek, Flee }
    //private enum ActionState { None, Attack, Heal, Running }

    private MovementAgent moveState = MovementAgent.None;
    private ActionAgent actionState = ActionAgent.None;
    private PhysicalState physicalState;

    public bool damageTaken;

    //float radEyes = 10f;
    int layerMask;
    public bool checkLayerTeam;

    private void Start() {
        InitializeComponents();
    }

    private void Update() {
        DecisionManager();
    }

    private void FixedUpdate() {
        PerceptionManager();

        // Asegurarse de que target sea nulo si no se ha encontrado ningún objetivo en la percepción
        if (target == null) {
            moveState = MovementAgent.None;
            actionState = ActionAgent.None;
        }
    }

    private void InitializeComponents() {
        statsBA = GetComponent<StatsBasicAgent>();
        //m_stb = new SteeringBehaviour2();
        m_rigidbody = GetComponent<Rigidbody>();

        AgentBasic2 agenteMelee = new AgentBasic2(TypeAgent2.Melee);

        statsBA.Vida = agenteMelee.Vida;
        statsBA.Velocidad = agenteMelee.Velocidad;
        statsBA.Armadura = agenteMelee.Armadura;
        statsBA.Fuerza = agenteMelee.Fuerza;
        statsBA.MaxVel = agenteMelee.MaxVel;
        statsBA.SteeringForce = agenteMelee.SteeringForce;
        statsBA.TypeAgent = TypeAgent2.Melee;
        statsBA.EyesPerceptionRad = 10f;
        statsBA.ProtectedAttack = false;

        layerMask = checkLayerTeam ? 1 << 6 : 1 << 7;
    }

    private void PerceptionManager() {
        eyePercived = Physics.OverlapSphere(statsBA.EyePerception.position, statsBA.EyesPerceptionRad, layerMask);
    }

    private void DecisionManager() {
        if (eyePercived == null) {
            // Si no se percibe ningún objetivo, establecer el objetivo como nulo (ninguno)
            target = null;
            moveState = MovementAgent.None;
            actionState = ActionAgent.None;
            return;
        }

        foreach (Collider obj in eyePercived) {
            float dist = Vector3.Distance(transform.position, obj.transform.position);
            TypeAgent2 tempType = obj.GetComponent<StatsBasicAgent>().TypeAgent;

            if (dist > 20) {
                moveState = MovementAgent.None;
                actionState = ActionAgent.None;
                continue;
            }

            if (tempType == TypeAgent2.Shooter) {
                target = obj.transform;

                if (dist > 5f) {
                    moveState = MovementAgent.Seek;
                    actionState = ActionAgent.None;
                } else {
                    moveState = MovementAgent.None;
                    actionState = ActionAgent.Attack;
                }

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

    private void MovementManager(MovementAgent movementState) {
        switch (movementState) {
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

    private void ActionManager(ActionAgent actionAgent) {
        switch (actionAgent) {
            case ActionAgent.None:
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

    private IEnumerator ChangeColorFlee() {
        GetComponent<Renderer>().material.color = Color.red;
        physicalState = PhysicalState.Afraid;
        yield return new WaitForSeconds(4f);
        GetComponent<Renderer>().material.color = Color.white;
    }

    private IEnumerator StopFleeing() {
        m_rigidbody.velocity = SteeringBehaviour2.Flee(transform, target.position);
        SteeringBehaviour2.lookAt(transform);
        yield return new WaitForSeconds(4f);
        moveState = MovementAgent.None;
        actionState = ActionAgent.None;
        physicalState = PhysicalState.Tired;
    }

    private IEnumerator StartChasing() {
        GetComponent<Renderer>().material.color = Color.black;
        m_rigidbody.velocity = SteeringBehaviour2.Seek(transform, target.position);
        SteeringBehaviour2.lookAt(transform);
        yield return new WaitForSeconds(3f);
    }

    //private void OnDrawGizmos() {
    //    Gizmos.color = Color.green;
    //    Gizmos.DrawWireSphere(statsBA.EyePerception.position, radEyes);
    //}

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

