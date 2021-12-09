using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Constitucion : IEnergia
{
    static int minimo = 0, maximo = 100;
    [SerializeField] int m_constitucion;
    static float m_costo = 10;

    public int ConstitucionValor => m_constitucion;
    public void NuevoValor(int valor) => m_constitucion = Mathf.Min(maximo, Mathf.Max(minimo, valor));
    public void Aumentar(int cantidad) => Recibir(AtributoAEnergia(cantidad));
    public void Disminuir(int cantidad) => Dar(AtributoAEnergia(cantidad));

    public Constitucion(int constitucion)
    {
        NuevoValor(constitucion);
    }


    /*
     * Disminuir es saca energia y devuelve cuanto consumio
     * Aumentar es dar energia y devuelve cuanto no pudo agarrar
     */
    public EnergiaCoin Recibir(EnergiaCoin energia)
    {
        int constitucionAAgregar = EnergiaAAtributo(energia);
        int constitucionPosible = Mathf.Min(maximo - m_constitucion, constitucionAAgregar);

        NuevoValor(m_constitucion + constitucionPosible);

        return AtributoAEnergia(constitucionAAgregar - constitucionPosible);
    }

    public EnergiaCoin EnergiaCapazDeRecibir(EnergiaCoin energiaDeseada)
    {
        EnergiaCoin capacidadMaxima = AtributoAEnergia(Mathf.Max(minimo, maximo - ConstitucionValor));
        return capacidadMaxima.MenorEnergia(energiaDeseada);
    }

    public EnergiaCoin Dar(EnergiaCoin energia)
    {
        int constitucionASacar = EnergiaAAtributo(energia);
        constitucionASacar = Mathf.Min(m_constitucion, constitucionASacar);

        NuevoValor(m_constitucion - constitucionASacar);

        return AtributoAEnergia(constitucionASacar);
    }

    public EnergiaCoin EnergiaCapazDeDar(EnergiaCoin energiaDeseada)
    {
        EnergiaCoin capacidadMaxima = AtributoAEnergia(ConstitucionValor);
        return capacidadMaxima.MenorEnergia(energiaDeseada);
    }

    public int EnergiaAAtributo(EnergiaCoin energia)
    {
        float porcentajeEnergia = energia.EnergiaActualInterpolada();
        float conversion = PorcentajeDeEnergiaAConstitucion(porcentajeEnergia); 
        return Mathf.FloorToInt(Mathf.Lerp(minimo, maximo, conversion));
    }

    public EnergiaCoin AtributoAEnergia(int constitucion)
    {
        float constitucionRelativa = (float)constitucion / maximo;
        EnergiaCoin energia = new EnergiaCoin();
        energia.ValorActualEnergia(PorcentajeDeConstitucionAEnergia(constitucionRelativa));
        return energia;
    }

    private float PorcentajeDeConstitucionAEnergia(float porcentajeDeConstitucion)
    {
        return porcentajeDeConstitucion * m_costo;
    }

    private float PorcentajeDeEnergiaAConstitucion(float porcentajeDeEnergia)
    {
        return porcentajeDeEnergia / m_costo;
    }

    // el otro tiene desventaja
    public bool Atraviesa(Constitucion otro)
    {
        int valorReal = maximo - ConstitucionValor;
        return otro.ConstitucionValor < valorReal;
    }
}
