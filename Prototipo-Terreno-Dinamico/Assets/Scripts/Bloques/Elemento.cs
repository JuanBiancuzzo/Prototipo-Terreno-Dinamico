using System;
using UnityEngine;
using System.Collections.Generic;

public abstract class Elemento : IContenible
{
    static Vector3Int gravedad = new Vector3Int(0, -1, 0);

    public Vector3Int m_posicion, m_velocidad, m_aceleracion;
    protected int m_densidad, m_temperatura, m_iluminacion;

    protected float m_valor;
    protected Color m_color;

    protected bool m_cambiePosicion, m_colisiono;

    public Elemento(Vector3Int posicion)
    {
        m_posicion = posicion;
        m_velocidad = Vector3Int.zero;
        m_aceleracion = Vector3Int.zero;
        m_densidad = 1;
        m_temperatura = 273;
        m_iluminacion = 0;

        m_valor = 0f;
        m_color = new Color(0, 0, 0, 1);
    }

    public void Avanzar(IContenedorConDatos mapa, int dt)
    {
        m_cambiePosicion = false;
        m_colisiono = false;
        ActualizarVelocidad(dt);

        List<Vector3Int> posiciones = PosicionesEnMovimiento(dt);
        Vector3Int direccion = Direccion(dt);

        foreach (Vector3Int posicion in posiciones)
        {
            ActualizarAlrededores(posicion, direccion, mapa);

            Elemento elemento = (Elemento)mapa.EnPosicion(posicion);
            bool puedoIntercambiar = true;
            if (elemento != null)
                puedoIntercambiar = elemento.Intercambiar(this, dt);

            if (!puedoIntercambiar)
            {
                m_colisiono = true;
                break;
            }

            puedoIntercambiar = mapa.Intercambiar(m_posicion, posicion);
            if (!puedoIntercambiar)
            {
                m_colisiono = true;
                break;
            }

            m_cambiePosicion = true;
        }

        if (Reacciona())
            NecesitoActualizar(this);
    }

    private void ActualizarAlrededores(Vector3Int posicion, Vector3Int direccion, IContenedorConDatos mapa)
    {
        List<Vector3Int> posibilidades;

        if (direccion == Vector3.zero)
        {
            posibilidades = new List<Vector3Int>
            {
                new Vector3Int(0, 1, 0), new Vector3Int(0, -1, 0),
                new Vector3Int(1, 0, 0), new Vector3Int(-1, 0, 0),
                new Vector3Int(0, 0, 1), new Vector3Int(0, 0, -1)
            };
        }
        else
        {
            posibilidades = AlrededoresDeDireccion(direccion);
        }

        foreach (Vector3Int def in posibilidades)
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
    protected virtual bool Intercambiar(Elemento elemento, int dt)
    {
        return true;
    }

    protected virtual bool Reacciona()
    {
        return false;
    }

    public void AplicarAceleracion(Vector3Int aceleracionNueva)
    {
        m_aceleracion += aceleracionNueva;
    }

    protected void ActualizarVelocidad(int dt)
    {
        m_velocidad += (m_aceleracion + gravedad) * dt;
        m_aceleracion.Set(0, 0, 0);
    }

    protected List<Vector3Int> PosicionesEnMovimiento(int dt)
    {
        Vector3Int inicio = m_posicion;
        Vector3Int direccion = m_velocidad * dt;
        Vector3Int final = inicio + direccion;

        return Mathfs.PosicionesEntre(inicio, final);
    }

    private Vector3Int Direccion(int dt)
    {
        Vector3Int velocidad = m_velocidad * dt;
        Vector3Int min = new Vector3Int(-1, -1, -1), max = new Vector3Int(1, 1, 1);

        velocidad.Clamp(min, max);

        return velocidad;
    }

    public override float GetValor()
    {
        return m_valor;
    }

    public override Color GetColor()
    {
        return m_color;
    }

    public override Vector3Int Posicion()
    {
        return m_posicion;
    }

    public override void ActualizarPosicion(Vector3Int posicionNueva)
    {
        m_posicion = posicionNueva;
    }
}
