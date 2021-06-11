using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimation;
using UnityEngine;
using UnityEngine.AI;

public static class CombatUtils
{
    public static Vector3 centroTransforms(List<Transform> list, Transform excluded = null)
    {
        Vector3 centre = Vector3.zero;
        int i = 0;
        foreach (Transform t in list)
        {
            if (t != excluded)
            {
                centre += t.position;
                ++i;
            }
        }

        return centre / i;
    }

    public static bool hayLineaVision(Vector3 a, Transform b)
    {
        Vector3 dir = b.position - a;
        Ray ray = new Ray(a, dir);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            return hit.transform == b;
        }

        return false;
    }

    public static Vector3 puntoCercanoConVision(Transform desde, Transform hasta, float maximo,float paso =1f)
    {
        Vector3 v = hasta.position - desde.position;//Hayamos la recta que une ambos puntos
        v = v / 2; //Elegimos el punto medio
        v = Quaternion.AngleAxis(-45, Vector3.up) * v; //Elegimos un vector perpendicular
        v = v.normalized;

        float f = 0f;
        Vector3 cercano = Vector3.positiveInfinity;
        Ray ray = new Ray();
        RaycastHit hit;
        while (f < maximo && cercano == Vector3.positiveInfinity)
        {
            ray.origin = v.normalized * f;
            ray.direction = hasta.position - ray.origin;
            if (Physics.Raycast(ray, out hit) && hayLineaVision(hit.point, hasta))
            {
                Debug.DrawLine(desde.position,hasta.position,Color.green);
                cercano = ray.origin;
            }
            f += paso;
        }

        return cercano;
    }

    public static Vector3 lineaDeVisionCercana(Transform from, Transform to, float circleSample, float max = 10f,
        float step = 1f)
    {
        Vector3 visionMasCercana = Vector3.positiveInfinity;
        float distancia = float.MaxValue;
        Ray ray = new Ray();
        RaycastHit hit;
        float stepAngle = 360f / circleSample;
        float radio = 0;
        NavMeshPath path = new NavMeshPath();
        bool found = false;

        while (radio < max && !found)
        {
            for (int paso= 0; paso < circleSample; paso++)
            {
                ray.origin = from.position + (Quaternion.AngleAxis(stepAngle * paso, Vector3.up) * Vector3.forward * radio);
                ray.direction = to.position - ray.origin;
                //Debug.DrawLine(ray.origin,to.position,Color.magenta,5f);    
                //GameObject.Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cube), ray.origin, Quaternion.identity);

                if (Physics.Raycast(ray, out hit))
                {
                    if (hayLineaVision(hit.point, to))
                    {
                        Debug.DrawLine(ray.origin,to.position,Color.blue,2);
                        GetPath(path, from.position, ray.origin, NavMesh.AllAreas);
                        float distAux = GetPathLength(path);
                        if (distAux < distancia)
                        {
                            found = true;
                            visionMasCercana = ray.origin;
                            distancia = distAux;
                        }
                    }
                }
            }

            radio += step;
        }
        
        //GameObject.Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cylinder), visionMasCercana, Quaternion.identity);
        return visionMasCercana;
    }

    public static bool GetPath( NavMeshPath path, Vector3 fromPos, Vector3 toPos, int passableMask )
    {
        path.ClearCorners();
       
        if ( NavMesh.CalculatePath( fromPos, toPos, passableMask, path ) == false )
            return false;
       
        return true;
    }
       
    public static float GetPathLength( NavMeshPath path )
    {
        float lng = 0.0f;
       
        if (( path.status != NavMeshPathStatus.PathInvalid ))
        {
            for ( int i = 1; i < path.corners.Length; ++i )
            {
                lng += Vector3.Distance( path.corners[i-1], path.corners[i] );
            }
        }
       
        return lng;
    }
}