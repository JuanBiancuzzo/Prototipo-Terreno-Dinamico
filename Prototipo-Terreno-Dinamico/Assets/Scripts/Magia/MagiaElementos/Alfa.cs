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
     * Disminuir es saca energia y devuelve cuanto consumio
     * Aumentar es dar energia y devuelve cuanto no pudo agarrar
     */
    public EnergiaCoin Aumentar(EnergiaCoin energia)
    {
        float alfaAAgregar = AtributoFloat.EnergiaAAtributo(energia);
        float alfaPosible = Mathf.Min(maximo - m_alfa.Valor, alfaAAgregar);

        m_alfa.Aumentar(alfaPosible);

        return AtributoFloat.AtributoAEnergia(alfaAAgregar - alfaPosible, energia);
    }

    public EnergiaCoin Disminuir(EnergiaCoin energia)
    {
        float alfaASacar = AtributoFloat.EnergiaAAtributo(energia);
        alfaASacar = Mathf.Min(m_alfa.Valor, alfaASacar);

        m_alfa.Disminuir(alfaASacar);

        return AtributoFloat.AtributoAEnergia(alfaASacar, energia);
    }
}
