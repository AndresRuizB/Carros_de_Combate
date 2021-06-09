using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public static class CombatUtils
    {
        public static Vector3 centroTransforms(List<Transform> list,Transform excluded=null)
        {
            Vector3 centre = Vector3.zero;
            int i = 0;
            foreach (Transform t in list)
            {
                if (t!=excluded)
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

        public static Vector3 lineaDeVisionCercana(Transform from, Transform to, float circleSample, float max = 10f, float step = 1f)
        {
            Vector3 closest = Vector3.positiveInfinity;
            Ray ray = new Ray(from.position, Vector3.forward);
            RaycastHit hit;
            float stepAngle = 360f / circleSample;
            float i = 0;
            while (i < max)
            {
                for (int j = 0; j < circleSample; j++)
                {
                Debug.Log("fuegos");
                ray.origin = from.position + (Quaternion.AngleAxis(stepAngle * i, Vector3.up) * Vector3.forward*i);
                ray.direction = to.position - ray.origin;
                Debug.DrawLine(ray.origin,to.position,Color.magenta,5f);    
                if (Physics.Raycast(ray, out hit) && hayLineaVision(hit.point, to))
                    {
                        closest = ray.origin;
                        return closest;
                    }
                }
                i += step;
            }
            return closest;

        }
    }
