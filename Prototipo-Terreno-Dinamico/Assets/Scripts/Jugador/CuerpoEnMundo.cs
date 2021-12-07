using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuerpoEnMundo : EntidadMagica
{
    public int m_extensionCarga;
    public FallingSand m_mundo = null;

    Vector3Int m_posicion => Vector3Int.FloorToInt(transform.position);
    Extremo m_extremo;

    void Start()
    {
        m_extremo = new Extremo(Vector3Int.zero, Vector3Int.zero);
    }

    void FixedUpdate()
    {
        if (m_mundo == null)
            return;

        PedirColision();
    }

    void PedirColision()
    {
        CalcularExtremo();
        m_mundo.GenerarMeshColision(m_extremo, m_constitucion);
    }

    void CalcularExtremo()
    {
        Vector3Int posicion = Vector3Int.FloorToInt(m_mundo.PosicionEnMundo(m_posicion));
        m_extremo.m_minimo = posicion - Vector3Int.one * (m_extensionCarga - 1);
        m_extremo.m_maximo = posicion + Vector3Int.one * (m_extensionCarga - 1);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, 0.1f);

        Vector3Int posicion = Vector3Int.FloorToInt(m_mundo.PosicionEnMundo(m_posicion));
        Vector3Int extension = ((posicion + Vector3Int.one * m_extensionCarga) - (posicion - Vector3Int.one * m_extensionCarga));
        Gizmos.DrawWireCube(Vector3Int.FloorToInt(m_mundo.PosicionEnMundo(transform.position)), extension);
    }
}
