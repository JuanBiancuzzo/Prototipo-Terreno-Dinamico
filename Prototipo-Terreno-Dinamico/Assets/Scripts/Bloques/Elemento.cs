using System;
using UnityEngine;
using System.Collections.Generic;

public abstract class Elemento
{
    static protected int m_minimoValor = 0, m_maximoValor = 100;

    protected uint id;

    public Vector3Int m_posicion;
    protected int m_densidad, m_temperatura, m_iluminacion;

    protected Color m_color;

    protected IConetenedorGeneral m_mundo;

    public Elemento(Vector3Int posicion, IConetenedorGeneral mundo)
    {
        m_posicion = posicion;
        m_densidad = 55;
        m_temperatura = 273;
        m_iluminacion = 0;

        m_color = new Color(0, 0, 0, 1);

        m_mundo = mundo;
    }

    public abstract void Avanzar(int dt);

    public abstract void Reaccionar();

    public abstract Elemento Expandir(Vector3Int posicion);

    public abstract bool PermiteDesplazar();

    public abstract bool PermiteIntercambiar();

    public abstract void Desplazar();

    // tal vez tener un metodo que en globe la idea de que un elemento quiere estar en esa posicion
    // dificultad es que depende de que material estemos hablando entonces puede ser una paja

    public void Intercambiar(Elemento elemento)
    {
        m_mundo.Intercambiar(this, elemento);
    }

    public bool Vacio()
    {
        return m_densidad == 0;
    }

    public bool MismoElemento(Elemento elemento)
    {
        return id == elemento.id;
    }

    public abstract bool MismoTipo(Elemento elemento);

    public abstract bool MismoTipo(Solido solido);
    public abstract bool MismoTipo(Liquido liquido);
    public abstract bool MismoTipo(Gaseoso gaseoso);

    public virtual bool Visible()
    {
        return true;
    }

    public float GetValor()
    {
        return (Visible()) ? Mathf.InverseLerp(m_minimoValor, m_maximoValor, m_densidad) : 0.0f;
    }

    public Color GetColor()
    {
        return ModificarColor();
    }

    protected virtual Color ModificarColor()
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

    public static Elemento ElementoConMayorDensidad(Elemento e1, Elemento e2)
    {
        if (e1 == null && e2 == null)
            return null;

        if (e1 == null)
            return e2;
        if (e2 == null)
            return e1;

        return (e1.m_densidad > e2.m_densidad) ? e1 : e2;
    }
        /*
        public abstract void Avanzar(IContenedorConDatos mapa, int dt);

        // afectar a al elemento que tenemos en el camino
        public abstract void ActuanEnElemento(Elemento elemento, int dt);

        public virtual void ActuarEnOtro(Solido elemento, int dt)
        {
        }

        public virtual void ActuarEnOtro(Liquido elemento, int dt)
        {
        }

        public virtual void ActuarEnOtro(Gaseoso elemento, int dt)
        {
        }

        public virtual bool Visible()
        {
            return true;
        }

        // como afecta el hecho de lo ultimo que hicimos - tal vez no sea necesario
        public abstract bool Reacciona(IContenedorConDatos mapa);

        // si dejamos pasar un elemento
        public virtual bool PermitoIntercambiar(Solido elemento, int dt)
        {
            return false;
        }

        public virtual bool PermitoIntercambiar(Liquido elemento, int dt)
        {
            return false;
        }

        public virtual bool PermitoIntercambiar(Gaseoso elemento, int dt)
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
            m_aceleracion *= 0;
        }

        public override float GetValor()
        {
            return (Visible()) ? Mathf.InverseLerp(m_minimoValor, m_maximoValor, m_densidad) : 0.0f; 
        }

        public override Color GetColor()
        {
            return ModificarColor();
        }

        protected virtual Color ModificarColor()
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
        }*/
}
