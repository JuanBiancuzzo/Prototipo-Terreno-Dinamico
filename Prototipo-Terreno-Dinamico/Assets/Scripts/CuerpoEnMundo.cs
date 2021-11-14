using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuerpoEnMundo : MonoBehaviour
{
    public int m_extensionCarga;
    public FallingSand m_mundo = null;

    Extremo m_extremo;

    void Start()
    {
        m_extremo = new Extremo(Vector3Int.zero, Vector3Int.zero);
    }

    void FixedUpdate()
    {
        if (m_mundo == null)
            return;

        CalcularExtremo();

        m_mundo.GenerarMeshColision(m_extremo);
    }

    void CalcularExtremo()
    {
        Vector3Int posicion = Vector3Int.FloorToInt(m_mundo.PosicionEnMundo(transform.position));
        m_extremo.m_minimo = posicion - Vector3Int.one * m_extensionCarga;
        m_extremo.m_maximo = posicion + Vector3Int.one * m_extensionCarga;
    }
}
