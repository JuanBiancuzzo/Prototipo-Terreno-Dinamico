using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshData
{
    public List<Vector3> m_vertices;
    public List<Color> m_colores;
    public List<int> m_triangulos;
    public List<Vector3> m_normales;
    public List<Vector2> m_uv0;
    public List<Vector2> m_uv1;
    public Dictionary<Vector3, int> m_repetirVertices;

    public MeshData()
    {
        m_vertices = new List<Vector3>();
        m_colores = new List<Color>();
        m_triangulos = new List<int>();
        m_normales = new List<Vector3>();
        m_uv0 = new List<Vector2>();
        m_uv1 = new List<Vector2>();
        m_repetirVertices = new Dictionary<Vector3, int>();
    }

    public void Clear()
    {
        m_vertices.Clear();
        m_colores.Clear();
        m_triangulos.Clear();
        m_normales.Clear();
        m_uv0.Clear();
        m_uv1.Clear();
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
                m_triangulos.Add(index);
                posicionDesfasada--;
            }
            else
            {
                m_vertices.Add(vertice);
                m_triangulos.Add(otro.m_triangulos[i] + posicionDesfasada);

                if (otro.m_normales.Count > 0)
                    m_normales.Add(otro.m_normales[i]);

                if (otro.m_colores.Count > 0)
                    m_colores.Add(otro.m_colores[i]);
            }
        }

        for (int i = otro.m_vertices.Count; i < otro.m_triangulos.Count; i++)
            m_triangulos.Add(otro.m_triangulos[i] + posicionDesfasada);
    }

    public void RellenarMesh(Mesh mesh, int submesh = 0)
    {
        mesh.Clear();
        mesh.SetVertices(m_vertices);
        mesh.SetTriangles(m_triangulos, submesh);
        if (m_colores.Count > 0)
            mesh.SetColors(m_colores);

        if (m_uv0.Count > 0)
            mesh.SetUVs(0, m_uv0);

        if (m_uv1.Count > 0)
            mesh.SetUVs(1, m_uv1);

        if (m_normales.Count > 0)
            mesh.SetNormals(m_normales);
        else
            mesh.RecalculateNormals();
    }
}
