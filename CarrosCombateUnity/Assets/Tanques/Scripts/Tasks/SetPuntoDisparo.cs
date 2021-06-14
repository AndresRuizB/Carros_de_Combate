using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviorDesigner.Runtime.Tasks.IAV.CarrosCombate
{
    [TaskCategory("Tanks")]
    public class SetPuntoDisparo : Action
    {
        [Tooltip("Transform del enemigo")] public SharedTransform enemyTransform;
        [Tooltip("Canal de voz del tanque")] public SharedCanalEscuadron canal;
        [Tooltip("id del tanque")] public SharedInt id;

        [Tooltip("Punto desde el que disparamos")]
        public SharedTransform puntoDisparo;

        public int numOfRays = 20;

        // Component references
        private Vector3 destination;

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
           //destination = CombatUtils.lineaDeVisionCercana(transform, enemyTransform.Value, numOfRays);
           //destination = CombatUtils.puntoCercanoConVision(puntoDisparo.Value, enemyTransform.Value, 10); 
           destination = CombatUtils.lineaVisionEnCamino(transform, enemyTransform.Value);

           if (destination == Vector3.positiveInfinity)
                return TaskStatus.Failure;
           NavMeshHit hit;
           canal.Value.objetivosEquipo[id.Value] = destination;

            NavMeshPath path = new NavMeshPath();
            CombatUtils.GetPath(path, transform.position, destination, NavMesh.AllAreas);
            canal.Value.tiempoParaAtacar[id.Value] = CombatUtils.GetPathLength(path) / agent.speed;

            return TaskStatus.Success;
        }

        public override void OnDrawGizmos()
        {
            //Gizmos.color = Color.yellow;

            //Gizmos.DrawWireSphere(destination,1f);
        }
    }
}