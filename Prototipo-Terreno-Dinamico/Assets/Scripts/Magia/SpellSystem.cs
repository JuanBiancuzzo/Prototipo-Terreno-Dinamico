using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Grupo
{
    public ElementoMagico elemento;
    public EnergiaCoin energia;
    public TipoDeMagia tipoDeMagia;
};

public static class SpellSystem
{
    public static bool Spell(List<Grupo> dar, List<Grupo> recibir)
    {
        EnergiaCoin energiaTotal = new EnergiaCoin();

        foreach (Grupo grupo in dar)
        {
            grupo.elemento.DarMagia();
            EnergiaCoin energia = EventHandlerMagia.current.SacarEnergia(grupo.tipoDeMagia, grupo.energia);
            grupo.elemento.DejarDeDarMagia();

            if (energia == null)
                return false;

            energiaTotal.AumentarEnergia(energia);
        }

        recibir.Sort((a, b) => (b.energia.Valor).CompareTo(a.energia.Valor));

        foreach (Grupo grupo in recibir)
        {
            grupo.elemento.RecibirMagia();

            EnergiaCoin cantidadARecibir = energiaTotal.MenorEnergia(grupo.energia);

            EnergiaCoin energia = EventHandlerMagia.current.DarEnergia(grupo.tipoDeMagia, cantidadARecibir);
            grupo.elemento.DejarDeRecibirMagia();

            if (energia == null)
                return false;

            energiaTotal.AumentarEnergia(energia);
        }

        return true;
    }

    public static bool Spell(Grupo dar, Grupo recibir)
    {
        return Spell(new List<Grupo> { dar }, new List<Grupo> { recibir });
    }

    public static bool Spell(List<Grupo> dar, Grupo recibir)
    {
        return Spell(dar, new List<Grupo> { recibir });
    }

    public static bool Spell(Grupo dar, List<Grupo> recibir)
    {
        return Spell(new List<Grupo> { dar }, recibir);
    }
}
