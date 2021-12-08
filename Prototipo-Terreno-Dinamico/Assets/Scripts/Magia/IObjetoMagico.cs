using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjetoMagico
{
    public void DarMagia();
    public void DejarDeDarMagia();
    public EnergiaCoin Dar(TipoDeMagia tipoDeMagia, EnergiaCoin cantidad);
    public EnergiaCoin EnergiaCapazDeDar(TipoDeMagia tipoDeMagia, EnergiaCoin energiaDeseada = null);

    public void RecibirMagia();
    public void DejarDeRecibirMagia();
    public EnergiaCoin Recibir(TipoDeMagia tipoDeMagia, EnergiaCoin cantidad);
    public EnergiaCoin EnergiaCapazDeRecibir(TipoDeMagia tipoDeMagia, EnergiaCoin energiaDeseada = null);
}
