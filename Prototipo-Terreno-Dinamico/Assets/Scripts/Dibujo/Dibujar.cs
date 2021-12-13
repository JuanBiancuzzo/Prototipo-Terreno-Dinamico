using System;
using System.Collections.Generic;
using UnityEngine;

public class Dibujar : MonoBehaviour
{
    [SerializeField] float m_distanciaDelJugador = 3f;
    [SerializeField] float m_distanciaMinimaSeparacion;

    [SerializeField] GameObject m_prefabPunto;
    [SerializeField] Camera m_camara;

    private bool m_enMovimiento = false;

    List<Vector3> m_posiciones = new List<Vector3>();

    List<GameObject> m_cubos = new List<GameObject>();

    Vector3 Posicion => m_camara.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, m_distanciaDelJugador));

    public static event Action<List<Vector3>> m_puntoActuales;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            foreach (GameObject obj in m_cubos)
                Destroy(obj, 1);
            m_cubos.Clear();
        }

        bool disparando = Input.GetButton("Fire1");

        if (!m_enMovimiento && disparando)
            EmpezarMovimiento();
        else if (m_enMovimiento && !disparando)
            TerminarMovimiento();
        else if (m_enMovimiento && disparando)
            UpdateMovimiento();
    }

    void EmpezarMovimiento()
    {
        m_posiciones.Clear();

        m_enMovimiento = true;
        AgregarPosicion(Posicion);
    }

    void TerminarMovimiento()
    {
        m_enMovimiento = false;
        m_puntoActuales?.Invoke(m_posiciones);
    }

    void UpdateMovimiento()
    {
        Vector3 nuevaPosicion = Posicion;
        Vector3 posicionAnterior = m_posiciones[m_posiciones.Count - 1];

        if ((nuevaPosicion - posicionAnterior).sqrMagnitude > m_distanciaMinimaSeparacion * m_distanciaMinimaSeparacion)
            AgregarPosicion(nuevaPosicion);
    }

    void AgregarPosicion(Vector3 posicion)
    {
        m_posiciones.Add(posicion);
        m_cubos.Add(Instantiate(m_prefabPunto, posicion, Quaternion.identity));
    }
}
