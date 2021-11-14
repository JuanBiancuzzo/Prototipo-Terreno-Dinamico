using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtyCube : IRenderizable
{
    Extremo m_extremos;
    List<Vector3Int> m_puntos;

    MeshData m_meshData;
    bool m_necesitaActualizarse;

    public DirtyCube()
    {
        m_extremos.m_maximo = Vector3Int.zero;
        m_extremos.m_minimo = Vector3Int.zero;

        m_puntos = new List<Vector3Int>();
        m_meshData = new MeshData();
    }

    public void Insertar(Vector3Int punto)
    {
        bool vacio = Vacio();

        NecesitaRenderizarse();
        m_puntos.Add(punto);

        if (vacio)
            SetInicial(punto);
        else
            ExpandirBordes(punto);
    }

    public void Eliminar(Vector3Int punto)
    {
        NecesitaRenderizarse();
        m_puntos.Remove(punto);

        bool recalcularBordes = false;
        for (int i = 0; i < 3 && !recalcularBordes; i++)
            recalcularBordes = punto[i] == m_extremos.m_maximo[i] || punto[i] == m_extremos.m_minimo[i];

        if (recalcularBordes)
            RecalcularBordes();
    }

    public bool PuntoDentroDelCubo(Vector3Int punto)
    {
        bool dentro = true;
        for (int i = 0; i < 3 && dentro; i++)
            dentro = (punto[i] <= m_extremos.m_maximo[i] && punto[i] >= m_extremos.m_minimo[i]);
        return dentro;
    }

    public float DistanciaAlPunto(Vector3Int punto)
    {
        Vector3 posicionRelativa = Vector3.zero;

        for (int i = 0; i < 3; i++)
        {
            float distanciaDelMaximo = Mathf.Abs(punto[i] - m_extremos.m_maximo[i]);
            float distanciaDelMinimo = Mathf.Abs(punto[i] - m_extremos.m_minimo[i]);

            posicionRelativa[i] = Mathf.Min(distanciaDelMinimo, distanciaDelMinimo);
        }

        return posicionRelativa.magnitude;
    }

    
    public void Renderizar(IRender render, ISacarDatos contenedor = null)
    {
        if (NecesitaActualizarse())
        {
            m_meshData.Clear();
            render.GenerarMesh(m_extremos, contenedor, ref m_meshData);
        }       

        EmpezarARenderizar();
    }

    private void NecesitaRenderizarse()
    {
        m_necesitaActualizarse = true;
    }

    private void EmpezarARenderizar()
    {
        m_necesitaActualizarse = false;
    }

    int cantidadSinNuevoRender = 0;
    static int cantidadMinima = 2;
    private bool NecesitaActualizarse()
    {
        if (!m_necesitaActualizarse)
        {
            if (cantidadSinNuevoRender >= cantidadMinima)
                return false;
            else
                cantidadSinNuevoRender++;
        }
        else
        {
            cantidadSinNuevoRender = 0;
        }

        return true;
    }

    public void RecopilarMesh(ref MeshData meshDataIncial)
    {
        meshDataIncial.Sumar(m_meshData);
    }

    public Extremo GetExtremos()
    {
        return m_extremos;
    }

    public bool Vacio()
    {
        return m_puntos.Count == 0;
    }

    public bool ContienePunto(Vector3Int punto)
    {
        return m_puntos.Contains(punto);
    }

    public void Merge(DirtyCube otro)
    {
        foreach (Vector3Int punto in otro.m_puntos)
            Insertar(punto);
        m_extremos = m_extremos.Union(otro.m_extremos);
    }

    public bool Intersecta(DirtyCube otro)
    {
        return m_extremos.Interseccion(otro.m_extremos).Valido();
    }

    private void SetInicial(Vector3Int punto)
    {
        m_extremos.m_maximo.Set(punto.x, punto.y, punto.z);
        m_extremos.m_minimo.Set(punto.x, punto.y, punto.z);
    }

    private void ExpandirBordes(Vector3Int punto)
    {
        for (int i = 0; i < 3; i++)
            if (punto[i] > m_extremos.m_maximo[i])
                m_extremos.m_maximo[i] = punto[i];
            else if (punto[i] < m_extremos.m_minimo[i])
                m_extremos.m_minimo[i] = punto[i];
    }

    private void RecalcularBordes()
    {
        if (Vacio())
            return;

        SetInicial(m_puntos[0]);
        for (int i = 1; i < m_puntos.Count; i++)
            ExpandirBordes(m_puntos[i]);
    }
}
