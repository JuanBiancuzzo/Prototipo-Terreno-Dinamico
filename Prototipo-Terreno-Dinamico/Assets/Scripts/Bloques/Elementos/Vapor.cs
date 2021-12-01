using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vapor : Gaseoso
{
    public Vapor(Vector3Int posicion, IConetenedorGeneral mundo) : base(posicion, mundo)
    {
        m_color = new Color(0.1f, 0.1f, 0.1f, 1);
        id = 4;
    }

    public override Elemento Expandir(Vector3Int posicion)
    {
        Vapor aireNueva = new Vapor(posicion, m_mundo);
        base.DividirAtributos(aireNueva);
        return aireNueva;
    }

    public override void Reaccionar()
    {
        if (m_temperatura.Valor() >= 373)
            return;

        Agua agua = new Agua(m_posicion, m_mundo);
        IgualarAtributos(agua);
        m_mundo.Reemplazar(this, agua);
    }
}
