using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviorDesigner.Runtime.Tasks.IAV.CarrosCombate
{
    [TaskCategory("Tanks")]
    public class Flanquea : Action
    {
        [Tooltip("Lista de transforms del escuadron")]
        public SharedTransformList posicionesEscuadron;
        [Tooltip("Transform del enemigo")]
        public SharedTransform enemyTransform;

        // Component references
        protected UnityEngine.AI.NavMeshAgent navMeshAgent;

        /// <summary>
        /// Cache the component references.
        /// </summary>
        public override void OnAwake()
        {
            navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        }

        public Vector3 centroEquipo(bool incluyeActivo = false)
        {
            Vector3 centre = Vector3.zero;
            foreach(Transform t in posicionesEscuadron.Value)
            {
                if (t != transform)
                    centre += t.position;
            }
            return centre / (float)(posicionesEscuadron.Value.Count - 1);
        }

        // Seek the destination. Return success once the agent has reached the destination.
        // Return running if the agent hasn't reached the destination yet
        bool LineOfSight(Vector3 a, Transform b)
        {
            Vector3 dir = b.position - a;
            Ray ray = new Ray(a, dir);
            RaycastHit hit;
            if (Physics.Raycast(ray,out hit))
            {
                return hit.transform == b;
            }
            return false;
        }
        Vector3 findCoverClose(Vector3 to, float distance)
        {
            Vector3 pos;
            NavMeshHit hit;
            Vector3 enemyPos = enemyTransform.Value.position;
            int i = 0;
            do
            {
                ++i;
                pos = to + Random.insideUnitSphere * distance;
                NavMesh.SamplePosition(pos, out hit, 1f, NavMesh.AllAreas);

            } while (i<100 && LineOfSight(hit.position+Vector3.up*2f, enemyTransform.Value));
            if (i >= 100)
            {
                Debug.LogError("No funciona er findcover");
            }
            else
            {
                Debug.Log(i);
                Debug.Log(hit.position);
            }
            return hit.position;
        }
        public override TaskStatus OnUpdate()
        {
            Vector3 centroSquad = centroEquipo();
            Vector3 dirToEnemy = enemyTransform.Value.position - centroSquad;
           

            Ray ray = new Ray(enemyTransform.Value.position,dirToEnemy);
            Debug.DrawRay(enemyTransform.Value.position,dirToEnemy,Color.red,10);
            Debug.DrawRay(centroSquad, Vector3.up,Color.yellow,10);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit,10000f))
            {
                if (LineOfSight(transform.position, enemyTransform.Value))
                    Debug.Log("aaa");
                else
                {
                    Debug.Log("bbb");
                }

               GetComponent<NavMeshAgent>().SetDestination(findCoverClose(hit.point,5));
            }


            return TaskStatus.Success;
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