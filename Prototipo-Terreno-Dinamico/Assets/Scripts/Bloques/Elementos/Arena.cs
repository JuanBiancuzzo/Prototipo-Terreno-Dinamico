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
        // modificar su velocidad para q se caiga a los costados
        foreach (Vector3Int desfase in BuscarPosicionesDisponibles())
        {
            m_velocidad += desfase;
            break;
        }
    }

    public override bool Reacciona(IContenedorConDatos mapa)
    {
        foreach (Vector3Int desfase in BuscarPosicionesDisponibles())
            if (ElementoDejaIntercambiarEn(mapa, m_posicion + desfase + Vector3Int.down, 1))
                return true;
        return false;
    }
}
