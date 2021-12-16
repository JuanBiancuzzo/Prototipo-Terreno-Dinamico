using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Glyph/Elemento")]
public class GlyphElemento : ScriptableObject
{
    [SerializeField] protected string m_nombre;
    [SerializeField] protected GlyphRango[] m_rangos; 

    public virtual string[] GetNombres()
    {
        return new string[1] { m_nombre };
    }

    public string GetNombreCompleto()
    {
        return m_nombre;
    }

    public virtual bool ConNombre(string nombre)
    {
        return nombre == m_nombre;
    }

    public bool EnRango(Vector3 punto)
    {
        foreach (GlyphRango rango in m_rangos)
            if (rango.EnRango(punto))
                return true;
        return false;
    }

    public virtual PosicionesSpell Posicion(Vector3 punto, string nombre)
    {
        foreach (GlyphRango rango in m_rangos)
            if (rango.EnRango(punto))
                return rango.GetPosicion();
        return PosicionesSpell.Invalido;
    }
}