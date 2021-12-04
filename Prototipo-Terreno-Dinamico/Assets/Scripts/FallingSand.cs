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
            elemento.AntesDeAvanzar();

        foreach (Elemento elemento in m_mapa.ElementoParaActualizar())
            if (!elemento.EstaActualizado())
            {
                elemento.Avanzar(dt);
                elemento.Actualizado();
            }

        foreach (Elemento elemento in m_mapa.ElementoParaActualizar())
            elemento.DespuesDeAvanzar();

        m_mapa.CalcularIluminacion();
        Renderizar();
    }

    public void Renderizar()
    {
        m_mapa.Renderizar(render);
    }

    public void GenerarMeshColision(Extremo rangoEntidad, Constitucion entidad)
    {
        m_mapa.GenerarMeshColision(render, rangoEntidad, entidad);
    }

    public void SeleccionarElemento(Vector3Int posicion)
    {
        m_mapa.SeleccionarElemento(render, posicion);
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


    public ElementoMagico DarElementoMagico(Vector3 posicion)
    {
        return m_mapa.EnPosicion(Vector3Int.FloorToInt(posicion));
    }
}
