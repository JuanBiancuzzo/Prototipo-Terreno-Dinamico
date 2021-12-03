using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alfa : IEnergia
{
    static float minimo = 0, maximo = 1;
    AtributoFloat m_alfa;

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
        float temperaturaAAgregar = AtributoFloat.EnergiaAAtributo(energia);
        float temperaturaPosible = Mathf.Min(maximo - m_alfa.Valor, temperaturaAAgregar);

        m_alfa.Aumentar(temperaturaPosible);

        return AtributoFloat.AtributoAEnergia(temperaturaAAgregar - temperaturaPosible, energia);
    }

    public EnergiaCoin Disminuir(EnergiaCoin energia)
    {
        float temperaturaASacar = AtributoFloat.EnergiaAAtributo(energia);
        temperaturaASacar = Mathf.Min(m_alfa.Valor, temperaturaASacar);

        m_alfa.Disminuir(temperaturaASacar);

        return AtributoFloat.AtributoAEnergia(temperaturaASacar, energia);
    }
}
