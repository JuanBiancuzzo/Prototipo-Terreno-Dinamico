using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arena : Solido
{
    public Arena(Vector3Int posicion, IConetenedorGeneral mundo) : base(posicion, mundo)
    {
        m_color = new Color(1, 0.88f, 0.29f, 1);
        id = 0;
    }

    public override Elemento Expandir(Vector3Int posicion)
    {
        Arena arenaNueva = new Arena(posicion, m_mundo);
        arenaNueva.m_densidad = m_densidad / 2;
        m_densidad /= 2;
        return arenaNueva;
    }

    public override void Reaccionar()
    {
    }    
}
