using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoiseGenerador : IGenerador
{
    public override int ValorEnPosicion(int x, int y, int minimo, int maximo)
    {
        float alturaNormalizado = Mathf.PerlinNoise(x / 20f, y / 20f) / 2;
        return Mathf.FloorToInt(Mathf.Lerp(minimo, maximo, alturaNormalizado));
    }
}
