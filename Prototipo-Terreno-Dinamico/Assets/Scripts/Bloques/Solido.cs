using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Solido : Elemento
{
    protected const int gravedad = -1;

    protected int m_velocidad, m_aceleracion;
    protected Vector3Int m_estabilidad;
    protected int m_ficDinamica, m_ficEstatica, m_filtracion;

    protected int m_flowRate;

    protected Solido(Vector3Int posicion, Mundo mundo) : base(posicion, mundo)
    {
        m_estabilidad = Vector3Int.zero;
        m_ficDinamica = 2;
        m_ficEstatica = 4;
        m_filtracion = 1;
        m_velocidad = 0;
        m_aceleracion = 0;
        m_flowRate = 25;
    }

    public override void Avanzar(int dt)
    {
        if (Vacio())
            return;

        ActualizarVelocidad(dt);

        foreach (Vector3Int desfase in Opciones())
        {
            Elemento elemento = m_mundo.EnPosicion(m_posicion + desfase);
            if (elemento == null)
                continue;

            if (MismoElemento(elemento))
            {                
                Solido solido = (Solido)elemento;

                if (solido.MaximoParaRecibir() == 0)
                    continue;

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

    protected void ActualizarVelocidad(int dt)
    {
        m_velocidad += (m_aceleracion + gravedad) * dt;
    }

    public override bool PermiteMoverse(Elemento elemento)
    {
        return ConcentracionValor * 5 < elemento.ConcentracionValor * 2;
    }

    public override int CantidadADar()
    {
        return DarCantidad(m_flowRate);
    }

    public override void Desplazar()
    {
        Extremo extremo = new Extremo(new Vector3Int(-1, -1, -1), new Vector3Int(1, 0, 1));
        DesplazarEntreExtremos(extremo);
    }

    public override bool PermiteDesplazar()
    {
        Extremo extremo = new Extremo(new Vector3Int(-1, -1, -1), new Vector3Int(1, 0, 1));
        int cantidadAdmitida = CantidadAdmitidaEnExtremos(extremo);
        return ConcentracionValor < cantidadAdmitida * 0.9f;

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
        return true;
    }

    public override bool MismoTipo(Liquido liquido)
    {
        return false;
    }

    public override bool MismoTipo(Gaseoso gaseoso)
    {
        return false;
    }

    public override void DividirAtributos(Elemento otro)
    {
        base.DividirAtributos(otro);
        Solido solido = (Solido)otro;
        solido.m_velocidad = m_velocidad;
    }
}
