using System;
using UnityEngine;
using System.Collections.Generic;

public abstract class Elemento : IContenible
{
    Vector3Int m_posicion, m_velocidad, m_aceleracion;
    int m_densidad, m_temperatura, m_filtracion, m_iluminacion;
    int m_ficDinamica, m_ficEstatica;
    Vector3Int m_estabilidad;

    // tener un callback al falling sand, para que sepa que necesita actualizarse
    Action<Elemento> necesitoActualizar;

    float m_valor;
    Color m_color;

    public void Avanzar(IContenedorConDatos mapa, int dt)
    {
        ActualizarVelocidad(dt);

        List<Vector3Int> posiciones = PosicionesEnMovimiento(dt);
        Vector3Int direccion = Direccion(dt);

        for (int i = 0; i < posiciones.Count; i++)
        {
            ActualizarAlrededores(posiciones[i], direccion, mapa);

            Elemento elemento = (Elemento)mapa.EnPosicion(posiciones[i]);
            if (elemento == null)
            {
                ActualizarPosicion(posiciones[i]);
                continue;
            }

            bool pudeIntercambiar = Intercambiar(elemento, dt);
            if (!pudeIntercambiar)
                break;

            ActualizarPosicion(posiciones[i]);
        }

        if (Reacciona())
            necesitoActualizar.Invoke(this);
    }

    private void ActualizarAlrededores(Vector3Int posicion, Vector3Int direccion, IContenedorConDatos mapa)
    {
        List<Vector3Int> posibilidades;

        if (direccion == Vector3.zero)
            posibilidades = new List<Vector3Int>
            {
                new Vector3Int(0, 1, 0), new Vector3Int(0, -1, 0),
                new Vector3Int(1, 0, 0), new Vector3Int(-1, 0, 0),
                new Vector3Int(0, 0, 1), new Vector3Int(0, 0, -1)
            };
        else
            posibilidades = AlrededoresDeDireccion(direccion);

        foreach (Vector3Int def in posibilidades)
        {
            Elemento elemento = (Elemento)mapa.EnPosicion(def + posicion);
            elemento?.necesitoActualizar(elemento);
        }
    }

    private List<Vector3Int> AlrededoresDeDireccion(Vector3Int direccion)
    {
        List<Vector3Int> posibilidades = new List<Vector3Int>();
        for (int x = -1; x <= 1; x++)
            for (int y = -1; y <= 1; y++)
                for (int z = -1; z <= 1; z++)
                {
                    if (x == 0 && y == 0 && z == 0)
                        continue;

                    if (direccion.x * x + direccion.y * y + direccion.z * z == 0)
                        posibilidades.Add(new Vector3Int(x, y, z));
                }
        return posibilidades;
    }

    /* aca tener en cuenta la reduccion de velocidad al moverse en 
     * algo de mayor densidad, y tambien tener en cuenta que uno
     * puede dejar pasar un material que tenga cierta velocidad
     */
    protected abstract bool Intercambiar(Elemento elemento, int dt);

    protected abstract bool Reacciona();

    public void AplicarAceleracion(Vector3Int aceleracionNueva)
    {
        m_aceleracion += aceleracionNueva;
    }

    protected void ActualizarVelocidad(int dt)
    {
        m_velocidad += m_aceleracion * dt;
    }

    protected List<Vector3Int> PosicionesEnMovimiento(int dt)
    {
        Vector3Int inicio = m_posicion;
        Vector3Int direccion = m_velocidad * dt;
        Vector3Int final = inicio + direccion;

        List<Vector3Int> resultado = Mathfs.PosicionesEntre(inicio, final);
        return (resultado.Count > 0) ? resultado : null;
    }

    private Vector3Int Direccion(int dt)
    {
        Vector3Int velocidad = m_velocidad * dt;
        Vector3Int min = new Vector3Int(-1, -1, -1), max = new Vector3Int(1, 1, 1);

        velocidad.Clamp(min, max);

        return velocidad;
    }

    public float GetValor()
    {
        return m_valor;
    }

    public Color GetColor()
    {
        return m_color;
    }

    public Vector3Int Posicion()
    {
        return m_posicion;
    }

    public void ActualizarPosicion(Vector3Int posicionNueva)
    {
        m_posicion = posicionNueva;
    }
}
