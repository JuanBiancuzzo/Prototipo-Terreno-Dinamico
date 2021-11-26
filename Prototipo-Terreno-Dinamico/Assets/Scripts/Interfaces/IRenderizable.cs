using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRenderizable
{
    void Renderizar(IRender render, ISacarDatos contenedor = null, int LOD = 1, bool overrideActualizacion = false);
}
