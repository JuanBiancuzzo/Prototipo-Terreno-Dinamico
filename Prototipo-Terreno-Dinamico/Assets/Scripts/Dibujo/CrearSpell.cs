using PDollarGestureRecognizer;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class CrearSpell : MonoBehaviour
{
    GameObject spellPrefab = null;
    Spell spellActual = null;

    private void OnEnable() => ReconocimientoBasico.PlanoCreado += NuevoGlyph;
    private void OnDisable() => ReconocimientoBasico.PlanoCreado -= NuevoGlyph;


    void NuevoGlyph(PlanoDireccionado plano, List<Vector3> puntos)
    {
        if (spellPrefab == null)
        {
            Debug.LogError("Falta el prefab");
            return;
        }

        // se pone una base
        if (spellActual == null)
        {
            GameObject spell = Instantiate(spellPrefab);
            spellActual = spell.GetComponent<Spell>();
            if (spellActual == null)
            {
                Debug.LogError("El prefab no tiene el spell");
                return;
            }

            spellActual.AgregarBase(plano, puntos);
            return;
        }

        spellActual.AgregarGlyph(puntos, plano.posicion);        
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