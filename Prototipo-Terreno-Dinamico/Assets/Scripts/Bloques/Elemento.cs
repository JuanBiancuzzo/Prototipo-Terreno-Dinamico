using System;
using UnityEngine;
using System.Collections.Generic;

public abstract class Elemento : ElementoMagico, ITenerDatos
{
    static protected int m_minimoValor = 0, m_maximoValor = 100;
    static protected int m_minimoLuz = 0, m_maximoLuz = 15;

    public Vector3Int m_posicion;

    public bool m_actualizado = false;
    protected Mundo m_mundo;

    public Elemento(Vector3Int posicion, Mundo mundo) 
        : base(13, new Color(1, 1, 1, 1), 290, 25, 50) // iluminacion, color, temperatura, concentracion y constitucion
    {
        m_posicion = posicion;
        m_mundo = mundo;
    }

    protected void NuevoColor(Color color)
    {
        m_alfa.NuevoValor(color.a);
        m_rgb.NuevoValor(color);
        ActualizarColor();
    }

    public void AntesDeAvanzar()
    {
        EmpezarAActualizar();
        ExpandirTemperatura();
    }

    public abstract void Avanzar(int dt);

    public void DespuesDeAvanzar()
    {
        ExpandirLuz();
        Reaccionar();
    }

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
                    temperaturaTotal += elemento.TemperaturaValor;
                    cantidad++;
                }

        int temperaturaPromedio = (cantidad == 0) ? 0 : temperaturaTotal / cantidad;
        m_temperatura.NuevoValor(temperaturaPromedio);
    }

    public void ExpandirLuz()
    {
        if (Emisor())
            return;

        List<Vector3Int> opciones = new List<Vector3Int>()
        {
            new Vector3Int( 1, 0, 0), new Vector3Int(0,  1, 0), new Vector3Int(0, 0,  1),
            new Vector3Int(-1, 0, 0), new Vector3Int(0, -1, 0), new Vector3Int(0, 0, -1)
        };

        foreach (Vector3Int desfase in opciones)
        {
            Elemento elemento = m_mundo.EnPosicion(m_posicion + desfase);
            if (elemento == null)
                continue;

            int iluminacionElemento = elemento.IluminacionValor - 3;
            ActualizarLuz(Mathf.Max(iluminacionElemento, IluminacionValor));
        }
    }

    protected virtual void ActualizarLuz(int luz)
    {
        int nuevoValor = (Visible()) ? 0 : luz;
        m_iluminacion.NuevoValor(nuevoValor);
    }

    public void Actualizado()
    {
        m_actualizado = true;
    }

    public void EmpezarAActualizar()
    {
        m_actualizado = false;
        if (!Emisor())
            m_iluminacion.NuevoValor(0);
    }

    public bool EstaActualizado()
    {
        return m_actualizado;
    }

    public int PasarTemperatura(Elemento elemento, int cantidad)
    {
        int cantidadADar = cantidad;
        if (TemperaturaValor < cantidad)
            cantidadADar = TemperaturaValor;

        m_temperatura.Disminuir(cantidadADar);
        elemento.m_temperatura.Aumentar(cantidad);

        return cantidad - cantidadADar;
    }

    public int RecibirTemperatura(Elemento elemento, int cantidad)
    {
        return elemento.PasarTemperatura(this, cantidad);
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

        elementoDelMismoElemento.Sort((a, b) => 
            b.ConcentracionValor.CompareTo(a.ConcentracionValor)
        );

        for (int i = 0; i < elementoDelMismoElemento.Count; i++)
        {
            Elemento elemento = elementoDelMismoElemento[i];

            int cantidadADar = DarCantidad(ConcentracionValor / (elementoDelMismoElemento.Count - i));
            int cantidadExtra = elemento.Agregar(cantidadADar);
            Agregar(cantidadExtra);
        }

        if (ConcentracionValor > 0)
            Debug.LogError("Se esta perdiendo: " + ConcentracionValor + " concentracion");
    }

    public virtual bool PermiteMoverse(Elemento elemento)
    {
        return true;
    }

    public abstract int CantidadADar();

    public virtual int DarCantidad(int cantidad)
    {
        cantidad = Mathf.Min(cantidad, ConcentracionValor);
        m_concentracion.Disminuir(cantidad);
        return cantidad;
    }

    public virtual int Agregar(int cantidadDensidad)
    {
        m_concentracion.Aumentar(cantidadDensidad);
        int resto = ConcentracionValor - m_maximoValor;
        if (resto > 0)
            m_concentracion.NuevoValor(m_maximoValor);
        return (resto < 0) ? 0 : resto;
    }

    public virtual int MaximoParaRecibir()
    {
        return m_maximoValor - ConcentracionValor;
    }

    // tal vez tener un metodo que en globe la idea de que un elemento quiere estar en esa posicion
    // dificultad es que depende de que material estemos hablando entonces puede ser una paja

    public void Intercambiar(Elemento elemento)
    {
        m_mundo.Intercambiar(this, elemento);
    }

    public bool Vacio()
    {
        return ConcentracionValor == 0;
    }

    public bool MismoElemento(Elemento elemento)
    {

        Debug.Log("Tenemos: " + GetType() + " contra " + elemento.GetType());

        return GetType() == elemento.GetType();
    }

    public abstract bool MismoTipo(Elemento elemento);

    public abstract bool MismoTipo(Solido solido);
    public abstract bool MismoTipo(Liquido liquido);
    public abstract bool MismoTipo(Gaseoso gaseoso);

    public virtual void DividirAtributos(Elemento otro)
    {
        IgualarAtributos(otro);
        otro.m_concentracion.NuevoValor(ConcentracionValor / 2);
        m_concentracion.Disminuir(otro.ConcentracionValor);
        otro.Actualizado();
    }

    public void IgualarAtributos(Elemento otro)
    {
        otro.m_concentracion = m_concentracion;
        otro.m_temperatura = m_temperatura;
        otro.m_iluminacion = m_iluminacion;
        otro.m_alfa = m_alfa;
        otro.m_rgb = m_rgb;
        otro.ActualizarColor();
    }

    public bool Visible()
    {
        return AlfaValor > 0;
    }

    public bool Translucido()
    {
        return AlfaValor < 1;
    }

    public bool Solido(Constitucion entidad)
    {
        return !m_consitucion.Atraviesa(entidad);
    }

    public virtual bool Emisor()
    {
        return false;
    }

    public float GetValor(TipoMaterial tipoMaterial, float defaultValor)
    {
        return (DarDefualt(tipoMaterial)) ? defaultValor : ValorInterpolado();
    }

    private float ValorInterpolado()
    {
        float t = Mathf.InverseLerp(m_minimoValor, m_maximoValor, ConcentracionValor);
        return Mathf.Lerp(0.19f, 1f, t);
    }

    public Color GetColor(TipoMaterial tipoMaterial, Color colorDefault)
    {
        return (DarDefualt(tipoMaterial)) ? colorDefault : ColorValor;
    }

    public int GetIluminacion()
    {
        return IluminacionValor;
    }

    public float GetColision(Constitucion otro, float defaultColision)
    {
        return (Solido(otro)) ? ValorInterpolado() : defaultColision;
    }

    private bool DarDefualt(TipoMaterial tipoMaterial)
    {
        bool seTieneQueDar = false;

        switch (tipoMaterial)
        {
            case TipoMaterial.Opaco: seTieneQueDar = !(Visible() && !Translucido()); break;
            case TipoMaterial.Translucido: seTieneQueDar = !(Visible() && Translucido()); break;
            case TipoMaterial.Trnasparente: seTieneQueDar = Visible(); break;
        }

        return seTieneQueDar;
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
        if (e1 == null)
            return e2;
        if (e2 == null)
            return e1;

        return (e1.ConcentracionValor >= e2.ConcentracionValor) ? e1 : e2;
    }

    public static Elemento ElementoConMayorConcentracion(Vector3Int posicion, Mundo mundo, Elemento excepcion = null)
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
}
