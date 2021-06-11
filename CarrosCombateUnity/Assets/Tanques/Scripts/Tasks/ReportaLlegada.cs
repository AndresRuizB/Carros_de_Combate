using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviorDesigner.Runtime.Tasks.IAV.CarrosCombate
{
    [TaskCategory("Tanks")]
    public class ReportaLlegada : Action
    {
        [Tooltip("Canal de voz del tanque")] public SharedCanalEscuadron canal;
        [Tooltip("id del tanque")] public SharedInt id;

        // navmeshAgent
        private NavMeshAgent agent;

        /// <summary>
        /// Cache the component references.
        /// </summary>
        public override void OnAwake()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        // Seek the destination. Return success once the agent has reached the destination.
        // Return running if the agent hasn't reached the destination yet
        public override TaskStatus OnUpdate()
        {
            canal.Value.listoParaAtacar(id.Value, canal.Value.tiempoParaAtacar[id.Value]/ agent.speed);

            return TaskStatus.Success;
        }

        public override void OnDrawGizmos()
        {
            //Gizmos.color = Color.yellow;

            //Gizmos.DrawWireSphere(destination,1f);
        }
    }
}