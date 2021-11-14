using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class Gizmosfs
{
    public static void DrawSquare(Vector3 centro, float radio)
    {
        Vector3 arribaIzquierda = centro + Vector3.back * radio + Vector3.left * radio;
        Vector3 arribaDerecha = centro + Vector3.back * radio + Vector3.right * radio;
        Vector3 abajoIzquierda = centro + Vector3.forward * radio + Vector3.left * radio;
        Vector3 abajoDerecha = centro + Vector3.forward * radio + Vector3.right * radio;


        Gizmos.DrawLine(arribaDerecha, arribaIzquierda);
        Gizmos.DrawLine(arribaIzquierda, abajoIzquierda);
        Gizmos.DrawLine(abajoIzquierda, abajoDerecha);
        Gizmos.DrawLine(abajoDerecha, arribaDerecha);
    }
}
