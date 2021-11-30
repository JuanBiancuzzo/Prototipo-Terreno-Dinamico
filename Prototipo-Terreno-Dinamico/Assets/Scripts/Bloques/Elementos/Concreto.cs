using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Concreto : Solido
{
    public Concreto (Vector3Int posicion, IConetenedorGeneral mundo) : base (posicion, mundo)
    {
        m_color = new Color(0.75f, 0.75f, 0.75f, 1);
        id = 2;
    }

    public override void Avanzar(int dt)
    {
    }

    public override Elemento Expandir(Vector3Int posicion)
    {
        Concreto arenaNueva = new Concreto(posicion, m_mundo);
        arenaNueva.m_densidad = m_densidad / 2;
        m_densidad /= 2;
        return arenaNueva;
    }

    public override void Reaccionar()
    {
    }
}
