using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBounce : MonoBehaviour
{
    private Rigidbody rb;
    private int rebotesRestantes;
    private Vector3 velocity;

    public float speed = 1f;
    public int nRebotes = 2;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        velocity = transform.forward * speed;
        rebotesRestantes = nRebotes;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (rebotesRestantes > 0 && !collision.collider.gameObject.CompareTag("Player")) {
            Vector3 newDir = Vector3.Reflect(velocity, collision.GetContact(0).normal);
            newDir.y = 0;
            velocity = newDir;
            this.transform.LookAt(transform.position + velocity);
            rebotesRestantes--;
        }else Destroy(this.gameObject);
    }

	private void Update()
	{
        this.transform.position += velocity * Time.deltaTime;
    }
}
