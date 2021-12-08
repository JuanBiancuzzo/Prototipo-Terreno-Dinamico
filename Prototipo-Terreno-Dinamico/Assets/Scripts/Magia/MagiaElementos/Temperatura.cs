using UnityEngine;

[System.Serializable]
public class Temperatura : IEnergia
{
    static int minimo = 0, maximo = 3000;
    int m_conductividad = 50;
    private float Costo => (m_conductividad * 10f) / maximo;
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

        return m_temperatura.AtributoAEnergia(temperaturaAAgregar - temperaturaPosible, energia);
    }

    public EnergiaCoin EnergiaCapazDeRecibir(EnergiaCoin energiaDeseada)
    {
        EnergiaCoin capacidadMaxima = m_temperatura.AtributoAEnergia(Mathf.Max(minimo, maximo - TemperaturaValor));
        return capacidadMaxima.MenorEnergia(energiaDeseada);
    }

    public EnergiaCoin Disminuir(EnergiaCoin energia)
    {
        int temperaturaASacar = m_temperatura.EnergiaAAtributo(energia);
        temperaturaASacar = Mathf.Min(m_temperatura.Valor, temperaturaASacar);

        m_temperatura.Disminuir(temperaturaASacar);

        return m_temperatura.AtributoAEnergia(temperaturaASacar, energia);
    }
    public EnergiaCoin EnergiaCapazDeDar(EnergiaCoin energiaDeseada)
    {
        EnergiaCoin capacidadMaxima = m_temperatura.AtributoAEnergia(TemperaturaValor);
        return capacidadMaxima.MenorEnergia(energiaDeseada);
    }

    public float IluminacionPorTemperatrua()
    { 
        if (TemperaturaValor < 800)
            return 0;
        if (TemperaturaValor > 1000)
            return 100;
        return (TemperaturaValor - 800) / 2f;
    }
}
