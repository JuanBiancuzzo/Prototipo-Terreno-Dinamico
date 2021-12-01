using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISacarDatos
{
    float GetValor(Vector3Int posicion, TipoMaterial tipoMaterial, float defaultValor = 0f);

    Color GetColor(Vector3Int posicion, TipoMaterial tipoMaterial, Color defaultColor = new Color());
}

public interface ITenerDatos
{
    float GetValor(TipoMaterial tipoMaterial);

    Color GetColor(TipoMaterial tipoMaterial);
}