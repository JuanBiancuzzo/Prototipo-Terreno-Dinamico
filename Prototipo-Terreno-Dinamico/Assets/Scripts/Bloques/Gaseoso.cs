using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gaseoso : Elemento
{
    protected Gaseoso(Vector3Int posicion, Mundo mundo) : base(posicion, mundo)
    {
        m_concentracion.NuevoValor(5);
        m_consitucion.NuevoValor(0);
    }

    public override void Avanzar(int dt)
    {
        if (Vacio())
            return;

        foreach (Vector3Int desfase in Opciones())
        {
            Elemento elemento = m_mundo.EnPosicion(m_posicion + desfase);
            if (elemento == null || !MismoTipo(elemento))
                continue;

            if (!elemento.PermiteIntercambiar())
                continue;

            Gaseoso gaseoso = (Gaseoso)elemento;
            if (Elemento.ComparacionEntreElemento(gaseoso, this) == gaseoso)
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
        Extremo extremo = new Extremo(new Vector3Int(-1, -1, -1), new Vector3Int(1, 1, 1));
        DesplazarEntreExtremos(extremo);
    }

    public override bool PermiteDesplazar()
    {
        Extremo extremo = new Extremo(new Vector3Int(-1, -1, -1), new Vector3Int(1, 1, 1));
        int cantidadAdmitida = CantidadAdmitidaEnExtremos(extremo);
        return ConcentracionValor < cantidadAdmitida;
    }

    public override int CantidadADar()
    {
        int cantidad = ConcentracionValor / 5;
        return DarCantidad(cantidad);
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
}
