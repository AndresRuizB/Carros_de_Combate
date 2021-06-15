using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;

public class isSquadMember : MonoBehaviour
{
    [SerializeField] private int id;
    private SharedCanalEscuadron canal;
    void Start()
    {
        canal = (SharedCanalEscuadron)GlobalVariables.Instance.GetVariable("canalEscuadron");
        canal.Value.transformsEquipo[id] = transform;
    }
    
}
