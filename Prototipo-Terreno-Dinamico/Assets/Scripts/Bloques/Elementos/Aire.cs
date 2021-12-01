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
        Aire aireNueva = new Aire(posicion, m_mundo);
        base.DividirAtributos(aireNueva);
        return aireNueva;
    }

    public override bool Visible()
    {
        return false;
    }

    public override void Reaccionar()
    {
    }
}
