using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Concentracion : IEnergia
{
    static int minimo = 0, maximo = 100;
    [SerializeField] int m_concentracion;
    static float m_costo = 1f;

    public int ConcentracionValor => m_concentracion;
    public void NuevoValor(int valor) => m_concentracion = Mathf.Min(maximo, Mathf.Max(minimo, valor));
    public void Aumentar(int cantidad) => Recibir(AtributoAEnergia(cantidad));
    public void Disminuir(int cantidad) => Dar(AtributoAEnergia(cantidad));

    public Concentracion(int concentracion)
    {
        NuevoValor(concentracion);
    }

    /*
     * Disminuir es saca energia y devuelve cuanto consumio
     * Aumentar es dar energia y devuelve cuanto no pudo agarrar
     */
    public EnergiaCoin Recibir(EnergiaCoin energia)
    {
        int concentracionAAgregar = EnergiaAAtributo(energia);
        int concentracionPosible = Mathf.Min(maximo - m_concentracion, concentracionAAgregar);

        NuevoValor(m_concentracion + concentracionPosible);

        return AtributoAEnergia(concentracionAAgregar - concentracionPosible);
    }

    public EnergiaCoin EnergiaCapazDeRecibir(EnergiaCoin energiaDeseada)
    {
        EnergiaCoin capacidadMaxima = AtributoAEnergia(Mathf.Max(minimo, maximo - ConcentracionValor));
        return capacidadMaxima.MenorEnergia(energiaDeseada);
    }

    public EnergiaCoin Dar(EnergiaCoin energia)
    {
        int concentracionASacar = EnergiaAAtributo(energia);
        concentracionASacar = Mathf.Min(m_concentracion, concentracionASacar);

        NuevoValor(m_concentracion - concentracionASacar);

        return AtributoAEnergia(concentracionASacar);
    }

    public EnergiaCoin EnergiaCapazDeDar(EnergiaCoin energiaDeseada)
    {
        EnergiaCoin capacidadMaxima = AtributoAEnergia(ConcentracionValor);
        return capacidadMaxima.MenorEnergia(energiaDeseada);
    }

    public int EnergiaAAtributo(EnergiaCoin energia)
    {
        float porcentajeEnergia = energia.EnergiaActualInterpolada();
        float conversion = PorcentajeDeEnergiaAConcentracion(porcentajeEnergia);
        return Mathf.FloorToInt(Mathf.Lerp(minimo, maximo, conversion));
    }

    public EnergiaCoin AtributoAEnergia(int concentracion)
    {
        float concentracionRelativa = (float)concentracion / maximo;
        EnergiaCoin energia = new EnergiaCoin();
        energia.ValorActualEnergia(PorcentajeDeConcentracionAEnergia(concentracionRelativa));
        return energia;
    }

    private float PorcentajeDeConcentracionAEnergia(float porcentajeDeConcentracion)
    {
        return porcentajeDeConcentracion * m_costo;
    }

    private float PorcentajeDeEnergiaAConcentracion(float porcentajeDeEnergia)
    {
        return porcentajeDeEnergia / m_costo;
    }
}
