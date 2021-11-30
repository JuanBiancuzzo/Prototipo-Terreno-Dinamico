using UnityEngine;
using System;

public abstract class IContenible : ITenerDatos
{
    public abstract Vector3Int Posicion();

    public abstract void ActualizarPosicion(Vector3Int posicionNueva);

    public abstract float GetValor();

    public abstract Color GetColor();
}