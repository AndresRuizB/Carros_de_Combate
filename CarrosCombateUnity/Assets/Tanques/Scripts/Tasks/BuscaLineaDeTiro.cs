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
        [Tooltip("Variable en la que guardar")]
        public SharedVector3 posicionDistancia;

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
        bool LineOfSight(Vector3 a, Transform b)
        {
            Vector3 dir = b.position - a;
            Ray ray = new Ray(a, dir);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit,1000f))
            {
                return hit.transform == b;
            }
            return false;
        }
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
                    Debug.Log($"{i}: {hit.distance}");
                    NavMeshHit navMeshPt;
                    NavMesh.SamplePosition(hit.point, out navMeshPt, 1f,NavMesh.AllAreas);
                    dist = hit.distance;
                    pos = navMeshPt.position;
                }
            }

            posicionDistancia.Value = pos;
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