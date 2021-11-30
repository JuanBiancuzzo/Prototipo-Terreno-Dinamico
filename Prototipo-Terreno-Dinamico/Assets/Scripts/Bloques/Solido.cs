using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Solido : Elemento
{
    protected const int gravedad = -1;

    protected int m_velocidad, m_aceleracion;
    protected Vector3Int m_estabilidad;
    protected int m_ficDinamica, m_ficEstatica, m_filtracion;

    protected Solido(Vector3Int posicion, IConetenedorGeneral mundo) : base(posicion, mundo)
    {
        m_estabilidad = Vector3Int.zero;
        m_ficDinamica = 2;
        m_ficEstatica = 4;
        m_filtracion = 1;
        m_velocidad = 0;
        m_aceleracion = 0;
    }

    public override void Avanzar(int dt)
    {
        if (Vacio())
            return;

        ActualizarVelocidad(dt);

        foreach (Vector3Int desfase in Opciones())
        {
            Elemento elemento = m_mundo.EnPosicion(m_posicion + desfase);
            if (elemento == null)
                continue;

            if (MismoElemento(elemento))
            {
                Solido solido = (Solido)elemento;

                if (solido.MaximoParaRecibir() == 0)
                    continue;

                int cantidadADar = CantidadADar();
                int cantidadExtra = solido.Agregar(cantidadADar);
                Agregar(cantidadExtra);

                if (cantidadExtra > 0)
                    continue;
            }
            else if (elemento.PermiteDesplazar())
            {
                elemento.Desplazar();
                Elemento remplazo = Expandir(m_posicion + desfase);
                m_mundo.Insertar(remplazo);
            } 
            else if (elemento.PermiteIntercambiar())
            {
                elemento.Intercambiar(this);
            }
            else
            {
                continue;
            }

            break;
        }
    }

    private IEnumerable<Vector3Int> Opciones()
    {
        yield return new Vector3Int(0, -1, 0);

        List<Vector3Int> diagonales = new List<Vector3Int>()
        {
            new Vector3Int( 1, -1, 1),  new Vector3Int(1, -1, 0), new Vector3Int( 1, -1, -1),
            new Vector3Int( 0, -1, 1),                            new Vector3Int( 0, -1, -1),
            new Vector3Int(-1, -1, 1), new Vector3Int(-1, -1, 0), new Vector3Int(-1, -1, -1)
        };

        int opciones = diagonales.Count;
        for (int i = 0; i < opciones; i++)
        {
            int index = Random.Range(0, diagonales.Count - 1);
            yield return diagonales[index];
            diagonales.RemoveAt(index);
        }
    }

    protected void ActualizarVelocidad(int dt)
    {
        m_velocidad += m_aceleracion * dt;
    }

    public override int CantidadADar()
    {
        int cantidad = (Mathf.Abs(m_velocidad) + 1) * 25;
        return DarCantidad(cantidad);
    }

    public override void Desplazar()
    {
        List<Elemento> elementoDelMismoElemento = new List<Elemento>();

        for (int x = -1; x <= 1; x++)
            for (int y = -1; y <= 0; y++)
                for (int z = -1; z <= 1; z++)
                {
                    if (x == 0 && z == 0 && y == 0)
                        continue;

                    Elemento elemento = m_mundo.EnPosicion(m_posicion + new Vector3Int(x, y, z));
                    if (elemento != null && MismoElemento(elemento) && elemento.MaximoParaRecibir() > 0)
                        elementoDelMismoElemento.Add(elemento);
                }

        elementoDelMismoElemento.Sort((a, b) => b.m_densidad.CompareTo(a.m_densidad));

        for (int i = 0; i < elementoDelMismoElemento.Count; i++)
        {
            Elemento elemento = elementoDelMismoElemento[i];

            int cantidadADar = DarCantidad(m_densidad / (elementoDelMismoElemento.Count - i));
            int cantidadExtra = elemento.Agregar(cantidadADar);
            Agregar(cantidadExtra);
        }

        if (m_densidad > 0)
            Debug.LogError("Se esta perdiendo: " + m_densidad + " densidad");
    }

    public override bool PermiteDesplazar()
    {
        List<Elemento> elementoDelMismoElemento = new List<Elemento>();

        for (int x = -1; x <= 1; x++)
            for (int y = -1; y <= 0; y++)
                for (int z = -1; z <= 1; z++)
                {
                    if (x == 0 && z == 0 && y == 0)
                        continue;

                    Elemento elemento = m_mundo.EnPosicion(m_posicion + new Vector3Int(x, y, z));
                    if (elemento != null && MismoElemento(elemento) && elemento.MaximoParaRecibir() > 0)
                        elementoDelMismoElemento.Add(elemento);
                }

        int cantidadAdimitida = 0;
        foreach (Elemento elemento in elementoDelMismoElemento)
            cantidadAdimitida += elemento.MaximoParaRecibir();

        return m_densidad < cantidadAdimitida * 0.9f;
    }

    public override bool PermiteIntercambiar()
    {
        return false;
    }

    public override bool MismoTipo(Elemento elemento)
    {
        return elemento.MismoTipo(this);
    }

    public override bool MismoTipo(Solido solido)
    {
        return true;
    }

    public override bool MismoTipo(Liquido liquido)
    {
        return false;
    }

    public override bool MismoTipo(Gaseoso gaseoso)
    {
        return false;
    }


    /*
    public override void ActuanEnElemento(Elemento elemento, int dt)
    {
        elemento.ActuarEnOtro(this, dt);
    }

    public override void Avanzar(IContenedorConDatos mapa, int dt)
    {
        ActualizarVelocidad(dt);
        Vector3Int direccion = Direccion(dt);

        foreach (Vector3Int posicion in PosicionesEnMovimiento(mapa, dt))
        {
            Vector3Int posicionAnterior = m_posicion;

            Elemento elemento = mapa.EnPosicion(posicion);
            if (elemento == null)
            {
                mapa.Intercambiar(m_posicion, posicion);
                ActualizarAlrededores(posicionAnterior, direccion, mapa);
                continue;
            }

            elemento.ActuanEnElemento(this, dt);

            bool puedoIntercambiar = elemento.PermitoIntercambiar(this, dt);
            if (puedoIntercambiar)
                mapa.Intercambiar(m_posicion, posicion);

            ActualizarAlrededores(posicionAnterior, direccion, mapa);
        }

        if (Reacciona(mapa))
        {
            //NecesitoActualizar(this);
        }
    }

    
    protected virtual IEnumerable<Vector3Int> PosicionesEnMovimiento(IContenedorConDatos mapa, int dt)
    {
        float tiempoRestante = dt;

        while (tiempoRestante > 0)
        {
            Vector3Int direccion = DireccionVelocidad(dt);
            float promedioTiempo = TiempoPorDireccion(direccion, dt);

            tiempoRestante -= promedioTiempo;

            Vector3Int posicionNueva = m_posicion + direccion;
            bool sePuedeMover = ElementoDejaIntercambiarEn(mapa, posicionNueva, dt);

            yield return m_posicion + direccion;
            
            if (promedioTiempo <= 0.01f)
                break;

            if (sePuedeMover)
                continue;

            foreach (Vector3Int des in BuscarPosicionesDisponibles())
            {
                if (!ElementoDejaIntercambiarEn(mapa, posicionNueva + des, dt))
                    continue;

                tiempoRestante -= TiempoPorDireccion(des, dt);
                yield return posicionNueva + des;
                break;
            }

            break;
        }
    }

    protected Vector3Int DireccionVelocidad(int dt)
    {
        Vector3Int direccion = Vector3Int.zero;

        int mayorComponente = m_velocidad[Mathfs.MayorComponente(m_velocidad)];
        int minimoEnDireccion = Mathf.Abs(Mathf.FloorToInt(mayorComponente / 2f));

        for (int i = 0; i < 3; i++)
            if (Mathf.Abs(m_velocidad[i]) >= minimoEnDireccion && m_velocidad[i] != 0)
                direccion[i] = (int)Mathf.Sign(m_velocidad[i]);

        return direccion;
    }

    private float TiempoPorDireccion(Vector3Int direccion, int dt)
    {
        float promedioTiempo = 0;
        for (int i = 0; i < 3; i++)
            if (direccion[i] != 0)
                promedioTiempo += (Mathf.Abs(direccion[i]) * dt) / (float)Mathf.Abs(m_velocidad[i]);
        promedioTiempo = promedioTiempo / 3f;
        return promedioTiempo;
    }

    protected IEnumerable<Vector3Int> BuscarPosicionesDisponibles()
    {
        List<Vector3Int> posibilidades = new List<Vector3Int>
        {
            new Vector3Int(-1, 0, -1), new Vector3Int(-1, 0, 0), new Vector3Int(-1, 0, 1),
            new Vector3Int( 0, 0, -1),                           new Vector3Int( 0, 0, 1),
            new Vector3Int( 1, 0, -1), new Vector3Int( 1, 0, 0), new Vector3Int( 1, 0, 1),
        };

        for (int i = 0; i < 8; i++)
        {
            int index = Random.Range(0, posibilidades.Count - 1);
            yield return posibilidades[index];
            posibilidades.RemoveAt(index);
        }
    }

    protected bool ElementoDejaIntercambiarEn(IContenedorConDatos mapa, Vector3Int posicion, int dt)
    {
        if (!mapa.EnRango(posicion))
            return false;

        Elemento elemento = mapa.EnPosicion(posicion);
        return elemento == null || elemento.PermitoIntercambiar(this, dt);
    }

    public override bool PermitoIntercambiar(Solido elemento, int dt)
    {
        return false;
    }

    public override bool PermitoIntercambiar(Liquido elemento, int dt)
    {
        return false;
    }

    public override bool PermitoIntercambiar(Gaseoso elemento, int dt)
    {
        return false;
    }

    public override bool Reacciona(IContenedorConDatos mapa)
    {
        return false;
    }


    protected void ActualizarAlrededores(Vector3Int posicion, Vector3Int direccion, IContenedorConDatos mapa)
    {
        foreach (Vector3Int def in AlrededoresDeDireccion(direccion))
        {
            Elemento elemento = (Elemento)mapa.EnPosicion(def + posicion);
            if (elemento == null)
                continue;
            if (elemento.Reacciona(mapa))
            {
                //elemento.NecesitoActualizar(elemento);
            }
        }
    }

    protected List<Vector3Int> AlrededoresDeDireccion(Vector3Int direccion)
    {
        List<Vector3Int> posibilidades = new List<Vector3Int>();
        for (int x = -1; x <= 1; x++)
            for (int y = -1; y <= 1; y++)
                for (int z = -1; z <= 1; z++)
                {
                    if (!(x == 0 && y == 0 && z == 0))
                        posibilidades.Add(new Vector3Int(x, y, z));
                }

        return posibilidades;
    }

    protected Vector3Int Direccion(int dt)
    {
        Vector3Int velocidad = m_velocidad * dt;
        Vector3Int min = new Vector3Int(-1, -1, -1), max = new Vector3Int(1, 1, 1);

        velocidad.Clamp(min, max);

        return velocidad;
    }*/
}
