using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviorDesigner.Runtime.Tasks.IAV.CarrosCombate
{
	[TaskCategory("Tanks")]
	public class disparaSniper : Action
	{
		[Tooltip("bool para el conditional abort")] public SharedBool hayLineaDisparo;
		[Tooltip("dir en la que disparar")] public SharedVector3 dirDisparo;

		public GameObject balaPrefab;
		public Transform torreta;
		public Transform puntoDisparo;
		public float tiempoEntreDisparos = 2f;

		private float tiempoActual;
		private float tiempoUltimoDisparo;

		public override void OnAwake()
		{
			tiempoActual = tiempoEntreDisparos;
			tiempoUltimoDisparo = 0;
		}

		public override TaskStatus OnUpdate()
		{			
			Debug.Log("vamo a disparar (si no podemos) " + tiempoActual);
			if (hayLineaDisparo.Value && Time.time - tiempoUltimoDisparo > tiempoEntreDisparos)
			{
				Debug.Log("vemos y podemos hacer pium");
				torreta.LookAt(torreta.position + dirDisparo.Value);

				GameObject.Instantiate(balaPrefab, puntoDisparo.position, puntoDisparo.rotation);
				hayLineaDisparo.SetValue(false);
				tiempoActual = tiempoEntreDisparos;
				tiempoUltimoDisparo = Time.time;
			}

			return TaskStatus.Success;
		}
	}
}