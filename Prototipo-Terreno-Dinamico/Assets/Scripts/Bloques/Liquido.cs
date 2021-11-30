using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Liquido : Elemento
{
    protected const int gravedad = -1;

    protected int m_velocidad, m_aceleracion;
    protected int m_ficDinamica, m_ficEstatica;

    protected Liquido(Vector3Int posicion, IConetenedorGeneral mundo) : base(posicion, mundo)
    {
        m_ficDinamica = 2;
        m_ficEstatica = 4;
    }

    public override void Avanzar(int dt)
    {
        ActualizarVelocidad(dt);

        foreach (Vector3Int desfase in Opciones())
        {
            Elemento elemento = m_mundo.EnPosicion(m_posicion + desfase);
            if (elemento == null)
                continue;

            if (MismoElemento(elemento))
            {
                Liquido solido = (Liquido)elemento;

                int cantidadADar = CantidadADar();
                int cantidadExtra = solido.Agregar(cantidadADar);
                Agregar(cantidadExtra);

                if (cantidadExtra > 0)
                    continue;
            }
            else if (elemento.PermiteDesplazar())
            {
                elemento.Desplazar();
                Elemento remplazo = Expandir(m_posicion + desfase);
                m_mundo.Insertar(remplazo);
            }
            else if (elemento.PermiteIntercambiar())
            {
                elemento.Intercambiar(this);
            }
            else
            {
                continue;
            }

            break;
        }
    }

    private IEnumerable<Vector3Int> Opciones()
    {
        yield return new Vector3Int(0, -1, 0);

        List<Vector3Int> diagonales = new List<Vector3Int>()
        {
            new Vector3Int( 1, -1, 1),  new Vector3Int(1, -1, 0), new Vector3Int( 1, -1, -1),
            new Vector3Int( 0, -1, 1),                            new Vector3Int( 0, -1, -1),
            new Vector3Int(-1, -1, 1), new Vector3Int(-1, -1, 0), new Vector3Int(-1, -1, -1)
        };

        int opciones = diagonales.Count;
        for (int i = 0; i < opciones; i++)
        {
            int index = Random.Range(0, diagonales.Count - 1);
            yield return diagonales[index];
            diagonales.RemoveAt(index);
        }
    }

    protected virtual int CantidadADar()
    {
        int cantidad = Mathf.Abs(m_velocidad) * 5;
        cantidad = Mathf.Max(cantidad, m_densidad);
        m_densidad -= cantidad;
        return cantidad;
    }

    protected void ActualizarVelocidad(int dt)
    {
        m_velocidad += m_aceleracion * dt;
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

    public override void Desplazar()
    {
        List<Liquido> liquidos = new List<Liquido>();

        for (int x = -1; x <= 1; x++)
            for (int y = -1; y <= 1; y++)
                for (int z = -1; z <= 1; z++)
                {
                    if (x == 0 && z == 0 && y == 0)
                        continue;

                    Elemento elemento = m_mundo.EnPosicion(m_posicion + new Vector3Int(x, y, z));
                    if (elemento != null && MismoElemento(elemento))
                        liquidos.Add((Liquido)elemento);
                }

        for (int i = 0; i < liquidos.Count; i++)
        {
            Liquido liquido = liquidos[i];

            int cantidadADar = m_densidad / (liquidos.Count - i);
            m_densidad -= cantidadADar;
            int cantidadExtra = liquido.Agregar(cantidadADar);
            Agregar(cantidadExtra);
        }

        if (m_densidad > 0)
            Debug.LogError("Mas densidad de lo que deberia");
    }

    public override bool PermiteDesplazar()
    {
        List<Liquido> liquidos = new List<Liquido>();

        for (int x = -1; x <= 1; x++)
            for (int y = -1; y <= 1; y++)
                for (int z = -1; z <= 1; z++)
                {
                    if (x == 0 && z == 0 && y == 0)
                        continue;

                    Elemento elemento = m_mundo.EnPosicion(m_posicion + new Vector3Int(x, y, z));
                    if (elemento != null && MismoElemento(elemento))
                        liquidos.Add((Liquido)elemento);
                }

        int cantidadAdimitida = 0;
        foreach (Liquido liquido in liquidos)
            cantidadAdimitida += liquido.MaximoParaRecibir();

        return m_densidad < cantidadAdimitida;
    }

    public override bool PermiteIntercambiar()
    {
        return false;
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
        return true;
    }

    public override bool MismoTipo(Gaseoso gaseoso)
    {
        return false;
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
