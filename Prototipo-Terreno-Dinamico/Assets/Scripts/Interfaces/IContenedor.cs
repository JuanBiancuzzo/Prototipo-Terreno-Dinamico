using UnityEngine;

public interface IContenedor
{
    public bool Insertar(Elemento contenible);

    public Elemento Eliminar(Vector3Int posicion);

    public Elemento Eliminar(Elemento contenible);

    public bool Intercambiar(Vector3Int origen, Vector3Int destino);

    public bool Intercambiar(Elemento contenibleOrigen, Elemento contenibleDestino);

    public Elemento EnPosicion(Vector3Int posicion);

    public bool EnRango(Vector3Int posicion);

    public bool EnRango(Elemento contenible);
}
