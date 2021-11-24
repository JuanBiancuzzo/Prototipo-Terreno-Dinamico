using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gaseoso : Elemento
{
    protected Gaseoso(Vector3Int posicion) : base(posicion)
    {
    }
    public override void ActuanEnElemento(Elemento elemento, int dt)
    {
        elemento.ActuarEnOtro(this, dt);
    }

    public override void Avanzar(IContenedorConDatos mapa, int dt)
    {
    }

    public override bool Reacciona(IContenedorConDatos mapa)
    {
        return false;
    }
}
