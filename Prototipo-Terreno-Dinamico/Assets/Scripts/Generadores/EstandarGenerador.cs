using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstandarGenerador : IGenerador
{
    public override int ValorEnPosicion(int x, int y, int minimo, int maximo)
    {
        return minimo + 2;
    }
}
