using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshData
{
    public List<Vector3> m_vertices;
    public List<Color> m_colores;
    public List<int> m_triangulos;
    public List<Vector3> m_normales;
    public Dictionary<Vector3, int> m_repetirVertices;

    public MeshData()
    {
        m_vertices = new List<Vector3>();
        m_colores = new List<Color>();
        m_triangulos = new List<int>();
        m_normales = new List<Vector3>();
        m_repetirVertices = new Dictionary<Vector3, int>();
    }

    public void Clear()
    {
        m_vertices.Clear();
        m_colores.Clear();
        m_triangulos.Clear();
        m_normales.Clear();
        m_repetirVertices.Clear();
    }

    public void Sumar(MeshData otro)
    {
        int posicionDesfasada = m_vertices.Count;

        for (int i = 0; i < otro.m_vertices.Count; i++)
        {
            Vector3 vertice = otro.m_vertices[i];

            if (m_repetirVertices.ContainsKey(vertice))
            {
                int index = m_repetirVertices[vertice];

                /* if (m_normales[index] != otro.m_normales[i])
                    m_normales[index] = (m_normales[index] + otro.m_normales[i]) / 2f;

                if (m_colores[index] != otro.m_colores[i])
                    m_colores[index] = (m_colores[index] + otro.m_colores[i]) / 2f; */

                m_triangulos.Add(index);
                posicionDesfasada--;
            }
            else
            {
                m_vertices.Add(vertice);
                m_normales.Add(otro.m_normales[i]);
                m_triangulos.Add(otro.m_triangulos[i] + posicionDesfasada);
                m_colores.Add(otro.m_colores[i]);
            }
        }

        for (int i = otro.m_vertices.Count; i < otro.m_triangulos.Count; i++)
            m_triangulos.Add(otro.m_triangulos[i] + posicionDesfasada);
    }
}
