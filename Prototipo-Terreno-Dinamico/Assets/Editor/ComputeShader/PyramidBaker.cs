using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PyramidBaker
{
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    private struct SourceVertex
    {
        public Vector3 position;
        public Vector2 uv;
    }

    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    private struct GeneratedVertex
    {
        public Vector3 position;
        public Vector3 normal;
        public Vector2 uv;
    }

    private const int SOURCE_VERT_STRIDE = sizeof(float) * (3 + 2);
    private const int SOURCE_INDEX_STRIDE = sizeof(int);
    private const int GENERATED_VERT_STRIDE = sizeof(float) * (3 + 3 + 2);
    private const int GENERATED_INDEX_STRIDE = sizeof(int);

    private static void DecomposeMesh(Mesh mesh, int subMeshIndex, out SourceVertex[] verts, out int[] indices)
    {
        var subMesh = mesh.GetSubMesh(subMeshIndex);

        Vector3[] allVertices = mesh.vertices;
        Vector2[] allUVs = mesh.uv;
        int[] allIndeces = mesh.triangles;

        verts = new SourceVertex[subMesh.vertexCount];
        indices = new int[subMesh.indexCount];

        for (int i = 0; i < subMesh.vertexCount; i++)
        {
            int wholeMeshIndex = i + subMesh.firstVertex;
            verts[i] = new SourceVertex() {
                position = allVertices[wholeMeshIndex],
                uv = allUVs[wholeMeshIndex],
            };
        }

        for (int i = 0; i < subMesh.indexCount; i++)
        {
            indices[i] = allIndeces[i + subMesh.indexStart] + subMesh.baseVertex - subMesh.firstVertex;
        }
    }

    private static Mesh ComposeMesh(GeneratedVertex[] verts, int[] indices)
    {
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[verts.Length];
        Vector3[] normals = new Vector3[verts.Length];
        Vector3[] uvs = new Vector3[verts.Length];

        for (int i = 0; i < verts.Length; i++)
        {
            var v = verts[i];
            vertices[i] = v.position;
            normals[i] = v.normal;
            uvs[i] = v.uv;
        }

        mesh.SetVertices(vertices);
        mesh.SetNormals(normals);
        mesh.SetUVs(0, uvs);
        mesh.SetIndices(indices, MeshTopology.Triangles, 0, true);
        mesh.Optimize();

        return mesh;
    }

    public static bool Run(ComputeShader shader, PyramidBakeSetting settings, out Mesh generatedMesh)
    {
        DecomposeMesh(settings.sourceMesh, settings.sourceSubMeshIndex, out var sourceVertices, out var sourceIndices);

        int numSourceTriangles = sourceIndices.Length / 3;

        GeneratedVertex[] generatedVertices = new GeneratedVertex[numSourceTriangles * 3 * 3];
        int[] generatedIndices = new int[generatedVertices.Length];

        GraphicsBuffer sourceVertBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, sourceVertices.Length, SOURCE_VERT_STRIDE);
        GraphicsBuffer sourceIndexBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, sourceIndices.Length, SOURCE_INDEX_STRIDE);
        GraphicsBuffer genVertBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, generatedVertices.Length, GENERATED_VERT_STRIDE);
        GraphicsBuffer genIndexBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, generatedIndices.Length, GENERATED_INDEX_STRIDE);

        int idGrassKernel = shader.FindKernel("Main");

        shader.SetBuffer(idGrassKernel, "_SourceVertices", sourceVertBuffer);
        shader.SetBuffer(idGrassKernel, "_SourceIndices", sourceIndexBuffer);
        shader.SetBuffer(idGrassKernel, "_GeneratedVertices", genVertBuffer);
        shader.SetBuffer(idGrassKernel, "_GeneratedIndeces", genIndexBuffer);

        shader.SetMatrix("_Transform", Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(settings.rotation), settings.scale));
        shader.SetFloat("_PyramidHeight", settings.pyramidHeight);
        shader.SetInt("_NumSourceTriangles", numSourceTriangles);

        sourceVertBuffer.SetData(sourceVertices);
        sourceIndexBuffer.SetData(sourceIndices);

        shader.GetKernelThreadGroupSizes(idGrassKernel, out uint threadGroupSize, out _ , out _ );
        int dispatchSize = Mathf.CeilToInt((float)numSourceTriangles / threadGroupSize);

        shader.Dispatch(idGrassKernel, dispatchSize, 1, 1);

        genVertBuffer.GetData(generatedVertices);
        genIndexBuffer.GetData(generatedIndices);

        generatedMesh = ComposeMesh(generatedVertices, generatedIndices);

        sourceVertBuffer.Release();
        sourceIndexBuffer.Release();
        genVertBuffer.Release();
        genIndexBuffer.Release();

        return true;
    }
}
