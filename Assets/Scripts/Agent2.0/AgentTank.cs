using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



/// <summary>
/// This class represents a tank agent that makes decisions based on perception and performs actions accordingly.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class AgentTank : MonoBehaviour {
    private StatsBasicAgent statsBA;
   // private SteeringBehaviour2 m_stb;
    private Rigidbody m_rigidbody;
    private Collider[] eyePercived;

    public Transform target;

    private enum MovementAgent { None, Seek, Flee }
    private enum ActionAgent { None, Attack, Heal, Running }

    private MovementAgent moveState = MovementAgent.None;
    private ActionAgent actionState = ActionAgent.None;

    [Header("Set to true for team blue")]
    public bool checkLayerTeam;
    private int layerMask;

    private bool isHealing = false;
    private float healingRate = 5f; // Velocidad de curación por segundo
    private float maxVida = 1000f; // Valor máximo de vida

    private void Start() {
        InitializeComponents();
    }

    private void Update() {
        DecisionManager();
    }

    private void FixedUpdate() {
        PerceptionManager();

        // Asegurarse de que target sea nulo si no se ha encontrado ningún objetivo en la percepción
        //if (target == null) {
        //    moveState = MovementAgent.None;
        //    actionState = ActionAgent.None;
        //}
    }

    private void InitializeComponents() {
        statsBA = GetComponent<StatsBasicAgent>();
       // agenteTank = new AgentBasic2(TypeAgent2.Tank);
       // m_stb = new SteeringBehaviour2();
        m_rigidbody = GetComponent<Rigidbody>();

        AgentBasic2 agenteTank = new AgentBasic2(TypeAgent2.Tank);

        statsBA.Vida = agenteTank.Vida;
        statsBA.Velocidad = agenteTank.Velocidad;
        statsBA.Armadura = agenteTank.Armadura;
        statsBA.Fuerza = agenteTank.Fuerza;
        statsBA.MaxVel = agenteTank.MaxVel;
        statsBA.SteeringForce = agenteTank.SteeringForce;
        statsBA.TypeAgent = TypeAgent2.Tank;
        statsBA.EyesPerceptionRad = 45f;

        // Establecer la máscara de capa según el equipo
        layerMask = checkLayerTeam ? 1 << 6 : 1 << 7;
    }

    private void PerceptionManager() {
        eyePercived = Physics.OverlapSphere(statsBA.EyePerception.position, statsBA.EyesPerceptionRad, layerMask);
    }

    private void DecisionManager() {
        if (eyePercived == null) {
            // Si no se percibe ningún objetivo, establecer el objetivo como nulo (ninguno)
           // target = null;
            //moveState = MovementAgent.None;
            //actionState = ActionAgent.None;
            return;
        }

        foreach (Collider obj in eyePercived) {
            float dist = Vector3.Distance(transform.position, obj.transform.position);
            TypeAgent2 tempType = obj.GetComponent<StatsBasicAgent>().TypeAgent;

            if (dist > 50) {
                moveState = MovementAgent.None;
                actionState = ActionAgent.Heal;
                continue;
            }

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

    private void ActionManager(ActionAgent actionAgent) {
        switch (actionAgent) {
            case ActionAgent.None:
                //m_particle[0].Stop();
                break;
            case ActionAgent.Attack:
                StartCoroutine(MentalAttack());
                break;
            case ActionAgent.Heal:
                if (!isHealing) {
                    StartCoroutine(HealingProcess());
                }
                if (statsBA.Vida >= maxVida) {
                    // Detener la curación si se alcanza el valor máximo de vida
                    isHealing = false;
                }
                break;
            case ActionAgent.Running:
                break;
        }
    }

    private IEnumerator StartChasing() {
        GetComponent<Renderer>().material.color = Color.cyan;
        m_rigidbody.velocity = SteeringBehaviour2.Seek(transform, target.position);
        SteeringBehaviour2.lookAt(transform);
        yield return new WaitForSeconds(3f);
    }

    private IEnumerator HealingProcess() {
        isHealing = true; // Comenzar proceso de curación
        GetComponent<Renderer>().material.color = Color.yellow;

        while (isHealing) {
            statsBA.Vida += healingRate * Time.deltaTime; // Curación gradual por segundo

            if (statsBA.Vida >= maxVida) {
                // Detener la curación si se alcanza el valor máximo de vida
                isHealing = false;
            }

            yield return null;
        }

        moveState = MovementAgent.None;
        GetComponent<Renderer>().material.color = Color.white;
    }

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
