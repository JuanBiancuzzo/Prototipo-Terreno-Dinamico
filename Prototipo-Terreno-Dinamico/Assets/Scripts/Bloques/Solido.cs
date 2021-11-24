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
        //Vector3Int direccion = Direccion(dt);

        foreach (Vector3Int posicion in PosicionesEnMovimiento(mapa, dt))
        {
            //Vector3Int posicionAnterior = m_posicion;

            Elemento elemento = (Elemento)mapa.EnPosicion(posicion);
            if (elemento == null)
            {
                mapa.Intercambiar(m_posicion, posicion);
                //ActualizarAlrededores(posicionAnterior, direccion, mapa);
                continue;
            }

            elemento.ActuanEnElemento(this, dt);

            bool puedoIntercambiar = elemento.PermitoIntercambiar(this, dt);
            if (puedoIntercambiar)
                mapa.Intercambiar(m_posicion, posicion);

            //ActualizarAlrededores(posicionAnterior, direccion, mapa);
        }

        if (Reacciona(mapa))
            NecesitoActualizar(this);
    }

    protected IEnumerable<Vector3Int> BuscarPosicionesDisponibles()
    {
        List<Vector3Int> posibilidades = new List<Vector3Int>
        {
            new Vector3Int(-1, 0, -1), new Vector3Int(-1, 0, 0), new Vector3Int(-1, 0, 1),
            new Vector3Int( 0, 0, -1),                           new Vector3Int( 0, 0, 1),
            new Vector3Int( 1, 0, -1), new Vector3Int( 1, 0, 0), new Vector3Int( 1, 0, 1),
        };

        for (int i = 0; i < 8; i++)
        {
            int index = Random.Range(0, posibilidades.Count - 1);
            yield return posibilidades[index];
            posibilidades.RemoveAt(index);
        }
    }

    private IEnumerable<Vector3Int> PosicionesEnMovimiento(IContenedorConDatos mapa, int dt)
    {
        Vector3Int inicio = m_posicion;
        Vector3Int final = inicio + m_velocidad * dt;
        Vector3Int direccion = final - inicio;
        int variable = Mathfs.MayorComponente(direccion);

        Vector3Int desfase = new Vector3Int(0, 0, 0);

        if (direccion[variable] != 0)
        {
            int desvios = 0;

            for (int v = 1; v <= Mathf.Abs(direccion[variable]) + desvios; v++)
            {
                int dirAvance = (Mathf.Sign(direccion[variable]) == 1) ? 1 : -1;
                int valorVariable = inicio[variable] + v * dirAvance;
                Vector3Int posicionNueva = new Vector3Int();
                for (int i = 0; i < 3; i++)
                {
                    float valor = ((float)direccion[i] * (valorVariable - inicio[variable])) / direccion[variable] + inicio[i];
                    posicionNueva[i] = (i == variable) ? valorVariable : Mathf.CeilToInt(valor);
                }

                posicionNueva += desfase;

                bool sePuedeMover = ElementoDejaIntercambiarEn(mapa, posicionNueva, dt);

                yield return posicionNueva;

                if (sePuedeMover)
                    continue;

                foreach (Vector3Int des in BuscarPosicionesDisponibles())
                {
                    if (!ElementoDejaIntercambiarEn(mapa, posicionNueva + des, dt))
                        continue;

                    yield return posicionNueva + des;

                    desfase += des;
                    desvios++;
                    break;
                }

                break;
            }
        }
    }

    protected bool ElementoDejaIntercambiarEn(IContenedorConDatos mapa, Vector3Int posicion, int dt)
    {
        if (!mapa.EnRango(posicion))
            return false;

        Elemento elemento = (Elemento)mapa.EnPosicion(posicion);
        return elemento == null || elemento.PermitoIntercambiar(this, dt);
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


    protected void ActualizarAlrededores(Vector3Int posicion, Vector3Int direccion, IContenedorConDatos mapa)
    {
        foreach (Vector3Int def in AlrededoresDeDireccion(direccion))
        {
            Elemento elemento = (Elemento)mapa.EnPosicion(def + posicion);
            elemento?.NecesitoActualizar(elemento);
        }
    }

    protected List<Vector3Int> AlrededoresDeDireccion(Vector3Int direccion)
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

    protected Vector3Int Direccion(int dt)
    {
        Vector3Int velocidad = m_velocidad * dt;
        Vector3Int min = new Vector3Int(-1, -1, -1), max = new Vector3Int(1, 1, 1);

        velocidad.Clamp(min, max);

        return velocidad;
    }
}
