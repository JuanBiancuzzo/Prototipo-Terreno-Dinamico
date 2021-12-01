using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingSand : MonoBehaviour
{
    public IConetenedorGeneral m_mapa = null;

    public static float m_default = 0f;
    public static Color m_defaultColor = new Color(1f, 1f, 1f, 1f);

    public MarchingCubes render;
    public int dt = 1;

    public int m_velocidadSimulacion = 5;

    int contador = 0;
    void FixedUpdate()
    {
        if (contador == 0)
            Avanzar();

        contador = (contador + 1) % m_velocidadSimulacion;
    }

    public void Avanzar()
    {
        foreach (Elemento elemento in m_mapa.ElementoParaActualizar())
            if (!elemento.EstaActualizado())
            {
                elemento.Actuar(dt);
                elemento.Actualizado();
            }

        Renderizar();

        foreach (Elemento elemento in m_mapa.ElementoParaActualizar())
        {
            elemento.EmpezarAActualizar();
            elemento.Reaccionar();
        }
    }

    public void Renderizar()
    {
        m_mapa.Renderizar(render);
    }

    public void GenerarMeshColision(Extremo rangoJugador)
    {
        //m_mapa.GenerarMeshColision(render, rangoJugador);
    }


    public Vector3 PosicionEnMundo(Vector3 posicion)
    {
        return posicion - transform.position;
    }

    public bool Insertar(Elemento elemento)
    {
        if (m_mapa == null)
            return false;

        bool sePudoInsertar = m_mapa.Insertar(elemento);
        if (!sePudoInsertar)
            return false;

        return true;
    }

    public static bool EsDefault(Color color)
    {
        return color == m_defaultColor;
    }
}
