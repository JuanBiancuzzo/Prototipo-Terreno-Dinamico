using PDollarGestureRecognizer;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class CrearSpell : MonoBehaviour
{
    PlanoDireccionado planoActual;

    List<Gesture> m_trainingSet = new List<Gesture>();

    private void Awake() => ReconocimientoBasico.PlanoCreado += NuevoGlyph;
    private void Disable() => ReconocimientoBasico.PlanoCreado -= NuevoGlyph;

    private void Start()
    {
        string[] gestureFiles = Directory.GetFiles(Application.persistentDataPath, "*.xml");
        foreach (string file in gestureFiles)
            m_trainingSet.Add(GestureIO.ReadGestureFromFile(file));
    }

    /*
     * para crear un simbolo nuevo
     * 
        newGesture.Name = m_nombre;
        m_trainingList.Add(newGesture);

        string fileName = Application.persistentDataPath + "/" + m_nombre + ".xml";
        GestureIO.WriteGesture(pointArray, m_nombre, fileName);
     */

    void NuevoGlyph(PlanoDireccionado plano, List<Vector3> puntos)
    {
        Result resultado = ReconocimientoBasico.ReconocerFigura(puntos, plano, m_trainingSet);
        Debug.Log(resultado.GestureClass + " con " + resultado.Score);
    }
}
