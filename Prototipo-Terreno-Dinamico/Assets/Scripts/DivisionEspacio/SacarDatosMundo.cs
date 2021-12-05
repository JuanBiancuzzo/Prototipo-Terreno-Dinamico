using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Mundo))]
public class SacarDatosMundo : MonoBehaviour, ISacarDatos
{
    protected Mundo mundo;

    private void Awake()
    {
        mundo = GetComponent<Mundo>();
    }

    public float GetColision(Vector3Int posicion, Constitucion otro, float defaultColision = 0)
    {
        if (!mundo.EnRango(posicion))
            return defaultColision;

        Elemento elemento = mundo.EnPosicion(posicion);
        if (elemento == null)
            return defaultColision;

        return elemento.GetColision(otro, defaultColision);
    }

    public Color GetColor(Vector3Int posicion, TipoMaterial tipoMaterial, Color defaultColor = default)
    {
        if (!mundo.EnRango(posicion))
            return defaultColor;

        Elemento elemento = mundo.EnPosicion(posicion);
        if (elemento == null)
            return defaultColor;

        return elemento.GetColor(tipoMaterial, defaultColor);
    }

    public int GetIluminacion(Vector3Int posicion, int defaultIluminacion = 0)
    {
        if (!mundo.EnRango(posicion))
            return defaultIluminacion;

        Elemento elemento = mundo.EnPosicion(posicion);
        if (elemento == null)
            return defaultIluminacion;

        return elemento.GetIluminacion(); 
    }

    public float GetValor(Vector3Int posicion, TipoMaterial tipoMaterial, float defaultValor = 0)
    {
        if (!mundo.EnRango(posicion))
            return defaultValor;

        Elemento elemento = mundo.EnPosicion(posicion);
        if (elemento == null)
            return defaultValor;

        return elemento.GetValor(tipoMaterial, defaultValor);
    }
}
