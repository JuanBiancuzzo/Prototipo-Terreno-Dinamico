using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class ElementoSeleccionado : MonoBehaviour
{
    Mesh m_mesh;

    private void Awake()
    {
        m_mesh = new Mesh();
        GetComponent<MeshFilter>().sharedMesh = m_mesh;
    }

    private void FixedUpdate()
    {
        m_mesh.Clear();
    }

    public void CargarNuevaMesh(MeshData meshData)
    {
        m_mesh.Clear();
        meshData.RellenarMesh(m_mesh);
    }
}
