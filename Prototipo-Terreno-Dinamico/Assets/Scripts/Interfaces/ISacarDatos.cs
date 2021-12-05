using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISacarDatos
{ 
    float GetValor(Vector3Int posicion, TipoMaterial tipoMaterial, float defaultValor = 0f);

    Color GetColor(Vector3Int posicion, TipoMaterial tipoMaterial, Color defaultColor = new Color());

    int GetIluminacion(Vector3Int posicion, int defaultIluminacion = 0);

    float GetColision(Vector3Int posicion, Constitucion otro, float defaultColision = 0f);
}

public interface ITenerDatos
{
    float GetValor(TipoMaterial tipoMaterial, float defaultValor);

    Color GetColor(TipoMaterial tipoMaterial, Color defaultColor);

    int GetIluminacion();

    float GetColision(Constitucion otro, float defaultColision);
}