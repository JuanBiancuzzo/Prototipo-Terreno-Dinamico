using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstandarGenerador : IGenerador
{
    [SerializeField] int altura;

    public override int ValorEnPosicion(int x, int y, int minimo, int maximo)
    {
        return altura;
    }
}
