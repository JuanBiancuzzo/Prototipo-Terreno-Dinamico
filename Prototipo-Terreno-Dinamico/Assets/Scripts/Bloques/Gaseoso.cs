using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gaseoso : Elemento
{
    protected Gaseoso(Vector3Int posicion, IConetenedorGeneral mundo) : base(posicion, mundo)
    {
    }

    public override void Avanzar(int dt)
    {
        foreach (Vector3Int desfase in Opciones())
        {
            Elemento elemento = m_mundo.EnPosicion(m_posicion + desfase);
            if (elemento == null || !MismoTipo(elemento))
                continue;

            if (!elemento.PermiteIntercambiar())
                continue;

            Gaseoso gaseoso = (Gaseoso)elemento;
            if (Elemento.ElementoConMayorDensidad(gaseoso, this) == gaseoso)
                continue;

            elemento.Intercambiar(this);
        }
    }

    private IEnumerable<Vector3Int> Opciones()
    {
        yield return new Vector3Int(0, 1, 0);

        List<Vector3Int> diagonales = new List<Vector3Int>()
        {
            new Vector3Int( 1, 1, -1), new Vector3Int( 1, 1, 0), new Vector3Int( 1, 1, -1),
            new Vector3Int( 0, 1, -1),                           new Vector3Int( 0, 1, -1),
            new Vector3Int(-1, 1, -1), new Vector3Int(-1, 1, 0), new Vector3Int(-1, 1, -1)
        };

        int opciones = diagonales.Count;
        for (int i = 0; i < opciones; i++)
        {
            int index = Random.Range(0, diagonales.Count - 1);
            yield return diagonales[index];
            diagonales.RemoveAt(index);
        }
    }

    public override void Desplazar()
    {
        List<Gaseoso> gaseosos = new List<Gaseoso>();

        for (int x = -1; x <= 1; x++)
            for (int y = -1; y <= 1; y++)
                for (int z = -1; z <= 1; z++)
                {
                    if (x == 0 && z == 0 && y == 0)
                        continue;

                    Elemento elemento = m_mundo.EnPosicion(m_posicion + new Vector3Int(x, y, z));
                    if (elemento != null && MismoElemento(elemento))
                        gaseosos.Add((Gaseoso)elemento);
                }

        for (int i = 0; i < gaseosos.Count; i++)
        {
            Gaseoso gaseoso = gaseosos[i];

            int cantidadADar = m_densidad / (gaseosos.Count - i);
            m_densidad -= cantidadADar;
            int cantidadExtra = gaseoso.Agregar(cantidadADar);
            Agregar(cantidadExtra);
        }

        if (m_densidad > 0)
            Debug.LogError("Mas densidad de lo que deberia");
    }

    public override bool PermiteDesplazar()
    {
        List<Gaseoso> gaseosos = new List<Gaseoso>();

        for (int x = -1; x <= 1; x++)
            for (int y = -1; y <= 1; y++)
                for (int z = -1; z <= 1; z++)
                {
                    if (x == 0 && z == 0 && y == 0)
                        continue;

                    Elemento elemento = m_mundo.EnPosicion(m_posicion + new Vector3Int(x, y, z));
                    if (elemento != null && MismoElemento(elemento))
                        gaseosos.Add((Gaseoso)elemento);
                }

        int cantidadAdimitida = 0;
        foreach (Gaseoso gaseoso in gaseosos)
            cantidadAdimitida += gaseoso.MaximoParaRecibir();

        return m_densidad < cantidadAdimitida;
    }

    protected virtual int CantidadADar()
    {
        int cantidad = m_densidad / 4;
        cantidad = Mathf.Max(cantidad, m_densidad);
        m_densidad -= cantidad;
        return cantidad;
    }

    protected int Agregar(int cantidadDensidad)
    {
        int resto = Mathf.Max(0, m_densidad + cantidadDensidad - m_maximoValor);

        if (m_densidad + cantidadDensidad < m_maximoValor)
            m_densidad += cantidadDensidad;
        else
            m_densidad = m_maximoValor;

        return resto;
    }

    protected int MaximoParaRecibir()
    {
        return m_maximoValor - m_densidad;
    }

    public override bool PermiteIntercambiar()
    {
        return true;
    }

    public override bool MismoTipo(Elemento elemento)
    {
        return elemento.MismoTipo(this);
    }

    public override bool MismoTipo(Solido solido)
    {
        return false;
    }

    public override bool MismoTipo(Liquido liquido)
    {
        return false;
    }

    public override bool MismoTipo(Gaseoso gaseoso)
    {
        return true;
    }



    /*
    public override void ActuanEnElemento(Elemento elemento, int dt)
    {
        elemento.ActuarEnOtro(this, dt);
    }

    public override void Avanzar(IContenedorConDatos mapa, int dt)
    {
    }

    public override bool Reacciona(IContenedorConDatos mapa)
    {
        return false;
    }*/
}
