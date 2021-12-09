using System;
using UnityEngine;

public enum TipoDeMagia
{
    Color,
    Alfa,
    Temperatura,
    Concentracion,
    Consitucion,
    Iluminacion
};

public class EventHandlerMagia : MonoBehaviour
{
    public static EventHandlerMagia current;

    private void Awake()
    {
        current = this;
    }

    public event Func<TipoDeMagia, EnergiaCoin, EnergiaCoin> darEnergia;
    public EnergiaCoin SacarEnergia(TipoDeMagia tipoDeMagia, EnergiaCoin cantidad)
    {
        return (darEnergia == null) ? null : darEnergia(tipoDeMagia, cantidad);
    }

    public event Func<TipoDeMagia, EnergiaCoin, EnergiaCoin> recibirEnergia;
    public EnergiaCoin DarEnergia(TipoDeMagia tipoDeMagia, EnergiaCoin cantidad)
    {
        return (recibirEnergia == null) ? null : recibirEnergia(tipoDeMagia, cantidad);
    }
}
