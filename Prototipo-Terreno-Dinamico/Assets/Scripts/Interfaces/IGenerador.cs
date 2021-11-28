using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IGenerador : MonoBehaviour
{
    public abstract int ValorEnPosicion(int x, int y, int minimo, int maximo);
}
