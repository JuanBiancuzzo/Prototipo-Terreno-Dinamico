using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arena : Solido
{
    public Arena(Vector3Int posicion) : base(posicion)
    {
        m_valor = 1f;
        m_color = new Color(1, 0.88f, 0.29f, 1);
    }

    protected override bool Intercambiar(Elemento elemento, int dt)
    {
        Vector3Int aceleracion = Vector3Int.RoundToInt((Vector3) elemento.m_velocidad * -1.8f);
        aceleracion += new Vector3Int(Random.Range(-3, 3), 0, Random.Range(-3, 3));

        elemento.AplicarAceleracion(aceleracion);
        return false;
    }

    protected override bool Reacciona()
    {        
        return m_cambiePosicion || !m_colisiono;
    }
}
