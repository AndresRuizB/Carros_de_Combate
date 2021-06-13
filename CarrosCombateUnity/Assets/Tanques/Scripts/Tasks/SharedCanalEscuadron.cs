using System;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;

[System.Serializable]
public class CanalEscuadron
{
	[SerializeField] public int minParaAtacar;
	[SerializeField] private int size;
	[SerializeField] public List<Transform> transformsEquipo;
	[SerializeField] public List<Vector3> objetivosEquipo;
	[SerializeField] public List<float> tiempoParaAtacar;
	[SerializeField] public List<float> atacaEn;
	public List<bool> pideCobertura;

	public bool alguienEnPeligro(int id)
	{
		int i = 0;
		while (i < size)
		{
			if (i != id && pideCobertura[i])
				return true;
			i++;
		}

		return false;
	}
	public int getListosParaAtacar(out float ataqueEn)
	{
		var i = 0;
		ataqueEn = float.MinValue;
		foreach (var t in tiempoParaAtacar)
			if (t >= 0)
			{
				i++;
				ataqueEn = Math.Max(ataqueEn, t);
			}

		return i;

	}
	
	public void listoParaAtacar(int id, float cuando)
	{
		tiempoParaAtacar[id] = cuando;
		float tiempoAtaque;
		
		//Cuando suficientes tanques esten listos, se sincronizan
		if (getListosParaAtacar(out tiempoAtaque) >= minParaAtacar)
		{
			Debug.Log($"Atacamos en {tiempoAtaque}");
			for (var i = 0; i < size; i++)
			{
				atacaEn[i] = tiempoAtaque - tiempoParaAtacar[i];
				Debug.Log($"{i} tarda {tiempoParaAtacar[i]} asi que espera {atacaEn[i]}");
			}
		}
		else
		{
			Debug.Log($"{id} a la espera con {tiempoParaAtacar[id]}");
		}
	}

	public void descansa(int id)
	{
		atacaEn[id] = -1;
		tiempoParaAtacar[id] = -1;
	}
}

[System.Serializable]
public class SharedCanalEscuadron : SharedVariable<CanalEscuadron>
{
	public override string ToString() { return mValue == null ? "null" : mValue.ToString(); }
	public static implicit operator SharedCanalEscuadron(CanalEscuadron value) { return new SharedCanalEscuadron { mValue = value }; }
}