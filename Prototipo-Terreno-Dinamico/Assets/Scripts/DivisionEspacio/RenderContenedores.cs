using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Mundo))]
public class RenderContenedores : MonoBehaviour, IRenderizable
{
    protected Mundo mundo;
    Mesh m_mesh;

    private void Awake()
    {
        mundo = GetComponent<Mundo>();
        m_mesh = new Mesh();
        GetComponent<MeshFilter>().sharedMesh = m_mesh;
    }

    public void Renderizar(IRender render, ISacarDatos contenedor = null)
    {
        MeshData meshDataOpaco = new MeshData();
        render.GenerarMesh(mundo.contenedor.m_extremo, contenedor, ref meshDataOpaco, TipoMaterial.Opaco);
        Mesh meshOpaco = new Mesh();
        meshDataOpaco.RellenarMesh(meshOpaco);

        MeshData meshDataTranslucido = new MeshData();
        render.GenerarMesh(mundo.contenedor.m_extremo, contenedor, ref meshDataTranslucido, TipoMaterial.Translucido);
        Mesh meshTranslucido = new Mesh();
        meshDataTranslucido.RellenarMesh(meshTranslucido);

        List<CombineInstance> finalCombiner = new List<CombineInstance>();
        foreach (Mesh mesh in new List<Mesh> { meshOpaco, meshTranslucido })
        {
            CombineInstance ci = new CombineInstance();
            ci.mesh = mesh;
            ci.subMeshIndex = 0;
            ci.transform = Matrix4x4.identity;
            finalCombiner.Add(ci);
        }
        m_mesh.CombineMeshes(finalCombiner.ToArray(), false);
    }
}
