using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.IAV.CarrosCombate
{
    [TaskCategory("Tanks")]
    public class VeAObjetivo : Action
    {
        [Tooltip("Velocidad del agente")]
        public SharedFloat velocidad = 10;
        [Tooltip("Velocidad angular del agente")]
        public SharedFloat velocidadAngular = 120;
        [Tooltip("Distancia a la que consideraremos que hemos llegado al objetivo. Debe superar la del NavMeshAgent")]
        public SharedFloat distanciaLlegada = 0.2f;

        [Tooltip("The GameObject that the agent is seeking")]
        public SharedCanalEscuadron canal;

        [Tooltip("id del tanque")]
        public SharedInt id;

        // Component references
        protected UnityEngine.AI.NavMeshAgent navMeshAgent;

        /// <summary>
        /// Cache the component references.
        /// </summary>
        public override void OnAwake()
        {
            navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        }

        /// <summary>
        /// Allow pathfinding to resume.
        /// </summary>
        public override void OnStart()
        {
            navMeshAgent.speed = velocidad.Value;
            navMeshAgent.angularSpeed = velocidadAngular.Value;
#if UNITY_5_1 || UNITY_5_2 || UNITY_5_3 || UNITY_5_4 || UNITY_5_5
            navMeshAgent.Resume();
#else
            navMeshAgent.isStopped = false;
#endif

            SetDestination(Target());
        }

        // Seek the destination. Return success once the agent has reached the destination.
        // Return running if the agent hasn't reached the destination yet
        public override TaskStatus OnUpdate()
        {
            transform.LookAt(navMeshAgent.steeringTarget);
            if (HasArrived())
            {
                return TaskStatus.Success;
            }
            SetDestination(Target());

            return TaskStatus.Running;
        }

        // Return targetPosition if target is null
        private Vector3 Target()
        {
            return canal.Value.objetivosEquipo[id.Value];
        }

        /// <summary>
        /// Set a new pathfinding destination.
        /// </summary>
        /// <param name="destination">The destination to set.</param>
        /// <returns>True if the destination is valid.</returns>
        private bool SetDestination(Vector3 destination)
        {
#if UNITY_5_1 || UNITY_5_2 || UNITY_5_3 || UNITY_5_4 || UNITY_5_5
            navMeshAgent.Resume();
#else
            navMeshAgent.isStopped = false;
#endif
            return navMeshAgent.SetDestination(destination);
        }

        /// <summary>
        /// Has the agent arrived at the destination?
        /// </summary>
        /// <returns>True if the agent has arrived at the destination.</returns>
        private bool HasArrived()
        {
            //return Vector3.Distance(transform.position, target.Value.transform.position) <= arriveDistance.Value;
            // The path hasn't been computed yet if the path is pending.
            float remainingDistance;
            if (navMeshAgent.pathPending)
            {
                remainingDistance = float.PositiveInfinity;
            }
            else
            {
                remainingDistance = navMeshAgent.remainingDistance;
            }

            return remainingDistance <= distanciaLlegada.Value;
        }

        /// <summary>
        /// Stop pathfinding.
        /// </summary>
        private void Stop()
        {
            if (navMeshAgent.hasPath)
            {
#if UNITY_5_1 || UNITY_5_2 || UNITY_5_3 || UNITY_5_4 || UNITY_5_5
                navMeshAgent.Stop();
#else
                navMeshAgent.isStopped = true;
#endif
            }
        }

        /// <summary>
        /// The task has ended. Stop moving.
        /// </summary>
        public override void OnEnd()
        {
            Stop();
        }

        /// <summary>
        /// The behavior tree has ended. Stop moving.
        /// </summary>
        public override void OnBehaviorComplete()
        {
            Stop();
        }
    }
}
