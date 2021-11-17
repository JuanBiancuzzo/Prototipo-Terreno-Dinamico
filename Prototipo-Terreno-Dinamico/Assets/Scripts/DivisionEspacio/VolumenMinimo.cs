using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumenMinimo : IRenderizable
{
    List<DirtyCube> m_volumenMinimo;
    float m_distanciaMaxima; // que tan separada pueden estar

    public VolumenMinimo(float distanciaMaxima)
    {
        m_volumenMinimo = new List<DirtyCube>();
        m_distanciaMaxima = distanciaMaxima;
    }

    public void Insertar(Vector3Int punto)
    {
        DirtyCube cuboParaInsertar = CuboMasCercano(punto);
        cuboParaInsertar.Insertar(punto);
        Optimizar();
    }

    public void Eliminar(Vector3Int punto)
    {
        DirtyCube cuboParaEliminar = CuboContienePunto(punto);
        if (cuboParaEliminar == null)
            return;

        cuboParaEliminar.Eliminar(punto);
        if (cuboParaEliminar.Vacio())
            m_volumenMinimo.Remove(cuboParaEliminar);
    }

    private void Optimizar()
    {
        for (int i = 0; i < m_volumenMinimo.Count; i++)
            for (int j = i + 1; j < m_volumenMinimo.Count; j++)
            {
                DirtyCube primero = m_volumenMinimo[i];
                DirtyCube segundo = m_volumenMinimo[j];

                if (primero.Intersecta(segundo))
                {
                    primero.Merge(segundo);
                    m_volumenMinimo.Remove(segundo);
                    return;
                }
            }
    }

    public void Renderizar(IRender render, ISacarDatos contenedor = null)
    {
        foreach (DirtyCube cube in m_volumenMinimo)
            cube.Renderizar(render, contenedor);
    }

    public void RecopilarMesh(ref MeshData meshDataIncial)
    {
        foreach (DirtyCube cube in m_volumenMinimo)
            cube.RecopilarMesh(ref meshDataIncial);
    }

    public bool Vacio()
    {
        return m_volumenMinimo.Count == 0;
    }

    public List<Extremo> GetExtremos()
    {
        List<Extremo> extremos = new List<Extremo>();
        foreach (DirtyCube cube in m_volumenMinimo)
            extremos.Add(cube.GetExtremos());
        return extremos;
    }

    private DirtyCube CuboContienePunto(Vector3Int punto)
    {
        DirtyCube cuboFinal = null;
        foreach (DirtyCube cubo in m_volumenMinimo)
            if (cubo.ContienePunto(punto))
                cuboFinal = cubo;
        return cuboFinal;
    }

    private DirtyCube CuboMasCercano(Vector3Int punto)
    {
        if (Vacio())
            return CrearCubo();

        DirtyCube masCercano = m_volumenMinimo[0];
        if (masCercano.PuntoDentroDelCubo(punto))
            return masCercano;

        float distanciaMinima = masCercano.DistanciaAlPunto(punto);
        foreach (DirtyCube cubo in m_volumenMinimo)
        {
            if (cubo.PuntoDentroDelCubo(punto))
            {
                distanciaMinima = 0;
                masCercano = cubo;
                break;
            }

            float distancia = cubo.DistanciaAlPunto(punto);
            if (distancia < distanciaMinima)
            {
                distanciaMinima = distancia;
                masCercano = cubo;
            }
        }

        return (distanciaMinima < m_distanciaMaxima) ? masCercano : CrearCubo();
    }    

    private DirtyCube CrearCubo()
    {
        DirtyCube cubo = new DirtyCube();
        m_volumenMinimo.Add(cubo);
        return cubo;
    }
}
