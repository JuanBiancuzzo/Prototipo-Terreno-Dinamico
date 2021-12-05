using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class ElementoSeleccionado : MonoBehaviour
{
    Mesh m_mesh;
    MeshFilter m_meshFilter;

    private void Awake()
    {
        m_mesh = new Mesh();
        GetComponent<MeshFilter>().sharedMesh = m_mesh;
        
    }

    public void LimplearMesh()
    {
        m_mesh.Clear();
    }

    public void CargarNuevaMesh(MeshData meshData)
    {
        meshData.RellenarMesh(m_mesh);
    }
}
