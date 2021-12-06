using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMundo
{
    Mundo m_mundo;

    public TargetMundo(Mundo mundo)
    {
        m_mundo = mundo;
    }

    public IObjetoMagico Punto(Vector3 posicion)
    {
        return ElementoEnPosicion(Vector3Int.FloorToInt(posicion));
    }

    public List<IObjetoMagico> Linea(Vector3 origen, Vector3 final)
    {
        Vector3Int origenPos = Vector3Int.FloorToInt(origen), finalPos = Vector3Int.FloorToInt(final);
        return Linea(origenPos, finalPos);
    }

    List<IObjetoMagico> Linea(Vector3Int origen, Vector3Int final)
    {
        List<IObjetoMagico> objetos = new List<IObjetoMagico>();

        foreach (Vector3Int posicion in Mathfs.PosicioneEntreYield(origen, final))
            objetos.Add(ElementoEnPosicion(posicion));

        return objetos;
    }

    public List<IObjetoMagico> Area(Vector3 primeraEsquina, Vector3 segundaEsquina)
    {
        Vector3Int esquina1 = Vector3Int.FloorToInt(primeraEsquina), esquina2 = Vector3Int.FloorToInt(segundaEsquina);
        return Area(esquina1, esquina2);
    }

    List<IObjetoMagico> Area(Vector3Int primeraEsquina, Vector3Int segundaEsquina)
    {
        List<IObjetoMagico> objetos = new List<IObjetoMagico>();

        int alturaIncial = (primeraEsquina.y > segundaEsquina.y) ? segundaEsquina.y : primeraEsquina.y;
        int alturaFinal = (primeraEsquina.y > segundaEsquina.y) ? primeraEsquina.y : segundaEsquina.y;

        for (int i = alturaIncial; i <= alturaFinal; i++)
        {
            primeraEsquina.y = i;
            segundaEsquina.y = i;
            objetos.AddRange(Linea(primeraEsquina, segundaEsquina));
        }

        return objetos;
    }

    public List<IObjetoMagico> Volumen(Vector3 primeraEsquina, Vector3 segundaEsquina, Vector3 terceraEsquina)
    {
        throw new System.NotImplementedException();
    }

    public List<IObjetoMagico> Circulo(Vector3 posicion, Vector3 direccion, float radio)
    {
        Vector3Int pos = Vector3Int.FloorToInt(posicion), dir = Vector3Int.FloorToInt(direccion);
        int r = Mathf.FloorToInt(radio);

        throw new System.NotImplementedException();
    }

    public List<IObjetoMagico> Esfera(Vector3 posicion, float radio)
    {
        throw new System.NotImplementedException();
    }

    private IObjetoMagico ElementoEnPosicion(Vector3Int posicion)
    {
        return m_mundo.EnPosicion(posicion);
    }
}
