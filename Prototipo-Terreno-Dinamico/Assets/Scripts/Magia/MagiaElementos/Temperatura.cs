using System;
using UnityEngine;

[System.Serializable]
public class Temperatura : IEnergia
{
    static int minimo = 0, maximo = 3000;
    int m_conductividad = 50;
    //private float Costo => (m_conductividad * 10f) / maximo;
    static float m_costo = 30f;

    public int m_temperatura;
    public int TemperaturaValor => m_temperatura;
    public void NuevoValor(int valor) => m_temperatura = Mathf.Max(minimo, Mathf.Min(maximo, valor));
    public void Aumentar(int cantidad) => Recibir(AtributoAEnergia(cantidad));
    public void Disminuir(int cantidad) => Dar(AtributoAEnergia(cantidad));

    public int Conductividad => m_conductividad;
    public void NuevaConductividad(int valor) => m_conductividad = valor;

    public EnergiaCoin EnergiaActual => AtributoAEnergia(TemperaturaValor);

    public Temperatura(int temperatura)
    {
        NuevoValor(temperatura);
    }


    /*
     * Disminuir es saca energia y devuelve cuanto consumio
     * Aumentar es dar energia y devuelve cuanto no pudo agarrar
     */

    public EnergiaCoin Recibir(EnergiaCoin energia)
    {
        int temperaturaAAgregar = EnergiaAAtributo(energia);
        int temperaturaPosible = Mathf.Min(maximo - m_temperatura, temperaturaAAgregar);

        NuevoValor(m_temperatura + temperaturaPosible);

        return AtributoAEnergia(temperaturaAAgregar - temperaturaPosible);
    }

    public EnergiaCoin EnergiaCapazDeRecibir(EnergiaCoin energiaDeseada)
    {
        EnergiaCoin capacidadMaxima = AtributoAEnergia(Mathf.Max(minimo, maximo - TemperaturaValor));
        return capacidadMaxima.MenorEnergia(energiaDeseada);
    }

    public EnergiaCoin Dar(EnergiaCoin energia)
    {
        int temperaturaASacar = EnergiaAAtributo(energia);
        temperaturaASacar = Mathf.Min(m_temperatura, temperaturaASacar);

        NuevoValor(m_temperatura - temperaturaASacar);

        return AtributoAEnergia(temperaturaASacar);
    }

    public EnergiaCoin EnergiaCapazDeDar(EnergiaCoin energiaDeseada)
    {
        EnergiaCoin capacidadMaxima = AtributoAEnergia(TemperaturaValor);
        return capacidadMaxima.MenorEnergia(energiaDeseada);
    }

    public int EnergiaAAtributo(EnergiaCoin energia)
    {
        float porcentajeEnergia = energia.EnergiaActualInterpolada();
        float conversion = PorcentajeDeEnergiaATemperatura(porcentajeEnergia);
        return Mathf.FloorToInt(Mathf.Lerp(minimo, maximo, conversion));
    }

    public EnergiaCoin AtributoAEnergia(int temperatura)
    {
        float temperaturaRelativa = ((float)temperatura / maximo);
        EnergiaCoin energia = new EnergiaCoin();
        energia.ValorActualEnergia(PorcentajeDeTemperaturaAEnergia(temperaturaRelativa));
        return energia;
    }

    private float PorcentajeDeTemperaturaAEnergia(float porcentajeDeTemperatura)
    {
        return porcentajeDeTemperatura * m_costo;
    }

    private float PorcentajeDeEnergiaATemperatura(float porcentajeDeEnergia)
    {
        return porcentajeDeEnergia / m_costo;
    }

    public int IluminacionPorTemperatrua()
    { 
        if (TemperaturaValor < 800)
            return 0;
        if (TemperaturaValor > 1000)
            return 100;
        return (TemperaturaValor - 800) / 2;
    }
}
