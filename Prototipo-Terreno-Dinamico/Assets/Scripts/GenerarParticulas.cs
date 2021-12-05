using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum MaterialSel
{
	Aire = 0,
	Arena,
	Concreto,
	Hielo,
	Agua,
	Vapor,
	Lava
}

public class GenerarParticulas : MonoBehaviour
{
    public FallingSand simulacion;

	public MaterialSel m_materialSeleccionado;

	public int anchoPlancha = 0;
	public bool tirar = true;
	public bool continuo = false;

    void FixedUpdate()
	{
		//simulacion.SeleccionarElemento(new Vector3Int(0, -3, 0));
		//simulacion.SeleccionarElemento(Vector3Int.FloorToInt(transform.position));

		
		if (!tirar)
			return;
		 
		TirarPlancha(anchoPlancha, Vector3Int.FloorToInt(transform.position), (int)m_materialSeleccionado);
		if (!continuo)
			tirar = false; 
	}

	void TirarPlancha(int ancho, Vector3Int posicion, int index)
    {
		for (int x = -ancho; x <= ancho; x++)
			for (int z = -ancho; z <= ancho; z++)
				AgregarElemento(new Vector3Int(x + posicion.x, posicion.y, z + posicion.z), index);
	}

	bool AgregarElemento(Vector3Int posicion, int index)
	{
		Elemento elemento = GetElemento(posicion, index);
		return simulacion.Insertar(elemento);
	}

	private Elemento GetElemento(Vector3Int posicion, int index)
    {
		switch(index)
        {
			case 0: return new Aire(posicion, simulacion.Mapa);
			case 1: return new Arena(posicion, simulacion.Mapa);
			case 2: return new Concreto(posicion, simulacion.Mapa);
			case 3: return new Hielo(posicion, simulacion.Mapa);
			case 4: return new Agua(posicion, simulacion.Mapa);
			case 5: return new Vapor(posicion, simulacion.Mapa);
			case 6: return new Lava(posicion, simulacion.Mapa);
        }

		return null;
    }

	void OnDrawGizmos()
    {
		Gizmos.DrawSphere(transform.position, 0.25f);
	}
}
