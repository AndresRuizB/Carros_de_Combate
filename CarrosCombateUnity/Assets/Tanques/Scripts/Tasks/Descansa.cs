using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace BehaviorDesigner.Runtime.Tasks.IAV.CarrosCombate
{
	[TaskCategory("Tanks")]
	public class Descansa : Action
	{
		[Tooltip("Canal de voz del tanque")] public SharedCanalEscuadron canal;
		[Tooltip("id del tanque")] public SharedInt id;

		public override void OnStart()
		{

		}

		public override TaskStatus OnUpdate()
		{
			canal.Value.descansa(id.Value);
			return TaskStatus.Success;
		}
	}
}