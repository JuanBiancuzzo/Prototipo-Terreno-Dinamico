using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iluminacion : IEnergia
{
    static int minimo = 0, maximo = 100;
    [SerializeField] AtributoInt m_iluminacion = null;
    Temperatura m_temperatura;

    public int IluminacionValor()
    {
        int iluminacionPorTemperatura = m_temperatura.IluminacionPorTemperatrua();
        return Mathf.Max(m_iluminacion.Valor, iluminacionPorTemperatura);
    }

    public void NuevoValor(int valor) => m_iluminacion.NuevoValor(valor);
    public void Aumentar(int cantidad)
    {
        Recibir(m_iluminacion.AtributoAEnergia(cantidad));
    }
    public void Disminuir(int cantidad)
    {
        Dar(m_iluminacion.AtributoAEnergia(cantidad));
    }

    public Iluminacion(int iluminacion, Temperatura temperatura)
    {
        m_iluminacion = new AtributoInt(iluminacion, minimo, maximo);
        m_temperatura = temperatura;
    }

    /*
     * Aumentar es dar energia y devuelve cuanto no pudo agarrar
     * Disminuir es saca energia y devuelve cuanto consumio
     */

    public EnergiaCoin Recibir(EnergiaCoin energia)
    {
        int iluminacionARecibir = m_iluminacion.EnergiaAAtributo(energia);
        int iluminacionPosible = Mathf.Min(maximo - m_iluminacion.Valor, iluminacionARecibir);

        m_iluminacion.Aumentar(iluminacionPosible);

        return m_iluminacion.AtributoAEnergia(iluminacionARecibir - iluminacionPosible, energia);
    }

    public EnergiaCoin EnergiaCapazDeRecibir(EnergiaCoin energiaDeseada = null)
    {
        EnergiaCoin capacidadMaxima = m_iluminacion.AtributoAEnergia(Mathf.Max(minimo, maximo - IluminacionValor()));
        return capacidadMaxima.MenorEnergia(energiaDeseada);
    }

    public EnergiaCoin Dar(EnergiaCoin energia)
    {
        EnergiaCoin energiaASacar = new EnergiaCoin(energia.Valor);
        
        EnergiaCoin energiaIluminacion = m_iluminacion.AtributoAEnergia(m_iluminacion.Valor);
        EnergiaCoin energiaTemperatura = m_temperatura.EnergiaActual;

        energiaASacar = energiaIluminacion.DisminuirEnergia(energiaASacar);
        m_iluminacion.NuevoValor(m_iluminacion.EnergiaAAtributo(energiaIluminacion));

        energiaASacar = energiaTemperatura.DisminuirEnergia(energiaASacar);
        m_temperatura.NuevoValor(m_temperatura.m_temperatura.EnergiaAAtributo(energiaTemperatura));

        return new EnergiaCoin(energia.Valor - energiaASacar.Valor);
    }

    public EnergiaCoin EnergiaCapazDeDar(EnergiaCoin energiaDeseada = null)
    {
        EnergiaCoin capacidadMaxima = m_iluminacion.AtributoAEnergia(IluminacionValor());
        return capacidadMaxima.MenorEnergia(energiaDeseada);
    }
}
