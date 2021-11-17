using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Extremo
{
    public Vector3Int m_minimo, m_maximo;

    public Extremo(Vector3Int minimo, Vector3Int maximo)
    {
        m_minimo = minimo;
        m_maximo = maximo;
    }

    public Extremo Interseccion(Extremo otro)
    {
        Vector3Int nuevoMinimo = Vector3Int.zero, nuevoMaximo = Vector3Int.zero;

        for (int i = 0; i < 3; i++)
        {
            nuevoMinimo[i] = (m_minimo[i] > otro.m_minimo[i]) ? m_minimo[i] : otro.m_minimo[i]; // el mas grande
            nuevoMaximo[i] = (m_maximo[i] < otro.m_maximo[i]) ? m_maximo[i] : otro.m_maximo[i]; // el mas chico
        }

        return new Extremo(nuevoMinimo, nuevoMaximo);
    }

    public Extremo Union(Extremo otro)
    {
        Vector3Int nuevoMinimo = Vector3Int.zero, nuevoMaximo = Vector3Int.zero;

        for (int i = 0; i < 3; i++)
        {
            nuevoMinimo[i] = (m_minimo[i] < otro.m_minimo[i]) ? m_minimo[i] : otro.m_minimo[i]; // el mas chico
            nuevoMaximo[i] = (m_maximo[i] > otro.m_maximo[i]) ? m_maximo[i] : otro.m_maximo[i]; // el mas grande
        }

        return new Extremo(nuevoMinimo, nuevoMaximo);
    }

    public bool Valido()
    {
        return m_minimo.Equals(Vector3Int.Min(m_maximo, m_minimo));
    }
}