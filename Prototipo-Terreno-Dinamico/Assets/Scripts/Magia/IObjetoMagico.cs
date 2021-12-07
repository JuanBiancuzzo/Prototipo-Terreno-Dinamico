using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjetoMagico
{
    public void DarMagia();
    public void DejarDeDarMagia();
    public EnergiaCoin Dar(TipoDeMagia tipoDeMagia, EnergiaCoin cantidad);

    public void RecibirMagia();
    public void DejarDeRecibirMagia();
    public EnergiaCoin Recibir(TipoDeMagia tipoDeMagia, EnergiaCoin cantidad);
}
