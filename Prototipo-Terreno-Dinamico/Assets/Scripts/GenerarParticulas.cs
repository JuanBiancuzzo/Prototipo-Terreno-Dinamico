using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum MaterialSel
{
	Aire = 0,
	Arena,
	Agua,
	Grava,
	Humo,
	Muro
}

public class GenerarParticulas : MonoBehaviour
{
    public FallingSand simulacion;

	public MaterialSel m_materialSeleccionado;

	public int anchoPlancha = 2;
	public bool tirar = true;
	public bool continuo = false;
	public bool ponerPiso = true;

    private void Start()
    {
		if (ponerPiso)
			TirarPlancha(50, new Vector3Int(0, 1, 0), (int)MaterialSel.Arena);
	}

    void FixedUpdate()
	{
		if (!tirar)
			return;

		TirarPlancha(anchoPlancha, Vector3Int.FloorToInt(transform.position + Vector3.up), (int)m_materialSeleccionado);
		if (!continuo)
			tirar = false;
	}

	void TirarPlancha(int ancho, Vector3Int posicion, int index)
    {
		for (int x = -ancho; x <= ancho; x++)
			for (int z = -ancho; z <= ancho; z++)
					AgregarElemento(new Vector3Int(x + posicion.x, posicion.y, z + posicion.z), index);
	}

	void AgregarElemento(Vector3Int posicion, int index)
	{
		simulacion.Insertar(GetElemento(posicion, index));
	}

	private Elemento GetElemento(Vector3Int posicion, int index)
    {
		switch(index)
        {
			case 0: return new Aire(posicion);
			case 1: return new Arena(posicion);
			default: return new Aire(posicion);
        }
    }

	void OnDrawGizmos()
    {
		Gizmos.DrawSphere(transform.position, 0.5f);
	}
}
