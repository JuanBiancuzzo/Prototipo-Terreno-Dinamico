using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AtributoInt
{
    int m_minimo, m_maximo;
    [SerializeField] int m_valor;

    public int Valor => m_valor;
    public void NuevoValor(int valor)
    {
        m_valor = Mathf.Max(m_minimo, Mathf.Min(m_maximo, valor));
    }
    public void Aumentar(int cantidad)
    {
        m_valor += cantidad;
        m_valor = Mathf.Min(m_valor, m_maximo);
    }
    public void Disminuir(int cantidad)
    {
        m_valor -= cantidad;
        m_valor = Mathf.Max(m_valor, m_minimo);
    }

    public AtributoInt(int valor, int minimo, int maximo)
    {
        m_valor = Mathf.Max(minimo, Mathf.Min(maximo, valor));
        m_minimo = minimo;
        m_maximo = maximo;
    }

    public int EnergiaAAtributo(EnergiaCoin energia, float modificador = 1f)
    {
        float t = energia.EnergiaActualInterpolada() * modificador;
        return ValorAtributo(t);
    }

    public EnergiaCoin AtributoAEnergia(int valor, EnergiaCoin energiaAModificar = null, float modificador = 1f)
    {
        float t = AtributoInterpolada(valor) * modificador;
        if (energiaAModificar == null)
            energiaAModificar = new EnergiaCoin();

        energiaAModificar.ValorActualEnergia(t);
        return energiaAModificar;
    }

    public int ValorAtributo(float t)
    {
        return Mathf.FloorToInt(Mathf.Lerp(m_minimo, m_maximo, t));
    }

    public float AtributoInterpolada(int valor)
    {
        return Mathf.InverseLerp(m_minimo, m_maximo, valor);
    }
}

[System.Serializable]
public class AtributoFloat
{
     float m_minimo, m_maximo;
     [SerializeField] float m_valor;

    public float Valor => m_valor;
    public void NuevoValor(float valor)
    {
        m_valor = Mathf.Max(m_minimo, Mathf.Min(m_maximo, valor));
    }
    public void Aumentar(float cantidad){
        m_valor += cantidad;
        m_valor = Mathf.Min(m_valor, m_maximo);
    }
    public void Disminuir(float cantidad){
        m_valor -= cantidad;
        m_valor = Mathf.Max(m_valor, m_minimo);
    }

    public AtributoFloat(float valor, float minimo, float maximo)
    {
        m_valor = Mathf.Max(minimo, Mathf.Min(maximo, valor));
        m_minimo = minimo;
        m_maximo = maximo;
    }
    public float EnergiaAAtributo(EnergiaCoin energia, float modificador = 1f)
    {
        float t = energia.EnergiaActualInterpolada() * modificador;
        return ValorAtributo(t);
    }

    public EnergiaCoin AtributoAEnergia(float valor, EnergiaCoin energiaAModificar = null, float modificador = 1f)
    {
        float t = AtributoInterpolada(valor) * modificador;
        if (energiaAModificar == null)
            energiaAModificar = new EnergiaCoin();

        energiaAModificar.ValorActualEnergia(t);
        return energiaAModificar;
    }

    public float ValorAtributo(float t)
    {
        return Mathf.Lerp(m_minimo, m_maximo, t);
    }

    public float AtributoInterpolada(float valor)
    {
        return Mathf.InverseLerp(m_minimo, m_maximo, valor);
    }
}
