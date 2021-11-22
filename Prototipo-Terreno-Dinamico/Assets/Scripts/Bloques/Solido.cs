using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Solido : Elemento
{
    protected Vector3Int m_estabilidad;
    protected int m_ficDinamica, m_ficEstatica, m_filtracion;

    protected Solido(Vector3Int posicion) : base(posicion)
    {
        m_estabilidad = Vector3Int.zero;
        m_ficDinamica = 2;
        m_ficEstatica = 4;
        m_filtracion = 1;
    }
}
