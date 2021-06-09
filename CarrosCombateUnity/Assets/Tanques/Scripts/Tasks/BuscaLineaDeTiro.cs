using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviorDesigner.Runtime.Tasks.IAV.CarrosCombate
{
    [TaskCategory("Tanks")]
    public class BuscaDistancia : Action
    {
        [Tooltip("Transform del enemigo")]
        public SharedTransform enemyTransform;

        [Tooltip("Id del tanque")]
        public SharedInt id;
        
        [Tooltip("Id del tanque")]
        public SharedCanalEscuadron canal;

        public int numOfRays = 20;
        private float stepAngle;

        // Component references
        private Vector3 squadCentre, back, destination, enemyDir;

        /// <summary>
        /// Cache the component references.
        /// </summary>
        public override void OnAwake()
        {
            stepAngle = 360f / numOfRays;
        }

        // Seek the destination. Return success once the agent has reached the destination.
        // Return running if the agent hasn't reached the destination yet
        public override TaskStatus OnUpdate()
        {
            Ray ray = new Ray(enemyTransform.Value.position, Vector3.forward);
            RaycastHit hit;
            float dist = float.MinValue;
            Vector3 pos = transform.position;
           for(int i = 0; i < numOfRays; i++)
           {
                ray.direction = Quaternion.AngleAxis(stepAngle * i, Vector3.up) * Vector3.forward;
                if (Physics.Raycast(ray,out hit) && hit.distance > dist)
                {
                    NavMeshHit navMeshPt;
                    NavMesh.SamplePosition(hit.point, out navMeshPt, 1f,NavMesh.AllAreas);
                    dist = hit.distance;
                    pos = navMeshPt.position;
                }
           }
           canal.Value.objetivosEquipo[id.Value] = pos;
           return TaskStatus.Success;
        }

        /// <summary>
        /// Stop pathfinding.
        /// </summary>

        public override void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            for (int i = 0; i < numOfRays; i++)
            {
                Vector3 dir = Quaternion.AngleAxis(stepAngle * i, Vector3.up) * Vector3.forward;
                Gizmos.DrawRay(enemyTransform.Value.position, dir);
            }
            Gizmos.DrawWireCube(destination, Vector3.one);
        }
    }
}