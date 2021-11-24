using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Solido : Elemento
{
    protected Vector3Int m_estabilidad;
    protected int m_ficDinamica, m_ficEstatica, m_filtracion;

    protected Solido(Vector3Int posicion) : base(posicion)
    {
        m_estabilidad = Vector3Int.zero;
        m_ficDinamica = 2;
        m_ficEstatica = 4;
        m_filtracion = 1;
    }

    public override void ActuanEnElemento(Elemento elemento, int dt)
    {
        elemento.ActuarEnOtro(this, dt);
    }

    public override void Avanzar(IContenedorConDatos mapa, int dt)
    {
        ActualizarVelocidad(dt);
        Vector3Int direccion = Direccion(dt);

        Vector3Int desfase = Vector3Int.zero;

        foreach (Vector3Int posicion in PosicionesEnMovimiento(dt))
        {
            Vector3Int posicionAnterior = m_posicion;

            Elemento elemento = (Elemento)mapa.EnPosicion(posicion + desfase);
            if (elemento == null)
            {
                mapa.Intercambiar(m_posicion, posicion + desfase);
                ActualizarAlrededores(posicionAnterior, direccion, mapa);
                continue;
            }

            elemento.ActuanEnElemento(this, dt);

            bool puedoIntercambiar = elemento.PermitoIntercambiar(this, dt);
            if (puedoIntercambiar)
                puedoIntercambiar = mapa.Intercambiar(m_posicion, posicion + desfase);

            bool puedoMoverme = false;
            if (!puedoIntercambiar)
                puedoMoverme = MovimientoDiagonal(mapa, dt, posicion, ref desfase);

            if (!puedoMoverme)
                break;

            ActualizarAlrededores(posicionAnterior, direccion, mapa);
        }

        if (Reacciona(mapa))
            NecesitoActualizar(this);
    }

    private bool MovimientoDiagonal(IContenedorConDatos mapa, int dt, Vector3Int posicion, ref Vector3Int desfase)
    {
        bool puedoMoverme = false;
        foreach (Vector3Int des in BuscarPosicionesDisponibles())
        {
            Elemento elemento = (Elemento)mapa.EnPosicion(posicion + desfase + des);
            if (elemento == null || elemento.PermitoIntercambiar(this, dt))
            {
                bool puedoIntercambiar = mapa.Intercambiar(m_posicion, posicion + desfase + des);
                if (puedoIntercambiar)
                {
                    desfase += des;
                    puedoMoverme = true;
                    break;
                }
            }
        }
        return puedoMoverme;
    }

    protected IEnumerable<Vector3Int> BuscarPosicionesDisponibles()
    {
        for (int x = -1; x <= 1; x++)
            for (int z = -1; z <= 1; z++)
                if (x != 0 || z != 0)
                    yield return new Vector3Int(x, 0, z);
    }

    public override bool PermitoIntercambiar(Solido elemento, int dt)
    {
        return false;
    }

    public override bool PermitoIntercambiar(Liquido elemento, int dt)
    {
        return false;
    }

    public override bool PermitoIntercambiar(Gaseoso elemento, int dt)
    {
        return false;
    }

    public override bool Reacciona(IContenedorConDatos mapa)
    {
        return false;
    }


    private void ActualizarAlrededores(Vector3Int posicion, Vector3Int direccion, IContenedorConDatos mapa)
    {
        foreach (Vector3Int def in AlrededoresDeDireccion(direccion))
        {
            Elemento elemento = (Elemento)mapa.EnPosicion(def + posicion);
            elemento?.NecesitoActualizar(elemento);
        }
    }

    private List<Vector3Int> AlrededoresDeDireccion(Vector3Int direccion)
    {
        List<Vector3Int> posibilidades = new List<Vector3Int>();
        for (int x = -1; x <= 1; x++)
            for (int y = -1; y <= 1; y++)
                for (int z = -1; z <= 1; z++)
                {
                    if (!(x == 0 && y == 0 && z == 0))
                        posibilidades.Add(new Vector3Int(x, y, z));
                }

        return posibilidades;
    }

    private Vector3Int Direccion(int dt)
    {
        Vector3Int velocidad = m_velocidad * dt;
        Vector3Int min = new Vector3Int(-1, -1, -1), max = new Vector3Int(1, 1, 1);

        velocidad.Clamp(min, max);

        return velocidad;
    }

    private IEnumerable<Vector3Int> PosicionesEnMovimiento(int dt)
    {
        Vector3Int inicio = m_posicion;
        Vector3Int direccion = m_velocidad * dt;
        Vector3Int final = inicio + direccion;
        return Mathfs.PosicioneEntreYield(inicio, final);
    }
}
