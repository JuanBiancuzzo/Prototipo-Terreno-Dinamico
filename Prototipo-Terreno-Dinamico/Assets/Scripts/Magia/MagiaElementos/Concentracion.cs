using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Concentracion : IEnergia
{
    static int minimo = 0, maximo = 100;
    static float m_costeParaAgregar = 3f;
    static float m_costeParaSacar = 5f;
    [SerializeField] AtributoInt m_concentracion;

    public int ConcentracionValor => m_concentracion.Valor;
    public void NuevoValor(int valor)
    {
        m_concentracion.NuevoValor(valor);
    }
    public void Aumentar(int cantidad)
    {
        m_concentracion.Aumentar(cantidad);
    }
    public void Disminuir(int cantidad)
    {
        m_concentracion.Disminuir(cantidad);
    }

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
        int concentracionAAgregar = m_concentracion.EnergiaAAtributo(energia);
        int concentracionPosible = Mathf.Min(maximo - m_concentracion.Valor, concentracionAAgregar);

        m_concentracion.Aumentar(concentracionPosible);

        return m_concentracion.AtributoAEnergia(concentracionAAgregar - concentracionPosible, energia, m_costeParaAgregar);
    }

    public EnergiaCoin Disminuir(EnergiaCoin energia)
    {
        int concentracionASacar = m_concentracion.EnergiaAAtributo(energia);
        concentracionASacar = Mathf.Min(m_concentracion.Valor, concentracionASacar);

        m_concentracion.Disminuir(concentracionASacar);

        return m_concentracion.AtributoAEnergia(concentracionASacar, energia, m_costeParaSacar);
    }
}
