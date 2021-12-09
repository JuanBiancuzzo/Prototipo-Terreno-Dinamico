using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iluminacion : IEnergia
{
    static int minimo = 0, maximo = 100;
    int m_iluminacion;
    static float m_costo = 1f;
    Temperatura m_temperatura;

    public int IluminacionValor()
    {
        int iluminacionPorTemperatura = m_temperatura.IluminacionPorTemperatrua();
        return Mathf.Max(m_iluminacion, iluminacionPorTemperatura);
    }

    public void NuevoValor(int valor) => m_iluminacion = Mathf.Min(maximo, Mathf.Max(minimo, valor));
    public void Aumentar(int cantidad) => Recibir(AtributoAEnergia(cantidad));
    public void Disminuir(int cantidad) => Dar(AtributoAEnergia(cantidad));

    public Iluminacion(int iluminacion, Temperatura temperatura)
    {
        NuevoValor(iluminacion);
        m_temperatura = temperatura;
    }

    /*
     * Aumentar es dar energia y devuelve cuanto no pudo agarrar
     * Disminuir es saca energia y devuelve cuanto consumio
     */

    public EnergiaCoin Recibir(EnergiaCoin energia)
    {
        int iluminacionARecibir = EnergiaAAtributo(energia);
        int iluminacionPosible = Mathf.Min(maximo - m_iluminacion, iluminacionARecibir);

        NuevoValor(m_iluminacion + iluminacionPosible);

        return AtributoAEnergia(iluminacionARecibir - iluminacionPosible);
    }

    public EnergiaCoin EnergiaCapazDeRecibir(EnergiaCoin energiaDeseada = null)
    {
        EnergiaCoin capacidadMaxima = AtributoAEnergia(Mathf.Max(minimo, maximo - IluminacionValor()));
        return capacidadMaxima.MenorEnergia(energiaDeseada);
    }

    public EnergiaCoin Dar(EnergiaCoin energia)
    {
        EnergiaCoin energiaASacar = new EnergiaCoin(energia.Valor);
        
        EnergiaCoin energiaIluminacion = AtributoAEnergia(m_iluminacion);
        EnergiaCoin energiaTemperatura = m_temperatura.EnergiaActual;

        energiaASacar = energiaIluminacion.DisminuirEnergia(energiaASacar);
        NuevoValor(EnergiaAAtributo(energiaIluminacion));

        energiaASacar = energiaTemperatura.DisminuirEnergia(energiaASacar);
        m_temperatura.NuevoValor(m_temperatura.EnergiaAAtributo(energiaTemperatura));

        return new EnergiaCoin(energia.Valor - energiaASacar.Valor);
    }

    public EnergiaCoin EnergiaCapazDeDar(EnergiaCoin energiaDeseada = null)
    {
        EnergiaCoin capacidadMaxima = AtributoAEnergia(IluminacionValor());
        return capacidadMaxima.MenorEnergia(energiaDeseada);
    }

    public int EnergiaAAtributo(EnergiaCoin energia)
    {
        float porcentajeEnergia = energia.EnergiaActualInterpolada();
        float conversion = PorcentajeDeEnergiaAIluminacion(porcentajeEnergia);
        return Mathf.FloorToInt(Mathf.Lerp(minimo, maximo, conversion));
    }

    public EnergiaCoin AtributoAEnergia(int iluminacion)
    {
        float iluminacionRelativa = (float)iluminacion / maximo;
        EnergiaCoin energia = new EnergiaCoin();
        energia.ValorActualEnergia(PorcentaDeIluminacionAEnergia(iluminacionRelativa));
        return energia;
    }
    private float PorcentaDeIluminacionAEnergia(float porcentajeDeIluminacion)
    {
        return porcentajeDeIluminacion * m_costo;
    }

    private float PorcentajeDeEnergiaAIluminacion(float porcentajeDeEnergia)
    {
        return porcentajeDeEnergia / m_costo;
    }
}
