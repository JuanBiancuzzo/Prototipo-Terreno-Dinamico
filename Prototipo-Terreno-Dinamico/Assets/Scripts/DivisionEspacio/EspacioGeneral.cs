using System.Collections.Generic;
using UnityEngine;

public class EspacioGeneral : MonoBehaviour, IContenedorRenderizable, IContenedorConDatos
{
    public Vector3Int m_posicion; // posicion del centro
    public int m_alturaMinima = 0, m_alturaMaxima = 100;

    [Range(10, 50)] public int m_chunkAncho;

    Dictionary<Vector3Int, Chunk> m_contenedores = new Dictionary<Vector3Int, Chunk>();
    List<Chunk> m_chunks = new List<Chunk>();

    public GameObject m_chunkPrefab;

    public float m_defaultValor;
    public Color m_defaultColor;

    public bool Insertar(Elemento contenible)
    {
        if (contenible == null)
            return false;

        Vector3Int posicion = contenible.Posicion();
        if (!EnRango(posicion))
            return false;

        return ChunkValidoEnPosicion(posicion).Insertar(contenible);
    }

    public Elemento Eliminar(Vector3Int posicion)
    {
        if (!EnRango(posicion))
            return null;

        Chunk chunk = ChunkEnPosicion(posicion);
        return (chunk == null) ? null : chunk.Eliminar(posicion); 
    }

    public Elemento Eliminar(Elemento contenible)
    {
        return (contenible == null) ? null : Eliminar(contenible.Posicion());
    }

    public bool Intercambiar(Vector3Int origen, Vector3Int destino)
    {
        if (!EnRango(origen) || !EnRango(destino))
            return false;

        if (WTC(origen) == WTC(destino))
            return ChunkEnPosicion(origen).Intercambiar(origen, destino);

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

        Chunk chunk = ChunkEnPosicion(posicion);
        return (chunk == null) ? null : chunk.EnPosicion(posicion); 
    }

    public bool EnRango(Vector3Int posicion)
    {
        return posicion.y > m_alturaMinima && posicion.y < m_alturaMaxima;
    }

    public bool EnRango(Elemento contenible)
    {
        return (contenible == null) ? false : EnRango(contenible.Posicion());
    }

    public float GetValor(Vector3Int posicion, float defaultValor = 0f)
    {
        if (!EnRango(posicion))
            return m_defaultValor;

        Chunk chunk = ChunkEnPosicion(posicion);
        return (chunk == null) ? m_defaultValor : chunk.GetValor(posicion, m_defaultValor);
    }

    public Color GetColor(Vector3Int posicion, Color defaultColor = new Color())
    {
        if (!EnRango(posicion))
            return m_defaultColor;

        Chunk chunk = ChunkEnPosicion(posicion);
        return (chunk == null) ? m_defaultColor : chunk.GetColor(posicion, m_defaultColor);
    }

    public void Renderizar(IRender render, ISacarDatos contenedor = null, bool overrideActualizacion = false)
    {
        foreach (Chunk chunk in m_chunks)
            chunk.Renderizar(render, this, overrideActualizacion);
    }

    public void GenerarMeshColision(IRender render, Extremo rangoJugador)
    {
        foreach (Chunk chunk in m_chunks)
            chunk.ClearMeshColision();

        Extremo extremo = new Extremo(WTC(rangoJugador.m_minimo), WTC(rangoJugador.m_maximo));

        for (int i = extremo.m_minimo.x; i <= extremo.m_maximo.x; i++)
            for (int j = extremo.m_minimo.y; j <= extremo.m_maximo.y; j++)
                for (int k = extremo.m_minimo.z; k <= extremo.m_maximo.z; k++)
                {
                    Vector3Int posicion = new Vector3Int(i, j, k);
                    if (m_contenedores.ContainsKey(posicion))
                        m_contenedores[posicion].GenerarMeshColision(render, rangoJugador, this);
                }
    }

    

    private Vector3Int WTC(Vector3Int posicionMundo)
    {
        Vector3Int posicionFinal = Vector3Int.FloorToInt((Vector3) posicionMundo / m_chunkAncho);
        return posicionFinal;
    }

    private Chunk ChunkEnPosicion(Vector3Int posicionMundo)
    {
        Vector3Int posicion = WTC(posicionMundo);
        return (m_contenedores.ContainsKey(posicion)) ? m_contenedores[posicion] : null;
    }

    private Chunk ChunkValidoEnPosicion(Vector3Int posicionMundo)
    {
        Vector3Int posicion = WTC(posicionMundo);
        return (m_contenedores.ContainsKey(posicion)) ? m_contenedores[posicion] : CrearChunk(posicion);
    }

    private Chunk CrearChunk(Vector3Int posicionChunk)
    {
        Vector3Int posicion = posicionChunk * m_chunkAncho + Vector3Int.one * (m_chunkAncho / 2);
        Vector3Int extension = new Vector3Int(m_chunkAncho / 2, m_chunkAncho / 2, m_chunkAncho / 2);

        GameObject chunkObjeto = Instantiate(m_chunkPrefab, transform);
        chunkObjeto.transform.position = transform.position;
        chunkObjeto.name = posicionChunk.ToString();

        Chunk chunkFinal = chunkObjeto.GetComponent(typeof(Chunk)) as Chunk;
        chunkFinal.Inicializar(posicion, extension, new Vector2Int(m_alturaMinima, m_alturaMaxima));

        m_contenedores.Add(posicionChunk, chunkFinal);
        m_chunks.Add(chunkFinal);

        return chunkFinal;
    }
}
