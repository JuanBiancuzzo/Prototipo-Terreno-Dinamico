using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IContenible : ITenerDatos
{
    public Vector3Int Posicion();

    public void ActualizarPosicion(Vector3Int posicionNueva);
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
