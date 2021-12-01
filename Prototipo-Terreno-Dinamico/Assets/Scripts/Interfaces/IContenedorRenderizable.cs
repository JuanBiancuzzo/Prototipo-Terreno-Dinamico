using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IContenedorRenderizable : MonoBehaviour, IRenderizable, IContenedor
{
    public abstract Elemento Eliminar(Vector3Int posicion);
    public abstract Elemento Eliminar(Elemento elemento);
    public abstract Elemento EnPosicion(Vector3Int posicion);
    public abstract bool EnRango(Vector3Int posicion);
    public abstract bool EnRango(Elemento elemento);
    public abstract bool Insertar(Elemento elemento);
    public abstract bool Intercambiar(Vector3Int origen, Vector3Int destino);
    public abstract bool Intercambiar(Elemento elementoOrigen, Elemento elementoDestino);
    public abstract void Renderizar(IRender render, ISacarDatos contenedor = null);
}

public abstract class IContenedorConDatos : MonoBehaviour, ISacarDatos, IContenedor
{
    public abstract Elemento Eliminar(Vector3Int posicion);
    public abstract Elemento Eliminar(Elemento elemento);
    public abstract Elemento EnPosicion(Vector3Int posicion);
    public abstract bool EnRango(Vector3Int posicion);
    public abstract bool EnRango(Elemento elemento);
    public abstract Color GetColor(Vector3Int posicion, Color defaultColor = default);
    public abstract float GetValor(Vector3Int posicion, float defaultValor = 0);
    public abstract bool Insertar(Elemento elemento);
    public abstract bool Intercambiar(Vector3Int origen, Vector3Int destino);
    public abstract bool Intercambiar(Elemento elementoOrigen, Elemento elementoDestino);
}

public abstract class IConetenedorGeneral : MonoBehaviour, IContenedor, ISacarDatos, IRenderizable
{
    public abstract Elemento Eliminar(Vector3Int posicion);
    public abstract Elemento Eliminar(Elemento elemento);
    public abstract Elemento EnPosicion(Vector3Int posicion);
    public abstract bool EnRango(Vector3Int posicion);
    public abstract bool EnRango(Elemento elemento);
    public abstract Color GetColor(Vector3Int posicion, Color defaultColor = default);
    public abstract float GetValor(Vector3Int posicion, float defaultValor = 0);
    public abstract bool Insertar(Elemento elemento);
    public abstract bool Reemplazar(Elemento elemento, Elemento reemplazo);
    public abstract bool Intercambiar(Vector3Int origen, Vector3Int destino);
    public abstract bool Intercambiar(Elemento elementoOrigen, Elemento elementoDestino);
    public abstract void Renderizar(IRender render, ISacarDatos contenedor = null);
    public abstract IEnumerable<Elemento> ElementoParaActualizar();
}
