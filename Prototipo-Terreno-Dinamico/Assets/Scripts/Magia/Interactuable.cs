using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Interactuable : MonoBehaviour
{
    public void DarMagia()
    {
        EventHandlerMagia.current.darEnergia += Dar;
    }

    public void DejarDeDarMagia()
    { 
        EventHandlerMagia.current.darEnergia -= Dar;
    }

    public abstract int Dar(TipoDeMagia tipoDeMagia, int cantidad, Vector3 posicion);

    public void RecibirMagia()
    {
        EventHandlerMagia.current.recibirEnergia += Recibir;
    }

    public void DejarDeRecibirMagia()
    {
        EventHandlerMagia.current.recibirEnergia -= Recibir;
    }

    public abstract int Recibir(TipoDeMagia tipoDeMagia, int cantidad, Vector3 posicion);
}
