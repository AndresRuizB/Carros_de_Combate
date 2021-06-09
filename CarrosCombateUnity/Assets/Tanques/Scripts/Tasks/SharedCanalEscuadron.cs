using System;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;

[System.Serializable]
public class CanalEscuadron
{
	[SerializeField] private int size;
	[SerializeField] public List<Transform> transformsEquipo;
	[SerializeField] public List<Vector3> objetivosEquipo;
	[SerializeField] public List<float> tiempoParaAtacar;
	[SerializeField] public List<bool> atacando;
}

[System.Serializable]
public class SharedCanalEscuadron : SharedVariable<CanalEscuadron>
{
	public override string ToString() { return mValue == null ? "null" : mValue.ToString(); }
	public static implicit operator SharedCanalEscuadron(CanalEscuadron value) { return new SharedCanalEscuadron { mValue = value }; }
}