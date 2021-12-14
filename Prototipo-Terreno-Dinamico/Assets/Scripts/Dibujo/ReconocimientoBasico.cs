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
    [SerializeField] [Range(0, 1)] float m_cambioDeDireccion;
    [SerializeField] Camera m_camara;

    List<Vector3> m_puntos = new List<Vector3>();

    Vector3 m_posicionPromedio, m_direccionCamara;
    Vector3 m_maximo, m_minimo;
    bool m_cambioDireccion;

    bool m_empezandoNuevoGlyph = true;

    PlanoDireccionado m_plano;

    private void Awake()
    {
        CrearPuntos.EmpezarMovimiento += EmpezarMovimiento;
        CrearPuntos.UpdateMovimiento += UpdateMovimiento;
        CrearPuntos.FinalizarMovimiento += TerminarMovimiento;
        CrearPuntos.TerminarGlyph += DetermianrPlano;
    }
    private void Disable()
    {
        CrearPuntos.EmpezarMovimiento -= EmpezarMovimiento;
        CrearPuntos.UpdateMovimiento -= UpdateMovimiento;
        CrearPuntos.FinalizarMovimiento -= TerminarMovimiento;
        CrearPuntos.TerminarGlyph -= DetermianrPlano;
    }

    public static event Action<PlanoDireccionado, List<Vector3>> PlanoCreado;

    private void EmpezarMovimiento(Vector3 punto)
    {
        if (m_empezandoNuevoGlyph)
            InicializarValores(punto);
        m_empezandoNuevoGlyph = false;
    }

    private void InicializarValores(Vector3 punto)
    {
        m_puntos.Clear();
        m_posicionPromedio = punto;
        m_direccionCamara = m_camara.transform.forward;
        m_maximo = punto;
        m_minimo = punto;

        m_puntos.Add(punto);
        m_cambioDireccion = false;
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

        Vector3 direccionCamara = m_camara.transform.forward;
        if (!m_cambioDireccion && 1 - Vector3.Dot(direccionCamara, m_direccionCamara) > m_cambioDeDireccion)
            m_cambioDireccion = true;
        m_direccionCamara += direccionCamara;

        m_puntos.Add(punto);
    }

    private void TerminarMovimiento(Vector3 punto)
    {
        UpdateMovimiento(punto);
    }

    private void DetermianrPlano()
    {
        m_empezandoNuevoGlyph = true;

        if (m_puntos.Count <= 2)
            return;

        m_posicionPromedio /= m_puntos.Count;

        Vector3 direccionNormal = m_direccionCamara.normalized;
        Vector3 direccionArriba = Vector3.up;
        if (m_cambioDireccion || Mathf.Abs(direccionNormal.y) > m_cambioDeDireccion)
        {
            direccionArriba = m_puntos[0] - m_posicionPromedio;
            direccionNormal = Vector3.up * Mathf.Sign(m_direccionCamara.y);
        }
        else
        {
            direccionNormal.y = 0;
        }

        Plane plano = new Plane(direccionNormal, m_posicionPromedio);
        Vector3 extension = m_maximo - m_minimo;

        direccionArriba = (plano.ClosestPointOnPlane(direccionArriba + m_posicionPromedio) - m_posicionPromedio).normalized;
        Vector3 direccionDerecha = Vector3.Cross(direccionArriba, direccionNormal);

        direccionArriba = Vector3.Project(extension, direccionArriba);
        direccionDerecha = Vector3.Project(extension, direccionDerecha);

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
