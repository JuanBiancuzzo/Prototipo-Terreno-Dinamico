using UnityEngine;

[System.Serializable]
public class Temperatura : IEnergia
{
    static int minimo = 0, maximo = 1000;
    [SerializeField] AtributoInt m_temperatura = null;

    public int TemperaturaValor => m_temperatura.Valor;
    public void NuevoValor(int valor) => m_temperatura.NuevoValor(valor);
    public void Aumentar(int cantidad) => m_temperatura.Aumentar(cantidad);
    public void Disminuir(int cantidad) => m_temperatura.Disminuir(cantidad);

    public Temperatura(int temperatura)
    {
        m_temperatura = new AtributoInt(temperatura, minimo, maximo);
    }


    /*
     * Disminuir es saca energia y devuelve cuanto consumio
     * Aumentar es dar energia y devuelve cuanto no pudo agarrar
     */

    public EnergiaCoin Aumentar(EnergiaCoin energia)
    {
        int temperaturaAAgregar = AtributoInt.EnergiaAAtributo(energia);
        int temperaturaPosible = Mathf.Min(maximo - m_temperatura.Valor, temperaturaAAgregar);

        m_temperatura.Aumentar(temperaturaPosible);

        return AtributoInt.AtributoAEnergia(temperaturaAAgregar - temperaturaPosible, energia);
    }

    public EnergiaCoin Disminuir(EnergiaCoin energia)
    {
        int temperaturaASacar = AtributoInt.EnergiaAAtributo(energia);
        temperaturaASacar = Mathf.Min(m_temperatura.Valor, temperaturaASacar);

        m_temperatura.Disminuir(temperaturaASacar);

        return AtributoInt.AtributoAEnergia(temperaturaASacar, energia);
    }

    public float IluminacionPorTemperatrua()
    {
        if (TemperaturaValor < 200)
            return TemperaturaValor / 20f;
        if (TemperaturaValor > 800)
            return (TemperaturaValor - 800) / 20f + 90;
        return (2 * TemperaturaValor) / 15f - 50f / 3f;
    }
}
