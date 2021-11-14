using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kernel
{
    private int m_columnas, m_filas, m_profundidad;

    private float[,,] m_kernel;

    private float m_default = 0f;

    public Kernel(int columnas, int filas, int profundidad)
    {
        m_columnas = columnas;
        m_filas = filas;
        m_profundidad = profundidad;
        m_kernel = new float[columnas, filas, profundidad];
    }

    public void InicializarKernel(float[,,] kernel)
    {
        m_kernel = kernel;
    }

    private bool enRango(Vector3Int posicion, Vector3Int tope)
    {
        for (int i = 0; i < 3; i++)
            if (posicion[i] < 0 || posicion[i] >= tope[i])
                return false;
        return true;
    }

    public float Convolucion(float[,,] mapa, Vector3Int posicion, Vector3Int tope)
    {
        float resultado = 0f;
        for (int i = 0; i < m_columnas; i++)
            for (int j = 0; j < m_filas; j++)
                for (int k = 0; k < m_profundidad; k++)
                { 
                    Vector3Int vectorPosicion = new Vector3Int(posicion.x + i - m_columnas / 2, posicion.y + j - m_filas / 2, posicion.z + k - m_profundidad / 2);
                    resultado += (enRango(vectorPosicion, tope)) ? m_kernel[i, j, k] * mapa[vectorPosicion.x, vectorPosicion.y, vectorPosicion.z] : m_default;
                }
        return resultado;
    }
}
