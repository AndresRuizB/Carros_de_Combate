using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviorDesigner.Runtime.Tasks.IAV.CarrosCombate
{
	[TaskCategory("Tanks")]
	public class CalculaPosSniper : Action
	{
		[Tooltip("Transform del enemigo")] public SharedTransform enemyTransform;
		[Tooltip("Canal de voz del tanque")] public SharedCanalEscuadron canal;
		[Tooltip("id del tanque")] public SharedInt id;

		public float radioMiedoJugador;
		public float velocidadHuir;

		protected UnityEngine.AI.NavMeshAgent navMeshAgent;
		protected Vector3 dirMerodeo;

		public override void OnAwake()
		{
			navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
			dirMerodeo = Random.insideUnitSphere.normalized;
		}

		public override TaskStatus OnUpdate()
		{
			Vector3 posicionAMoverse = new Vector3();

			if (estaJugadorCerca())	//Si el jugador está cerca huimos, cuanto más cerca este, mas lejos huimos
			{
				Vector3 dirAlejarse = transform.position - enemyTransform.Value.position;

				float distancia = dirAlejarse.magnitude;

				posicionAMoverse = transform.position + Vector3.RotateTowards(dirAlejarse.normalized * velocidadHuir / distancia, -transform.position, 0.1f, 0);
			}
			else {  //Si no merodeamos buscando un buen tiro

				dirMerodeo = Vector3.RotateTowards(dirMerodeo, Random.insideUnitSphere.normalized, 0.0001f, 0); 
				posicionAMoverse = transform.position + Vector3.RotateTowards(dirMerodeo.normalized, -transform.position, 0.1f, 0);
			}

			NavMeshHit hit = new NavMeshHit();
			NavMesh.SamplePosition(posicionAMoverse, out hit, 100000f, NavMesh.AllAreas);
			canal.Value.objetivosEquipo[id.Value] = hit.position;

			Debug.DrawLine(transform.position, hit.position, Color.green);

			return TaskStatus.Success;
		}

		/// <summary>
		/// Calcula la distancia en el navmesh hasta el jugador y devuelve true si esta cerca
		/// </summary>
		/// <returns></returns>
		bool estaJugadorCerca() {
			NavMeshPath path = new NavMeshPath();
			NavMesh.CalculatePath(transform.position, enemyTransform.Value.position, NavMesh.AllAreas, path);
			return CombatUtils.GetPathLength(path) < radioMiedoJugador;
		}
	}
}