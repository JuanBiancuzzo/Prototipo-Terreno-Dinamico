using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Concreto : Solido
{
    public Concreto (Vector3Int posicion, IConetenedorGeneral mundo) : base (posicion, mundo)
    {
        m_color = new Color(0.75f, 0.75f, 0.75f, 1);
        id = 2;
        m_densidad = 100;
    }

    public override void Avanzar(int dt)
    {
    }

    public override Elemento Expandir(Vector3Int posicion)
    {
        Concreto concretoNueva = new Concreto(posicion, m_mundo);
        base.DividirAtributos(concretoNueva);
        return concretoNueva;
    }

    public override void Reaccionar()
    {
    }
}
