using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gaseoso : Elemento
{
    protected Gaseoso(Vector3Int posicion, IConetenedorGeneral mundo) : base(posicion, mundo)
    {
        m_densidad = 5;
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

        elementoDelMismoElemento.Sort((a, b) => b.m_densidad.CompareTo(a.m_densidad));

        for (int i = 0; i < elementoDelMismoElemento.Count; i++)
        {
            Elemento elemento = elementoDelMismoElemento[i];

            int cantidadADar = DarCantidad(m_densidad / (elementoDelMismoElemento.Count - i));
            int cantidadExtra = elemento.Agregar(cantidadADar);
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

    public override int CantidadADar()
    {
        int cantidad = m_densidad / 5;
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
