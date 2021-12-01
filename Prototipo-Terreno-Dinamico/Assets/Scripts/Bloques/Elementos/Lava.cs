using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : Liquido
{
    public Lava(Vector3Int posicion, IConetenedorGeneral mundo) : base(posicion, mundo)
    {
        m_color = new Color(0.72f, 0.15f, 0.16f, 0.80f);
        m_iluminacion = new ValorTemporal(m_maximoLuz);
        id = 6;
        m_flowRate = 10;
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

    public override bool Emisor()
    {
        return true;
    }
}