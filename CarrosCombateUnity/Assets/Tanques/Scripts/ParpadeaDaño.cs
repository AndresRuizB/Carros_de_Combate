using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParpadeaDaño : MonoBehaviour
{
	public string tagBala;
	public int nFlahes;
	public float tiempoEntreFlashes;

	private float tFlash;
	private int flashesRestantes;
	private MeshRenderer[] renderers;
	private bool visible = false;

	void Start()
	{
		renderers = GetComponentsInChildren<MeshRenderer>();
		tFlash = tiempoEntreFlashes/2;
	}

	// Update is called once per frame
	void Update()
	{
		if (flashesRestantes > 0) {
			if (tFlash < 0) {
				foreach (MeshRenderer mr in renderers) {
					mr.enabled = visible;
				}
				tFlash = tiempoEntreFlashes/2;

				visible = !visible;
				flashesRestantes--;

			}

		}

		tFlash -= Time.deltaTime;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.transform.gameObject.CompareTag(tagBala)) {
			flashea();
		}
	}

	private void flashea() {
		flashesRestantes += nFlahes * 2;
	}
}
