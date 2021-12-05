using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agua : Liquido
{
    public Agua(Vector3Int posicion, Mundo mundo) : base(posicion, mundo)
    {
        NuevoColor(new Color(0.12f, 0.56f, 1, 0.5f));
    }

    public override Elemento Expandir(Vector3Int posicion)
    {
        Agua aireNueva = new Agua(posicion, m_mundo);
        base.DividirAtributos(aireNueva);
        return aireNueva;
    }

    public override void Reaccionar()
    {
        Elemento elemento = null;

        if (TemperaturaValor > 373)
            elemento = new Vapor(m_posicion, m_mundo);
        else if (TemperaturaValor < 273)
            elemento = new Hielo(m_posicion, m_mundo);

        if (elemento == null)
            return;

        IgualarAtributos(elemento);
        m_mundo.Reemplazar(this, elemento);
    }
}
