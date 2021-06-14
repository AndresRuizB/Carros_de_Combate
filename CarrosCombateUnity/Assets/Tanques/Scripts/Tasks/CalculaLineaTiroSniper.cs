using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviorDesigner.Runtime.Tasks.IAV.CarrosCombate
{
	[TaskCategory("Tanks")]
	public class CalculaLineaTiroSniper : Action
	{
		[Tooltip("Transform del enemigo")] public SharedTransform enemyTransform;
		[Tooltip("Canal de voz del tanque")] public SharedCanalEscuadron canal;
		[Tooltip("id del tanque")] public SharedInt id;
		[Tooltip("bool para el conditional abort")] public SharedBool hayLineaDisparo;
		[Tooltip("dir en la que disparar")] public SharedVector3 dirDisparo;

		public int numRayos = 4;
		public int capaParedes = 6;
		public int capaObjetivo = 7;
		public float distMaxima = 20f;
		public int numeroRebotes = 1;
		public float cooldown = 1;

		protected LayerMask bitMCombinada;
		protected float cooldownActual;

		public override void OnAwake()
		{
			bitMCombinada = (1 << capaObjetivo) | (1 << capaParedes);
			cooldownActual = cooldown;
		}

		bool calculaRebotes(Transform tirador, Transform objetivo, int nRayos, int nRebotes, out Vector3 direccion)
		{
			Vector3 dir = tirador.position - objetivo.position;
			dir.y *= 0;
			float incremento = 360f / nRayos;
			direccion = dir;

			for (int iRayo = 0; iRayo < nRayos; iRayo++)
			{
				dir = Quaternion.AngleAxis(incremento * iRayo * ((iRayo % 2 * -1) | 1), Vector3.up) * dir;
				direccion = dir;
				if (calcReboteRec(nRebotes, tirador.position, dir, distMaxima, bitMCombinada))
				{
					Debug.DrawLine(tirador.position, objetivo.position, Color.red, 0.5f);
					return true;
				}
			}

			return false;
		}

		bool calcReboteRec(int rRestantes, Vector3 origen, Vector3 direccion, float distMax, int layerM)
		{
			Ray ray = new Ray(origen, direccion);
			RaycastHit hitInfo;


			if (Physics.Raycast(ray, out hitInfo, distMax, layerM))
			{

				float distance = (origen - hitInfo.point).magnitude;

				if (hitInfo.transform.gameObject.GetComponent<PlayerMovement>() != null)
				{
					Debug.DrawLine(origen, hitInfo.point, Color.green, 1f);
					return true;
				}
				else if (rRestantes > 0 && distance > 1f)
				{
					Debug.DrawLine(origen, hitInfo.point, Color.red, 1f);
					return calcReboteRec(rRestantes - 1, hitInfo.point, Vector3.Reflect(direccion, hitInfo.normal), distMax, layerM);
				}
			}

			return false;
		}

		public override TaskStatus OnUpdate()
		{
			if (cooldownActual <= 0f)
			{
				Debug.Log("vamo a ver si podemos pium pium");
				Vector3 d = new Vector3();

				hayLineaDisparo.SetValue(calculaRebotes(transform, enemyTransform.Value, numRayos, numeroRebotes, out d));
				dirDisparo.SetValue(d);
				cooldownActual = cooldown;

				if (hayLineaDisparo.Value)
				{
					Debug.DrawLine(transform.position, transform.position + d, Color.blue, 1f);
					Debug.Log("Le vemos al pana");
				}
			}
			else Debug.Log("toco esperar  " + cooldownActual + Time.deltaTime);
			cooldownActual -= Time.deltaTime;

			return TaskStatus.Success;
		}
	}
}
