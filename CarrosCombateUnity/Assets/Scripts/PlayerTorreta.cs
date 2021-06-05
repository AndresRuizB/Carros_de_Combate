using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTorreta : MonoBehaviour
{
    Vector3 point;
    Transform tr;

    int bitMask;

    public int layerMask = 3;
    public Texture2D mouseSkin;

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
        bitMask = 1 << layerMask;
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
