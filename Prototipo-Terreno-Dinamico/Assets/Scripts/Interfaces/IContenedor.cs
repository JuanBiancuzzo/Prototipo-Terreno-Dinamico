using System;
using UnityEngine;

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

public interface IContenedor
{
    public bool Insertar(IContenible contenible);

    public IContenible Eliminar(Vector3Int posicion);

    public IContenible Eliminar(IContenible contenible);

    public bool Intercambiar(Vector3Int origen, Vector3Int destino);

    public bool Intercambiar(IContenible contenibleOrigen, IContenible contenibleDestino);

    public IContenible EnPosicion(Vector3Int posicion);

    public bool EnRango(Vector3Int posicion);

    public bool EnRango(IContenible contenible);
}
