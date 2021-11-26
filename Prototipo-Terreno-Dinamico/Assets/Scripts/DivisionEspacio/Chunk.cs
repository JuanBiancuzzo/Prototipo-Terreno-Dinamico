using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class Chunk : MonoBehaviour, IContenedor, ISacarDatos, IRenderizable
{
    public Vector3Int m_posicion; // posicion del centro
    Vector3Int m_extension; // "radio" del cuadrado

    Elemento[,,] m_contenido;
    VolumenMinimo m_volumenMinimo = null;
    int m_cantidad;

    Mesh m_meshVisual;
    Mesh m_meshColision;

    MeshCollider m_meshColliderComponent;

    [Space]
    public float m_distanciaMinima = 10f;

    public void Inicializar(Vector3Int posicion, Vector3Int extension, Vector2Int extremo)
    {
        m_posicion = posicion;
        m_extension = extension;
        m_contenido = new Elemento[extension.x * 2, extension.y * 2, extension.z * 2];

        m_volumenMinimo = new VolumenMinimo(m_distanciaMinima);
        m_cantidad = 0;

        for (int x = posicion.x - extension.x; x < posicion.x + extension.x; x++)
            for (int z = posicion.z - extension.z; z < posicion.z + extension.z; z++)
            {
                float alturaNormalizado = Mathf.Clamp(Mathf.PerlinNoise(x / 20f, z / 20f) + 0.5f, 0, 1);
                int altura = Mathf.FloorToInt(Mathf.Lerp(extremo.x, extension.y, alturaNormalizado));

                for (int y = posicion.y - extension.y; y < posicion.y + extension.y; y++)
                    if (y < altura)
                        Insertar(new Concreto(new Vector3Int(x, y, z)));
                    else if (y == altura)
                        Insertar(new Arena(new Vector3Int(x, y, z)));
                    else
                        Insertar(new Aire(new Vector3Int(x, y, z)));
            }
    }

    public void Awake()
    {
        m_meshVisual = new Mesh();
        GetComponent<MeshFilter>().sharedMesh = m_meshVisual;

        m_meshColision = new Mesh();
        m_meshColliderComponent = GetComponent<MeshCollider>() as MeshCollider;
    }

    public bool Insertar(Elemento contenible)
    {
        if (contenible == null)
            return false;

        Vector3Int posicion = contenible.Posicion();
        if (!EnRango(posicion))
            return false;

        Elemento enEspacio = EnPosicion(posicion);
        if (enEspacio != null)
            return false;
        
        if (contenible.Visible())
            m_volumenMinimo.Insertar(posicion);

        posicion = WTM(posicion); // Cambiamos la posicion para que sea relativa a la matriz
        m_contenido[posicion.x, posicion.y, posicion.z] = contenible;

        m_cantidad++;

        return true;
    }

    public Elemento Eliminar(Vector3Int posicion)
    {
        if (!EnRango(posicion))
            return null;

        Elemento enEspacio = EnPosicion(posicion);
        if (enEspacio == null)
            return null;

        m_volumenMinimo.Eliminar(posicion);

        posicion = WTM(posicion); // Cambiamos la posicion para que sea relativa a la matriz
        m_contenido[posicion.x, posicion.y, posicion.z] = null;

        m_cantidad--;

        return enEspacio;
    }

    public Elemento Eliminar(Elemento contenible)
    {
        return (contenible == null) ? null : Eliminar(contenible.Posicion());
    }

    public bool Intercambiar(Vector3Int origen, Vector3Int destino)
    {
        if (!EnRango(origen) || !EnRango(destino))
            return false;

        Elemento contenibleOrigen = Eliminar(origen);
        Elemento contenibleDestino = Eliminar(destino);

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

    public bool Intercambiar(Elemento contenibleOrigen, Elemento contenibleDestino)
    {
        if (contenibleOrigen == null || contenibleDestino == null)
            return false;
        return Intercambiar(contenibleOrigen.Posicion(), contenibleDestino.Posicion());
    }

    public Elemento EnPosicion(Vector3Int posicion)
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

    public bool EnRango(Elemento contenible)
    {
        return (contenible == null) ? false : EnRango(contenible.Posicion());
    }

    public float GetValor(Vector3Int posicion, float defaultValor = 0f)
    {
        if (!EnRango(posicion))
            return defaultValor;

        posicion = WTM(posicion);
        Elemento contenible = m_contenido[posicion.x, posicion.y, posicion.z];
        return (contenible == null) ? defaultValor : contenible.GetValor();
    }

    public Color GetColor(Vector3Int posicion, Color defaultColor = new Color())
    {
        if (!EnRango(posicion))
            return defaultColor;

        posicion = WTM(posicion);
        Elemento contenible = m_contenido[posicion.x, posicion.y, posicion.z];
        return (contenible == null) ? defaultColor : contenible.GetColor();
    }

    public void ExtremosMinimos(ref Extremo extremoMinimo)
    {
        foreach (Extremo extremo in m_volumenMinimo.GetExtremos())
            extremoMinimo = extremoMinimo.Union(extremo);
    }

    public void Renderizar(IRender render, ISacarDatos contenedor = null, bool overrideActualizacion = false)
    {
        if (!overrideActualizacion)
            if (!NecesitaActualizarse() || m_volumenMinimo.Vacio())
            {
                if (m_volumenMinimo.Vacio())
                    m_meshVisual.Clear();
                return;
            }

        Extremo extremo = new Extremo(Vector3Int.zero, Vector3Int.zero, false);
        ExtremosMinimos(ref extremo);

        MeshData meshData = new MeshData();
        render.GenerarMeshCompute(extremo, contenedor, ref meshData);
        m_volumenMinimo.EmpezarARenderizar();
        LlenarMesh(m_meshVisual, meshData);
    }

    public bool NecesitaActualizarse()
    {
        return m_volumenMinimo.NecesitaActualizarse();
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
        else
            mesh.RecalculateNormals();
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
