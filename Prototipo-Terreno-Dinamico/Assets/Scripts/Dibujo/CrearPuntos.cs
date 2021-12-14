using UnityEngine;
using System;

public class CrearPuntos : MonoBehaviour
{
    [SerializeField] float m_distanciaDelJugador = 1.5f;
    [SerializeField] Camera m_camara;
    private bool m_enMovimiento = false;

    Vector3 Posicion => m_camara.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, m_distanciaDelJugador));

    public static event Action<Vector3> EmpezarMovimiento, UpdateMovimiento, FinalizarMovimiento;
    public static event Action TerminarGlyph, TerminarSpell;

    private void Update()
    {
        bool disparando = Input.GetButton("Fire1");

        if (!m_enMovimiento && disparando)
        {
            m_enMovimiento = true;
            EmpezarMovimiento?.Invoke(Posicion);
        }
        else if (m_enMovimiento && !disparando)
        {
            m_enMovimiento = false;
            FinalizarMovimiento?.Invoke(Posicion);
        }
        else if (m_enMovimiento && disparando)
        {
            UpdateMovimiento(Posicion);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            DejarDeCrearPuntos();
        }

        bool terminar = Input.GetKeyDown(KeyCode.Mouse1);
        if (terminar)
        {
            DejarDeCrearPuntos();
            TerminarSpell?.Invoke();
        }
    }

    void DejarDeCrearPuntos()
    {
        m_enMovimiento = false;
        TerminarGlyph?.Invoke();
    }
}
