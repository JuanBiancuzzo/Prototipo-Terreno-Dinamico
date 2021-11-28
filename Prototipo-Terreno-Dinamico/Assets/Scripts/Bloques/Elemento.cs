using System;
using UnityEngine;
using System.Collections.Generic;

public abstract class Elemento : IContenible
{
    static Vector3Int gravedad = new Vector3Int(0, -1, 0);

    static int m_minimoValor = 0, m_maximoValor = 100;

    public Vector3Int m_posicion, m_velocidad, m_aceleracion;
    protected int m_densidad, m_temperatura, m_iluminacion;

    protected Color m_color;

    public Elemento(Vector3Int posicion)
    {
        m_posicion = posicion;
        m_velocidad = Vector3Int.zero;
        m_aceleracion = Vector3Int.zero;
        m_densidad = 50;
        m_temperatura = 273;
        m_iluminacion = 0;

        m_color = new Color(0, 0, 0, 1);
    }

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
    }
}
