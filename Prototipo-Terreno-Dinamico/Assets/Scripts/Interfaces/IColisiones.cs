using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IColisiones
{
    void GenerarMeshColision(IRender render, Extremo rangoEntidad, Constitucion entidad);
}