using UnityEngine;

[System.Serializable] // plantearme si se puede hacer HVS en vez de RGB
public class RGB : IEnergia
{
    static float minimo = 0, maximo = 15;
    [SerializeField] AtributoFloat r, g, b;

    public Vector3 RGBValor => new Vector3(r.Valor, g.Valor, b.Valor);
    public void NuevoValor(Color color)
    {
        r.NuevoValor(color.r);
        g.NuevoValor(color.g);
        b.NuevoValor(color.b);
    }

    public RGB(Vector3 valor)
    {
        r = new AtributoFloat(valor.x, minimo, maximo);
        g = new AtributoFloat(valor.y, minimo, maximo);
        b = new AtributoFloat(valor.z, minimo, maximo);
    }

    /*
     * Disminuir es saca energia y devuelve cuanto consumio
     * Aumentar es dar energia y devuelve cuanto no pudo agarrar
     */
    public EnergiaCoin Recibir(EnergiaCoin energia)
    {
        float energiaPorColor = r.EnergiaAAtributo(energia);
        Color color = new Color(r.Valor, g.Valor, b.Valor);
        float H, S, V;
        Color.RGBToHSV(color, out H, out S, out V);

        float energiaAAgregar = Mathf.Min(1, V + energiaPorColor);
        V += energiaAAgregar;
        S += Mathf.Min(1, S + energiaPorColor);

        color = Color.HSVToRGB(H, S, V);
        AtributoFloat[] rgb = { r, g, b };
        for (int i = 0; i < 3; i++)
            rgb[i].NuevoValor(color[i]);

        float energiaRestanteTotal = Mathf.Min((V + energiaPorColor) - 1, 0);
        return r.AtributoAEnergia(energiaRestanteTotal, energia);
    }

    public EnergiaCoin Dar(EnergiaCoin energia)
    {
        float energiaPorColor = r.EnergiaAAtributo(energia);

        Color color = new Color(r.Valor, g.Valor, b.Valor);
        float H, S, V;
        Color.RGBToHSV(color, out H, out S, out V);

        float energiaAAgregar = Mathf.Min(V, energiaPorColor);
        V -= energiaAAgregar;
        S -= Mathf.Min(S, energiaPorColor);

        color = Color.HSVToRGB(H, S, V);
        AtributoFloat[] rgb = { r, g, b };
        for (int i = 0; i < 3; i++)
            rgb[i].NuevoValor(color[i]);

        return r.AtributoAEnergia(energiaAAgregar, energia);
    }

    public EnergiaCoin EnergiaCapazDeDar(EnergiaCoin energiaDeseada)
    {
        float H, S, V;
        Color color = new Color(r.Valor, g.Valor, b.Valor);
        Color.RGBToHSV(color, out H, out S, out V);

        EnergiaCoin capacidadMaxima = r.AtributoAEnergia(V);
        return capacidadMaxima.MenorEnergia(energiaDeseada);
    }

    public EnergiaCoin EnergiaCapazDeRecibir(EnergiaCoin energiaDeseada)
    {
        float H, S, V;
        Color color = new Color(r.Valor, g.Valor, b.Valor);
        Color.RGBToHSV(color, out H, out S, out V);

        EnergiaCoin capacidadMaxima = r.AtributoAEnergia(Mathf.Max(minimo, maximo - V));
        return capacidadMaxima.MenorEnergia(energiaDeseada);
    }
}
