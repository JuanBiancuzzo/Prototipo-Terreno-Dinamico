using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergiaCoin 
{
    static int minimo = 0;
    static int densidad = 100;
    int m_cantidad;

    public int Valor => m_cantidad;

    public EnergiaCoin(int cantidad = 0)
    {
        m_cantidad = Mathf.Max(minimo, cantidad);
    }

    public void AumentarEnergia(EnergiaCoin energia)
    {
        m_cantidad += energia.Valor;
    }
    
    public void DisminuirEnergia(EnergiaCoin energia)
    {
        m_cantidad -= energia.Valor;
    }

    public EnergiaCoin MayorEnergia(EnergiaCoin energia)
    {
        return (m_cantidad > energia.m_cantidad) ? this : energia;
    }

    public EnergiaCoin MenorEnergia(EnergiaCoin energia)
    {
        return (m_cantidad < energia.m_cantidad) ? this : energia;
    }

    public void ValorActualEnergia(float t)
    {
        m_cantidad = ValorEnergia(t);
    }

    public float EnergiaActualInterpolada()
    {
        return EnergiaInterpolada(m_cantidad);
    }

    public static int ValorEnergia(float t)
    {
        return Mathf.FloorToInt(t * densidad);
    }

    public static float EnergiaInterpolada(int cantidad)
    {
        return (float) cantidad / densidad;
    }
}
