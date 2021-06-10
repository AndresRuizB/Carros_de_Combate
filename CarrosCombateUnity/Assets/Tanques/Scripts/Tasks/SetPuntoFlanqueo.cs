using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviorDesigner.Runtime.Tasks.IAV.CarrosCombate
{
    [TaskCategory("Tanks")]
    public class Flanquea : Action
    {
        [Tooltip("Id del tanque")]
        public SharedInt id;
        [Tooltip("Transform del enemigo")]
        public SharedTransform enemyTransform;

        [Tooltip("Canal de voz del tanque")] 
        public SharedCanalEscuadron canal;

        public float coverRad = 5f;

        // Component references
        protected UnityEngine.AI.NavMeshAgent navMeshAgent;
        private Vector3 squadCentre, back, destination, enemyDir;

        /// <summary>
        /// Cache the component references.
        /// </summary>
        public override void OnAwake()
        {
            navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        }
        
        Vector3 findCoverClose(Vector3 to, float distance)
        {
            Vector3 pos;
            NavMeshHit hit = new NavMeshHit();
            int i = 0;
            do
            {
                ++i;
                pos = to + Random.insideUnitSphere * distance;

            } while (i < 100 && (!NavMesh.SamplePosition(pos, out hit, coverRad, NavMesh.AllAreas)
            || CombatUtils.hayLineaVision(hit.position + Vector3.up * 0.5f, enemyTransform.Value)));
            if (i >= 100)
            {
                Debug.LogError("No funciona findcover");
                NavMesh.SamplePosition(pos, out hit, 5000f, NavMesh.AllAreas);
            }
            else
            {
                //Debug.Log(i);
                //Debug.Log(hit.position);
            }
            return hit.position;
        }
        public override TaskStatus OnUpdate()
        {
            squadCentre = CombatUtils.centroTransforms(canal.Value.transformsEquipo ,transform);
            enemyDir = enemyTransform.Value.position - squadCentre;


            Ray ray = new Ray(enemyTransform.Value.position, enemyDir);
            Debug.DrawRay(enemyTransform.Value.position, enemyDir, Color.red, 10);
            Debug.DrawRay(squadCentre, Vector3.up, Color.yellow, 10);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 10000f))
            {
                back = hit.point;
                //TODO distinguir entre obstaculos y muros finales
                destination = findCoverClose(hit.point, coverRad);
                //GameObject.Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cube), destination, Quaternion.identity);
                canal.Value.objetivosEquipo[id.Value] = destination;
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

        public override void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(back, coverRad);
            Gizmos.DrawWireCube(destination, Vector3.one);
        }
    }
}