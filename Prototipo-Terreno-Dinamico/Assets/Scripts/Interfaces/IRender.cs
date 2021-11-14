using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRender
{
    public void GenerarMesh(Extremo extremo, ISacarDatos datos, ref MeshData preInfo);
}
