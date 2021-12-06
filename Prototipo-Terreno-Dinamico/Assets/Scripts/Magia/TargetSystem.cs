using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TargetSystem
{
    public static Mundo m_mundo;

    public static IObjetoMagico ObjetoEnPunto(Vector3 posicion, Vector3 direccion, float distancia)
    {

        float distanciaDelObjetoMundo;
        IObjetoMagico elementoEnElMundo = ObjetoEnDireccionMundo(posicion, direccion, distancia, out distanciaDelObjetoMundo);

        float distanciaDeEntidad;
        IObjetoMagico entidad = ObjetoEnDireccionEntidades(posicion, direccion, distancia, out distanciaDeEntidad);

        if (entidad == null || distanciaDeEntidad == -1f)
            return elementoEnElMundo;

        if (elementoEnElMundo == null || distanciaDelObjetoMundo == -1f)
            return entidad;

        return (distanciaDeEntidad < distanciaDelObjetoMundo) ? entidad : elementoEnElMundo;
    }

    private static IObjetoMagico ObjetoEnDireccionMundo(Vector3 posicion, Vector3 direccion, float distancia, out float distanciaFinal)
    {
        Vector3 dirNormalizada = Vector3.Normalize(direccion);
        Vector3 posicionFinal = posicion + dirNormalizada * distancia;

        Vector3Int inicio = Vector3Int.FloorToInt(posicion), final = Vector3Int.FloorToInt(posicionFinal);

        Elemento elementoMejor = null;
        foreach (Vector3Int posicionMundo in Mathfs.PosicioneEntreYield(inicio, final))
        {
            Elemento elemento = m_mundo.EnPosicion(posicionMundo);
            if (elemento == null)
                continue;

            elementoMejor = elemento;
            if (elementoMejor.Visible())
                break;
        }

        distanciaFinal = (elementoMejor == null) ? -1f : (posicion - elementoMejor.m_posicion).magnitude;
        return elementoMejor;
    }

    private static IObjetoMagico ObjetoEnDireccionEntidades(Vector3 posicion, Vector3 direccion, float distancia, out float distanciaFinal)
    {
        RaycastHit hit;

        bool intersecto = Physics.Raycast(posicion, direccion, out hit, distancia);
        if (!intersecto)
        {
            distanciaFinal = -1f;
            return null;
        }

        EntidadMagica entidad = hit.transform.GetComponent<EntidadMagica>();
        distanciaFinal = (entidad == null) ? -1f : (posicion - hit.point).magnitude;
        return entidad;
    }

    public static List<IObjetoMagico> ObjetoEnAmbiente(Vector3 posicion, float radio, EntidadMagica noAfectado = null)
    {
        List<IObjetoMagico> objetos = new List<IObjetoMagico>();

        Vector3 extensionMedia = new Vector3(radio, radio, radio);

        objetos.AddRange(ObjetosEnAmbienteMundo(posicion, extensionMedia));
        objetos.AddRange(EntidadesEnAmbiente(posicion, extensionMedia, noAfectado));
        
        return objetos;
    }

    private static List<IObjetoMagico> EntidadesEnAmbiente(Vector3 posicion, Vector3 extensionMedia, EntidadMagica noAfectado)
    {
        List<IObjetoMagico> objetos = new List<IObjetoMagico>();

        Collider[] colliderArray = Physics.OverlapBox(posicion, extensionMedia);
        foreach (Collider collider in colliderArray)
        {
            EntidadMagica entidad = collider.GetComponent<EntidadMagica>();
            if (entidad == null || entidad.Equals(noAfectado))
                continue;
            objetos.Add(entidad);
        }

        return objetos;
    }

    private static List<IObjetoMagico> ObjetosEnAmbienteMundo(Vector3 posicionV, Vector3 extensionMediaV)
    {
        List<IObjetoMagico> objetos = new List<IObjetoMagico>();
        Vector3Int posicion = Vector3Int.FloorToInt(posicionV);
        Vector3Int extensionMedia = Vector3Int.FloorToInt(extensionMediaV);

        Vector3Int minimo = posicion - extensionMedia, maximo = posicion + extensionMedia;

        for (int x = minimo.x; x <= maximo.x; x++)
            for (int y = minimo.y; y <= maximo.y; y++)
                for (int z = minimo.z; z <= maximo.z; z++)
                {
                    Elemento elemento = m_mundo.EnPosicion(new Vector3Int(x, y, z));
                    if (elemento == null)
                        continue;
                    objetos.Add(elemento);
                }


        return objetos;
    }
}
