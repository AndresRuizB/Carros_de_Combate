using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using BehaviorDesigner.Runtime;
using UnityEngine;
using YamlDotNet.Core.Tokens;

public class DbgPideCobertura : MonoBehaviour
{
    [SerializeField] private KeyCode helpKey;
    private SharedCanalEscuadron canal;
    [SerializeField] private int id;
    void Start()
    {
        canal = (SharedCanalEscuadron)GlobalVariables.Instance.GetVariable("canalEscuadron");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(helpKey))
        {
            canal.Value.pideCobertura[id] = true;
            Debug.Log($"Socorro {id}");
        }

        if (Input.GetKeyUp(helpKey))
        {
            canal.Value.pideCobertura[id] = false;
            Debug.Log($"Tamos bien {id}");
        }

    }
}
