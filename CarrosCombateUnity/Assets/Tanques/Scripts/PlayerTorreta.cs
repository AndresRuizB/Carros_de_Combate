using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTorreta : MonoBehaviour
{
    Vector3 point;
    Transform tr;

    int bitMask;

    float shotTime;

    public int layerMask = 3;
    public Texture2D mouseSkin;
    public Transform puntoDisparo;
    public GameObject bullet;
    public float timeBetweenShots = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
        bitMask = 1 << layerMask;
        shotTime = timeBetweenShots + 1;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, bitMask))
        {
            point = hit.point;
            point.y = transform.position.y;
        }

        tr.LookAt(point);

        if (Input.GetMouseButtonDown(0) && shotTime> timeBetweenShots) {
            Instantiate(bullet, puntoDisparo.position, puntoDisparo.rotation);
            shotTime = 0;
        }

        shotTime += Time.deltaTime;
    }

	private void OnDrawGizmos()
	{
        //Gizmos.DrawSphere(point, 0.2f);
    }

	private void OnApplicationFocus(bool focus)
	{
        Cursor.SetCursor(mouseSkin, new Vector2(16, 16), CursorMode.ForceSoftware);
    }

}
