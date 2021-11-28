using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Concreto : Solido
{
    public Concreto(Vector3Int posicion) : base(posicion)
    {
        m_densidad = 100;
        m_color = new Color(.75f, .75f, .75f, 1);
    }

    public override bool Reacciona(IContenedorConDatos mapa)
    {
        return false;
    }

    protected override IEnumerable<Vector3Int> PosicionesEnMovimiento(IContenedorConDatos mapa, int dt)
    {
        yield return m_posicion;
    }
}
