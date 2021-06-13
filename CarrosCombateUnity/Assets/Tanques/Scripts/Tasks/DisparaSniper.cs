using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviorDesigner.Runtime.Tasks.IAV.CarrosCombate
{
	[TaskCategory("Tanks")]
	public class DisparaSniper : Action
	{
		[Tooltip("bool para el conditional abort")] public SharedBool hayLineaDisparo;
		[Tooltip("dir en la que disparar")] public SharedVector3 dirDisparo;

		public GameObject balaSniper;
		public Transform torretaSniper;
		public Transform puntoDisparo;


		public override TaskStatus OnUpdate()
		{
			if (hayLineaDisparo.Value)
			{
				torretaSniper.LookAt(torretaSniper.position + dirDisparo.Value);

				GameObject.Instantiate(balaSniper, puntoDisparo.position, puntoDisparo.rotation);
			}

			return TaskStatus.Success;
		}
	}
}