using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lenia
{
    public int m_columnas, m_filas, m_profundidad;
    public float[,,] m_mapa, m_mapa_reserva;

    public GMaping m_maping;
    public Kernel m_kernel;

    public Lenia(GMaping maping, Kernel kernel, int columnas, int filas, int profundidad)
    {
        m_columnas = columnas;
        m_filas = filas;
        m_profundidad = profundidad;

        m_mapa = new float[m_columnas, m_filas, m_profundidad];
        m_mapa_reserva = new float[m_columnas, m_filas, m_profundidad];

        m_maping = maping;
        m_kernel = kernel;
    }

    private float clap(float valor, float extremo_inferios, float extremo_superior)
    {
        if (valor > extremo_superior)
            return extremo_superior;
        if (valor < extremo_inferios)
            return extremo_inferios;
        return valor;
    }

    public void Avanzar(float dt)
    {
        Vector3Int tope = new Vector3Int(m_columnas, m_filas, m_profundidad);

        for (int i = 0; i < m_columnas; i++)
            for (int j = 0; j < m_filas; j++)
                for (int k = 0; k < m_profundidad; k++)
                {
                    float resultado = m_kernel.Convolucion(m_mapa, new Vector3Int(i, j, k), tope);
                    m_mapa_reserva[i, j, k] = clap(m_mapa[i, j, k] + dt * m_maping.Map(resultado) * resultado, 0, 1);
                }

        for (int i = 0; i < m_columnas; i++)
            for (int j = 0; j < m_filas; j++)
                for (int k = 0; k < m_profundidad; k++)
                    m_mapa[i, j, k] = m_mapa_reserva[i, j, k];
    }

    public float Valor(int i, int j, int k)
    {
        return m_mapa[i, j, k];
    }

    public void Insertar(int i, int j, int k, float valor)
    {
        m_mapa[i, j, k] += valor;
    }
}