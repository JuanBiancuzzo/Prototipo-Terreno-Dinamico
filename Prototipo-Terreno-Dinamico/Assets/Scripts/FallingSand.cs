using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingSand : MonoBehaviour
{
    public EspacioGeneral m_mapa = null;
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
        List<Elemento> actualizarEstaIteracion = new List<Elemento>();
        foreach (Elemento elemento in m_paraActualizar)
            actualizarEstaIteracion.Add(elemento);
        m_paraActualizar.Clear();

        foreach (Elemento elemento in actualizarEstaIteracion)
            elemento.Avanzar(m_mapa, dt);

        Renderizar();
    }

    public void Renderizar()
    {
        m_mapa.Renderizar(render);
    }

    public void GenerarMeshColision(Extremo rangoJugador)
    {
        m_mapa.GenerarMeshColision(render, rangoJugador);
    }

    public Vector3 PosicionEnMundo(Vector3 posicion)
    {
        return posicion - transform.position;
    }

    public void Insertar(Elemento elemento)
    {
        if (m_mapa == null)
            return;

        bool sePudoInsertar = m_mapa.Insertar(elemento);
        if (!sePudoInsertar)
            return;

        ContenibleNecesitaActualizarse(elemento);
        elemento.necesitoActualizar += ContenibleNecesitaActualizarse;
    }

    private void ContenibleNecesitaActualizarse(IContenible elemento)
    {
        AgregarSinRepetir(m_paraActualizar, (Elemento) elemento);
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
