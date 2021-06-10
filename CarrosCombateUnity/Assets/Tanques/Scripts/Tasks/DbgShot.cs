using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace BehaviorDesigner.Runtime.Tasks.IAV.CarrosCombate
{
    public class DbgShot : Action
    {
        [Tooltip("Hacia donde disparamos")] 
        public SharedTransform enemyTransform;


        public override TaskStatus OnUpdate()
        {
            Debug.DrawLine(transform.position,enemyTransform.Value.position,Color.magenta,2f);
            return TaskStatus.Success;
        }
    }
}