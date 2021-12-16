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

    public PlanoDireccionado(Plane plano, Vector3 posicion, Vector3 arriba, Vector3 derecha)
    {
        this.plano = plano;
        this.posicion = posicion;
        this.arriba = arriba;
        this.derecha = derecha;
    }
}

public class ReconocimientoBasico : MonoBehaviour
{
    [SerializeField] float m_distanciaMinimaSeparacion;
    [SerializeField] [Range(0, 1)] float m_cambioDeDireccion;
    [SerializeField] Camera m_camara;

    List<Vector3> m_puntos = new List<Vector3>();

    Vector3 m_posicionPromedio, m_direccionCamara;
    bool m_cambioDireccion;

    bool m_empezandoNuevoGlyph = true;

    PlanoDireccionado m_plano;

    private void Awake()
    {
        CrearPuntos.EmpezarMovimiento += EmpezarMovimiento;
        CrearPuntos.UpdateMovimiento += UpdateMovimiento;
        CrearPuntos.FinalizarMovimiento += UpdateMovimiento;
        CrearPuntos.TerminarGlyph += DetermianrPlano;
    }
    private void Disable()
    {
        CrearPuntos.EmpezarMovimiento -= EmpezarMovimiento;
        CrearPuntos.UpdateMovimiento -= UpdateMovimiento;
        CrearPuntos.FinalizarMovimiento -= UpdateMovimiento;
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

        m_puntos.Add(punto);
        m_cambioDireccion = false;
    }

    private void UpdateMovimiento(Vector3 punto)
    {
        Vector3 puntoAnterior = m_puntos[m_puntos.Count - 1];

        if ((punto - puntoAnterior).sqrMagnitude < m_distanciaMinimaSeparacion * m_distanciaMinimaSeparacion)
            return;

        m_posicionPromedio += punto;

        Vector3 direccionCamara = m_camara.transform.forward;
        if (!m_cambioDireccion && 1 - Vector3.Dot(direccionCamara, m_direccionCamara) > m_cambioDeDireccion)
            m_cambioDireccion = true;
        m_direccionCamara += direccionCamara;

        m_puntos.Add(punto);
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

        direccionArriba = (plano.ClosestPointOnPlane(direccionArriba + m_posicionPromedio) - m_posicionPromedio).normalized;
        Vector3 direccionDerecha = Vector3.Cross(direccionArriba, direccionNormal).normalized;

        Vector3 mayorEnDireccionArriba = Vector3.zero, mayorEnDireccionDerecha = Vector3.zero;
        foreach (Vector3 punto in m_puntos)
        {
            Vector3 puntoEnPlano = plano.ClosestPointOnPlane(punto);

            Vector3 enDireccionArriba = Vector3.Project(puntoEnPlano - m_posicionPromedio, direccionArriba);
            Vector3 enDireccionDerecha = Vector3.Project(puntoEnPlano - m_posicionPromedio, direccionDerecha);

            if (enDireccionArriba.magnitude > mayorEnDireccionArriba.magnitude)
                mayorEnDireccionArriba = enDireccionArriba * Mathf.Sign(Vector3.Dot(enDireccionArriba, direccionArriba)); 

            if (enDireccionDerecha.magnitude > mayorEnDireccionDerecha.magnitude)
                mayorEnDireccionDerecha = enDireccionDerecha * Mathf.Sign(Vector3.Dot(enDireccionDerecha, direccionDerecha));
        }

        m_plano = new PlanoDireccionado
        (
            plano,
            m_posicionPromedio,
            mayorEnDireccionArriba,
            mayorEnDireccionDerecha
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

    // posicionado con respecto al (0, 0)
    public static Vector2 ProyeccionEnPlano(PlanoDireccionado planoDireccionado, Vector3 punto)
    {
        Vector3 puntoEnPlano = planoDireccionado.plano.ClosestPointOnPlane(punto) - planoDireccionado.posicion;

        if (puntoEnPlano == Vector3.zero)
            return Vector2.zero;

        Vector3 proyArriba = Vector3.Project(puntoEnPlano, planoDireccionado.arriba);
        int direccionArriba = (Vector3.Dot(proyArriba, planoDireccionado.arriba) > 0) ? 1 : -1;

        Vector3 proyDerecha = Vector3.Project(puntoEnPlano, planoDireccionado.derecha);
        int direccionDerecha = (Vector3.Dot(proyDerecha, planoDireccionado.derecha) > 0) ? 1 : -1;

        float maximaDireccion;
        if (planoDireccionado.derecha.magnitude > planoDireccionado.arriba.magnitude)
            maximaDireccion = planoDireccionado.derecha.magnitude;
        else
            maximaDireccion = planoDireccionado.arriba.magnitude;

        return new Vector2(
            direccionDerecha * proyDerecha.magnitude / maximaDireccion,
            direccionArriba * proyArriba.magnitude / maximaDireccion
        );
    }
}
