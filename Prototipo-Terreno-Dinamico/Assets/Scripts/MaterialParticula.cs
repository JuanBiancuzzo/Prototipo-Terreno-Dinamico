using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Material", menuName = "FallingSand/Material", order = 2)]
public class MaterialParticula : ScriptableObject, IContenible
{
    public bool Intercambiable, Reemplazable;
    public List<Vector3Int> m_opciones;
    public Dictionary<string, Vector3Int> m_opciones_hash;


    public float m_valor;
    public Color m_color;
    private Vector3Int m_posicion;

    /* public const float densidadAire = 1.28f;
    private Vector3 fuerzaReservada = new Vector3(0, 0, 0);
    public Vector3Int m_velocidad = new Vector3Int(0, 0, 0);

    private const float volumen = 1f;
    public float m_densidad = 1f;
    public float m_masa => m_densidad * volumen ;
    [Range(0.01f, 0.99f)] public float m_coeEstatico = 0.5f;
    [Range(0.01f, 0.99f)] public float m_coeDinamico = 0.25f;

    public float m_temperatura = 20; */

    // public IContenedorConDatos m_mapa = null;

    public void Inicializar(Vector3Int posicion)
    {
        m_posicion = posicion;
        m_opciones_hash = new Dictionary<string, Vector3Int>();
        foreach (Vector3Int opcion in m_opciones)
            m_opciones_hash.Add(Utilidades.Hash(opcion), opcion);
    }

    public void Avanzar(IContenedorConDatos mapa, ref List<MaterialParticula> paraActualizar)
    {
        foreach (Vector3Int opcion in m_opciones)
        {
            Vector3Int posicionVieja = m_posicion;
            Vector3Int desfase = new Vector3Int(opcion.x, opcion.y, opcion.z);
            Vector3Int nuevaPosicion = m_posicion + desfase;
                            
            if (Interpretar(nuevaPosicion, mapa))
            {
                ActualizarAlrededores(posicionVieja, mapa, ref paraActualizar);
                paraActualizar.Add(this);
                break;
            }
        }
    }

    /*
     
    public bool Moverse(float dt, out List<MaterialParticula> necesitanActualizarse)
    {
        necesitanActualizarse = new List<MaterialParticula>();
        if (m_mapa == null)
        {
            Debug.Log("No hay mapa");
            return false;
        }

        Vector3Int posicionInicio = m_posicion;
        Vector3Int posicionFin = m_posicion + m_velocidad;

        List<Vector3Int> posiciones = Mathfs.PosicionesEntre(posicionInicio, posicionFin);
        if (posiciones.Count == 0)
            return false;

        foreach (Vector3Int posicion in posiciones)
        {
            MaterialParticula material = (MaterialParticula) m_mapa.EnPosicion(posicion);

            float densidad = (material == null) ? densidadAire : material.m_densidad;
            bool chocar = (m_densidad > densidad) ? !m_mapa.Intercambiar(m_posicion, posicion) : true;

            if (chocar)
            {
                Vector3 fuerzaImpacto = ((Vector3)m_velocidad * m_masa) / dt;
                AplicarFuerza(-fuerzaImpacto, dt);
                if (material != null)
                {
                    material.AplicarFuerza(fuerzaImpacto, dt);
                    necesitanActualizarse.Add(material);
                }
                break;
            }
        }

        // necesitanActualizarse.Add(this);
        return true;
    }

    public void AplicarAceleracion(Vector3 aceleracion, float dt)
    {
        AplicarFuerza(aceleracion, dt);
    }

    public void AplicarFuerza(Vector3 fuerza, float dt)
    {
        Vector3 fuerzaNormal = CalcularFuerzaNormal(fuerza);
        Vector3 fuerzaRozamiento = CalcularFuerzaRozamiento(fuerzaNormal, fuerza);
        fuerzaReservada += ((fuerza + fuerzaNormal + fuerzaRozamiento) * dt);
        //Debug.Log("Fuerza reservada: " + fuerzaReservada);

        Vector3Int velocidadNueva = m_velocidad + Mathfs.FloorToInt(fuerzaReservada);
        //Debug.Log("Velocidad nueva: " + velocidadNueva);
        
        if (velocidadNueva != m_velocidad)
        {
            Vector3Int diff = velocidadNueva - m_velocidad;
            for (int i = 0; i < 3; i++)
                if (diff[i] != 0)
                    fuerzaReservada[i] = 0;
            m_velocidad = velocidadNueva;
        }
    }

    private Vector3 CalcularFuerzaNormal(Vector3 fuerza)
    {
        if (m_mapa == null)
            return Vector3.zero;

        Vector3Int direccionNormal = Mathfs.Normal(-fuerza);
        for (int i = 0; i < 3; i++)
        {
            if (direccionNormal[i] == 0)
                continue;

            Vector3Int posicionMaterial = m_posicion;
            posicionMaterial[i] += direccionNormal[i];

            IContenible contenible = m_mapa.EnPosicion(posicionMaterial);
            if (m_mapa.EnRango(posicionMaterial))
                direccionNormal[i] *= (contenible != null) ? 1 : 0;
        }

        Vector3 fuerzaNormal = ((Vector3)direccionNormal).normalized * fuerza.magnitude;

        return fuerzaNormal;
    }

    private Vector3 CalcularFuerzaRozamiento(Vector3 normal, Vector3 fuerza)
    {
        Vector3 direcionMovimiento = Mathfs.CombinacionMayor(m_velocidad, fuerza + normal).normalized;

        Vector3 fuerzaRozamiento = direcionMovimiento * normal.magnitude * -1f;
        fuerzaRozamiento *= (m_velocidad == Vector3Int.zero) ? m_coeEstatico : m_coeDinamico;

        return fuerzaRozamiento;
    }

    */
    public void ActualizarAlrededores(Vector3Int posicionVieja, IContenedorConDatos mapa, ref List<MaterialParticula> paraActualizar)
    {
        for (int i = -1; i <= 1; i++)
            for (int j = -1; j <= 1; j++)
                for (int k = -1; k <= 1; k++)
                {
                    Vector3Int desfase = new Vector3Int(i, j, k);
                    Vector3Int posicion = posicionVieja + desfase;

                    MaterialParticula material = (MaterialParticula)mapa.EnPosicion(posicion);
                    if (material == null || !material.PuedeMoverse(posicionVieja))
                        continue;                 

                    paraActualizar.Add(material);
                }
    }

    private bool PuedeMoverse(Vector3Int destino)
    {
        return m_opciones_hash.ContainsKey(Utilidades.Hash(destino - m_posicion));
    }

    public bool SeActualiza()
    {
        return m_opciones.Count > 0;
    }

    private bool Interpretar(Vector3Int nuevaPosicion, IContenedorConDatos mapa)
    {
        MaterialParticula material = (MaterialParticula) mapa.EnPosicion(nuevaPosicion);

        if (material == null || (material.Intercambiable && !Intercambiable))
            return Intercambiar(nuevaPosicion, mapa);

        if (material == null || (material.Reemplazable && !Reemplazable))
            return Reemplazar(nuevaPosicion, mapa);

        return false;
    }

    private bool Intercambiar(Vector3Int nuevaPosicion, IContenedorConDatos mapa)
    {
        return mapa.Intercambiar(m_posicion, nuevaPosicion);
    }

    private bool Reemplazar(Vector3Int nuevaPosicion, IContenedorConDatos mapa)
    {
        Vector3Int posicionVieja = m_posicion;

        if (!Intercambiar(nuevaPosicion, mapa))
            return false;

        mapa.Eliminar(posicionVieja);
        return true;
    }

    public MaterialParticula Clone(Vector3Int posicion)
    {
        MaterialParticula inst = Object.Instantiate(this);
        inst.name = this.name;
        inst.Inicializar(posicion);
        return inst;
    }

    public Vector3Int Posicion()
    {
        return m_posicion;
    }

    public void ActualizarPosicion(Vector3Int nuevaPosicion)
    {
        m_posicion = nuevaPosicion;
    }

    public float GetValor()
    {
        return m_valor;
    }

    public Color GetColor()
    {
        return m_color;
    }
}
