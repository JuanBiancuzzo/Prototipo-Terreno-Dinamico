using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingSand : MonoBehaviour
{
    public IConetenedorGeneral m_mapa = null;
    List<Elemento> m_paraActualizar = new List<Elemento>();

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
        /*List<Elemento> actualizarEstaIteracion = new List<Elemento>();
        foreach (Elemento elemento in m_paraActualizar)
            actualizarEstaIteracion.Add(elemento);
        m_paraActualizar.Clear(); */

        foreach (Elemento elemento in m_mapa.ElementoParaActualizar())
            elemento.Avanzar(dt);

        Renderizar();
    }

    public void Renderizar()
    {
        m_mapa.Renderizar(render);
    }

    public void GenerarMeshColision(Extremo rangoJugador)
    {
        //m_mapa.GenerarMeshColision(render, rangoJugador);
    }

    /*public void AgregarJugador(Transform jugador, List<int> LODLevels)
    {
        m_mapa.AgregarJugador(jugador, LODLevels);
    }*/

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

        ContenibleNecesitaActualizarse(elemento);
        return true;
        //elemento.necesitoActualizar += ContenibleNecesitaActualizarse;
    }

    private void ContenibleNecesitaActualizarse(Elemento elemento)
    {
        AgregarSinRepetir(m_paraActualizar, elemento);
    }

    private void AgregarSinRepetir<T>(List<T> lista, T elemento)
    {
        foreach (T e in lista)
            if (e.Equals(elemento))
                return;
        lista.Add(elemento);
    }

    public static bool EsDefault(Color color)
    {
        return color == m_defaultColor;
    }
}
