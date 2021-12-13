using PDollarGestureRecognizer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlanoDeMagia
{
    public Plane plano;
    public Vector3 posicion;
    public Vector3 arriba;
    public Vector3 derecha;

    public PlanoDeMagia(Plane plano, Vector3 posicion, Vector3 arriba, Vector3 derecha)
    {
        this.plano = plano;
        this.posicion = posicion;
        this.arriba = arriba;
        this.derecha = derecha;
    }
}

public class DetectarFigura : MonoBehaviour
{
    [SerializeField] bool m_crearFigura = true;
    [SerializeField] string m_nombre;

    private List<Gesture> m_trainingList = new List<Gesture>();

    private void Awake() => Dibujar.m_puntoActuales += FiguraDePuntos;
    private void Disable() => Dibujar.m_puntoActuales -= FiguraDePuntos;

    private void FiguraDePuntos(List<Vector3> puntos)
    {
        if (puntos.Count <= 3)
            return;

        PlanoDeMagia plano = DeterminarPlano(puntos);

        Point[] pointArray = new Point[puntos.Count];
        for (int i = 0; i < puntos.Count; i++)
        {
            Vector2 proyeccion = ProyeccionEnPlano(plano, puntos[i]);
            pointArray[i] = new Point(proyeccion.x, proyeccion.y, 0);
        }

        Gesture newGesture = new Gesture(pointArray);

        if (m_crearFigura)
        {
            newGesture.Name = m_nombre;
            m_trainingList.Add(newGesture);
        }
        else
        {
            Result resultado = PointCloudRecognizer.Classify(newGesture, m_trainingList.ToArray());
            Debug.Log(resultado.GestureClass + " " + resultado.Score);
        }
    }

    private PlanoDeMagia DeterminarPlano(List<Vector3> puntos)
    {
        Vector3 promedio = Vector3.zero;
        Vector3 direccionPromedio = Vector3.zero;

        Vector3 maximo = puntos[0], minimo = puntos[0];

        for (int i = 0; i < puntos.Count; i++)
        {
            Vector3 punto1 = puntos[i], punto2 = puntos[(i + 1) % puntos.Count], punto3 = puntos[(i + 2) % puntos.Count];
            Vector3 direccionNueva = Vector3.Cross(punto2 - punto1, punto3 - punto1);

            for (int j = 0; j < 3; j++)
            {
                maximo[j] = Mathf.Max(maximo[j], puntos[i][j]);
                minimo[j] = Mathf.Min(minimo[j], puntos[i][j]);                    
            }

            direccionPromedio += direccionNueva;
            promedio += punto1;
        }
        promedio /= puntos.Count;
        direccionPromedio = direccionPromedio.normalized;

        Vector3 extension = maximo - minimo;
        Vector3 direccionArriba = (extension.y < extension.x || extension.y < extension.z) ? (puntos[0] - promedio) : Vector3.up;

        Plane plano = new Plane(direccionPromedio, promedio);

        direccionArriba = (plano.ClosestPointOnPlane(direccionArriba + promedio) - promedio).normalized;
        Vector3 direccionDerecha = Vector3.Cross(direccionArriba, direccionPromedio);

        return new PlanoDeMagia(
            plano, 
            promedio,
            direccionArriba,
            direccionDerecha
        );
    }

    private Vector2 ProyeccionEnPlano(PlanoDeMagia planoMagico, Vector3 punto)
    {
        Vector3 puntoEnPlano = planoMagico.plano.ClosestPointOnPlane(punto) - planoMagico.posicion;

        if (puntoEnPlano == Vector3.zero)
            return Vector2.zero;

        Vector3 proyArriba = Vector3.Project(puntoEnPlano, planoMagico.arriba);
        int direccionArriba = (Vector3.Dot(proyArriba, planoMagico.arriba) > 0) ? 1 : -1;

        Vector3 proyDerecha = Vector3.Project(puntoEnPlano, planoMagico.derecha);
        int direccionDerecha = (Vector3.Dot(proyDerecha, planoMagico.derecha) > 0) ? 1 : -1;

        return new Vector2(direccionArriba * proyArriba.magnitude, direccionDerecha * proyDerecha.magnitude);
    }
}
