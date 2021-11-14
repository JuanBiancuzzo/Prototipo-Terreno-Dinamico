using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MaterialSel
{
	Arena = 0,
	Agua,
	Grava,
	Humo,
	Muro
}

public class GenerarParticulas : MonoBehaviour
{
    public FallingSand simulacion;

	public List<MaterialParticula> m_materiales;
	public MaterialSel m_materialSeleccionado;
	public Vector3Int m_posicion;

	public int anchoPlancha = 2;
	public bool tirar = true;
	public bool continuo = false;

	int contador = 0;
	void FixedUpdate()
	{
		if (contador == 0) { }
			//AgregarElemento(m_posicion);
		contador = (contador + 1) % 5;

		if (tirar)
        {
			TirarPlancha();
			if (!continuo)
				tirar = false;
        }

	}

	void TirarPlancha()
    {
		for (int x = -anchoPlancha; x <= anchoPlancha; x++)
			for (int z = -anchoPlancha; z <= anchoPlancha; z++)
				// if (x % 2 == 0 || z % 2 == 0)
					AgregarElemento(new Vector3Int(x + m_posicion.x, m_posicion.y, z + m_posicion.z));
	}

	bool AgregarElemento(Vector3Int posicion)
	{
		if (m_materiales.Count == 0)
			return false;

		MaterialParticula materialReferencia = m_materiales[(int)m_materialSeleccionado];
		if (materialReferencia == null)
			return false;

		MaterialParticula material = materialReferencia.Clone(posicion);
		// simulacion.AplicarGravedad(material);
		simulacion.Insertar(material);
		
		return true;
	}

	void OnDrawGizmos()
    {
		Gizmos.DrawSphere(transform.position + m_posicion + Vector3.down, 1f);
	}
}
