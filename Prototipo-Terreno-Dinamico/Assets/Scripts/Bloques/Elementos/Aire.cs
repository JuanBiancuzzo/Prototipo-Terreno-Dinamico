using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aire : Gaseoso
{
    public Aire(Vector3Int posicion, IConetenedorGeneral mundo) : base(posicion, mundo)
    {
        m_color = new Color(1, 1, 1, 1);
        id = 1;
    }

    public override Elemento Expandir(Vector3Int posicion)
    {
        Aire arenaNueva = new Aire(posicion, m_mundo);
        arenaNueva.m_densidad = m_densidad / 2;
        m_densidad /= 2;
        return arenaNueva;
    }

    public override bool Visible()
    {
        return false;
    }

    public override void Reaccionar()
    {
    }
}
