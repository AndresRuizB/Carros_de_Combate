using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviorDesigner.Runtime.Tasks.IAV.CarrosCombate
{
    [TaskCategory("Tanks")]
    public class SetPuntoDisparo : Action
    {
        [Tooltip("Transform del enemigo")]
        public SharedTransform enemyTransform;
        [Tooltip("Variable en la que guardar")]
        public SharedVector3 posicionDisparo;
        [Tooltip("Canal de voz del tanque")] public SharedCanalEscuadron canal;
        [Tooltip("id del tanque")] public SharedInt id;

        public int numOfRays = 20;

        // Component references
        private Vector3 destination;

        /// <summary>
        /// Cache the component references.
        /// </summary>
        public override void OnAwake()
        {
           
        }

        // Seek the destination. Return success once the agent has reached the destination.
        // Return running if the agent hasn't reached the destination yet
        public override TaskStatus OnUpdate()
        {
           destination = CombatUtils.lineaDeVisionCercana(transform, enemyTransform.Value, numOfRays);
            posicionDisparo.Value = destination;
            canal.Value.listoParaAtacar(id.Value,Vector3.Distance(transform.position,destination));

            return (destination != Vector3.positiveInfinity)?TaskStatus.Success:TaskStatus.Failure;
        }

        /// <summary>
        /// Stop pathfinding.
        /// </summary>

        public override void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;

            Gizmos.DrawWireCube(destination, Vector3.one);
        }
    }
}