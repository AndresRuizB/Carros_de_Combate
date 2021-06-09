using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace BehaviorDesigner.Runtime.Tasks.IAV.CarrosCombate
{
	[TaskCategory("Tanks")]
	public class GetTiempoAtaque : Action
	{
		[Tooltip("Canal de voz del tanque")] public SharedCanalEscuadron canal;
		[Tooltip("id del tanque")] public SharedInt id;
		[Tooltip("timer del ataque")] public SharedFloat tiempo;

		public override void OnStart()
		{

		}

		public override TaskStatus OnUpdate()
		{
			tiempo.Value = canal.Value.atacaEn[id.Value];
			if (tiempo.Value < 0)
				return TaskStatus.Running;
			return TaskStatus.Success;
		}
	}
}