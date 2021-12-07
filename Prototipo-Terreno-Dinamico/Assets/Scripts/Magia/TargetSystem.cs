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
        foreach (Vector3Int posicionMundo in Mathfs.PosicionesEntreYield(inicio, final))
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

        foreach (Vector3Int posicionMundo in Mathfs.PosicionesEntreYield(inicio, final))
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
        Vector3 versorX = Vector3.right * extension.x, versorY = Vector3.up * extension.y;
        Quaternion rotacion = Quaternion.LookRotation(direccion);

        Vector3 versorXRotado = rotacion * versorX, versorYRotado = rotacion * versorY;

        Vector3Int pos = Vector3Int.FloorToInt(posicion), dirX = Vector3Int.FloorToInt(versorXRotado), dirY = Vector3Int.FloorToInt(versorYRotado);

        Vector3Int arribaDerecha = pos + dirX + dirY;
        Vector3Int abajoIzquierda = pos - dirX - dirY;
        Vector3Int abajoDerecha = pos + dirX - dirY;

        foreach (Vector3Int posicionVertica in Mathfs.PosicionesEntreYield(abajoDerecha, arribaDerecha))
        {
            Vector3Int diferencia = posicionVertica - abajoDerecha;
            Vector3Int posicionDesfasada = abajoIzquierda + diferencia;

            foreach (Vector3Int posicionHorizontal in Mathfs.PosicionesEntreYield(posicionDesfasada, posicionVertica))
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

    public static List<IObjetoMagico> ObjetoEnVolumen(Vector3 posicion, Vector3 direccion, Vector3 extension)
    {
        List<IObjetoMagico> objetos = new List<IObjetoMagico>();

        ObjetosEnVolumenMundo(posicion, direccion, extension, ref objetos);
        EntidadesEnVolumen(posicion, direccion, extension, ref objetos);

        return objetos;
    }

    private static void EntidadesEnVolumen(Vector3 posicion, Vector3 direccion, Vector3 extension, ref List<IObjetoMagico> objetos)
    {
        Collider[] colliderArray = Physics.OverlapBox(posicion, extension, Quaternion.LookRotation(direccion));
        foreach (Collider collider in colliderArray)
        {
            EntidadMagica entidad = collider.GetComponent<EntidadMagica>();
            if (entidad == null)
                continue;

            objetos.Add(entidad);
        }
    }

    private static void ObjetosEnVolumenMundo(Vector3 posicion, Vector3 direccion, Vector3 extension, ref List<IObjetoMagico> objetos)
    {
        Vector3 versorZ = Vector3.forward * extension.z;
        Quaternion rotacion = Quaternion.LookRotation(direccion);
        Vector3 versorZRotado = rotacion * versorZ;

        Vector3Int pos = Vector3Int.FloorToInt(posicion);
        Vector3Int nuevoVersorZ = Vector3Int.FloorToInt(versorZRotado);

        Vector3Int centroAdelante = pos + nuevoVersorZ, centroAtras = pos - nuevoVersorZ;
        foreach (Vector3Int posicionCentro in Mathfs.PosicionesEntreYield(centroAtras, centroAdelante))
        {
            Vector2 extensionPlano = new Vector2(extension.x, extension.y);
            ObjetosEnAreaMundo(posicionCentro, direccion, extensionPlano, ref objetos);
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
        Vector3 extension = new Vector3(radio, radio, radio);

        Vector3 minimoV = posicion - extension, maximoV = posicion + extension;
        Vector3Int minimo = Vector3Int.FloorToInt(minimoV), maximo = Vector3Int.CeilToInt(maximoV);

        for (int x = minimo.x; x < maximo.x; x++)
            for (int y = minimo.y; y < maximo.y; y++)
                for (int z = minimo.z; z < maximo.z; z++)
                {
                    Vector3 posActual = new Vector3(x, y, z);
                    if ((posicion - posActual).magnitude >= radio)
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
