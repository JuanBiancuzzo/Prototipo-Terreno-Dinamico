using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aire : Gaseoso
{
    public Aire(Vector3Int posicion) : base(posicion)
    {
        m_valor = 0f;
        m_color = new Color(0, 0, 0, 0);
    }
}