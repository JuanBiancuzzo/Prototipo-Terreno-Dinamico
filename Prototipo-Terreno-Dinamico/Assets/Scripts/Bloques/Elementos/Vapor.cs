using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vapor : Gaseoso
{
    public Vapor(Vector3Int posicion, IContenedorGeneral mundo) : base(posicion, mundo)
    {
        NuevoColor(new Color(0.1f, 0.1f, 0.1f, 0.5f));
    }

    public override Elemento Expandir(Vector3Int posicion)
    {
        Vapor aireNueva = new Vapor(posicion, m_mundo);
        base.DividirAtributos(aireNueva);
        return aireNueva;
    }

    public override void Reaccionar()
    {
        if (TemperaturaValor >= 373)
            return;

        Agua agua = new Agua(m_posicion, m_mundo);
        IgualarAtributos(agua);
        m_mundo.Reemplazar(this, agua);
    }
}
