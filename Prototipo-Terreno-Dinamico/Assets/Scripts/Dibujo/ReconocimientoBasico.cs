using System;
using PDollarGestureRecognizer;
using System.Collections.Generic;
using UnityEngine;

public struct PlanoDireccionado
{
    public Plane plano;
    public Vector3 posicion;
    public Vector3 arriba;
    public Vector3 derecha;
    public Vector3 extension;

    public PlanoDireccionado(Plane plano, Vector3 posicion, Vector3 arriba, Vector3 derecha, Vector3 extension)
    {
        this.plano = plano;
        this.posicion = posicion;
        this.arriba = arriba;
        this.derecha = derecha;
        this.extension = extension;
    }
}

public class ReconocimientoBasico : MonoBehaviour
{
    [SerializeField] float m_distanciaMinimaSeparacion;
    [SerializeField] float m_cambioDeDireccion;
    [SerializeField] Camera m_camara;

    List<Vector3> m_puntos = new List<Vector3>();
    bool m_enMovimiento = false;

    Vector3 m_posicionPromedio, m_direccionNormal, m_direccionCamaraInicial;
    Vector3 m_maximo, m_minimo;
    bool m_cambioDireccion;

    PlanoDireccionado m_plano;

    private void Awake() => CrearPuntos.CrearPunto += TomarPunto;
    private void Disable() => CrearPuntos.CrearPunto -= TomarPunto;

    public static event Action<PlanoDireccionado, List<Vector3>> PlanoCreado;

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
        m_puntos.Clear();
        m_posicionPromedio = punto;
        m_direccionNormal = Vector3.zero;
        m_direccionCamaraInicial = m_camara.transform.forward;
        m_maximo = punto;
        m_minimo = punto;

        m_puntos.Add(punto);
        m_enMovimiento = true;
    }

    private void UpdateMovimiento(Vector3 punto)
    {
        Vector3 puntoAnterior = m_puntos[m_puntos.Count - 1];

        if ((punto - puntoAnterior).sqrMagnitude < m_distanciaMinimaSeparacion * m_distanciaMinimaSeparacion)
            return;

        m_posicionPromedio += punto;
        for (int i = 0; i < 3; i++)
        {
            m_maximo[i] = Mathf.Max(m_maximo[i], punto[i]);
            m_minimo[i] = Mathf.Min(m_minimo[i], punto[i]);
        }

        int cantidad = m_puntos.Count;
        if (cantidad <= 2)
        {
            m_puntos.Add(punto);
            return;
        }

        Vector3 punto2 = m_puntos[cantidad - 1], punto3 = m_puntos[cantidad - 2];
        Vector3 nuevaNormal = Vector3.Cross(punto2 - punto, punto3 - punto);

        for (int i = 0; i < 3; i++)
            nuevaNormal[i] = Mathf.Abs(nuevaNormal[i]);

        m_direccionNormal += nuevaNormal;


        m_puntos.Add(punto);
    }

    private void TerminarMovimiento(Vector3 punto)
    {
        UpdateMovimiento(punto);
        m_enMovimiento = false;

        if (m_puntos.Count <= 2)
            return;

        m_posicionPromedio /= m_puntos.Count;

        Vector3 extension = m_maximo - m_minimo;
        Vector3 direccionArriba = Vector3.up;
        if (extension.y < extension.x || extension.y < extension.z)
        {
            direccionArriba = m_puntos[0] - m_posicionPromedio;

            m_direccionNormal = m_direccionCamaraInicial;
            m_direccionNormal.y = 0;
        }

        m_direccionNormal = m_direccionNormal.normalized;

        Plane plano = new Plane(m_direccionNormal, m_posicionPromedio);

        direccionArriba = (plano.ClosestPointOnPlane(direccionArriba + m_posicionPromedio) - m_posicionPromedio).normalized;
        Vector3 direccionDerecha = Vector3.Cross(direccionArriba, m_direccionNormal);

        m_plano = new PlanoDireccionado
        (
            plano,
            m_posicionPromedio,
            direccionArriba,
            direccionDerecha,
            extension
        );

        PlanoCreado?.Invoke(m_plano, m_puntos);
    }

    public static Result ReconocerFigura(List<Vector3> puntos, PlanoDireccionado plano, List<Gesture> trainginSet)
    {
        Point[] pointArray = new Point[puntos.Count];
        for (int i = 0; i < puntos.Count; i++)
        {
            Vector2 proyeccion = ProyeccionEnPlano(plano, puntos[i]);
            pointArray[i] = new Point(proyeccion.x, proyeccion.y, 0);
        }

        return PointCloudRecognizer.Classify(new Gesture(pointArray), trainginSet.ToArray());
    }

    public static Vector2 ProyeccionEnPlano(PlanoDireccionado planoDireccionado, Vector3 punto)
    {
        Vector3 puntoEnPlano = planoDireccionado.plano.ClosestPointOnPlane(punto) - planoDireccionado.posicion;

        if (puntoEnPlano == Vector3.zero)
            return Vector2.zero;

        Vector3 proyArriba = Vector3.Project(puntoEnPlano, planoDireccionado.arriba);
        int direccionArriba = (Vector3.Dot(proyArriba, planoDireccionado.arriba) > 0) ? 1 : -1;

        Vector3 proyDerecha = Vector3.Project(puntoEnPlano, planoDireccionado.derecha);
        int direccionDerecha = (Vector3.Dot(proyDerecha, planoDireccionado.derecha) > 0) ? 1 : -1;

        return new Vector2(direccionDerecha * proyDerecha.magnitude, direccionArriba * proyArriba.magnitude);
    }
}
