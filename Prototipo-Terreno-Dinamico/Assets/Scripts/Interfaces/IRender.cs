using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRender
{
    public void GenerarMesh(Extremo extremo, ISacarDatos datos, ref MeshData preInfo, int LOD = 1);

    public void GenerarMeshCompute(Extremo extremo, ISacarDatos datos, ref MeshData preInfo, int LOD = 1);
}
