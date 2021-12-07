using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntidadMagica))]
[RequireComponent(typeof(Renderer))]
public class Visibilidad : MonoBehaviour
{
    Material m_material;
    EntidadMagica m_entidadMagica;

    private void Awake()
    {
        m_material = GetComponent<Renderer>().sharedMaterial;
        m_entidadMagica = GetComponent<EntidadMagica>();
    }

    private void Update()
    {
        m_material.SetVector("_Color", m_entidadMagica.ColorValor);
        m_material.SetInt("_Iluminacion", m_entidadMagica.IluminacionValor);
        m_material.SetInt("_Temperatura", m_entidadMagica.TemperaturaValor);
        m_material.SetInt("_Concentracion", m_entidadMagica.ConcentracionValor);
        m_material.SetInt("_Constitucion", m_entidadMagica.ConstitucionValor);
    }
}
