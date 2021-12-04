using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRenderizable
{
    void GenerarMeshColision(IRender render, Extremo rangoEntidad, Constitucion entidad);
    void Renderizar(IRender render, ISacarDatos contenedor = null);
}
