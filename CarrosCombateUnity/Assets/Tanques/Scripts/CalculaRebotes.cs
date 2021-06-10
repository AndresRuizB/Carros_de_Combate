using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculaRebotes : MonoBehaviour
{
	public GameObject jugador;
	public int numRayos = 4;
	public int capaParedes = 6;
	public int capaObjetivo = 7;
	public float distMaxima = 20f;
	public float distMinima = 1f;
	public int numeroRebotes = 1;

	public bool debug = true;
	LayerMask bitMCombinada;

	// Start is called before the first frame update
	void Start()
	{
		bitMCombinada = (1 << capaObjetivo) | (1 << capaParedes);
	}


	bool calculaRebotes(GameObject tirador, GameObject objetivo, int nRayos, int nRebotes, out Vector3 direccion)
	{
		Vector3 dir = objetivo.transform.position - tirador.transform.position;
		dir.y *= 0;
		float incremento = 360f / nRayos;
		direccion = dir;

		for (int iRayo = 0; iRayo < nRayos; iRayo++)
		{
			dir = Quaternion.AngleAxis(incremento * iRayo * ((iRayo % 2 * -1) | 1), Vector3.up) * dir;
			direccion = dir;
			if(calcReboteRec(nRebotes,tirador.transform.position, dir, distMaxima, bitMCombinada))
			{
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

			if (hitInfo.transform.gameObject.GetComponent<PlayerMovement>() != null)
			{
				Debug.DrawLine(origen, hitInfo.point, Color.green);
				return true;
			}
			else if(rRestantes > 0)
			{
				Debug.DrawLine(origen, hitInfo.point, Color.red);
				return calcReboteRec(rRestantes - 1, hitInfo.point, Vector3.Reflect(direccion, hitInfo.normal), distMax,  layerM);
			}
		}

		return false;
	}

	void Update()
	{
		Vector3 d;

		if (calculaRebotes(this.gameObject, jugador, numRayos, numeroRebotes, out d)) {
			Debug.DrawRay(new Vector3(0, 1, 0), d, Color.blue);
			Debug.Log("adwda");
		}


		//Vector3 dir = transform.position - jugador.transform.position;  //inicializamos con el vector que va directamente del jug al sniper

		//dir.y *= 0;

		//float incremento = 360f / numRayos;

		//for (int i = 0; i < numRayos; i++)
		//{
		//	//Vamos a tantear priorizando los rayos que apuntan en la dirección general del jugador, alternando entre un lado y el otro
		//	dir = Quaternion.AngleAxis(incremento * i * ((i % 2 * -1) | 1), Vector3.up) * dir;

		//	Ray ray = new Ray(jugador.transform.position, dir);
		//	RaycastHit hitInfo;

		//	if (Physics.Raycast(ray, out hitInfo, distMaxima, bitMParedes))
		//	{
		//		if (hitInfo.distance > distMinima)  //Si estamos muy cerca de la pared, no disparamos
		//		{
		//			if (debug) Debug.DrawLine(jugador.transform.position, hitInfo.point, new Color((1f - i / (float)numRayos), 0, 0, 1));

		//			Ray ray2 = new Ray(hitInfo.point, Vector3.Reflect(dir, hitInfo.normal));
		//			RaycastHit hitInfo2;

		//			if (Physics.Raycast(ray2, out hitInfo2, distMaxima, bitMEnemigos))
		//			{
		//				if (debug) Debug.DrawLine(hitInfo.point, hitInfo2.point, Color.yellow);

		//				if (hitInfo2.transform.tag == "Sniper")
		//				{
		//					if (debug) Debug.DrawLine(hitInfo.point, hitInfo2.point, Color.green);
		//					if (debug) Debug.Log("le doy");
		//				};
		//			}
		//		}
		//	}
		//}
	}
}
