using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontalAxis;
    private float verticalAxis;

    private Rigidbody rb;

    public float speed = 1f;
    public float maxSpeed = 20f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalAxis = Input.GetAxisRaw("Horizontal");
        verticalAxis = Input.GetAxisRaw("Vertical");
    }

	private void FixedUpdate()
	{
        if(rb.velocity.magnitude < maxSpeed) rb.AddForce(new Vector3(horizontalAxis, 0, verticalAxis).normalized * speed);
	}
}
