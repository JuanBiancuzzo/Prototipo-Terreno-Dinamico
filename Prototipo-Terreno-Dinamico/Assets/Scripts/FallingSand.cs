using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingSand : MonoBehaviour
{
    public EspacioGeneral m_mapa = null;
    List<MaterialParticula> m_paraActualizar = new List<MaterialParticula>();

    public static float m_default = 0f;
    public static Color m_defaultColor = new Color(1f, 1f, 1f, 1f);

    public MarchingCubes render;
    public float dt = 0.1f;

    int cantidad = 0;
    void FixedUpdate()
    {
        if (cantidad % 5 == 0)
            Avanzar();
        cantidad++;
    }

    private void AgregarSinRepetir<T>(List<T> lista, T elemento)
    {
        foreach (T material in lista)
            if (material.Equals(elemento))
                return;
        lista.Add(elemento);
    }

    public void Avanzar()
    {
        List<MaterialParticula> nuevosParaActualizarse = new List<MaterialParticula>();

        foreach (MaterialParticula material in m_paraActualizar)
        {
            List<MaterialParticula> paraActualizarse = new List<MaterialParticula>();
            material.Avanzar(m_mapa, ref paraActualizarse);

            foreach (MaterialParticula necesitaActualizarse in paraActualizarse)
                AgregarSinRepetir<MaterialParticula>(nuevosParaActualizarse, necesitaActualizarse);
        }

        m_paraActualizar.Clear();
        foreach (MaterialParticula material in nuevosParaActualizarse)
            m_paraActualizar.Add(material);

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

    public void Insertar(MaterialParticula material)
    {
        if (m_mapa == null)
            return;

        if (m_mapa.Insertar(material) && material.SeActualiza())
            m_paraActualizar.Add(material);
    }

    public static bool EsDefault(Color color)
    {
        return color == m_defaultColor;
    }
}
