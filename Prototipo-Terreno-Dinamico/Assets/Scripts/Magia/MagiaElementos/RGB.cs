using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RGB : IEnergia
{
    static float minimo = 0, maximo = 15;
    AtributoFloat r, g, b;

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

    public EnergiaCoin Aumentar(EnergiaCoin energia)
    {
        float energiaPorColor = AtributoFloat.EnergiaAAtributo(energia);

        List<AtributoFloat> rgb = new List<AtributoFloat> { r, g, b };

        float energiaRestanteTotal = 0;
        foreach (AtributoFloat a in rgb)
        {
            float energiaAAgregarPorColor = Mathf.Min(maximo - a.Valor, energiaPorColor);
            a.Aumentar(energiaAAgregarPorColor);
            energiaRestanteTotal += energiaPorColor - energiaAAgregarPorColor;
        }

        return AtributoFloat.AtributoAEnergia(energiaRestanteTotal, energia);
    }

    public EnergiaCoin Disminuir(EnergiaCoin energia)
    {
        float energiaPorColor = AtributoFloat.EnergiaAAtributo(energia);

        List<AtributoFloat> rgb = new List<AtributoFloat> { r, g, b };

        float energiaRestanteTotal = 0;
        foreach (AtributoFloat a in rgb)
        {
            float energiaAAgregarPorColor = Mathf.Min(a.Valor, energiaPorColor);
            a.Disminuir(energiaAAgregarPorColor);
            energiaRestanteTotal += energiaAAgregarPorColor;
        }

        return AtributoFloat.AtributoAEnergia(energiaRestanteTotal, energia);
    }
}
