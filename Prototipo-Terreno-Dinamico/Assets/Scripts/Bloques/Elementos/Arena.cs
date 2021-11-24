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

    public override void ActuarEnOtro(Solido elemento, int dt)
    {
        /*Vector3Int direccion = elemento.m_posicion - m_posicion;

        float magnitud = ((Vector3)m_velocidad).magnitude;
        if (Random.Range(0, 99) > 50)
            magnitud *= -1;

        Vector3 nuevaVelocidad = Vector3.one * magnitud;
        for (int i = 0; i < 3; i++)
            nuevaVelocidad[i] *= (direccion[i] == 0 && Random.Range(0, 99) > 50) ? 1 : 0;

        elemento.m_velocidad = Vector3Int.RoundToInt(nuevaVelocidad * (2f / 3f));
        m_velocidad = Vector3Int.RoundToInt(-nuevaVelocidad * (2f / 3f));*/
    }

    public override bool Reacciona(IContenedorConDatos mapa)
    {
        Vector3Int abajo = m_posicion + Vector3Int.down;
        bool puedoMovermeAbajo = mapa.EnRango(abajo);
        if (!puedoMovermeAbajo)
            return false;

        if (mapa.EnPosicion(abajo) != null)
            return false;

        return true;
    }
}
