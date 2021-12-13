using UnityEngine;
using System;

public class CrearPuntos : MonoBehaviour
{
    [SerializeField] float m_distanciaDelJugador = 1.5f;
    [SerializeField] Camera m_camara;
    private bool m_enMovimiento = false;

    Vector3 Posicion => m_camara.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, m_distanciaDelJugador));

    public static event Action<Vector3, bool> CrearPunto;
    public static event Action TerminarSpell; 

    private void Update()
    {
        bool disparando = Input.GetButton("Fire1");

        if (!m_enMovimiento && disparando)
        {
            m_enMovimiento = true;
            CrearPunto?.Invoke(Posicion, m_enMovimiento);
        }    
        else if (m_enMovimiento && !disparando)
        {
            m_enMovimiento = false;
            CrearPunto?.Invoke(Posicion, m_enMovimiento);
        }
        else if (m_enMovimiento && disparando)
        {
            CrearPunto?.Invoke(Posicion, m_enMovimiento);
        }

        bool terminar = Input.GetButtonDown("Fire2");
        if (terminar)
            TerminarSpell?.Invoke();
    }
}
