using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TipoMaterial
{
    Opaco,
    Translucido,
    Trnasparente
}

public interface IRender
{
    public void GenerarMesh(Extremo extremo, ISacarDatos datos, ref MeshData preInfo, TipoMaterial tipoMaterial = TipoMaterial.Opaco, int LOD = 1);
    public void GenerarMeshColision(Extremo extremo, ISacarDatos datos, ref MeshData preInfo, Constitucion entidad);
}
