using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Glyph/Rango")]
public class GlyphRango : ScriptableObject
{
    [System.Serializable] struct Rango
    {
        [SerializeField] Vector2 minimo, maximo;

        public bool EnRango(Vector2 punto)
        {
            for (int i = 0; i < 2; i++)
                if (punto[i] < minimo[i] || punto[i] > maximo[i])
                    return false;
            return true;
        }
    }

    [SerializeField] PosicionesSpell m_posicion;
    [SerializeField] List<Rango> m_rangos;

    public bool EnRango(Vector2 punto)
    {
        foreach (Rango rango in m_rangos)
            if (rango.EnRango(punto))
                return true;
        return false;
    }

    public PosicionesSpell GetPosicion()
    {
        return m_posicion;
    }
}
