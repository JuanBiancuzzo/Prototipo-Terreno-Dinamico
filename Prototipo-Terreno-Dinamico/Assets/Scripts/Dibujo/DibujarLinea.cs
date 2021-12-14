using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DibujarLinea : MonoBehaviour
{
    [SerializeField] GameObject m_lineaPrefab;
    LineRenderer m_linea;
    int m_index;

    List<GameObject> m_lineas = new List<GameObject>();

    private void Awake()
    {
        CrearPuntos.EmpezarMovimiento += EmpezarMovimiento;
        CrearPuntos.UpdateMovimiento += UpdateMovimiento;
        CrearPuntos.FinalizarMovimiento += TerminarMovimiento;
        CrearPuntos.TerminarSpell += Reiniciar;
    }
    private void Disable()
    {
        CrearPuntos.EmpezarMovimiento -= EmpezarMovimiento;
        CrearPuntos.UpdateMovimiento -= UpdateMovimiento;
        CrearPuntos.FinalizarMovimiento -= TerminarMovimiento;
        CrearPuntos.TerminarSpell -= Reiniciar;
    }

    private void EmpezarMovimiento(Vector3 punto)
    {
        CrearLinea();
        UpdateMovimiento(punto);
    }

    private void UpdateMovimiento(Vector3 punto)
    {
        m_linea.positionCount = m_index + 1;
        m_linea.SetPosition(m_index, punto);
        m_index++;
    }

    private void TerminarMovimiento(Vector3 punto)
    {
        UpdateMovimiento(punto);
    }

    private void CrearLinea()
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
