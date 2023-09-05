using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AgentShooter : MonoBehaviour
{
    private AgentBasic2 agenteShooter;
    private StatsBasicAgent statsBA;
    private SteeringBehaviour2 m_stb;
    public Transform target;
    private Rigidbody m_rigidbody;
    Collider[] eyePercived;
    [Header("Verdadero para team blue")]
    public bool checkLayerTeam;
    private int layerMask;
    private ActionAgent actionState;
    private MovementAgent moveState;
    [SerializeField] GameObject bullet;

    private void Start()
    {
        statsBA = GetComponent<StatsBasicAgent>();
        // Crear un agente de tipo Shooter
        agenteShooter = new AgentBasic2(TypeAgent2.Shooter);
        m_stb = new SteeringBehaviour2();
        m_rigidbody = GetComponent<Rigidbody>();
        statsBA.Vida = agenteShooter.Vida;
        statsBA.Velocidad = agenteShooter.Velocidad;
        statsBA.Armadura = agenteShooter.Armadura;
        statsBA.Fuerza = agenteShooter.Fuerza;
        statsBA.MaxVel = agenteShooter.MaxVel;
        statsBA.SteeringForce = agenteShooter.SteeringForce; 
        statsBA.TypeAgent = TypeAgent2.Shooter;
        statsBA.EyesPerceptionRad = 30f;

        if (checkLayerTeam)
        {
            layerMask = 1 << 6;
        }
        else
        {
            layerMask = 1 << 7;
        }

        // Mostrar los atributos del agente Shooter en la consola
        //Debug.Log("Tipo de Agente: Shooter");
        //Debug.Log("Vida: " + agenteShooter.Vida);
        //Debug.Log("Armadura: " + agenteShooter.Armadura);
        //Debug.Log("Velocidad: " + agenteShooter.Velocidad);
        //Debug.Log("Fuerza: " + agenteShooter.Fuerza);
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
            if (dist > 50)
            {
                moveState = MovementAgent.None;
                actionState = ActionAgent.None;
                continue;
            }

            tempType = obj.GetComponent<StatsBasicAgent>().TypeAgent;
            if (tempType == TypeAgent2.Melee && dist > 30f)
            {
                target = obj.transform;
                moveState = MovementAgent.None;
                actionState = ActionAgent.Attack;
                //targetOn = true;
                break;
            }
            if (tempType == TypeAgent2.Melee && dist < 5f)
            {
                target = obj.transform;
                moveState = MovementAgent.Flee;
                actionState = ActionAgent.None;
                // targetOn = true;
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
               
                break;
            case MovementAgent.Flee:
                StartCoroutine(StopFleing());
                break;
        }
    }
    private void ActionManager(ActionAgent actionAgent)
    {
        switch (actionAgent)
        {
            case ActionAgent.None:
                //m_particle[0].Stop();
                break;
            case ActionAgent.Attack:
               StartCoroutine(StartAtack());
                break;
            case ActionAgent.Heal:
               
               
                break;
            case ActionAgent.Running:

                break;
        }
    }

    private IEnumerator StopFleing()
    {
        m_rigidbody.velocity = m_stb.Flee(this.transform, target);
        yield return new WaitForSeconds(4f);
        moveState = MovementAgent.None;
        actionState = ActionAgent.None;
       

    }

    private IEnumerator StartAtack()
    {
        Instantiate(bullet, transform.position, transform.rotation);
        Color c = Color.gray;
        target.GetComponent<StatsBasicAgent>().MeAtacan(c);
        yield return new WaitForSeconds(2f);
    }
}
