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
    private StatsBasicAgent statsBA;
   // private SteeringBehaviour2 m_stb;
    private Rigidbody m_rigidbody;
    private Collider[] eyePercived;
   

    public Transform target;

    //private enum MovementAgent { None, Seek, Flee }
    //private enum ActionAgent { None, Attack, Heal, Running }

    private MovementAgent moveState = MovementAgent.None;
    private ActionAgent actionState = ActionAgent.None;

    [SerializeField] private GameObject bullet;
    [Header("Set to true for team blue")]
    public bool checkLayerTeam;
    private int layerMask;

    private bool isAttacking = false;
    private float attackCooldown = 2f;

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
        //agenteShooter = new AgentBasic2(TypeAgent2.Shooter);
        //m_stb = new SteeringBehaviour2();
        m_rigidbody = GetComponent<Rigidbody>();
        AgentBasic2 agenteShooter = new AgentBasic2(TypeAgent2.Shooter);

        statsBA.Vida = agenteShooter.Vida;
        statsBA.Velocidad = agenteShooter.Velocidad;
        statsBA.Armadura = agenteShooter.Armadura;
        statsBA.Fuerza = agenteShooter.Fuerza;
        statsBA.MaxVel = agenteShooter.MaxVel;
        statsBA.SteeringForce = agenteShooter.SteeringForce;
        statsBA.TypeAgent = TypeAgent2.Shooter;
        statsBA.EyesPerceptionRad = 30f;

        // Set the layer mask based on team
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

            if (dist > 50) {
                moveState = MovementAgent.None;
                actionState = ActionAgent.None;
                continue;
            }

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

    private IEnumerator StopFleeing() {
        m_rigidbody.velocity = SteeringBehaviour2.Flee(transform, target.position);
        SteeringBehaviour2.lookAt(transform);
        yield return new WaitForSeconds(4f);
        moveState = MovementAgent.None;
        actionState = ActionAgent.None;
    }

    private IEnumerator StartAttack() {
        if (isAttacking) {
            yield break;
        }

        isAttacking = true;

        while (target != null) {
            Instantiate(bullet, transform.position, transform.rotation);
            GetComponent<Renderer>().material.color = Color.green;
            Color c = Color.gray;
            target.GetComponent<StatsBasicAgent>().MeAtacan(c);

            yield return new WaitForSeconds(attackCooldown);

            GetComponent<Renderer>().material.color = Color.white;
        }

        isAttacking = false;
    }
}
