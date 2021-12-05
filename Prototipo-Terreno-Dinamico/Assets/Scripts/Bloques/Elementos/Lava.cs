using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : Liquido
{
    int temperaturaMaxima = 400;

    public Lava(Vector3Int posicion, Mundo mundo) : base(posicion, mundo)
    {
        NuevoColor(new Color(0.72f, 0.15f, 0.16f, 0.80f));
        m_iluminacion.NuevoValor(m_maximoLuz);
        m_flowRate = 10;
        m_temperatura.NuevoValor(temperaturaMaxima);
    }

    public override Elemento Expandir(Vector3Int posicion)
    {
        Lava lavaNueva = new Lava(posicion, m_mundo);
        base.DividirAtributos(lavaNueva);
        return lavaNueva;
    }

    public override void Reaccionar()
    {
        float t = Mathf.InverseLerp(0, temperaturaMaxima, TemperaturaValor);
        int nuevaIluminacion = Mathf.RoundToInt(Mathf.Lerp(m_minimoLuz, m_maximoLuz, t));
        m_temperatura.NuevoValor(Mathf.Max(0, TemperaturaValor - 1));
        m_iluminacion.NuevoValor(nuevaIluminacion);
    }

    public override bool Emisor()
    {
        return true;
    }
}