using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hielo : Solido
{
    public Hielo(Vector3Int posicion, IConetenedorGeneral mundo) : base(posicion, mundo)
    {
        m_color = new Color(1, 1, 1, 1);
        id = 3;
        m_densidad = 100;
    }

    public override void Avanzar(int dt)
    {
    }

    public override Elemento Expandir(Vector3Int posicion)
    {
        Hielo concretoNueva = new Hielo(posicion, m_mundo);
        base.DividirAtributos(concretoNueva);
        return concretoNueva;
    }

    public override void Reaccionar()
    {
        if (m_temperatura.Valor() <= 273)
            return;

        Agua agua = new Agua(m_posicion, m_mundo);
        IgualarAtributos(agua);
        m_mundo.Reemplazar(this, agua);
    }
}
