using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Liquido : Elemento
{
    protected int m_ficDinamica, m_ficEstatica;

    protected Liquido(Vector3Int posicion) : base(posicion)
    {
        m_ficDinamica = 2;
        m_ficEstatica = 4;
    }
}
