using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static class Utilidades
{
    static public bool EnRango(Vector3Int posicion, Vector3Int tope)
    {
        for (int i = 0; i < 3; i++)
            if (posicion[i] < 0 || posicion[i] >= tope[i])
                return false;
        return true;
    }
    
    static public int Clap(int valor, int valorMinimo, int valorMaximo)
    {
        if (valor >= valorMaximo)
            return valorMaximo;

        if (valor <= valorMinimo)
            return valorMinimo;

        return valor;
    }

    static public void ClearLog()
    {
        var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
        var type = assembly.GetType("UnityEditor.LogEntries");
        var method = type.GetMethod("Clear");
        method.Invoke(new object(), null);
    }

    static public string Hash(Vector3Int vector)
    {
        return vector.x.ToString() + vector.y.ToString() + vector.z.ToString();
    }

    static public string Hash(Vector3 vector)
    {
        return vector.x.ToString() + vector.y.ToString() + vector.z.ToString();
    }

    static public int CubeIndex(float[] cubo, float nivel)
    {
        int index = 0;
        for (int i = 0; i < 8; i++)
            if (cubo[i] < nivel)
                index |= 1 << i;
        return index;
    }

    static public bool EsEntero(float valor)
    {
        float piso = Mathf.Floor(valor);
        float e = 0.01f;

        return (e > valor - piso && valor - piso > -e);
    }

    static public IEnumerable<T> Dar<T>(List<T> lista)
    {
        int cantidad = lista.Count;
        for (int i = 0; i < cantidad; i++)
        {
            int index = Random.Range(0, lista.Count - 1);
            yield return lista[index];
            lista.RemoveAt(index);
        }
    }
}
