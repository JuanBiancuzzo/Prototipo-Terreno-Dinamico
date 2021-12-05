using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRenderizable 
{
    void Renderizar(IRender render);

    void RenderizarElemento(IRender render, Vector3Int posicion);
}