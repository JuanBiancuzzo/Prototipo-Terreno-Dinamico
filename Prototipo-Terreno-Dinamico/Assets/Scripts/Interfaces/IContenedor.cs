using UnityEngine;

public interface IContenedor
{
    public bool Insertar(Elemento elemento);

    public Elemento Eliminar(Vector3Int posicion);

    public Elemento Eliminar(Elemento elemento);

    public bool Intercambiar(Vector3Int origen, Vector3Int destino);

    public bool Intercambiar(Elemento elementoOrigen, Elemento elementoDestino);

    public Elemento EnPosicion(Vector3Int posicion);

    public bool EnRango(Vector3Int posicion);

    public bool EnRango(Elemento elemento);
}
