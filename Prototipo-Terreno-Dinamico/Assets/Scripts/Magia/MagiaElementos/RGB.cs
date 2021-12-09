using UnityEngine;

[System.Serializable] 
public class RGB : IEnergia
{
    static float minimo = 0, maximo = 1;
    [SerializeField] float m_hue, m_intensidad;
    static float m_costo = 1f;

    public Color RGBValor => Color.HSVToRGB(m_hue, m_intensidad, m_intensidad);
    public void NuevoValor(Color color)
    {
        float s, v;
        Color.RGBToHSV(color, out m_hue, out s, out v);
        m_intensidad = (s + v) / 2;
    }

    public void NuevaIntensidad(float valor) => m_intensidad = Mathf.Min(maximo, Mathf.Max(minimo, valor)); 

    public RGB(Color color)
    {
        NuevoValor(color);
    }

    /*
     * Disminuir es saca energia y devuelve cuanto consumio
     * Aumentar es dar energia y devuelve cuanto no pudo agarrar
     */
    public EnergiaCoin Recibir(EnergiaCoin energia)
    {
        float intensidadAAgregar = EnergiaAAtributo(energia);
        float intensidadPosible = Mathf.Min(maximo - m_intensidad, intensidadAAgregar);

        NuevaIntensidad(m_intensidad + intensidadPosible);

        return AtributoAEnergia(intensidadAAgregar - intensidadPosible);
    }

    public EnergiaCoin EnergiaCapazDeRecibir(EnergiaCoin energiaDeseada)
    {
        EnergiaCoin capacidadMaxima = AtributoAEnergia(Mathf.Max(minimo, maximo - m_intensidad));
        return capacidadMaxima.MenorEnergia(energiaDeseada);
    }

    public EnergiaCoin Dar(EnergiaCoin energia)
    {
        float intensidadASacar = EnergiaAAtributo(energia);
        intensidadASacar = Mathf.Min(m_intensidad, intensidadASacar);

        NuevaIntensidad(m_intensidad - intensidadASacar);

        return AtributoAEnergia(intensidadASacar);
    }

    public EnergiaCoin EnergiaCapazDeDar(EnergiaCoin energiaDeseada)
    {
        EnergiaCoin capacidadMaxima = AtributoAEnergia(m_intensidad);
        return capacidadMaxima.MenorEnergia(energiaDeseada);
    }

    public float EnergiaAAtributo(EnergiaCoin energia)
    {
        float porcentajeEnergia = energia.EnergiaActualInterpolada();
        float conversion = PorcentajeDeEnergiaAColor(porcentajeEnergia);
        return Mathf.Lerp(minimo, maximo, conversion);
    }

    public EnergiaCoin AtributoAEnergia(float color)
    {
        float colorRelativa = color / maximo;
        EnergiaCoin energia = new EnergiaCoin();
        energia.ValorActualEnergia(PorcentajeDeColorAEnergia(colorRelativa));
        return energia;
    }

    private float PorcentajeDeColorAEnergia(float porcentajeDeColor)
    {
        return porcentajeDeColor * m_costo;
    }

    private float PorcentajeDeEnergiaAColor(float porcentajeDeEnergia)
    {
        return porcentajeDeEnergia / m_costo;
    }
}
