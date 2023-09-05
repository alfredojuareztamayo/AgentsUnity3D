using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class AgentMelee : MonoBehaviour
{
    // Start is called before the first frame update
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
    //private bool targetOn;
    Collider[] eyePercived;
   [SerializeField]ParticleSystem[] m_particle;

    [Header("Verdadero para team blue")]
    public  bool checkLayerTeam;
    public bool damageTaken;
    private void Start()
    {
        statsBA = GetComponent<StatsBasicAgent>();
        // Crear un agente de tipo Melee
        agenteMelee = new AgentBasic2(TypeAgent2.Melee);
        m_stb = new SteeringBehaviour2();
        m_rigidbody = GetComponent<Rigidbody>();
        statsBA.Vida = agenteMelee.Vida;
        statsBA.Velocidad = agenteMelee.Velocidad;
        statsBA.Armadura = agenteMelee.Armadura;
        statsBA.Fuerza = agenteMelee.Fuerza;
        statsBA.MaxVel = agenteMelee.MaxVel;
        statsBA.SteeringForce = agenteMelee.SteeringForce;
        statsBA.TypeAgent = TypeAgent2.Melee;
        statsBA.EyesPerceptionRad = 10f;
        statsBA.ProtectedAttack = false;
       // targetOn = false;

        if (checkLayerTeam)
        {
            layerMask = 1 << 6;
        }
        else
        {
            layerMask = 1 << 7;
        }
    }
    private void Update()
    {
        DecisionManager();
    }
    private void FixedUpdate()
    {
        PerceptionManager();
    }
    private void PerceptionManager()
    {
        eyePercived = Physics.OverlapSphere(statsBA.EyePerception.position, statsBA.EyesPerceptionRad, layerMask);
        
    }
    private void DecisionManager()
    {
        if (eyePercived == null)
        {
            return;
        }
        
            float dist;
            TypeAgent2 tempType;
            foreach (Collider obj in eyePercived)
            {
                dist = Vector3.Distance(this.transform.position, obj.transform.position);
                if (dist > 20)
                {
                    moveState = MovementAgent.None;
                    actionState = ActionAgent.None;
                continue;
                }

                tempType = obj.GetComponent<StatsBasicAgent>().TypeAgent;
                if (tempType == TypeAgent2.Shooter && dist > 5f)
                {
                    target = obj.transform;
                    moveState = MovementAgent.Seek;
                    actionState = ActionAgent.None;
                    //targetOn = true;
                    break;
                }
                if (tempType == TypeAgent2.Shooter && dist < 5f)
                {
                    target = obj.transform;
                    moveState = MovementAgent.None;
                    actionState = ActionAgent.Attack;
                   // targetOn = true;
                    break;
                }
                if(tempType == TypeAgent2.Tank && dist < 5f)
                {
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

    private void MovementManager(MovementAgent movementAgent)
    {
        switch (movementAgent)
        {
            case MovementAgent.None:
               
            break;
            case MovementAgent.Seek:
                StartCoroutine(StarChasing());
            break;
            case MovementAgent.Flee:
               
                StartCoroutine(StopFleing());
            break;
        }
    }
    private void ActionManager(ActionAgent actionAgent)
    {
        switch(actionAgent)
        {
            case ActionAgent.None:
                //m_particle[0].Stop();
                break;
            case ActionAgent.Attack:
                Debug.Log("Estoy atacando");
                break;
                case ActionAgent.Heal: 
                break;
            case ActionAgent.Running:
                StartCoroutine(ChangeColorFlee());
                
                break;
        }
    }

    private IEnumerator ChangeColorFlee()
    {
        
        GetComponent<Renderer>().material.color = Color.red;
        physicalState = PhysicalState.Afraid;
        yield return new WaitForSeconds(4f);
        GetComponent<Renderer>().material.color = Color.white;
        //
    }
    private IEnumerator StopFleing()
    {
        m_rigidbody.velocity = m_stb.Flee(this.transform, target);
        yield return new WaitForSeconds(4f);
        moveState = MovementAgent.None;
        actionState = ActionAgent.None;
        physicalState = PhysicalState.Tired;

    }
    private IEnumerator StarChasing()
    {
        GetComponent<Renderer>().material.color = Color.black;
        m_rigidbody.velocity = m_stb.Seek(this.transform, target);
        yield return new WaitForSeconds(3f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(m_eyePercivedAgent.position, radEyes);
    }

    public void BeingAttack()
    {
        switch (physicalState)
        {
                case PhysicalState.Tired:
                statsBA.Armadura -= 5f;
                break; 
                case PhysicalState.Afraid:
                statsBA.Vida -= 5f;
                break;
                case PhysicalState.Happy: break; 

        }
    }
    
}
