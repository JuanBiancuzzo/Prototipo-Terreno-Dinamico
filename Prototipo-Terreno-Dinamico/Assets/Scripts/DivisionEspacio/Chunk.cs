using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(MeshFilter))]
//[RequireComponent(typeof(MeshRenderer))]
public class Chunk : MonoBehaviour, IContenedor, ISacarDatos, IRenderizable
{
    public Vector3Int m_posicion; // posicion del centro
    Vector3Int m_extension; // "radio" del cuadrado

    IContenible[,,] m_contenido;
    VolumenMinimo m_volumenMinimo = null;
    int m_cantidad;

    MeshData m_meshData;

    //Mesh m_meshVisual;
    Mesh m_meshColision;

    MeshCollider m_meshColliderComponent;

    [Space]
    public float m_distanciaMinima = 10f;

    public void Inicializar(Vector3Int posicion, Vector3Int extension)
    {
        m_meshData = new MeshData();
        m_posicion = posicion;
        m_extension = extension;
        m_contenido = new IContenible[extension.x * 2, extension.y * 2, extension.z * 2];

        m_volumenMinimo = new VolumenMinimo(m_distanciaMinima);
        m_cantidad = 0;
    }

    public void Awake()
    {
        //m_meshVisual = new Mesh();
        //GetComponent<MeshFilter>().sharedMesh = m_meshVisual;

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

    public void ExtremosMinimos(ref Extremo extremoMinimo)
    {
        foreach (Extremo extremo in m_volumenMinimo.GetExtremos())
            extremoMinimo = extremoMinimo.Union(extremo);
    }

    public void Renderizar(IRender render, ISacarDatos contenedor = null)
    {
        if (!NecesitaActualizarse() || m_volumenMinimo.Vacio())
        {
            if (m_volumenMinimo.Vacio())
                m_meshData.Clear();
            return;
        }

        Extremo extremo = new Extremo(Vector3Int.zero, Vector3Int.zero, false);
        ExtremosMinimos(ref extremo);

        m_meshData.Clear();
        render.GenerarMeshCompute(extremo, contenedor, ref m_meshData);
        m_volumenMinimo.EmpezarARenderizar();
    }

    public bool NecesitaActualizarse()
    {
        return m_volumenMinimo.NecesitaActualizarse();
    }

    public void RecopilarMesh(ref MeshData meshData)
    {
        meshData.Sumar(m_meshData);
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

    public void ClearMeshColision()
    {
        m_meshColision.Clear();
        m_meshColliderComponent.sharedMesh = m_meshColision;
    }

    public static void LlenarMesh(Mesh mesh, MeshData meshData)
    {
        mesh.Clear();
        mesh.SetVertices(meshData.m_vertices);
        mesh.SetTriangles(meshData.m_triangulos, 0);

        if (meshData.m_colores.Count > 0)
            mesh.SetColors(meshData.m_colores);

        if (meshData.m_normales.Count > 0)
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

    void OnDrawGizmosSelected()
    {
        if (Vacio())
            Gizmos.color = Color.red;

        Gizmos.DrawWireCube(m_posicion + transform.position, m_extension * 2);

        if (m_volumenMinimo == null)
            return;
        
        List<Extremo> extremos = m_volumenMinimo.GetExtremos();
        foreach (Extremo extremo in extremos)
        {
            Vector3Int extension = extremo.m_maximo - extremo.m_minimo + Vector3Int.one;
            Vector3 posicion = ((Vector3)(extremo.m_maximo + extremo.m_minimo)) / 2f;
            Gizmos.DrawWireCube(transform.position + posicion, extension);
        }

        /*
        Extremo extremo = new Extremo();
        ExtremosMinimos(ref extremo);

        Vector3Int extension = extremo.m_maximo - extremo.m_minimo + Vector3Int.one * 2;
        Vector3 posicion = ((Vector3)(extremo.m_maximo + extremo.m_minimo)) / 2f + Vector3.up / 2f;
        Gizmos.DrawWireCube(transform.position + posicion, extension);*/

        Gizmos.color = Color.white;
    }
}
