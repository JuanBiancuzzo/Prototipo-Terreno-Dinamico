using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : Liquido
{
    int temperaturaMaxima = 800;

    public Lava(Vector3Int posicion, Mundo mundo) : base(posicion, mundo)
    {
        NuevoColor(new Color(0.72f, 0.15f, 0.16f, 0.80f));
        m_flowRate = 10;
        m_temperatura.NuevoValor(temperaturaMaxima);
        NuevaConductividad(10);
    }

    public override Elemento Expandir(Vector3Int posicion)
    {
        Lava lavaNueva = new Lava(posicion, m_mundo);
        base.DividirAtributos(lavaNueva);
        return lavaNueva;
    }

    public override void Reaccionar()
    {
    }
}