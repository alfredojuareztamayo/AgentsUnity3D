using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Class representing the behavior of a surveillance agent in the game.
/// </summary>
public class AgentVigila : MonoBehaviour
{
    private StatsBasicAgent statsBA;
   // private SteeringBehaviour2 m_stb;
    private Rigidbody m_rigidbody;
    private Collider[] eyePercived;

    [SerializeField] Transform target;

   // private enum MovementAgent { None, Seek, Flee }
    //private enum ActionAgent { None, Attack, Heal, Running }

    private MovementAgent moveState = MovementAgent.None;
    private ActionAgent actionState = ActionAgent.None;

    [Header("Set to true for team blue")]
    public bool checkLayerTeam;
    private int layerMask;
    public bool Wandering = false;
   
    //private float maxVida = 1000f; // Valor máximo de vida
    private float radEyes = 10f;
    [SerializeField] Transform eyesTrasform;
    [SerializeField] TMP_Text _Text; // Componente TextMeshPro para mostrar texto en Unity
    private void Start()
    {
        InitializeComponents();
    }

    private void Update()
    {
        PerceptionManager();
        DecisionManager();
        if (Wandering)
        {
            _Text.text = "Wandering";
            m_rigidbody.velocity = SteeringBehaviour2.Wander(transform, 5f, 10f, 0);
        }
       // Debug.Log(eyePercived.Length);
    }

    private void FixedUpdate()
    {
        eyePercived = Physics.OverlapSphere(statsBA.EyePerception.position, statsBA.EyesPerceptionRad, layerMask);

    }

    /// <summary>
    /// Manages perception of nearby agents.
    /// </summary>
    private void PerceptionManager()
    {
        if (eyePercived == null)
        {
            // Si no se percibe ningún objetivo, establecer el objetivo como nulo (ninguno)
            target = null;
            //moveState = MovementAgent.None;
            //actionState = ActionAgent.None;
            return;
        }
        foreach (Collider obj in eyePercived)
        {
            float dist = Vector3.Distance(transform.position, obj.transform.position);
            TypeAgent2 tempType = obj.GetComponent<StatsBasicAgent>().TypeAgent;
            //float life = obj.GetComponent<StatsBasicAgent>().Vida;

            if (dist > 50)
            {
                moveState = MovementAgent.None;
               
                continue;
            }

            //if (tempType == TypeAgent2.Melee && dist > 30f)
            //{
            //    target = obj.transform;
            //    moveState = MovementAgent.Seek;
            //    actionState = ActionAgent.None;
            //    break;
            //}


            if (tempType == TypeAgent2.Tank)
            {
                target = obj.transform;
                
            }

        }
    }
    /// <summary>
    /// Manages agent's decisions based on its perception.
    /// </summary>
    private void DecisionManager()
    {
       
        // Asegurarse de que target sea nulo si no se ha encontrado ningún objetivo en la percepción
        if (target == null)
        {
            moveState = MovementAgent.None;
            return;

        }
        float life = target.GetComponent<StatsBasicAgent>().Vida;
        float strengh = target.GetComponent<StatsBasicAgent>().Fuerza;
        float armor = target.GetComponent<StatsBasicAgent>().Armadura;
        if (life > 100)
        {
            moveState = MovementAgent.Flee;
            MovementManager(moveState);

        }
        if (life < 100)
        {
            if(strengh > 100)
            {
                if(armor > 100)
                {
                    moveState = MovementAgent.Arrival;
                    MovementManager(moveState);
                }
                if(armor < 100)
                {
                    moveState = MovementAgent.Pursuit;
                    MovementManager(moveState);
                }
                

            }
            if(strengh < 100)
            {
                moveState = MovementAgent.Seek;
                MovementManager(moveState);
            }
        }


        
        ActionManager(actionState);
       // eyePercived = null;
    }
    /// <summary>
    /// Manages agent's movement based on movement state.
    /// </summary>
    /// <param name="movementAgent">Agent's movement state.</param>
    private void MovementManager(MovementAgent movementAgent)
    {
        switch (movementAgent)
        {
            case MovementAgent.None:
                break;
            case MovementAgent.Seek:
                _Text.text = "Seeking";
                GetComponent<Renderer>().material.color = Color.blue;
                m_rigidbody.velocity = SteeringBehaviour2.Seek(transform, target.position);
                break;
            case MovementAgent.Flee:
                _Text.text = "Correle aqui roban";
                GetComponent<Renderer>().material.color = Color.red;
                m_rigidbody.velocity = SteeringBehaviour2.Flee(transform, target.position);
               // SteeringBehaviour2.lookAt(transform);
                break;
            case MovementAgent.Arrival:
                _Text.text = "Arrival";
                GetComponent<Renderer>().material.color = Color.black;
                m_rigidbody.velocity = SteeringBehaviour2.Arrival(transform, target.position);
                break;
            case MovementAgent.Wander:
               
                break;
            case MovementAgent.Pursuit:
                _Text.text = "Calculando para pedirte un cinco";
                GetComponent<Renderer>().material.color = Color.yellow;
                m_rigidbody.velocity = SteeringBehaviour2.Pursuit(transform, target);
                break;
        }
    }
    /// <summary>
    /// Manages agent's actions based on action state.
    /// </summary>
    /// <param name="actionAgent">Agent's action state.</param>
    private void ActionManager(ActionAgent actionAgent)
    {
        switch (actionAgent)
        {
            case ActionAgent.None:
                // Realizar alguna acción cuando el estado sea "None"
                break;
            case ActionAgent.Attack:
              
                break;
            case ActionAgent.Heal:
                
                break;
            case ActionAgent.Running:
                // Realizar alguna acción cuando el estado sea "Running"
                break;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(eyesTrasform.position, radEyes);
    }
    /// <summary>
    /// Coroutine to start the Arrival behavior.
    /// </summary>
    /// <returns>Yield instruction.</returns>
    IEnumerator StartArrival()
    {
        GetComponent<Renderer>().material.color = Color.black;
        m_rigidbody.velocity = SteeringBehaviour2.Arrival(transform, target.position);
        //SteeringBehaviour2.lookAt(transform);
        yield return new WaitForSeconds(3f);
    }
    private void InitializeComponents()
    {
        statsBA = GetComponent<StatsBasicAgent>();
        //m_stb = new SteeringBehaviour2();
        m_rigidbody = GetComponent<Rigidbody>();
        AgentBasic2 agentVigila = new AgentBasic2(TypeAgent2.Vigila);

        statsBA.Vida = agentVigila.Vida; // Valor predeterminado de vida
        statsBA.Velocidad = agentVigila.Velocidad; // Velocidad predeterminada
        statsBA.Armadura = agentVigila.Armadura; // Armadura predeterminada
        statsBA.Fuerza = agentVigila.Fuerza; // Fuerza predeterminada
        statsBA.MaxVel = agentVigila.MaxVel; // Velocidad máxima predeterminada
        statsBA.SteeringForce = agentVigila.SteeringForce; // Fuerza de dirección predeterminada
        statsBA.EyesPerceptionRad = 10f;
        statsBA.ArrivalRadius = 3.5f;
        statsBA.TypeAgent = TypeAgent2.Vigila;
        statsBA.WanderAngleDelta = 2f;
        // Establecer la máscara de capa según el equipo
        layerMask = checkLayerTeam ? 1 << 6 : 1 << 7;
    }
}
