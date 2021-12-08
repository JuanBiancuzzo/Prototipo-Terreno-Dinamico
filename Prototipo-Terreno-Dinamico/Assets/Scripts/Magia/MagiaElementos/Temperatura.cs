using UnityEngine;

[System.Serializable]
public class Temperatura : IEnergia
{
    static int minimo = 0, maximo = 1000;
    int m_conductividad = 50;
    [SerializeField] AtributoInt m_temperatura = null;

    public int TemperaturaValor => m_temperatura.Valor;
    public void NuevoValor(int valor) => m_temperatura.NuevoValor(valor);
    public void Aumentar(int cantidad) => m_temperatura.Aumentar(cantidad);
    public void Disminuir(int cantidad) => m_temperatura.Disminuir(cantidad);

    public int Conductividad => m_conductividad;
    public void NuevaConductividad(int valor) => m_conductividad = valor;

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
        int temperaturaAAgregar = m_temperatura.EnergiaAAtributo(energia);
        int temperaturaPosible = Mathf.Min(maximo - m_temperatura.Valor, temperaturaAAgregar);

        m_temperatura.Aumentar(temperaturaPosible);

        return m_temperatura.AtributoAEnergia(temperaturaAAgregar - temperaturaPosible, energia, m_conductividad / 100f);
    }

    public EnergiaCoin Disminuir(EnergiaCoin energia)
    {
        int temperaturaASacar = m_temperatura.EnergiaAAtributo(energia);
        temperaturaASacar = Mathf.Min(m_temperatura.Valor, temperaturaASacar);

        m_temperatura.Disminuir(temperaturaASacar);

        return m_temperatura.AtributoAEnergia(temperaturaASacar, energia, m_conductividad / 100f);
    }

    public float IluminacionPorTemperatrua()
    {
        if (TemperaturaValor < 100)
            return TemperaturaValor / 10f;
        if (TemperaturaValor > 700)
            return (TemperaturaValor - 700) / 30f + 90;

        //return (8 * TemperaturaValor - 1910f) / 41f;
        Debug.Log(TemperaturaValor);
        return (2 * TemperaturaValor) / 15f - 10f / 3f;
    }
}
