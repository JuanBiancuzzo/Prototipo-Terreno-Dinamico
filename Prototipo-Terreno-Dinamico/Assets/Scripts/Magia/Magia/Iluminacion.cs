using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iluminacion : IEnergia
{
    static int minimo = 0, maximo = 15;
    AtributoInt m_iluminacion;

    public int IluminacionValor => m_iluminacion.Valor;
    public void NuevoValor(int valor) => m_iluminacion.NuevoValor(valor);
    public void Aumentar(int cantidad) => m_iluminacion.Aumentar(cantidad);
    public void Disminuir(int cantidad) => m_iluminacion.Disminuir(cantidad);

    public Iluminacion(int iluminacion)
    {
        m_iluminacion = new AtributoInt(iluminacion, minimo, maximo);
    }

    /*
     * Disminuir es saca energia y devuelve cuanto consumio
     * Aumentar es dar energia y devuelve cuanto no pudo agarrar
     */
    public EnergiaCoin Aumentar(EnergiaCoin energia)
    {
        int temperaturaAAgregar = AtributoInt.EnergiaAAtributo(energia);
        int temperaturaPosible = Mathf.Min(maximo - m_iluminacion.Valor, temperaturaAAgregar);

        m_iluminacion.Aumentar(temperaturaPosible);

        return AtributoInt.AtributoAEnergia(temperaturaAAgregar - temperaturaPosible, energia);
    }

    public EnergiaCoin Disminuir(EnergiaCoin energia)
    {
        int temperaturaASacar = AtributoInt.EnergiaAAtributo(energia);
        temperaturaASacar = Mathf.Min(m_iluminacion.Valor, temperaturaASacar);

        m_iluminacion.Disminuir(temperaturaASacar);

        return AtributoInt.AtributoAEnergia(temperaturaASacar, energia);
    }
}
