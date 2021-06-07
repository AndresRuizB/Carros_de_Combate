using UnityEngine;

public class RotateBodyPlayer : MonoBehaviour
{
	private Rigidbody rb;

	// Start is called before the first frame update
	void Start()
	{
		rb = GetComponentInParent<Rigidbody>();
	}	

	// Update is called once per frame
	void Update()
	{
		if (rb.velocity.magnitude > 0.5f)
		{
			Vector3 v = this.transform.position + rb.velocity;
			v.y = v.y * 0 + this.transform.position.y;

			if (rb.velocity.magnitude > 0.1f) this.transform.LookAt(v);
		}
	}
}
