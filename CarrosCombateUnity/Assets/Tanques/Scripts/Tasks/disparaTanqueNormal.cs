using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviorDesigner.Runtime.Tasks.IAV.CarrosCombate
{
	[TaskCategory("Tanks")]
	public class disparaTanqueNormal : Action
	{

		[Tooltip("Transform del enemigo")]
		public SharedTransform enemyTransform;
		[Tooltip("Id del tanque")]
		public SharedInt id;

		public GameObject balaPrefab;
		public Transform torreta;
		public Transform puntoDisparo;

		public int LayerParedes;
		public int LayerJugador;

		private bool hayLineaDisparo = false;
		private int bitMask;

		private Vector3 dirDisparo;

		public override void OnAwake()
		{
			bitMask = (1 << LayerJugador) | (1 << LayerParedes);
		}

		public override TaskStatus OnUpdate()
		{
			dirDisparo = enemyTransform.Value.position - transform.position;
			dirDisparo.y = 0;

			RaycastHit hit = new RaycastHit();
			Ray rayo = new Ray(transform.position + new Vector3(0,0.8f,0), dirDisparo);

			//Debug.Log("comprobando");
			if (Physics.Raycast(rayo, out hit, 100f, bitMask))
			{
				hayLineaDisparo = hit.transform.gameObject.CompareTag("Player");
				Debug.Log("disparando" + hit.transform.gameObject.name);
			}

			if (hayLineaDisparo)
			{
				torreta.LookAt(torreta.position + dirDisparo);
				Debug.Log("pium " + id);
				GameObject go = GameObject.Instantiate(balaPrefab, puntoDisparo.position, puntoDisparo.rotation);
				Debug.Log("instanciado  " + go.name);
			}

			return TaskStatus.Success;
		}
	}
}