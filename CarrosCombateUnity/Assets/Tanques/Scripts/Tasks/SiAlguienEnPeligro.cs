namespace BehaviorDesigner.Runtime.Tasks.IAV.CarrosCombate
{
    namespace BehaviorDesigner.Runtime.Tasks.IAV.CarrosCombate
    {
        [TaskCategory("Tanks")]
        public class AlguienEnPeligro : Conditional
        {
            [Tooltip("Canal de voz del tanque")] public SharedCanalEscuadron canal;
            [Tooltip("id del tanque")] public SharedInt id;


            // Seek the destination. Return success once the agent has reached the destination.
            // Return running if the agent hasn't reached the destination yet
            public override TaskStatus OnUpdate()
            {
                if (canal.Value.alguienEnPeligro(id.Value))
                    return TaskStatus.Success;
                else
                    return TaskStatus.Failure;
            }

            public override void OnDrawGizmos()
            {
                //Gizmos.color = Color.yellow;

                //Gizmos.DrawWireSphere(destination,1f);
            }
        }
    }
}