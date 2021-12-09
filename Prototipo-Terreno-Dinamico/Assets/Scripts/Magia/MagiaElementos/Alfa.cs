using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Alfa : IEnergia
{
    static float minimo = 0f, maximo = 1f;
    [SerializeField] float m_alfa;
    static float m_costo = 1f;

    public float AlfaValor => m_alfa;
    public void NuevoValor(float valor) => m_alfa = Mathf.Max(minimo, Mathf.Min(maximo, valor));
    public void Aumentar(float cantidad) => Recibir(AtributoAEnergia(cantidad));
    public void Disminuir(float cantidad) => Dar(AtributoAEnergia(cantidad));

    public Alfa(float alfa)
    {
        NuevoValor(alfa);
    }

    /*
     * Aumentar es dar energia y devuelve cuanto no pudo agarrar
     * Disminuir es saca energia y devuelve cuanto consumio
     */
    public EnergiaCoin Recibir(EnergiaCoin energia)
    {
        float alfaAAgregar = EnergiaAAtributo(energia);
        float alfaPosible = Mathf.Min(maximo - m_alfa, alfaAAgregar);

        NuevoValor(m_alfa + alfaPosible);

        return AtributoAEnergia(alfaAAgregar - alfaPosible);
    }

    public EnergiaCoin EnergiaCapazDeRecibir(EnergiaCoin energiaDeseada)
    {
        EnergiaCoin capacidadMaxima = AtributoAEnergia(Mathf.Max(minimo, maximo - AlfaValor));
        return capacidadMaxima.MenorEnergia(energiaDeseada);
    }

    public EnergiaCoin Dar(EnergiaCoin energia)
    {
        float alfaASacar = EnergiaAAtributo(energia);
        alfaASacar = Mathf.Min(m_alfa, alfaASacar);

        NuevoValor(m_alfa - alfaASacar);

        return AtributoAEnergia(alfaASacar);
    }

    public EnergiaCoin EnergiaCapazDeDar(EnergiaCoin energiaDeseada)
    {
        EnergiaCoin capacidadMaxima = AtributoAEnergia(AlfaValor);
        return capacidadMaxima.MenorEnergia(energiaDeseada);
    }

    public float EnergiaAAtributo(EnergiaCoin energia)
    {
        float porcentajeEnergia = energia.EnergiaActualInterpolada();
        float conversion = PorcentajeDeEnergiaAAlfa(porcentajeEnergia); 
        return Mathf.Lerp(minimo, maximo, conversion);
    }

    public EnergiaCoin AtributoAEnergia(float alfa)
    {
        float alfaRelativa = alfa / maximo;
        EnergiaCoin energia = new EnergiaCoin();
        energia.ValorActualEnergia(PorcentajeDeAlfaAEnergia(alfaRelativa));
        return energia;
    }

    private float PorcentajeDeAlfaAEnergia(float porcentajeDeAlfa)
    {
        return porcentajeDeAlfa * m_costo;
    }

    private float PorcentajeDeEnergiaAAlfa(float porcentajeDeEnergia)
    {
        return porcentajeDeEnergia / m_costo;
    }
}
