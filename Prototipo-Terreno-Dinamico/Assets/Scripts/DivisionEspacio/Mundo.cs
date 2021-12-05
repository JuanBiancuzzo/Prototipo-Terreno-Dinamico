using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ColisionesMundo))]
[RequireComponent(typeof(SacarDatosMundo))]
[RequireComponent(typeof(RenderContenedores))]
[RequireComponent(typeof(ContenedorMundo))]
public class Mundo : MonoBehaviour, IContenedor, ISacarDatos, IRenderizable, IColisiones
{

    public RenderContenedores renderizable;
    public ColisionesMundo colisiones;
    public SacarDatosMundo sacarDatos;
    public ContenedorMundo contenedor;
    Vector3Int m_posicion => Vector3Int.FloorToInt(transform.position);


    public ElementoSeleccionado objectoSeleccionado;
    private void Awake()
    {
        renderizable = GetComponent<RenderContenedores>();
        colisiones = GetComponent<ColisionesMundo>();
        sacarDatos = GetComponent<SacarDatosMundo>();
        contenedor = GetComponent<ContenedorMundo>();
    }

    public IEnumerable<Elemento> ElementoParaActualizar()
    {
        for (int x = 0; x < contenedor.m_extension.x; x++)
            for (int y = 0; y < contenedor.m_extension.y; y++)
                for (int z = 0; z < contenedor.m_extension.z; z++)
                {
                    Elemento elemento = contenedor.EnPosicion(new Vector3Int(x, y, z));
                    if (elemento == null)
                        continue;
                    yield return elemento;
                }
    }

    public void CalcularIluminacion()
    {
        for (int x = 0; x < contenedor.m_extension.x; x++)
            for (int y = 0; y < contenedor.m_extension.y; y++)
                for (int z = 0; z < contenedor.m_extension.z; z++)
                    contenedor.EnPosicion(new Vector3Int(x, y, z))?.ExpandirLuz();

        for (int x = contenedor.m_extension.x - 1; x >= 0; x--)
            for (int y = contenedor.m_extension.y - 1; y >= 0; y--)
                for (int z = contenedor.m_extension.z - 1; z >= 0; z--)
                    contenedor.EnPosicion(new Vector3Int(x, y, z))?.ExpandirLuz();

    }

    public void SeleccionarElemento(IRender render, Vector3Int posicion)
    {
        if (objectoSeleccionado == null || !EnRango(posicion))
            return;

        MeshData meshData = new MeshData();
        render.GenerarMeshSeleccion(posicion, this, ref meshData);
        objectoSeleccionado.CargarNuevaMesh(meshData);
    }

    public bool Insertar(Elemento elemento)
    {
        return contenedor.Insertar(elemento);
    }

    public Elemento Eliminar(Vector3Int posicion)
    {
        return contenedor.Eliminar(posicion);
    }

    public Elemento Eliminar(Elemento elemento)
    {
        return contenedor.Eliminar(elemento);
    }

    public bool Intercambiar(Vector3Int origen, Vector3Int destino)
    {
        return contenedor.Intercambiar(origen, destino);
    }

    public bool Intercambiar(Elemento elementoOrigen, Elemento elementoDestino)
    {
        return contenedor.Intercambiar(elementoOrigen, elementoDestino);
    }

    public bool Reemplazar(Elemento elemento, Elemento reemplazo)
    {
        return contenedor.Reemplazar(elemento, reemplazo);
    }

    public Elemento EnPosicion(Vector3Int posicion)
    {
        return contenedor.EnPosicion(posicion);
    }

    public bool EnRango(Vector3Int posicion)
    {
        return contenedor.EnRango(posicion);
    }

    public bool EnRango(Elemento elemento)
    {
        return contenedor.EnRango(elemento);
    }

    public Color GetColor(Vector3Int posicion, TipoMaterial tipoMaterial, Color defaultColor = default)
    {
        return sacarDatos.GetColor(posicion, tipoMaterial, defaultColor);
    }

    public float GetValor(Vector3Int posicion, TipoMaterial tipoMaterial, float defaultValor = 0)
    {
        return sacarDatos.GetValor(posicion, tipoMaterial, defaultValor);
    }

    public int GetIluminacion(Vector3Int posicion, int defaultIluminacion = 0)
    {
        return sacarDatos.GetIluminacion(posicion, defaultIluminacion);
    }

    public float GetColision(Vector3Int posicion, Constitucion otro, float defaultColision = 0f)
    {
        return sacarDatos.GetColision(posicion, otro, defaultColision);
    }

    public void Renderizar(IRender render, ISacarDatos contenedor = null)
    {
        renderizable.Renderizar(render, this);
    }

    public void GenerarMeshColision(IRender render, Extremo rangoEntidad, Constitucion entidad)
    {
        colisiones.GenerarMeshColision(render, rangoEntidad, entidad);
    }

    private void OnDrawGizmos()
    {
        Vector3Int posicion = (contenedor.m_extremo.m_maximo + contenedor.m_extremo.m_minimo) / 2;

        Gizmos.DrawWireCube(posicion + m_posicion, contenedor.m_extension);
    }
}
