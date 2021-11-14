using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
public class Chunk : MonoBehaviour, IContenedor, ISacarDatos, IRenderizable
{
    public Vector3Int m_posicion; // posicion del centro
    Vector3Int m_extension; // "radio" del cuadrado

    IContenible[,,] m_contenido;
    VolumenMinimo m_volumenMinimo;
    int m_cantidad;

    Mesh m_meshVisual;
    Mesh m_meshColision;
    ISacarDatos m_contendedorGeneral;

    MeshCollider m_meshColliderComponent;

    [Space]
    public float m_distanciaMinima = 10f;

    public void Inicializar(Vector3Int posicion, Vector3Int extension, ISacarDatos contenedorGeneral)
    {
        m_posicion = posicion;
        m_extension = extension;
        m_contenido = new IContenible[extension.x * 2, extension.y * 2, extension.z * 2];
        m_contendedorGeneral = contenedorGeneral;

        m_volumenMinimo = new VolumenMinimo(m_distanciaMinima);
        m_cantidad = 0;
    }

    public void Awake()
    {
        m_meshVisual = new Mesh();
        GetComponent<MeshFilter>().sharedMesh = m_meshVisual;

        m_meshColision = new Mesh();
        m_meshColliderComponent = GetComponent<MeshCollider>() as MeshCollider;
    }

    public bool Insertar(IContenible contenible)
    {
        if (contenible == null)
            return false;

        Vector3Int posicion = contenible.Posicion();
        if (!EnRango(posicion))
            return false;

        IContenible enEspacio = EnPosicion(posicion);
        if (enEspacio != null)
            return false;

        m_volumenMinimo.Insertar(posicion);

        posicion = WTM(posicion); // Cambiamos la posicion para que sea relativa a la matriz
        m_contenido[posicion.x, posicion.y, posicion.z] = contenible;

        m_cantidad++;

        return true;
    }

    public IContenible Eliminar(Vector3Int posicion)
    {
        if (!EnRango(posicion))
            return null;

        IContenible enEspacio = EnPosicion(posicion);
        if (enEspacio == null)
            return null;

        m_volumenMinimo.Eliminar(posicion);

        posicion = WTM(posicion); // Cambiamos la posicion para que sea relativa a la matriz
        m_contenido[posicion.x, posicion.y, posicion.z] = null;

        m_cantidad--;

        return enEspacio;
    }

    public IContenible Eliminar(IContenible contenible)
    {
        return (contenible == null) ? null : Eliminar(contenible.Posicion());
    }

    public bool Intercambiar(Vector3Int origen, Vector3Int destino)
    {
        if (!EnRango(origen) || !EnRango(destino))
            return false;

        IContenible contenibleOrigen = Eliminar(origen);
        IContenible contenibleDestino = Eliminar(destino);

        if (contenibleOrigen != null)
        {
            contenibleOrigen.ActualizarPosicion(destino);
            Insertar(contenibleOrigen);
        }

        if (contenibleDestino != null)
        {
            contenibleDestino.ActualizarPosicion(origen);
            Insertar(contenibleDestino);
        }

        return contenibleOrigen != null || contenibleDestino != null;
    }

    public bool Intercambiar(IContenible contenibleOrigen, IContenible contenibleDestino)
    {
        if (contenibleOrigen == null || contenibleDestino == null)
            return false;
        return Intercambiar(contenibleOrigen.Posicion(), contenibleDestino.Posicion());
    }

    public IContenible EnPosicion(Vector3Int posicion)
    {
        if (!EnRango(posicion))
            return null;

        posicion = WTM(posicion);
        return m_contenido[posicion.x, posicion.y, posicion.z];
    }

    public bool EnRango(Vector3Int posicion)
    {
        Vector3Int minimo = m_posicion - m_extension, maximo = m_posicion + m_extension;
        for (int i = 0; i < 3; i++)
            if (minimo[i] > posicion[i] || maximo[i] <= posicion[i])
                return false;
        return true;
    }

    public bool EnRango(IContenible contenible)
    {
        return (contenible == null) ? false : EnRango(contenible.Posicion());
    }

    public float GetValor(Vector3Int posicion, float defaultValor = 0f)
    {
        if (!EnRango(posicion))
            return defaultValor;

        posicion = WTM(posicion);
        IContenible contenible = m_contenido[posicion.x, posicion.y, posicion.z];
        return (contenible == null) ? defaultValor : contenible.GetValor();
    }

    public Color GetColor(Vector3Int posicion, Color defaultColor = new Color())
    {
        if (!EnRango(posicion))
            return defaultColor;

        posicion = WTM(posicion);
        IContenible contenible = m_contenido[posicion.x, posicion.y, posicion.z];
        return (contenible == null) ? defaultColor : contenible.GetColor();
    }

    public void Renderizar(IRender render, ISacarDatos contenedor = null)
    {
        m_volumenMinimo.Renderizar(render, contenedor);

        MeshData preInfo = new MeshData();
        m_volumenMinimo.RecopilarMesh(ref preInfo);
        LlenarMesh(m_meshVisual, preInfo);
    }

    public void GenerarMeshColision(IRender render, Extremo rangoJugador, ISacarDatos contenedor)
    {
        List<Extremo> listaExtremos = m_volumenMinimo.GetExtremos();
        MeshData preInfo = new MeshData();

        foreach (Extremo extremoTotal in listaExtremos)
        {
            Extremo interseccion = extremoTotal.Interseccion(rangoJugador);
            if (!interseccion.Valido())
                continue;
            render.GenerarMesh(interseccion, contenedor, ref preInfo);
        }

        LlenarMesh(m_meshColision, preInfo);
        m_meshColliderComponent.sharedMesh = m_meshColision;
    }

    private static void LlenarMesh(Mesh mesh, MeshData meshData)
    {
        mesh.Clear();
        mesh.SetVertices(meshData.m_vertices);
        mesh.SetTriangles(meshData.m_triangulos, 0);
        mesh.SetColors(meshData.m_colores);
        mesh.SetNormals(meshData.m_normales);
    }

    public bool Vacio()
    {
        return m_cantidad == 0;
    }

    private Vector3Int WTM(Vector3Int posicionMundo)
    {
        return posicionMundo - (m_posicion - m_extension);
    }

    void OnDrawGizmos()
    {
        if (Vacio())
            Gizmos.color = Color.red;

        Gizmos.DrawWireCube(m_posicion + transform.position, m_extension * 2);

        List<Extremo> extremos = m_volumenMinimo.GetExtremos();
        foreach (Extremo extremo in extremos)
        {
            Vector3Int extension = extremo.m_maximo - extremo.m_minimo + Vector3Int.one;
            Vector3 posicion = ((Vector3)(extremo.m_maximo + extremo.m_minimo)) / 2f;
            Gizmos.DrawWireCube(transform.position + posicion, extension);
        }

        Gizmos.color = Color.white;
    }
}
