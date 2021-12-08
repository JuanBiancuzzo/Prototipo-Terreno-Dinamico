using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Solido : Elemento
{
    protected Vector3Int m_estabilidad;
    protected int m_ficDinamica, m_ficEstatica;

    protected int m_flowRate;
    bool m_enMovimiento;

    protected Solido(Vector3Int posicion, Mundo mundo) : base(posicion, mundo)
    {
        m_estabilidad = Vector3Int.zero;

        m_ficDinamica = 70;
        m_ficEstatica = 50;

        m_flowRate = 25;
        m_enMovimiento = true;
    }

    public override void Avanzar(int dt)
    {
        if (Vacio())
        {
            m_enMovimiento = false;
            return;
        }

        if (TieneSoporte())
        {
            m_enMovimiento = false;
            return;
        }

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

        m_enMovimiento = true;
    }

    private bool TieneSoporte()
    {
        List<Vector3Int> opciones = new List<Vector3Int>()
        {
            new Vector3Int( 1, 0, 0), new Vector3Int(0, 0,  1),
            new Vector3Int(-1, 0, 0), new Vector3Int(0, 0, -1)
        };

        foreach (Vector3Int desfase in opciones)
        {
            Elemento elemento = m_mundo.EnPosicion(m_posicion + desfase);
            if (elemento == null || !MismoTipo(elemento))
                continue;

            bool loSoportan = ElementoPuedeSoportar((Solido)elemento);
            if (loSoportan)
                return true; 
        }

        return false;
    }

    protected virtual bool ElementoPuedeSoportar(Solido solido)
    {
        if (m_enMovimiento)
            return ConcentracionValor > solido.m_ficDinamica;
        return ConcentracionValor > solido.m_ficEstatica;
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
}
