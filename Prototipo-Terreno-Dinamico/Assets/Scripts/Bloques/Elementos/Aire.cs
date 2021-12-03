using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aire : Gaseoso
{
    public Aire(Vector3Int posicion, IConetenedorGeneral mundo) : base(posicion, mundo)
    {
        NuevoColor(new Color(0, 0, 0, 0));
    }

    public override Elemento Expandir(Vector3Int posicion)
    {
        Aire aireNueva = new Aire(posicion, m_mundo);
        base.DividirAtributos(aireNueva);
        return aireNueva;
    }

    public override void Avanzar(int dt)
    {
        base.Avanzar(dt);
    }

    public override void Reaccionar()
    {
    }
}
