using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Mundo))]
public class ContenedorMundo : MonoBehaviour, IContenedor
{
    protected Mundo mundo;

    public Extremo m_extremo;
    public Vector3Int m_extension => m_extremo.m_maximo - m_extremo.m_minimo;
    Elemento[,,] m_elementos;
    public IGenerador m_generador;

    private void Awake()
    {
        mundo = GetComponent<Mundo>();
    }

    private void Start()
    {
        m_elementos = new Elemento[m_extension.x, m_extension.y, m_extension.z];

        for (int x = 0; x < m_extension.x; x++)
            for (int y = 0; y < m_extension.y; y++)
                for (int z = 0; z < m_extension.z; z++)
                {
                    int altura = m_generador.ValorEnPosicion(x, z, m_extremo.m_minimo.y, m_extremo.m_maximo.y);
                    m_elementos[x, y, z] = ElementoPorAltura(x, y, z, altura);
                }
    }

    private Elemento ElementoPorAltura(int x, int y, int z, int altura)
    {
        Vector3Int posicion = m_extremo.m_minimo + new Vector3Int(x, y, z);

        if (altura < posicion.y)
            return new Aire(posicion, mundo);
        else if (altura == posicion.y)
            return new Arena(posicion, mundo);
        else
            return new Concreto(posicion, mundo);
    }

    public bool Insertar(Elemento elemento)
    {
        if (elemento == null)
            return false;

        Vector3Int posicion = elemento.Posicion();
        if (!EnRango(posicion))
            return false;

        Elemento elementoEnPosicion = EnPosicion(posicion);
        if (elementoEnPosicion == null || elementoEnPosicion.Vacio())
        {
            AgregarEnPosicion(posicion, elemento);
            return true;
        }

        if (elemento.MismoElemento(elementoEnPosicion) && elementoEnPosicion.MaximoParaRecibir() >= elemento.ConcentracionValor)
        {
            int cantidadExtra = elementoEnPosicion.Agregar(elemento.ConcentracionValor);
            if (cantidadExtra > 0)
                Debug.LogWarning("Algo esta funcionando mal aca");
        }
        else if (elementoEnPosicion.PermiteDesplazar())
        {
            elementoEnPosicion.Desplazar();
            AgregarEnPosicion(posicion, elemento);
            return true;
        }

        return false;
    }
    private void AgregarEnPosicion(Vector3Int posicion, Elemento elemento)
    {
        Vector3Int posicionRelativa = PosicionRelativa(posicion);
        m_elementos[posicionRelativa.x, posicionRelativa.y, posicionRelativa.z] = elemento;
    }

    private Vector3Int PosicionRelativa(Vector3Int posicionMundo)
    {
        return posicionMundo - m_extremo.m_minimo;
    }

    public Elemento Eliminar(Vector3Int posicion)
    {
        if (!EnRango(posicion))
            return null;

        Elemento elementoConMayorDensidad = Elemento.ElementoConMayorConcentracion(posicion, mundo);
        if (elementoConMayorDensidad == null)
            return null;

        Elemento reemplazo = elementoConMayorDensidad.Expandir(posicion);
        Elemento actual = EnPosicion(posicion);

        AgregarEnPosicion(posicion, reemplazo);
        return actual;
    }

    public Elemento Eliminar(Elemento elemento)
    {
        return (elemento == null) ? null : Eliminar(elemento.Posicion());
    }

    public Elemento EnPosicion(Vector3Int posicion)
    {
        if (!EnRango(posicion))
            return null;

        Vector3Int posicionRelativa = PosicionRelativa(posicion);
        return m_elementos[posicionRelativa.x, posicionRelativa.y, posicionRelativa.z];
    }

    public bool EnRango(Vector3Int posicion)
    {
        Vector3Int posicionRelativa = PosicionRelativa(posicion);
        for (int i = 0; i < 3; i++)
            if (posicionRelativa[i] < 0 || posicionRelativa[i] >= m_extension[i])
                return false;
        return true;
    }

    public bool EnRango(Elemento elemento)
    {
        return (elemento == null) ? false : EnRango(elemento.Posicion());
    }

    public bool Intercambiar(Vector3Int origen, Vector3Int destino)
    {
        if (!EnRango(origen) || !EnRango(destino))
            return false;

        Elemento elementoOrigen = EnPosicion(origen);
        Elemento elementoDestino = EnPosicion(destino);

        AgregarEnPosicion(destino, elementoOrigen);
        AgregarEnPosicion(origen, elementoDestino);

        elementoOrigen?.ActualizarPosicion(destino);
        elementoDestino?.ActualizarPosicion(origen);

        return true;
    }

    public bool Intercambiar(Elemento elementoOrigen, Elemento elementoDestino)
    {
        if (elementoOrigen == null || elementoDestino == null)
            return false;
        return Intercambiar(elementoOrigen.Posicion(), elementoDestino.Posicion());
    }

    public bool Reemplazar(Elemento elemento, Elemento reemplazo)
    {
        if (elemento == null | reemplazo == null)
            return false;

        Vector3Int posicion = PosicionRelativa(elemento.Posicion());
        m_elementos[posicion.x, posicion.y, posicion.z] = reemplazo;

        return true;
    }
}
