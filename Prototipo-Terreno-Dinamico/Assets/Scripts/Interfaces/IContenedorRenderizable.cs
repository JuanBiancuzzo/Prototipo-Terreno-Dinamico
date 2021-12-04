using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IContenedorRenderizable : IRenderizable, IContenedor
{
}

public interface IContenedorConDatos : ISacarDatos, IContenedor
{
}

public abstract class IConetenedorGeneral : MonoBehaviour, IContenedor, ISacarDatos, IRenderizable
{   
    public abstract Elemento Eliminar(Vector3Int posicion);
    public abstract Elemento Eliminar(Elemento elemento);
    public abstract Elemento EnPosicion(Vector3Int posicion);
    public abstract bool EnRango(Vector3Int posicion);
    public abstract bool EnRango(Elemento elemento);
    public abstract Color GetColor(Vector3Int posicion, TipoMaterial tipoMaterial, Color defaultColor = default);
    public abstract float GetValor(Vector3Int posicion, TipoMaterial tipoMaterial, float defaultValor = 0);
    public abstract int GetIluminacion(Vector3Int posicion, int defaultIluminacion = 0);
    public abstract float GetColision(Vector3Int posicion, Constitucion otro, float defaultColision = 0f);
    public abstract bool Insertar(Elemento elemento);
    public abstract bool Reemplazar(Elemento elemento, Elemento reemplazo);
    public abstract bool Intercambiar(Vector3Int origen, Vector3Int destino);
    public abstract bool Intercambiar(Elemento elementoOrigen, Elemento elementoDestino);
    public abstract void Renderizar(IRender render, ISacarDatos contenedor = null);
    public abstract void GenerarMeshColision(IRender render, Extremo rangoEntidad, Constitucion entidad);
    public abstract IEnumerable<Elemento> ElementoParaActualizar();
    public abstract void CalcularIluminacion();
}

