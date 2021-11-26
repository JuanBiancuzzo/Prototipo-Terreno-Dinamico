using UnityEngine;
using System;

public abstract class IContenible : ITenerDatos
{
    public event Action<IContenible> necesitoActualizar;

    public void NecesitoActualizar(IContenible contenible)
    {
        necesitoActualizar?.Invoke(contenible);
    }

    public abstract Vector3Int Posicion();

    public abstract void ActualizarPosicion(Vector3Int posicionNueva);

    public abstract float GetValor();

    public abstract Color GetColor();
}