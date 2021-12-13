using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DibujarLinea : MonoBehaviour
{
    [SerializeField] GameObject m_lineaPrefab;
    LineRenderer m_linea;
    bool m_enMovimiento = false;
    int m_index;

    List<GameObject> m_lineas = new List<GameObject>();

    private void Awake()
    {
        CrearPuntos.CrearPunto += TomarPunto;
        CrearPuntos.TerminarSpell += Reiniciar;
    }
    private void Disable()
    {
        CrearPuntos.CrearPunto -= TomarPunto;
        CrearPuntos.TerminarSpell -= Reiniciar;
    }

    private void TomarPunto(Vector3 punto, bool movimientoActual)
    {
        if (!m_enMovimiento && movimientoActual)
            EmpezarMovimiento(punto);
        else if (m_enMovimiento && !movimientoActual)
            TerminarMovimiento(punto);
        else if (m_enMovimiento && movimientoActual)
            UpdateMovimiento(punto);
    }

    private void EmpezarMovimiento(Vector3 punto)
    {
        GameObject nuevaLinea = Instantiate(m_lineaPrefab);

        m_linea = nuevaLinea.GetComponent<LineRenderer>();
        if (m_linea == null)
        {
            Destroy(nuevaLinea);
            return;
        }
        m_lineas.Add(nuevaLinea);

        m_index = 0;
        UpdateMovimiento(punto);
    }

    private void UpdateMovimiento(Vector3 punto)
    {
        m_linea.positionCount = m_index + 1;
        m_linea.SetPosition(m_index, punto);
        m_index++;
        m_enMovimiento = true;
    }

    private void TerminarMovimiento(Vector3 punto)
    {
        UpdateMovimiento(punto);
        m_enMovimiento = false;
    }

    private void Reiniciar()
    {
        int cantidad = m_lineas.Count;
        for (int i = 0; i < cantidad; i++)
            EliminarUltimo();
    }

    private void EliminarUltimo()
    {
        if (m_lineas.Count == 0)
            return;
        
        GameObject obj = m_lineas[m_lineas.Count - 1];
        Destroy(obj);
        m_lineas.Remove(obj);        
    }
}
