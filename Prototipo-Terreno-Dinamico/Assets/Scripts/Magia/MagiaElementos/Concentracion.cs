using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Concentracion : IEnergia
{
    static int minimo = 0, maximo = 100;
    AtributoInt m_concentracion;

    public int ConcentracionValor => m_concentracion.Valor;
    public void NuevoValor(int valor) => m_concentracion.NuevoValor(valor);
    public void Aumentar(int cantidad) => m_concentracion.Aumentar(cantidad);
    public void Disminuir(int cantidad) => m_concentracion.Disminuir(cantidad);

    public Concentracion(int concentracion)
    {
        m_concentracion = new AtributoInt(concentracion, minimo, maximo);
    }

    /*
     * Disminuir es saca energia y devuelve cuanto consumio
     * Aumentar es dar energia y devuelve cuanto no pudo agarrar
     */
    public EnergiaCoin Aumentar(EnergiaCoin energia)
    {
        int iluminacionAAgregar = AtributoInt.EnergiaAAtributo(energia);
        int iluminacionPosible = Mathf.Min(maximo - m_concentracion.Valor, iluminacionAAgregar);

        m_concentracion.Aumentar(iluminacionPosible);

        return AtributoInt.AtributoAEnergia(iluminacionAAgregar - iluminacionPosible, energia);
    }

    public EnergiaCoin Disminuir(EnergiaCoin energia)
    {
        int iluminacionASacar = AtributoInt.EnergiaAAtributo(energia);
        iluminacionASacar = Mathf.Min(m_concentracion.Valor, iluminacionASacar);

        m_concentracion.Disminuir(iluminacionASacar);

        return AtributoInt.AtributoAEnergia(iluminacionASacar, energia);
    }
}
