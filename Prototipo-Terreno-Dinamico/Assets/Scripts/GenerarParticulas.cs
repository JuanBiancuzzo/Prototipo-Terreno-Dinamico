using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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

	public int anchoPlancha = 2;
	public bool tirar = true;
	public bool continuo = false;
	public bool ponerPiso = true;

    private void Start()
    {
		if (ponerPiso)
			TirarPlancha(20, new Vector3Int(0, 1, 0), (int)MaterialSel.Muro);
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
		if (m_materiales.Count == 0)
			return;

		MaterialParticula materialReferencia = m_materiales[index];
		if (materialReferencia == null)
			return;

		MaterialParticula material = materialReferencia.Clone(posicion);
		simulacion.Insertar(material);
	}

	void OnDrawGizmos()
    {
		Gizmos.DrawSphere(transform.position, 0.5f);
	}
}
