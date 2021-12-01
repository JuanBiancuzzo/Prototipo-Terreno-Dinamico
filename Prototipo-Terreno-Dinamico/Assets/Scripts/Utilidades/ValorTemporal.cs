using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValorTemporal
{
    private int actual, previo;

    public ValorTemporal(int valor)
    {
        actual = valor;
        previo = valor;
    }

    public void Actualizar()
    {
        previo = actual;
    }

    public int Valor()
    {
        return previo;
    }

    public void NuevoValor(int valor)
    {
        actual = valor;
    }

    public void Agregar(int cantidad)
    {
        actual += cantidad;
    }

    public void Quitar(int cantidad)
    {
        actual -= cantidad;
    }    
}
