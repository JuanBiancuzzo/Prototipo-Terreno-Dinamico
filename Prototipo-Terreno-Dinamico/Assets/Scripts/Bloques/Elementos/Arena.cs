using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arena : Solido
{
    public Arena(Vector3Int posicion, IConetenedorGeneral mundo) : base(posicion, mundo)
    {
        m_color = new Color(1, 0.88f, 0.29f, 1);
        id = 0;
        m_concentracion = 5;
    }

    public override Elemento Expandir(Vector3Int posicion)
    {
        Arena arenaNueva = new Arena(posicion, m_mundo);
        base.DividirAtributos(arenaNueva);
        return arenaNueva;
    }

    public override void Reaccionar()
    {
    }    
}
