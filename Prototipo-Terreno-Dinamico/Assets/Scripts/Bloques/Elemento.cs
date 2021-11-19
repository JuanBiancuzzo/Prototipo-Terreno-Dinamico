using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Elemento
{
    Vector3Int m_posicion;
    Vector3 m_velocidad, m_aceleracion;
    float m_densidad;
    float m_temperatura;
    float m_ficDinamica, m_ficEstatica;
    float m_filtracion;
    Vector3Int m_estabilidad;
    int m_iluminacion;

    // tener un callback al falling sand, para que sepa que necesita actualizarse

    public abstract void Avanzar(IContenedorConDatos mapa);
}
