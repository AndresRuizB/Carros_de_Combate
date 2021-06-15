using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using UnityEngine;

public class isTankTarget : MonoBehaviour
{
    private SharedTransform enemyTr;
    // Start is called before the first frame update
    void Start()
    {
        enemyTr = (SharedTransform)GlobalVariables.Instance.GetVariable("enemyTransform");
        enemyTr.Value = transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
