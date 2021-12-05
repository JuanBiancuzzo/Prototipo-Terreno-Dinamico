using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Colisiones))]
[RequireComponent(typeof(SacarDatos))]
[RequireComponent(typeof(Renderizable))]
[RequireComponent(typeof(Contenedor))]
public class Mundo : IContenedorGeneral
{
    Vector3Int m_posicion => Vector3Int.FloorToInt(transform.position);


    public ElementoSeleccionado objectoSeleccionado;
    private void Awake()
    {
        renderizable = GetComponent<Renderizable>();
        colisiones = GetComponent<Colisiones>();
        sacarDatos = GetComponent<SacarDatos>();
        contenedor = GetComponent<Contenedor>();
    }

    public override IEnumerable<Elemento> ElementoParaActualizar()
    {
        for (int x = 0; x < contenedor.m_extension.x; x++)
            for (int y = 0; y < contenedor.m_extension.y; y++)
                for (int z = 0; z < contenedor.m_extension.z; z++)
                {
                    Elemento elemento = contenedor.EnPosicion(new Vector3Int(x, y, z));
                    if (elemento == null)
                        continue;
                    yield return elemento;
                }
    }

    public override void CalcularIluminacion()
    {
        for (int x = 0; x < contenedor.m_extension.x; x++)
            for (int y = 0; y < contenedor.m_extension.y; y++)
                for (int z = 0; z < contenedor.m_extension.z; z++)
                    contenedor.EnPosicion(new Vector3Int(x, y, z))?.ExpandirLuz();

        for (int x = contenedor.m_extension.x - 1; x >= 0; x--)
            for (int y = contenedor.m_extension.y - 1; y >= 0; y--)
                for (int z = contenedor.m_extension.z - 1; z >= 0; z--)
                    contenedor.EnPosicion(new Vector3Int(x, y, z))?.ExpandirLuz();

    }

    public override void SeleccionarElemento(IRender render, Vector3Int posicion)
    {
        if (objectoSeleccionado == null || !EnRango(posicion))
            return;

        MeshData meshData = new MeshData();
        render.GenerarMeshSeleccion(posicion, this, ref meshData);
        objectoSeleccionado.CargarNuevaMesh(meshData);
    }

    public override bool Insertar(Elemento elemento)
    {
        return contenedor.Insertar(elemento);
    }

    public override Elemento Eliminar(Vector3Int posicion)
    {
        return contenedor.Eliminar(posicion);
    }

    public override Elemento Eliminar(Elemento elemento)
    {
        return contenedor.Eliminar(elemento);
    }

    public override bool Intercambiar(Vector3Int origen, Vector3Int destino)
    {
        return contenedor.Intercambiar(origen, destino);
    }

    public override bool Intercambiar(Elemento elementoOrigen, Elemento elementoDestino)
    {
        return contenedor.Intercambiar(elementoOrigen, elementoDestino);
    }

    public override bool Reemplazar(Elemento elemento, Elemento reemplazo)
    {
        return contenedor.Reemplazar(elemento, reemplazo);
    }

    public override Elemento EnPosicion(Vector3Int posicion)
    {
        return contenedor.EnPosicion(posicion);
    }

    public override bool EnRango(Vector3Int posicion)
    {
        return contenedor.EnRango(posicion);
    }

    public override bool EnRango(Elemento elemento)
    {
        return contenedor.EnRango(elemento);
    }

    public override Color GetColor(Vector3Int posicion, TipoMaterial tipoMaterial, Color defaultColor = default)
    {
        return sacarDatos.GetColor(posicion, tipoMaterial, defaultColor);
    }

    public override float GetValor(Vector3Int posicion, TipoMaterial tipoMaterial, float defaultValor = 0)
    {
        return sacarDatos.GetValor(posicion, tipoMaterial, defaultValor);
    }

    public override int GetIluminacion(Vector3Int posicion, int defaultIluminacion = 0)
    {
        return sacarDatos.GetIluminacion(posicion, defaultIluminacion);
    }

    public override float GetColision(Vector3Int posicion, Constitucion otro, float defaultColision = 0f)
    {
        return sacarDatos.GetColision(posicion, otro, defaultColision);
    }

    public override void Renderizar(IRender render, ISacarDatos contenedor = null)
    {
        renderizable.Renderizar(render, this);
    }

    public override void GenerarMeshColision(IRender render, Extremo rangoEntidad, Constitucion entidad)
    {
        colisiones.GenerarMeshColision(render, rangoEntidad, entidad);
    }

    private void OnDrawGizmos()
    {
        Vector3Int posicion = (contenedor.m_extremo.m_maximo + contenedor.m_extremo.m_minimo) / 2;

        Gizmos.DrawWireCube(posicion + m_posicion, contenedor.m_extension);
    }
}
