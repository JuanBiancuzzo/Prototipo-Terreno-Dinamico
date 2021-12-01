using System;
using UnityEngine;
using System.Collections.Generic;

public abstract class Elemento : ITenerDatos
{
    static protected int m_minimoValor = 0, m_maximoValor = 100;
    static protected int m_minimoLuz = 0, m_maximoLuz = 15;
    static float m_defaultValor = 0;
    static Color m_defualtColor = Color.white;

    protected uint id;

    public Vector3Int m_posicion;

    public int m_densidad;
    public ValorTemporal m_temperatura, m_iluminacion;

    protected Color m_color;
    public bool m_actualizado = false;

    protected IConetenedorGeneral m_mundo;

    public Elemento(Vector3Int posicion, IConetenedorGeneral mundo)
    {
        m_posicion = posicion;
        m_color = new Color(0, 0, 0, 1);
        m_mundo = mundo;

        m_densidad = 25;
        m_temperatura = new ValorTemporal(291);
        m_iluminacion = new ValorTemporal(0);
    }

    public void Actuar(int dt)
    {
        ExpandirTemperatura();
        Avanzar(dt);
        ExpandirLuz();
    }

    public abstract void Avanzar(int dt);

    protected void ExpandirTemperatura()
    {
        int cantidad = 0;
        int temperaturaTotal = 0;

        for (int x = -1; x <= 1; x++)
            for (int y = -1; y <= 1; y++)
                for (int z = -1; z <= 1; z++)
                {
                    Vector3Int desfase = new Vector3Int(x, y, z);
                    Elemento elemento = m_mundo.EnPosicion(m_posicion + desfase);
                    if (elemento == null)
                        continue;
                    temperaturaTotal += elemento.m_temperatura.Valor();
                    cantidad++;
                }

        int temperaturaPromedio = temperaturaTotal / cantidad;
        m_temperatura.NuevoValor(temperaturaPromedio);
    }

    protected void ExpandirLuz()
    {
        if (Emisor())
            return;

        List<Vector3Int> opciones = new List<Vector3Int>()
        {
            new Vector3Int( 1, 0, 0), new Vector3Int(0,  1, 0), new Vector3Int(0, 0,  1),
            new Vector3Int(-1, 0, 0), new Vector3Int(0, -1, 0), new Vector3Int(0, 0, -1)
        };

        int mayorIluminacion = m_iluminacion.Valor();

        foreach (Vector3Int desfase in opciones)
        {
            Elemento elemento = m_mundo.EnPosicion(m_posicion + desfase);
            if (elemento == null)
                continue;

            int iluminacionElemento = elemento.m_iluminacion.Valor();
            mayorIluminacion = Mathf.Max(iluminacionElemento - 2, mayorIluminacion);
        }
        
        m_iluminacion.NuevoValor(mayorIluminacion);
    }

    public void Actualizado()
    {
        m_actualizado = true;
    }

    public void EmpezarAActualizar()
    {
        m_actualizado = false;
        m_temperatura.Actualizar();
        m_iluminacion.Actualizar();
    }

    public bool EstaActualizado()
    {
        return m_actualizado;
    }

    public abstract void Reaccionar();

    public abstract Elemento Expandir(Vector3Int posicion);

    public abstract bool PermiteDesplazar();

    public abstract bool PermiteIntercambiar();

    public abstract void Desplazar();

    protected void DesplazarEntreExtremos(Extremo extremo)
    {
        List<Elemento> elementoDelMismoElemento = new List<Elemento>();

        Vector3Int minimo = extremo.m_minimo, maximo = extremo.m_maximo;

        for (int x = minimo.x; x <= maximo.x; x++)
            for (int y = minimo.y; y <= maximo.y; y++)
                for (int z = minimo.z; z <= maximo.z; z++)
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
            Debug.LogError("Se esta perdiendo: " + m_densidad + " densidad");
    }

    public abstract int CantidadADar();

    public virtual int DarCantidad(int cantidad)
    {
        cantidad = Mathf.Min(cantidad, m_densidad);
        m_densidad -= cantidad;
        return cantidad;
    }

    public virtual int Agregar(int cantidadDensidad)
    {
        m_densidad += cantidadDensidad;
        int resto = m_densidad - m_maximoValor;
        if (resto > 0)
            m_densidad = m_maximoValor;
        return (resto < 0) ? 0 : resto;
    }

    public virtual int MaximoParaRecibir()
    {
        return m_maximoValor - m_densidad;
    }

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

    public virtual void DividirAtributos(Elemento otro)
    {
        otro.m_densidad = m_densidad / 2;
        m_densidad -= otro.m_densidad;
        otro.m_iluminacion = m_iluminacion;
        otro.Actualizado();
    }

    public void IgualarAtributos(Elemento otro)
    {
        otro.m_densidad = m_densidad;
        otro.m_temperatura = m_temperatura;
        otro.m_iluminacion = m_iluminacion;
    }

    public virtual bool Visible()
    {
        return true;
    }

    public virtual bool Translucido()
    {
        return false;
    }

    public virtual bool Emisor()
    {
        return false;
    }

    public float GetValor(TipoMaterial tipoMaterial)
    {
        switch (tipoMaterial)
        {
            case TipoMaterial.Opaco:
                if (!(Visible() && !Translucido()))
                    return m_defaultValor;
                break;
            case TipoMaterial.Translucido:
                if (!(Visible() && Translucido()))
                    return m_defaultValor;
                break;
            case TipoMaterial.Trnasparente:
                if (Visible())
                    return m_defaultValor;
                break;
        }

        float t = Mathf.InverseLerp(m_minimoValor, m_maximoValor, m_densidad);
        return Mathf.Lerp(0.19f, 1f, t);
    }

    public Color GetColor(TipoMaterial tipoMaterial)
    {
        switch (tipoMaterial)
        {
            case TipoMaterial.Opaco:
                if (!(Visible() && !Translucido()))
                    return m_defualtColor;
                break;
            case TipoMaterial.Translucido:
                if (!(Visible() && Translucido()))
                    return m_defualtColor;
                break;
            case TipoMaterial.Trnasparente:
                if (Visible())
                    return m_defualtColor;
                break;
        }

        Color colorFinal = ModificarColor();

        if (!Emisor())
        {
            float luz = Mathf.InverseLerp(m_minimoLuz, m_maximoLuz, m_iluminacion.Valor());
            float alpha = colorFinal.a;
            colorFinal *= luz;
            colorFinal.a = alpha;
        }

        return colorFinal;
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

    public static Elemento ComparacionEntreElemento(Elemento e1, Elemento e2)
    {
        if (e1 == null && e2 == null)
            return null;

        if (e1 == null)
            return e2;
        if (e2 == null)
            return e1;

        return (e1.m_densidad > e2.m_densidad) ? e1 : e2;
    }

    public static Elemento ElementoConMayorDensidad(Vector3Int posicion, IConetenedorGeneral mundo, Elemento excepcion = null)
    {
        Elemento elementoConMayorDensidad = null;

        for (int x = -1; x <= 1; x++)
            for (int y = -1; y <= 1; y++)
                for (int z = -1; z <= 1; z++)
                {
                    Vector3Int desfase = new Vector3Int(x, y, z);
                    Elemento elemento = mundo.EnPosicion(posicion + desfase);

                    if (excepcion != null && elemento != null)
                        if (excepcion.MismoTipo(elemento))
                            continue;

                    elementoConMayorDensidad = ComparacionEntreElemento(elementoConMayorDensidad, elemento);
                }

        return elementoConMayorDensidad;
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
