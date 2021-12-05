using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Mundo))]
public class ColisionesMundo : MonoBehaviour, IColisiones
{
    protected Mundo mundo;
    Mesh m_mesh;
    MeshCollider m_meshCollider;

    private void Awake()
    {
        mundo = GetComponent<Mundo>();
        m_mesh = new Mesh();
        m_meshCollider = GetComponent<MeshCollider>();
    }

    public void GenerarMeshColision(IRender render, Extremo rangoEntidad, Constitucion entidad)
    {
        m_mesh.Clear();
        MeshData meshDataColision = new MeshData();

        render.GenerarMeshColision(rangoEntidad, mundo.sacarDatos, ref meshDataColision, entidad);
        meshDataColision.RellenarMesh(m_mesh);

        m_meshCollider.sharedMesh = m_mesh;
    }
}
