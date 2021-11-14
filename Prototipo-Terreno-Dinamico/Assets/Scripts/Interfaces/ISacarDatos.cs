using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISacarDatos
{
    float GetValor(Vector3Int posicion, float defaultValor = 0f);

    Color GetColor(Vector3Int posicion, Color defaultColor = new Color());
}

public interface ITenerDatos
{
    float GetValor();

    Color GetColor();
}