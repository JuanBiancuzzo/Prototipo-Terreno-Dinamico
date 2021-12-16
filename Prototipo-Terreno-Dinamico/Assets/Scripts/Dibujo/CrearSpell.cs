using PDollarGestureRecognizer;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class CrearSpell : MonoBehaviour
{
    PlanoDireccionado planoActual;
    //[SerializeField] bool crear;
    //[SerializeField] string nombre;

    List<Gesture> m_trainingSet = new List<Gesture>();

    private void Awake() => ReconocimientoBasico.PlanoCreado += NuevoGlyph;
    private void Disable() => ReconocimientoBasico.PlanoCreado -= NuevoGlyph;

    private void Start()
    {
        string[] files = Directory.GetFiles(Application.persistentDataPath, "*.xml");
        foreach (string file in files)
            m_trainingSet.Add(GestureIO.ReadGestureFromFile(file));
    }

    void NuevoGlyph(PlanoDireccionado plano, List<Vector3> puntos)
    {
       
        Result resultado = ReconocimientoBasico.ReconocerFigura(puntos, plano, m_trainingSet);
        Debug.Log(resultado.GestureClass + " con " + resultado.Score);
    }
}


/*if (crear)
       {
           Point[] pointArray = new Point[puntos.Count];
           for (int i = 0; i < puntos.Count; i++)
           {
               Vector2 proyeccion = ReconocimientoBasico.ProyeccionEnPlano(plano, puntos[i]);
               pointArray[i] = new Point(proyeccion.x, proyeccion.y, 0);
           }

           Gesture newGesture = new Gesture(pointArray);

           newGesture.Name = nombre;
           m_trainingSet.Add(newGesture);

           string fileName = Application.persistentDataPath + "/" + nombre + ".xml";
           GestureIO.WriteGesture(pointArray, nombre, fileName);

           Debug.Log("Spell " + nombre + " creado");
       }
       else
       {
       }*/