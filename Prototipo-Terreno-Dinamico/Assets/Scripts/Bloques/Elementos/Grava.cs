using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grava : Solido
{
    public Grava(Vector3Int posicion) : base(posicion)
    {
        m_valor = 1f;
        m_color = new Color(0.25f, 0.25f, 0.25f, 1);
    }

    public override void ActuarEnOtro(Solido elemento, int dt)
    {
    }

    public override void Avanzar(IContenedorConDatos mapa, int dt)
    {
        ActualizarVelocidad(dt);
        Vector3Int direccion = Direccion(dt);
        Vector3Int inicio = m_posicion;
        Vector3Int final = inicio + m_velocidad * dt;

        foreach (Vector3Int posicion in Mathfs.PosicioneEntreYield(inicio, final))
        {
            Vector3Int posicionAnterior = m_posicion;

            Elemento elemento = (Elemento)mapa.EnPosicion(posicion);
            if (elemento == null)
            {
                mapa.Intercambiar(m_posicion, posicion);
                ActualizarAlrededores(posicionAnterior, direccion, mapa);
                continue;
            }

            elemento.ActuanEnElemento(this, dt);

            bool puedoIntercambiar = elemento.PermitoIntercambiar(this, dt);
            if (puedoIntercambiar)
                puedoIntercambiar = mapa.Intercambiar(m_posicion, posicion);

            if (!puedoIntercambiar)
                break;

            ActualizarAlrededores(posicionAnterior, direccion, mapa);
        }

        if (Reacciona(mapa))
            NecesitoActualizar(this);
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
