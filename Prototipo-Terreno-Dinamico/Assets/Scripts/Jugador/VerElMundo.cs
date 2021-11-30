using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerElMundo : MonoBehaviour
{
    public FallingSand m_mundo = null;
    [SerializeField] [Range(0, 30)]
    List<int> LODLevels = new List<int>();

    int m_distanciaRender;
    int anchoChunk = 10;

    private void Start()
    {
        m_distanciaRender = 0;
        foreach (int LOD in LODLevels)
            m_distanciaRender += LOD;

        /*if (m_mundo != null)
            m_mundo.AgregarJugador(transform, LODLevels);*/
    }

    private void OnDrawGizmos()
    {
        if (m_mundo != null)
            anchoChunk = 10 * 2;
            //anchoChunk = m_mundo.m_mapa.m_chunkAncho * 2;

        int distancia = 0;
        foreach (int LOD in LODLevels)
        {
            distancia += LOD * anchoChunk;
            Gizmos.DrawWireCube(Vector3Int.FloorToInt(transform.position), new Vector3(distancia, distancia, distancia));
        }
    }
}
