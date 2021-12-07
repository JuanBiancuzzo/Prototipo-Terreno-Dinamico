using System;
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
        IObjetoMagico entidad = EntidadEnDireccion(posicion, direccion, distancia, out distanciaDeEntidad);

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

    private static IObjetoMagico EntidadEnDireccion(Vector3 posicion, Vector3 direccion, float distancia, out float distanciaFinal)
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

    public static List<IObjetoMagico> ObjetoEnLinea(Vector3 posicion, Vector3 direccion, float distancia)
    {
        List<IObjetoMagico> objetos = new List<IObjetoMagico>();

        ObjetosEnLineaMundo(posicion, direccion, distancia, ref objetos);
        EntidadesEnLinea(posicion, direccion, distancia, ref objetos);

        return objetos;
    }

    private static void EntidadesEnLinea(Vector3 posicion, Vector3 direccion, float distancia, ref List<IObjetoMagico> objetos)
    {
        RaycastHit[] hits = Physics.RaycastAll(posicion, direccion, distancia);

        foreach (RaycastHit hit in hits)
        {
            EntidadMagica entidad = hit.transform.GetComponent<EntidadMagica>();
            if (entidad == null)
                continue;
            objetos.Add(entidad);
        }
    }

    private static void ObjetosEnLineaMundo(Vector3 posicion, Vector3 direccion, float distancia, ref List<IObjetoMagico> objetos)
    {
        Vector3 dirNormalizada = Vector3.Normalize(direccion);
        Vector3 posicionFinal = posicion + dirNormalizada * distancia;

        Vector3Int inicio = Vector3Int.FloorToInt(posicion), final = Vector3Int.FloorToInt(posicionFinal);

        foreach (Vector3Int posicionMundo in Mathfs.PosicioneEntreYield(inicio, final))
        {
            Elemento elemento = m_mundo.EnPosicion(posicionMundo);
            if (elemento == null)
                continue;
            objetos.Add(elemento);
        }
    }

    public static List<IObjetoMagico> ObjetosEnArea(Vector3 posicion, Vector3 direccion, Vector2 extension)
    {
        List<IObjetoMagico> objetos = new List<IObjetoMagico>();

        ObjetosEnAreaMundo(posicion, direccion, extension, ref objetos);
        EntidadesEnArea(posicion, direccion, extension, ref objetos);

        return objetos;
    }

    private static void ObjetosEnAreaMundo(Vector3 posicion, Vector3 direccion, Vector2 extension, ref List<IObjetoMagico> objetos)
    {
        Vector3 direccionCostado = Vector3.right * extension.x;
        Vector3 direccionArriba = Vector3.up * extension.y;

        Quaternion rotacion = Quaternion.LookRotation(direccion);

        Vector3 direccionCostadoRotada = rotacion * direccionCostado;
        Vector3 direccionArribaRotada = rotacion * direccionArriba;

        Vector3Int pos = Vector3Int.FloorToInt(posicion), dirAcostado = Vector3Int.FloorToInt(direccionCostadoRotada), dirArriba = Vector3Int.FloorToInt(direccionArribaRotada);

        Vector3Int arribaDerecha = pos + dirAcostado + dirArriba;
        Vector3Int abajoIzquierda = pos - dirAcostado - dirArriba;
        Vector3Int abajoDerecha = pos + dirAcostado - dirArriba;

        foreach (Vector3Int posicionVertica in Mathfs.PosicioneEntreYield(abajoDerecha, arribaDerecha))
        {
            Vector3Int diferencia = posicionVertica - abajoDerecha;
            foreach (Vector3Int posicionHorizontal in Mathfs.PosicioneEntreYield(abajoIzquierda + diferencia, posicionVertica))
            {
                Elemento elemento = m_mundo.EnPosicion(posicionHorizontal);
                if (elemento == null)
                    continue;
                objetos.Add(elemento);
            }
        }
    }

    private static void EntidadesEnArea(Vector3 posicion, Vector3 direccion, Vector2 extension, ref List<IObjetoMagico> objetos)
    {
        Collider[] colliderArray = Physics.OverlapBox(posicion, new Vector3(extension.x, extension.y, 1), Quaternion.LookRotation(direccion));
        foreach (Collider collider in colliderArray)
        {
            EntidadMagica entidad = collider.GetComponent<EntidadMagica>();
            if (entidad == null)
                continue;

            objetos.Add(entidad);
        }
    }

    public static List<IObjetoMagico> ObjetoEnEsfera(Vector3 posicion, float radio)
    {
        List<IObjetoMagico> objetos = new List<IObjetoMagico>();

        ObjetosEnEsferaMundo(posicion, radio, ref objetos);
        EntidadesEnEsfera(posicion, radio, ref objetos);

        return objetos;
    }

    private static void EntidadesEnEsfera(Vector3 posicion, float radio, ref List<IObjetoMagico> objetos)
    {
        Collider[] colliderArray = Physics.OverlapSphere(posicion, radio);
        foreach (Collider collider in colliderArray)
        {
            EntidadMagica entidad = collider.GetComponent<EntidadMagica>();
            if (entidad == null)
                continue;

            objetos.Add(entidad);
        }
    }

    private static void ObjetosEnEsferaMundo(Vector3 posicion, float radio, ref List<IObjetoMagico> objetos)
    {
        Vector3Int pos = Vector3Int.FloorToInt(posicion);
        int r = Mathf.CeilToInt(radio);
        Vector3Int extension = new Vector3Int(r, r, r);

        Vector3Int minimo = pos - extension, maximo = pos + extension;

        for (int x = 0; x <= minimo.x; x++)
            for (int y = 0; y <= minimo.y; y++)
                for (int z = 0; z <= minimo.z; z++)
                {
                    Vector3 posActual = new Vector3(x, y, z);
                    if ((posicion - posActual).magnitude > radio)
                        continue;

                    Elemento elemento = m_mundo.EnPosicion(new Vector3Int(x, y, z));
                    if (elemento == null)
                        continue;

                    objetos.Add(elemento);
                }
    }

    public static List<IObjetoMagico> ObjetoEnAmbiente(Vector3 posicion, float radio)
    {
        List<IObjetoMagico> objetos = new List<IObjetoMagico>();

        Vector3 extensionMedia = new Vector3(radio, radio, radio);

        ObjetosEnAmbienteMundo(posicion, extensionMedia, ref objetos);
        EntidadesEnAmbiente(posicion, extensionMedia, ref objetos);
        
        return objetos;
    }

    private static void EntidadesEnAmbiente(Vector3 posicion, Vector3 extensionMedia, ref List<IObjetoMagico> objetos)
    {
        Collider[] colliderArray = Physics.OverlapBox(posicion, extensionMedia);
        foreach (Collider collider in colliderArray)
        {
            EntidadMagica entidad = collider.GetComponent<EntidadMagica>();
            if (entidad == null)
                continue;

            objetos.Add(entidad);
        }
    }

    private static void ObjetosEnAmbienteMundo(Vector3 posicionV, Vector3 extensionMediaV, ref List<IObjetoMagico> objetos)
    {
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
    }

    

}
