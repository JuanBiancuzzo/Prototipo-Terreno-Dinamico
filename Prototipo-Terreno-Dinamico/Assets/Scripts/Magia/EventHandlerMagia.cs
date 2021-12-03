using System;
using UnityEngine;

public enum TipoDeMagia
{
    Iluminacion,
    Color,
    Alfa,
    Temperatura,
    Velocidad
};

public class EventHandlerMagia
{
    public static EventHandlerMagia current;

    private void Awake()
    {
        current = this;
    }

    public event Func<TipoDeMagia, int, Vector3, int> darEnergia;
    public int SacarEnergia(TipoDeMagia tipoDeMagia, int cantidad, Vector3 posicion = default)
    {
        return (darEnergia == null) ? -1 : darEnergia(tipoDeMagia, cantidad, posicion);
    }

    public event Func<TipoDeMagia, int, Vector3, int> recibirEnergia;
    public int DarEnergia(TipoDeMagia tipoDeMagia, int cantidad, Vector3 posicion = default)
    {
        return (recibirEnergia == null) ? -1 : recibirEnergia(tipoDeMagia, cantidad, posicion);
    }
}
