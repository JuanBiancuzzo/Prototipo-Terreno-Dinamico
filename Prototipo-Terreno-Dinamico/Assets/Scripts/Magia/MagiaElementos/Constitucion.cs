using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Constitucion : IEnergia
{
    static int minimo = 0, maximo = 100;
    //static float m_costeParaAgregar = 1f;
    //static float m_costeParaSacar = 5f;
    [SerializeField] AtributoInt m_constitucion;

    public int ConstitucionValor => m_constitucion.Valor;
    public void NuevoValor(int valor) => m_constitucion.NuevoValor(valor);
    public void Aumentar(int cantidad) => Recibir(m_constitucion.AtributoAEnergia(cantidad));
    public void Disminuir(int cantidad) => Dar(m_constitucion.AtributoAEnergia(cantidad));

    public Constitucion(int constitucion)
    {
        m_constitucion = new AtributoInt(constitucion, minimo, maximo);
    }

    // el otro tiene desventaja
    public bool Atraviesa(Constitucion otro)
    {
        int valorReal = maximo - ConstitucionValor;
        return otro.ConstitucionValor < valorReal;
    }

    /*
     * Disminuir es saca energia y devuelve cuanto consumio
     * Aumentar es dar energia y devuelve cuanto no pudo agarrar
     */
    public EnergiaCoin Recibir(EnergiaCoin energia)
    {
        int constitucionAAgregar = m_constitucion.EnergiaAAtributo(energia);
        int constitucionPosible = Mathf.Min(maximo - m_constitucion.Valor, constitucionAAgregar);

        m_constitucion.Aumentar(constitucionPosible);

        return m_constitucion.AtributoAEnergia(constitucionAAgregar - constitucionPosible, energia);
    }

    public EnergiaCoin EnergiaCapazDeRecibir(EnergiaCoin energiaDeseada)
    {
        EnergiaCoin capacidadMaxima = m_constitucion.AtributoAEnergia(Mathf.Max(minimo, maximo - ConstitucionValor));
        return capacidadMaxima.MenorEnergia(energiaDeseada);
    }

    public EnergiaCoin Dar(EnergiaCoin energia)
    {
        int constitucionASacar = m_constitucion.EnergiaAAtributo(energia);
        constitucionASacar = Mathf.Min(m_constitucion.Valor, constitucionASacar);

        m_constitucion.Disminuir(constitucionASacar);

        return m_constitucion.AtributoAEnergia(constitucionASacar, energia);
    }

    public EnergiaCoin EnergiaCapazDeDar(EnergiaCoin energiaDeseada)
    {
        EnergiaCoin capacidadMaxima = m_constitucion.AtributoAEnergia(ConstitucionValor);
        return capacidadMaxima.MenorEnergia(energiaDeseada);
    }
}
