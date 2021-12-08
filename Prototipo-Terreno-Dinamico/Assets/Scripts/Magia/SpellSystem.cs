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
        EnergiaCoin energiaParaDar = EnergiaCapazDeDar(dar);
        EnergiaCoin energiaParaRecibir = EnergiaCapazDeRecibir(recibir);

        EnergiaCoin energiaMinima = energiaParaDar.MenorEnergia(energiaParaRecibir);

        EnergiaCoin energiaTotal = new EnergiaCoin();

        foreach (Grupo grupo in dar)
        {
            EnergiaCoin energiaCapazDeDar = grupo.elemento.EnergiaCapazDeDar(grupo.tipoDeMagia);
            EnergiaCoin energiaMaximaADar = energiaCapazDeDar.MenorEnergia(energiaMinima);
            EnergiaCoin energiaADar = energiaMaximaADar.MenorEnergia(energiaDeseada);

            grupo.elemento.DarMagia();
            EnergiaCoin energia = EventHandlerMagia.current.SacarEnergia(grupo.tipoDeMagia, energiaADar);
            grupo.elemento.DejarDeDarMagia();

            if (energia == null)
                return false;

            energiaTotal.AumentarEnergia(energia);
        }

        foreach (Grupo grupo in recibir)
        {
            grupo.elemento.RecibirMagia();

            EnergiaCoin energiaCapazDeDar = grupo.elemento.EnergiaCapazDeRecibir(grupo.tipoDeMagia);
            EnergiaCoin cantidadMaximaARecibir = energiaCapazDeDar.MenorEnergia(energiaTotal);
            EnergiaCoin cantidadARecibir = cantidadMaximaARecibir.MenorEnergia(energiaDeseada);

            EventHandlerMagia.current.DarEnergia(grupo.tipoDeMagia, cantidadARecibir);
            grupo.elemento.DejarDeRecibirMagia();

            energiaTotal.DisminuirEnergia(cantidadARecibir);
        }

        return true;
    }

    private static EnergiaCoin EnergiaCapazDeDar(List<Grupo> dar)
    {
        EnergiaCoin energiaCapaz = new EnergiaCoin();

        foreach (Grupo grupo in dar)
        {
            EnergiaCoin energia = grupo.elemento.EnergiaCapazDeDar(grupo.tipoDeMagia);
            if (energia == null)
                continue;
            energiaCapaz.AumentarEnergia(energia);
        }

        return energiaCapaz;
    }

    private static EnergiaCoin EnergiaCapazDeRecibir(List<Grupo> recibir)
    {
        EnergiaCoin energiaCapaz = new EnergiaCoin();

        foreach (Grupo grupo in recibir)
        {
            EnergiaCoin energia = grupo.elemento.EnergiaCapazDeRecibir(grupo.tipoDeMagia);
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
