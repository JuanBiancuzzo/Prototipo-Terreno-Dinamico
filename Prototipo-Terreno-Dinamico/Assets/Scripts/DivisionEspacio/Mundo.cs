using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mundo : IConetenedorGeneral
{
    Vector3Int m_posicion => Vector3Int.FloorToInt(transform.position);
    public Extremo m_extremo;
    public IGenerador m_generador;
    Vector3Int m_extension => m_extremo.m_maximo - m_extremo.m_minimo;

    Elemento[,,] m_elementos;

    Mesh m_mesh;
    Mesh m_meshColision;
    MeshCollider m_meshCollider;

    public ElementoSeleccionado objectoSeleccionado;
    private void Awake()
    {
        m_mesh = new Mesh();
        m_meshColision = new Mesh();
        GetComponent<MeshFilter>().sharedMesh = m_mesh;
        m_meshCollider = GetComponent<MeshCollider>();
    }

    private void Start()
    {
        m_elementos = new Elemento[m_extension.x, m_extension.y, m_extension.z];

        for (int x = 0; x < m_extension.x; x++)
            for (int y = 0; y < m_extension.y; y++)
                for (int z = 0; z < m_extension.z; z++)
                {
                    int altura = m_generador.ValorEnPosicion(x, z, m_extremo.m_minimo.y, m_extremo.m_maximo.y);
                    m_elementos[x, y, z] = ElementoPorAltura(x, y, z, altura);
                }
    }

    private Elemento ElementoPorAltura(int x, int y, int z, int altura)
    {
        Vector3Int posicion = m_extremo.m_minimo + new Vector3Int(x, y, z);

        if (altura < posicion.y)
            return new Aire(posicion, this);
        else if (altura == posicion.y)
            return new Arena(posicion, this);
        else
            return new Concreto(posicion, this);
    }

    public override IEnumerable<Elemento> ElementoParaActualizar()
    {
        for (int x = 0; x < m_extension.x; x++)
            for (int y = 0; y < m_extension.y; y++)
                for (int z = 0; z < m_extension.z; z++)
                {
                    Elemento elemento = m_elementos[x, y, z];
                    if (elemento == null)
                        continue;
                    yield return elemento;
                }
    }

    public override void CalcularIluminacion()
    {
        for (int x = 0; x < m_extension.x; x++)
            for (int y = 0; y < m_extension.y; y++)
                for (int z = 0; z < m_extension.z; z++)
                    m_elementos[x, y, z]?.ExpandirLuz();

        for (int x = m_extension.x - 1; x >= 0; x--)
            for (int y = m_extension.y - 1; y >= 0; y--)
                for (int z = m_extension.z - 1; z >= 0; z--)
                    m_elementos[x, y, z]?.ExpandirLuz();

    }

    public override bool Insertar(Elemento elemento)
    {
        if (elemento == null)
            return false;

        Vector3Int posicion = elemento.Posicion();
        if (!EnRango(posicion))
            return false;

        Elemento elementoEnPosicion = EnPosicion(posicion);
        if (elementoEnPosicion == null || elementoEnPosicion.Vacio())
        {
            AgregarEnPosicion(posicion, elemento);
            return true;
        }

        if (elemento.MismoElemento(elementoEnPosicion) && elementoEnPosicion.MaximoParaRecibir() >= elemento.ConcentracionValor)
        {
            int cantidadExtra = elementoEnPosicion.Agregar(elemento.ConcentracionValor);
            if (cantidadExtra > 0)
                Debug.LogWarning("Algo esta funcionando mal aca");
        }
        else if (elementoEnPosicion.PermiteDesplazar())
        {
            elementoEnPosicion.Desplazar();
            AgregarEnPosicion(posicion, elemento);
            return true;
        }

        return false;
    }

    public override Elemento Eliminar(Vector3Int posicion)
    {
        if (!EnRango(posicion))
            return null;

        Elemento elementoConMayorDensidad = Elemento.ElementoConMayorConcentracion(posicion, this);
        if (elementoConMayorDensidad == null)
            return null;

        Elemento reemplazo = elementoConMayorDensidad.Expandir(posicion);
        Elemento actual = EnPosicion(posicion);

        AgregarEnPosicion(posicion, reemplazo);
        return actual;
    }

    public override Elemento Eliminar(Elemento elemento)
    {
        return (elemento == null) ? null : Eliminar(elemento.Posicion());
    }

    public override bool Intercambiar(Vector3Int origen, Vector3Int destino)
    {
        if (!EnRango(origen) || !EnRango(destino))
            return false;

        Elemento elementoOrigen = EnPosicion(origen);
        Elemento elementoDestino = EnPosicion(destino);

        AgregarEnPosicion(destino, elementoOrigen);
        AgregarEnPosicion(origen, elementoDestino);

        elementoOrigen?.ActualizarPosicion(destino);
        elementoDestino?.ActualizarPosicion(origen);

        return true;
    }

    public override bool Intercambiar(Elemento elementoOrigen, Elemento elementoDestino)
    {
        if (elementoOrigen == null || elementoDestino == null)
            return false;
        return Intercambiar(elementoOrigen.Posicion(), elementoDestino.Posicion());
    }

    public override bool Reemplazar(Elemento elemento, Elemento reemplazo)
    {
        if (elemento == null | reemplazo == null)
            return false;

        Vector3Int posicion = PosicionRelativa(elemento.Posicion());
        m_elementos[posicion.x, posicion.y, posicion.z] = reemplazo;

        return true;
    }

    public override Elemento EnPosicion(Vector3Int posicion)
    {
        if (!EnRango(posicion))
            return null;

        Vector3Int posicionRelativa = PosicionRelativa(posicion);
        return m_elementos[posicionRelativa.x, posicionRelativa.y, posicionRelativa.z];
    }

    public void AgregarEnPosicion(Vector3Int posicion, Elemento elemento)
    {
        Vector3Int posicionRelativa = PosicionRelativa(posicion);
        m_elementos[posicionRelativa.x, posicionRelativa.y, posicionRelativa.z] = elemento;
    }

    public override bool EnRango(Vector3Int posicion)
    {
        Vector3Int posicionRelativa = PosicionRelativa(posicion);
        for (int i = 0; i < 3; i++)
            if (posicionRelativa[i] < 0 || posicionRelativa[i] >= m_extension[i])
                return false;
        return true;
    }

    public override bool EnRango(Elemento elemento)
    {
        return (elemento == null) ? false : EnRango(elemento.Posicion());
    }

    private Vector3Int PosicionRelativa(Vector3Int posicionMundo)
    {
        return posicionMundo - m_extremo.m_minimo;
    }

    public override Color GetColor(Vector3Int posicion, TipoMaterial tipoMaterial, Color defaultColor = default)
    {
        if (!EnRango(posicion))
            return defaultColor;

        Elemento elemento = EnPosicion(posicion);
        if (elemento == null)
            return defaultColor;

        return elemento.GetColor(tipoMaterial, defaultColor);
    }

    public override float GetValor(Vector3Int posicion, TipoMaterial tipoMaterial, float defaultValor = 0)
    {
        if (!EnRango(posicion))
            return defaultValor;

        Elemento elemento = EnPosicion(posicion);
        if (elemento == null)
            return defaultValor;

        return elemento.GetValor(tipoMaterial, defaultValor);
    }

    public override int GetIluminacion(Vector3Int posicion, int defaultIluminacion = 0)
    {
        if (!EnRango(posicion))
            return defaultIluminacion;

        Elemento elemento = EnPosicion(posicion);
        if (elemento == null)
            return defaultIluminacion;

        return elemento.GetIluminacion();
    }

    public override float GetColision(Vector3Int posicion, Constitucion otro, float defaultColision = 0f)
    {
        if (!EnRango(posicion))
            return defaultColision;

        Elemento elemento = EnPosicion(posicion);
        if (elemento == null)
            return defaultColision;

        return elemento.GetColision(otro, defaultColision);
    }

    public override void Renderizar(IRender render, ISacarDatos contenedor = null)
    {
        MeshData meshDataOpaco = new MeshData();
        render.GenerarMesh(m_extremo, this, ref meshDataOpaco, TipoMaterial.Opaco);
        Mesh meshOpaco = new Mesh();
        meshDataOpaco.RellenarMesh(meshOpaco);

        MeshData meshDataTranslucido = new MeshData();
        render.GenerarMesh(m_extremo, this, ref meshDataTranslucido, TipoMaterial.Translucido);
        Mesh meshTranslucido = new Mesh();
        meshDataTranslucido.RellenarMesh(meshTranslucido);

        List<CombineInstance> finalCombiner = new List<CombineInstance>();
        foreach ( Mesh mesh in new List<Mesh> { meshOpaco, meshTranslucido } )
        {
            CombineInstance ci = new CombineInstance();
            ci.mesh = mesh;
            ci.subMeshIndex = 0;
            ci.transform = Matrix4x4.identity;
            finalCombiner.Add ( ci );
        }
        m_mesh.CombineMeshes(finalCombiner.ToArray(), false);
    }

    public override void GenerarMeshColision(IRender render, Extremo rangoEntidad, Constitucion entidad)
    {
        m_meshColision.Clear();
        MeshData meshDataColision = new MeshData();

        render.GenerarMeshColision(rangoEntidad, this, ref meshDataColision, entidad);
        meshDataColision.RellenarMesh(m_meshColision);

        m_meshCollider.sharedMesh = m_meshColision;
    }

    public override void SeleccionarElemento(IRender render, Vector3Int posicion)
    {
        if (objectoSeleccionado == null || !EnRango(posicion))
            return;

        MeshData meshData = new MeshData();
        render.GenerarMeshSeleccion(posicion, this, ref meshData);
        objectoSeleccionado.CargarNuevaMesh(meshData);
    }

    private void OnDrawGizmos()
    {
        Vector3Int posicion = (m_extremo.m_maximo + m_extremo.m_minimo) / 2;

        Gizmos.DrawWireCube(posicion + m_posicion, m_extension);
    }
}
