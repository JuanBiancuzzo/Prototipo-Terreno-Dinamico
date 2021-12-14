using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visualizar : MonoBehaviour
{
    [SerializeField] LineRenderer m_linea;

    private void Awake() => ReconocimientoBasico.PlanoCreado += VisualizarPunto;
    private void Disable() => ReconocimientoBasico.PlanoCreado -= VisualizarPunto;

    int index = 0;
    void VisualizarPunto(PlanoDireccionado plano, List<Vector3> puntos)
    {
        index = 0;
        m_linea.positionCount = 0;
        foreach (Vector3 punto in puntos)
        {
            m_linea.positionCount = index + 1;
            Vector2 puntoEnPantalla = ReconocimientoBasico.ProyeccionEnPlano(plano, punto);

            puntoEnPantalla.y += 3;
            m_linea.SetPosition(index, puntoEnPantalla);
            index++;
        }
    }
}
