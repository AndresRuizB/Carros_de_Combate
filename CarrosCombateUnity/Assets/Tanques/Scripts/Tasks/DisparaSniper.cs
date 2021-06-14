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


		public override TaskStatus OnUpdate()
		{
			if (hayLineaDisparo.Value)
			{
				torreta.LookAt(torreta.position + dirDisparo.Value);

				GameObject.Instantiate(balaPrefab, puntoDisparo.position, puntoDisparo.rotation);
				hayLineaDisparo.SetValue(false);
			}

			return TaskStatus.Success;
		}
	}
}