using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Liquido : Elemento
{
    protected const int gravedad = -1;

    protected int m_velocidad, m_aceleracion;
    protected int m_ficDinamica, m_ficEstatica;
    protected int m_flowRate;

    protected Liquido(Vector3Int posicion, Mundo mundo) : base(posicion, mundo)
    {
        m_ficDinamica = 2;
        m_ficEstatica = 4;
        m_velocidad = 0;
        m_aceleracion = 0;
        m_flowRate = 50;
        m_consitucion.NuevoValor(60);
    }

    public override void Avanzar(int dt)
    {
        if (Vacio() || !Flowing())
            return;

        ActualizarVelocidad(dt);

        foreach (Vector3Int desfase in Opciones())
        {
            Elemento elemento = m_mundo.EnPosicion(m_posicion + desfase);
            if (elemento == null)
                continue;

            if (MismoElemento(elemento) && elemento.MaximoParaRecibir() > 0)
            {
                Liquido solido = (Liquido)elemento;

                int cantidadADar = CantidadADar();
                int cantidadExtra = solido.Agregar(cantidadADar);
                Agregar(cantidadExtra);

                if (cantidadExtra > 0)
                    continue;
            }
            else if (elemento.PermiteDesplazar() && elemento.PermiteMoverse(this))
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

        foreach (Vector3Int opcion in Utilidades.Dar<Vector3Int>(diagonales))
            yield return opcion;

        diagonales = new List<Vector3Int>()
        {
            new Vector3Int( 1, 0, 1),  new Vector3Int(1, 0, 0), new Vector3Int( 1, 0, -1),
            new Vector3Int( 0, 0, 1),                           new Vector3Int( 0, 0, -1),
            new Vector3Int(-1, 0, 1), new Vector3Int(-1, 0, 0), new Vector3Int(-1, 0, -1)
        };

        foreach (Vector3Int opcion in Utilidades.Dar<Vector3Int>(diagonales))
            yield return opcion;
    }

    private bool Flowing()
    {
        // pensar una forma para determinarlo
        return true;
    }

    protected void ActualizarVelocidad(int dt)
    {
        m_velocidad += (m_aceleracion + gravedad) * dt;
    }

    public override int CantidadADar()
    {
        int cantidad = Mathf.Abs(m_velocidad) * m_flowRate;
        return DarCantidad(cantidad);
    }

    public override void Desplazar()
    {
        Extremo extremo = new Extremo(new Vector3Int(-1, -1, -1), new Vector3Int(1, 1, 1));
        DesplazarEntreExtremos(extremo);
    }

    public override bool PermiteDesplazar()
    {
        List<Elemento> elementoDelMismoElemento = new List<Elemento>();

        for (int x = -1; x <= 1; x++)
            for (int y = -1; y <= 1; y++)
                for (int z = -1; z <= 1; z++)
                {
                    if (x == 0 && z == 0 && y == 0)
                        continue;

                    Elemento elemento = m_mundo.EnPosicion(m_posicion + new Vector3Int(x, y, z));
                    if (elemento != null && MismoElemento(elemento) && elemento.MaximoParaRecibir() > 0)
                        elementoDelMismoElemento.Add(elemento);
                }

        int cantidadAdimitida = 0;
        foreach (Elemento elemento in elementoDelMismoElemento)
            cantidadAdimitida += elemento.MaximoParaRecibir();

        return ConcentracionValor < cantidadAdimitida;
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
    public override void DividirAtributos(Elemento otro)
    {
        base.DividirAtributos(otro);
        Liquido liquido = (Liquido)otro;
        liquido.m_velocidad = m_velocidad;
    }
}
