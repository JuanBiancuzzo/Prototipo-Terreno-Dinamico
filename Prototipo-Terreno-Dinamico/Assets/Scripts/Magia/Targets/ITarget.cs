using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITarget 
{
    public IObjetoMagico Punto(Vector3 posicion);

    public IObjetoMagico Circulo(Vector3 posicion, float radio);

    public IObjetoMagico Linea(Vector3 origen, Vector3 final);

    public IObjetoMagico Area(Vector3 primeraEsquina, Vector3 segundaEsquina);

    public IObjetoMagico Volumen(Vector3 primeraEsquina, Vector3 segundaEsquina, Vector3 terceraEsquina);
}
