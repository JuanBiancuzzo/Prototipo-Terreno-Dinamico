using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IContenedorGeneral))]
public class ColisionesMundo : Colisiones
{
    IContenedorGeneral mundo;

    Mesh m_meshColision;
    MeshCollider m_meshCollider;

    private void Awake()
    {
        mundo = GetComponent<IContenedorGeneral>();
        m_meshColision = new Mesh();
        m_meshCollider = GetComponent<MeshCollider>();
    }

    public override void GenerarMeshColision(IRender render, Extremo rangoEntidad, Constitucion entidad)
    {
        m_meshColision.Clear();
        MeshData meshDataColision = new MeshData();

        render.GenerarMeshColision(rangoEntidad, mundo, ref meshDataColision, entidad);
        meshDataColision.RellenarMesh(m_meshColision);

        m_meshCollider.sharedMesh = m_meshColision;
    }
}
