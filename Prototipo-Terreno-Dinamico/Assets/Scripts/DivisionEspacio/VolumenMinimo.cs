using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumenMinimo
{
    List<DirtyCube> m_dirtyCubes;
    float m_distanciaMaxima; // que tan separada pueden estar

    public VolumenMinimo(float distanciaMaxima)
    {
        m_dirtyCubes = new List<DirtyCube>();
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
            m_dirtyCubes.Remove(cuboParaEliminar);
    }

    private void Optimizar()
    {
        for (int i = 0; i < m_dirtyCubes.Count; i++)
            for (int j = i + 1; j < m_dirtyCubes.Count; j++)
            {
                DirtyCube primero = m_dirtyCubes[i];
                DirtyCube segundo = m_dirtyCubes[j];

                if (primero.Intersecta(segundo))
                {
                    primero.Merge(segundo);
                    m_dirtyCubes.Remove(segundo);
                    return;
                }
            }
    }

    public void EmpezarARenderizar()
    {
        foreach (DirtyCube cube in m_dirtyCubes)
            cube.EmpezarARenderizar();
    }

    public bool NecesitaActualizarse()
    {
        bool seNecesitaActualizar = false;
        for (int i = 0; i < m_dirtyCubes.Count && !seNecesitaActualizar; i++)
            seNecesitaActualizar |= m_dirtyCubes[i].NecesitaActualizarse();
        return seNecesitaActualizar;
    }

    public bool Vacio()
    {
        return m_dirtyCubes.Count == 0;
    }

    public List<Extremo> GetExtremos()
    {
        List<Extremo> extremos = new List<Extremo>();
        foreach (DirtyCube cube in m_dirtyCubes)
            extremos.Add(cube.GetExtremos());
        return extremos;
    }

    private DirtyCube CuboContienePunto(Vector3Int punto)
    {
        DirtyCube cuboFinal = null;
        foreach (DirtyCube cubo in m_dirtyCubes)
            if (cubo.ContienePunto(punto))
                cuboFinal = cubo;
        return cuboFinal;
    }

    private DirtyCube CuboMasCercano(Vector3Int punto)
    {
        if (Vacio())
            return CrearCubo();

        DirtyCube masCercano = m_dirtyCubes[0];
        if (masCercano.PuntoDentroDelCubo(punto))
            return masCercano;

        float distanciaMinima = masCercano.DistanciaAlPunto(punto);
        foreach (DirtyCube cubo in m_dirtyCubes)
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
        m_dirtyCubes.Add(cubo);
        return cubo;
    }
}
