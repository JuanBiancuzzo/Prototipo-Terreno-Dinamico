using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Mathfs
{
    public static float e = 0.1f;

    public static Vector3Int Mul(Vector3Int vector, float x)
    {
        Vector3Int resultado = new Vector3Int();

        for (int i = 0; i < 3; i++)
            resultado[i] = Mathf.FloorToInt((float)vector[i] * x);

        return resultado;
    }

    public static List<Vector3Int> PosicionesEntre(Vector3Int inicio, Vector3Int fin)
    {
        // tenemos una recta -> R(t) = inicio + t * (fin - inicio); t entre [0, 1]

        List<Vector3Int> posiciones = new List<Vector3Int>();

        if (inicio == fin)
            return posiciones;

        Vector3Int direccion = fin - inicio;
        int variable = MayorComponente(direccion);

        if (direccion[variable] == 0)
            return posiciones;

        for (int v = 1; v <= Mathf.Abs(direccion[variable]); v++)
        {
            int dirAvance = (Mathf.Sign(direccion[variable]) == 1) ? 1 : -1;
            int valorVariable = inicio[variable] + v * dirAvance;
            Vector3Int posicionNueva = new Vector3Int();
            for (int i = 0; i < 3; i++)
            {
                float valor = ((float)direccion[i] * (valorVariable - inicio[variable])) / direccion[variable] + inicio[i];
                posicionNueva[i] = (i == variable) ? valorVariable : Mathf.CeilToInt(valor);
            }
            posiciones.Add(posicionNueva);
        }

        return posiciones;
    }

    public static IEnumerable<Vector3Int> PosicionesEntreYield(Vector3Int inicio, Vector3Int fin)
    {
        Vector3Int direccion = fin - inicio;
        int variable = MayorComponente(direccion);

        if (direccion[variable] != 0)
        {
            for (int v = 1; v <= Mathf.Abs(direccion[variable]); v++)
            {
                int dirAvance = (Mathf.Sign(direccion[variable]) == 1) ? 1 : -1;
                int valorVariable = inicio[variable] + v * dirAvance;
                Vector3Int posicionNueva = new Vector3Int();
                for (int i = 0; i < 3; i++)
                {
                    float valor = ((float)direccion[i] * (valorVariable - inicio[variable])) / direccion[variable] + inicio[i];
                    posicionNueva[i] = (i == variable) ? valorVariable : Mathf.CeilToInt(valor);
                }

                yield return posicionNueva;
            }
        }
    }

    public static int MayorComponente(Vector3Int vector)
    {
        int x = Mathf.Abs(vector.x), y = Mathf.Abs(vector.y), z = Mathf.Abs(vector.z);
        if (x >= y && x >= z)
            return 0;
        if (y >= x && y >= z)
            return 1;
        return 2;
    }

    public static Vector3Int Normal(Vector3 vector)
    {
        Vector3 versor = vector.normalized;
        Vector3Int respuesta = new Vector3Int();

        for (int i = 0; i < 3; i++)
            if (e > versor[i] && versor[i] > -e)
                respuesta[i] = 0;
            else
                respuesta[i] = (versor[i] > 0) ? 1 : -1;

        return respuesta;
    }

    public static Vector3 CombinacionMayor(Vector3 v1, Vector3 v2)
    {
        Vector3 resultado = new Vector3();

        for (int i = 0; i < 3; i++)
            resultado[i] = (Mathf.Abs(v1[i]) > Mathf.Abs(v2[i])) ? v1[i] : v2[i];

        return resultado;
    }

    public static Vector3Int FloorToInt(Vector3 vector)
    {
        Vector3Int resultado = Vector3Int.zero;

        for (int i = 0; i < 3; i++)
            resultado[i] = (vector[i] > 0) ? Mathf.FloorToInt(vector[i]) : Mathf.CeilToInt(vector[i]);

        return resultado;
    }
}
