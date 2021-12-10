using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class CrearDibujo : MonoBehaviour
{
    [SerializeField] List<Vector3> m_puntos;
    Mesh m_mesh;

    private void Awake()
    {
        m_mesh = new Mesh();
        GetComponent<MeshFilter>().sharedMesh = m_mesh;
        CrearMeshDePuntos();
    }

    public void CrearMeshDePuntos()
    {
        if (m_puntos.Count <= 1)
            return;

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangulos = new List<int>();

        CrearMesh(ref vertices, ref triangulos);

        m_mesh.Clear();
        m_mesh.SetVertices(vertices);
        m_mesh.SetTriangles(triangulos, 0);
    }

    private void CrearMesh(ref List<Vector3> vertices, ref List<int> triangulos)
    {

        for (int i = 0; i < m_puntos.Count - 1; i++)
        {
            Vector3 punto = m_puntos[i];
            Vector3 siguientePunto = m_puntos[i + 1];




        }
    }

    private void OnDrawGizmos()
    {
        foreach (Vector3 punto in m_puntos)
            Gizmos.DrawSphere(punto, 0.2f);
    }
}
