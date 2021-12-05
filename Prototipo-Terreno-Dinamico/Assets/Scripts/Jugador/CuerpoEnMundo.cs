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
        m_mundo.GenerarMeshColision(m_extremo, m_consitucion);
    }

    void CalcularExtremo()
    {
        Vector3Int posicion = Vector3Int.FloorToInt(m_mundo.PosicionEnMundo(transform.position));
        m_extremo.m_minimo = posicion - Vector3Int.one * m_extensionCarga;
        m_extremo.m_maximo = posicion + Vector3Int.one * m_extensionCarga;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, 0.1f);

        Vector3Int extension = (m_extremo.m_maximo - m_extremo.m_minimo);
        Gizmos.DrawWireCube(Vector3Int.FloorToInt(m_mundo.PosicionEnMundo(transform.position)), extension);
    }
}
