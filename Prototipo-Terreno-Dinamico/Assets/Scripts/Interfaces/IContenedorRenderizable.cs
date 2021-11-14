using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IContenedorRenderizable : IRenderizable, IContenedor
{
}

public interface IContenedorConDatos : ISacarDatos, IContenedor
{
}
