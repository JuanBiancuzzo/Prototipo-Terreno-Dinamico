using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Alfa : IEnergia
{
    static float minimo = 0f, maximo = 1f;
    [SerializeField] AtributoFloat m_alfa;

    public float AlfaValor => m_alfa.Valor;
    public void NuevoValor(float valor) {
        m_alfa.NuevoValor(valor);
    }
    public Alfa(float alfa)
    {
        m_alfa = new AtributoFloat(alfa, minimo, maximo);
    }

    /*
     * Aumentar es dar energia y devuelve cuanto no pudo agarrar
     * Disminuir es saca energia y devuelve cuanto consumio
     */
    public EnergiaCoin Recibir(EnergiaCoin energia)
    {
        float alfaAAgregar = m_alfa.EnergiaAAtributo(energia);
        float alfaPosible = Mathf.Min(maximo - m_alfa.Valor, alfaAAgregar);

        m_alfa.Aumentar(alfaPosible);

        return m_alfa.AtributoAEnergia(alfaAAgregar - alfaPosible, energia);
    }

    public EnergiaCoin EnergiaCapazDeRecibir(EnergiaCoin energiaDeseada)
    {
        EnergiaCoin capacidadMaxima = m_alfa.AtributoAEnergia(Mathf.Max(minimo, maximo - AlfaValor));
        return capacidadMaxima.MenorEnergia(energiaDeseada);
    }

    public EnergiaCoin Dar(EnergiaCoin energia)
    {
        float alfaASacar = m_alfa.EnergiaAAtributo(energia);
        alfaASacar = Mathf.Min(m_alfa.Valor, alfaASacar);

        m_alfa.Disminuir(alfaASacar);

        return m_alfa.AtributoAEnergia(alfaASacar, energia);
    }

    public EnergiaCoin EnergiaCapazDeDar(EnergiaCoin energiaDeseada)
    {
        EnergiaCoin capacidadMaxima = m_alfa.AtributoAEnergia(AlfaValor);
        return capacidadMaxima.MenorEnergia(energiaDeseada);
    }
}
