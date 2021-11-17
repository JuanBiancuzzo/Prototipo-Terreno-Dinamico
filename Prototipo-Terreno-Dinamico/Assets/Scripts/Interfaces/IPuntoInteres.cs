using UnityEngine;

public interface IPuntoInteres
{
    public void Insertar(Vector3Int punto);

    public Vector3Int GetPunto();

    public bool Avanzar();

    public bool PuedoAvanzar();

}
