using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Mundo))]
public class RenderContenedores : MonoBehaviour, IRenderizable
{
    protected Mundo mundo;
    Mesh m_mesh;
    public ElementoSeleccionado objectoSeleccionado;

    private void Awake()
    {
        mundo = GetComponent<Mundo>();
        m_mesh = new Mesh();
        GetComponent<MeshFilter>().sharedMesh = m_mesh;
    }

    public void Renderizar(IRender render)
    {
        MeshData meshDataOpaco = new MeshData();
        render.GenerarMesh(mundo.contenedor.m_extremo, mundo.sacarDatos, ref meshDataOpaco, TipoMaterial.Opaco);
        Mesh meshOpaco = new Mesh();
        meshDataOpaco.RellenarMesh(meshOpaco);

        MeshData meshDataTranslucido = new MeshData();
        render.GenerarMesh(mundo.contenedor.m_extremo, mundo.sacarDatos, ref meshDataTranslucido, TipoMaterial.Translucido);
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

    public void RenderizarElemento(IRender render, Vector3Int posicion)
    {
        if (objectoSeleccionado == null)
            return;

        if (!mundo.EnRango(posicion))
        {
            objectoSeleccionado.LimplearMesh();
            return;
        }

        MeshData meshData = new MeshData();
        render.GenerarMeshSeleccion(posicion, mundo.sacarDatos, ref meshData);
        objectoSeleccionado.CargarNuevaMesh(meshData);
    }
}
