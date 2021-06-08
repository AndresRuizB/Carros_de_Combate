using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculaRebotes : MonoBehaviour
{
	public GameObject jugador;
	public int nRayos = 4;
	public int capaParedes = 6;
	public int capaEnemigos = 7;
	public float distMaxima = 20f;
	public float distMinima = 1f;

	public bool debug = true;

	LayerMask bitMParedes;
	LayerMask bitMEnemigos;

	// Start is called before the first frame update
	void Start()
	{
		bitMParedes = 1 << capaParedes;
		bitMEnemigos = (1 << capaEnemigos) | (1 << capaParedes);
	}

	// Update is called once per frame
	void Update()
	{		
		Vector3 dir = transform.position - jugador.transform.position;  //inicializamos con el vector que va directamente del jug al sniper

		dir.y *= 0;

		float incremento = 360f / nRayos;

		for (int i = 0; i < nRayos; i++)
		{
			//Vamos a tantear priorizando los rayos que apuntan en la dirección general del jugador, alternando entre un lado y el otro
			dir = Quaternion.AngleAxis(incremento * i * ((i % 2 * -1)| 1), Vector3.up) * dir;

			Ray ray = new Ray(jugador.transform.position, dir);
			RaycastHit hitInfo;

			if (Physics.Raycast(ray, out hitInfo, distMaxima, bitMParedes))
			{
				if (hitInfo.distance > distMinima)  //Si estamos muy cerca de la pared, no disparamos
				{
					if (debug) Debug.DrawLine(jugador.transform.position, hitInfo.point, new Color((1f - i / (float)nRayos), 0, 0, 1));

					Ray ray2 = new Ray(hitInfo.point, Vector3.Reflect(dir, hitInfo.normal));
					RaycastHit hitInfo2;

					if (Physics.Raycast(ray2, out hitInfo2, distMaxima, bitMEnemigos))
					{
						if (debug) Debug.DrawLine(hitInfo.point, hitInfo2.point, Color.yellow);

						if (hitInfo2.transform.tag == "Sniper")
						{
							if (debug) Debug.DrawLine(hitInfo.point, hitInfo2.point, Color.green);
							if (debug) Debug.Log("le doy");
						};
					}
				}
			}
		}
	}
}
