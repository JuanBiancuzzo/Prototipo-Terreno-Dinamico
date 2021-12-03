using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtributoInt
{
    static int m_minimo, m_maximo;
    int m_valor;

    public int Valor => m_valor;

    public AtributoInt(int valor, int minimo, int maximo)
    {
        m_valor = Mathf.Max(minimo, Mathf.Min(maximo, valor));
        m_minimo = minimo;
        m_maximo = maximo;
    }
    
    public void Aumentar(int cantidad)
    {
        m_valor += cantidad;
    }

    public void Disminuir(int cantidad)
    {
        m_valor -= cantidad;
    }

    public static int EnergiaAAtributo(EnergiaCoin energia)
    {
        float t = energia.EnergiaActualInterpolada();
        return ValorTemperatura(t);
    }

    public static EnergiaCoin AtributoAEnergia(int valor, EnergiaCoin energiaAModificar = null)
    {
        float t = TemperaturaInterpolada(valor);
        if (energiaAModificar == null)
            energiaAModificar = new EnergiaCoin();

        energiaAModificar.ValorActualEnergia(t);
        return energiaAModificar;
    }

    public void ValorActualTemperatura(float t)
    {
        m_valor = ValorTemperatura(t);
    }

    public float TemeperaturaActualInterpolada()
    {
        return TemperaturaInterpolada(m_valor);
    }

    public static int ValorTemperatura(float t)
    {
        return Mathf.FloorToInt(Mathf.Lerp(m_minimo, m_maximo, t));
    }

    public static float TemperaturaInterpolada(int valor)
    {
        return Mathf.InverseLerp(m_minimo, m_maximo, valor);
    }
}

public class AtributoFloat
{
    static float m_minimo, m_maximo;
    float m_valor;

    public float Valor => m_valor;

    public AtributoFloat(float valor, float minimo, float maximo)
    {
        m_valor = Mathf.Max(minimo, Mathf.Min(maximo, valor));
        m_minimo = minimo;
        m_maximo = maximo;
    }
    public static float EnergiaAAtributo(EnergiaCoin energia)
    {
        float t = energia.EnergiaActualInterpolada();
        return ValorTemperatura(t);
    }

    public static EnergiaCoin AtributoAEnergia(float valor, EnergiaCoin energiaAModificar = null)
    {
        float t = TemperaturaInterpolada(valor);
        if (energiaAModificar == null)
            energiaAModificar = new EnergiaCoin();

        energiaAModificar.ValorActualEnergia(t);
        return energiaAModificar;
    }

    public void Aumentar(float cantidad)
    {
        m_valor += cantidad;
    }

    public void Disminuir(float cantidad)
    {
        m_valor -= cantidad;
    }


    public void ValorActualTemperatura(float t)
    {
        m_valor = ValorTemperatura(t);
    }

    public float TemeperaturaActualInterpolada()
    {
        return TemperaturaInterpolada(m_valor);
    }

    public static float ValorTemperatura(float t)
    {
        return Mathf.FloorToInt(Mathf.Lerp(m_minimo, m_maximo, t));
    }

    public static float TemperaturaInterpolada(float valor)
    {
        return Mathf.InverseLerp(m_minimo, m_maximo, valor);
    }
}
