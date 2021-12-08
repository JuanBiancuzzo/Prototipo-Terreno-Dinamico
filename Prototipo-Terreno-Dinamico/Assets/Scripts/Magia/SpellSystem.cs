using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Grupo
{
    public IObjetoMagico elemento;
    public TipoDeMagia tipoDeMagia;
};

public static class SpellSystem
{
    public static bool Spell(List<Grupo> dar, List<Grupo> recibir, EnergiaCoin energiaDeseada)
    {
        EnergiaCoin energiaParaDar = EnergiaCapazDeDar(dar, energiaDeseada);
        EnergiaCoin energiaParaRecibir = EnergiaCapazDeRecibir(recibir, energiaDeseada);

        EnergiaCoin energiaMinima = energiaParaDar.MenorEnergia(energiaParaRecibir);
        EnergiaCoin energiaTotal = new EnergiaCoin();

        foreach (Grupo grupo in dar)
        {
            EnergiaCoin energiaCapazDeDar = grupo.elemento.EnergiaCapazDeDar(grupo.tipoDeMagia, energiaDeseada);
            EnergiaCoin energiaADar = energiaCapazDeDar.MenorEnergia(energiaMinima);

            grupo.elemento.DarMagia();
            EnergiaCoin energia = EventHandlerMagia.current.SacarEnergia(grupo.tipoDeMagia, energiaADar);
            grupo.elemento.DejarDeDarMagia();

            if (energia == null)
                return false;

            energiaTotal.AumentarEnergia(energia);
            energiaMinima.DisminuirEnergia(energia);
        }

        foreach (Grupo grupo in recibir)
        {
            EnergiaCoin energiaCapazDeDar = grupo.elemento.EnergiaCapazDeRecibir(grupo.tipoDeMagia, energiaDeseada);
            EnergiaCoin cantidadARecibir = energiaCapazDeDar.MenorEnergia(energiaTotal);

            grupo.elemento.RecibirMagia();
            EventHandlerMagia.current.DarEnergia(grupo.tipoDeMagia, cantidadARecibir);
            grupo.elemento.DejarDeRecibirMagia();

            energiaTotal.DisminuirEnergia(cantidadARecibir);
        }

        return true;
    }

    private static EnergiaCoin EnergiaCapazDeDar(List<Grupo> dar, EnergiaCoin energiaDeseada)
    {
        EnergiaCoin energiaCapaz = new EnergiaCoin();

        foreach (Grupo grupo in dar)
        {
            EnergiaCoin energia = grupo.elemento.EnergiaCapazDeDar(grupo.tipoDeMagia, energiaDeseada);
            if (energia == null)
                continue;
            energiaCapaz.AumentarEnergia(energia);
        }

        return energiaCapaz;
    }

    private static EnergiaCoin EnergiaCapazDeRecibir(List<Grupo> recibir, EnergiaCoin energiaDeseada)
    {
        EnergiaCoin energiaCapaz = new EnergiaCoin();

        foreach (Grupo grupo in recibir)
        {
            EnergiaCoin energia = grupo.elemento.EnergiaCapazDeRecibir(grupo.tipoDeMagia, energiaDeseada);
            if (energia == null)
                continue;
            energiaCapaz.AumentarEnergia(energia);
        }

        return energiaCapaz;
    }

    public static bool Spell(Grupo dar, Grupo recibir, EnergiaCoin energia)
    {
        return Spell(new List<Grupo> { dar }, new List<Grupo> { recibir }, energia);
    }

    public static bool Spell(List<Grupo> dar, Grupo recibir, EnergiaCoin energia)
    {
        return Spell(dar, new List<Grupo> { recibir }, energia);
    }

    public static bool Spell(Grupo dar, List<Grupo> recibir, EnergiaCoin energia)
    {
        return Spell(new List<Grupo> { dar }, recibir, energia);
    }
}
