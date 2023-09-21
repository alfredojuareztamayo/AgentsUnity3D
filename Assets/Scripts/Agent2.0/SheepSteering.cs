using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class SheepSteering : MonoBehaviour
{
    [SerializeField] Transform leader;
    [SerializeField] float followDist;
    private StatsBasicAgent statsBA;
    private Rigidbody m_rigidbody;
    void Start()
    {
        InitializeComponents();
        followDist = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        if((Vector3.Distance(transform.position, leader.position) > followDist))
        {
            m_rigidbody.velocity = SteeringBehaviour2.Seek(transform, leader.position);
        }
        //Vector3 Follow = (SteeringBehaviour2.LeaderFollowing(transform,leader,followDist));
       
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
      //  layerMask = checkLayerTeam ? 1 << 6 : 1 << 7;
    }
}
