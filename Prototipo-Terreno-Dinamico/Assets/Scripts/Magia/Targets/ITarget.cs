using System.Collections.Generic;
using UnityEngine;

public interface ITarget 
{
    public IObjetoMagico Punto(Vector3 posicion);

    public List<IObjetoMagico> Circulo(Vector3 posicion, Vector3 direccion, float radio);

    public List<IObjetoMagico> Esfera(Vector3 posicion, float radio);

    public List<IObjetoMagico> Linea(Vector3 origen, Vector3 final);

    public List<IObjetoMagico> Area(Vector3 primeraEsquina, Vector3 segundaEsquina);

    public List<IObjetoMagico> Volumen(Vector3 primeraEsquina, Vector3 segundaEsquina, Vector3 terceraEsquina);
}
