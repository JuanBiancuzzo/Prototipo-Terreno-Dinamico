using PDollarGestureRecognizer;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    [SerializeField] Mundo m_mundo;

    [Space]

    [SerializeField] List<GlyphElemento> m_bases = new List<GlyphElemento>();
    [SerializeField] List<GlyphElemento> m_caracteristicas = new List<GlyphElemento>();
    [SerializeField] [Range(0, 1)] float m_margenDeError = 0.8f;

    [Space]

    [SerializeField] float m_distancia;
    [SerializeField] float m_extensionMedia;
    [SerializeField] float m_radio;

    List<Gesture> m_trainingSet = new List<Gesture>();

    PlanoDireccionado m_planoBase;
    bool m_planoSeleccionado = false;
    LugaresDeEfecto m_rangoDar = LugaresDeEfecto.Esfera, m_rangoRecibir = LugaresDeEfecto.Punto;
    List<TipoDeMagia> m_magiaDar = new List<TipoDeMagia>(), m_magiaRecibir = new List<TipoDeMagia>();

    private void OnEnable() => CrearPuntos.TerminarSpell += CastearSpell;
    private void OnDisable() => CrearPuntos.TerminarSpell -= CastearSpell;


    private void Awake()
    {
        TargetSystem.m_mundo = m_mundo;

        AgregarLista(m_bases);
        AgregarLista(m_caracteristicas);
    }

    private void AgregarLista(List<GlyphElemento> glyphs)
    {
        foreach (GlyphElemento glyph in glyphs)
            foreach (string nombre in glyph.GetNombres())
            {
                string[] files = Directory.GetFiles(Application.persistentDataPath, nombre + ".xml");
                m_trainingSet.Add(GestureIO.ReadGestureFromFile(files[0]));
            }
    }

    public void AgregarBase(PlanoDireccionado plano, List<Vector3> puntos)
    {
        Result respuesta = ReconocimientoBasico.ReconocerFigura(puntos, plano, m_trainingSet);
        if (respuesta.Score < m_margenDeError)
        {
            Debug.Log("No tiene suficientes puntos con: " + respuesta.Score + " de tipo " + respuesta.GestureClass);
            return;
        }

        GlyphElemento basePosible = null;
        foreach (GlyphElemento baseActual in m_bases)
            if (baseActual.ConNombre(respuesta.GestureClass))
            {
                basePosible = baseActual;
                break;
            }

        if (basePosible == null)
        {
            Debug.Log("No se encontro base");
            return;
        }

        Reiniciar();
        m_planoBase = plano;
        transform.position = plano.posicion;
        transform.forward = plano.plano.normal;
        m_planoSeleccionado = true;
    }

    void Reiniciar()
    {

    }

    public void AgregarGlyph(List<Vector3> puntos, Vector3 posicion)
    {
        if (!m_planoSeleccionado)
            return;

        Result respuesta = ReconocimientoBasico.ReconocerFigura(puntos, m_planoBase, m_trainingSet);
        if (respuesta.Score < m_margenDeError)
        {
            Debug.Log("No tiene suficientes puntos con: " + respuesta.Score + " de tipo " + respuesta.GestureClass);
            return;
        }

        GlyphElemento caracteristicaPosible = null;
        foreach (GlyphElemento caracteristica in m_caracteristicas)
            if (caracteristica.ConNombre(respuesta.GestureClass))
            {
                caracteristicaPosible = caracteristica;
                break;
            }

        if (caracteristicaPosible == null)
        {
            Debug.Log("No se encontro la magia/lugar");
            return;
        }

        posicion = ReconocimientoBasico.ProyeccionEnPlano(m_planoBase, posicion);

        if (!caracteristicaPosible.EnRango(posicion))
        {
            Debug.Log("De tipo: " + respuesta.GestureClass);
            Debug.Log("no esta en su rango " + posicion);
            return;
        }

        switch (caracteristicaPosible.Posicion(posicion, respuesta.GestureClass))
        {
            case PosicionesSpell.Dar:
                m_rangoDar = LugarPorNombre(caracteristicaPosible.GetNombreCompleto());
                break;
            case PosicionesSpell.EfectoDar:
                m_magiaDar.Add(MagiaPorNombre(caracteristicaPosible.GetNombreCompleto()));
                break;
            case PosicionesSpell.EfectoRecibir:
                m_magiaRecibir.Add(MagiaPorNombre(caracteristicaPosible.GetNombreCompleto()));
                break;
            case PosicionesSpell.Recibir:
                m_rangoRecibir = LugarPorNombre(caracteristicaPosible.GetNombreCompleto());
                break;
        }

        Debug.Log(respuesta.GestureClass);
    }

    private LugaresDeEfecto LugarPorNombre(string nombre)
    {
        switch (nombre)
        {
            case "Punto": return LugaresDeEfecto.Punto;
            case "Linea": return LugaresDeEfecto.Linea;
            case "Area": return LugaresDeEfecto.Area;
            case "Ambiente": return LugaresDeEfecto.Ambiente;
            case "Circulo": return LugaresDeEfecto.Esfera;
        }

        return LugaresDeEfecto.Invalido;
    }

    private TipoDeMagia MagiaPorNombre(string nombre)
    {
        switch (nombre)
        {
            case "Color": return TipoDeMagia.Color;
            case "Alfa": return TipoDeMagia.Alfa;
            case "Temperatura": return TipoDeMagia.Temperatura;
            case "Concentracion": return TipoDeMagia.Concentracion;
            case "Constitucion": return TipoDeMagia.Constitucion;
            case "Luz": return TipoDeMagia.Iluminacion;
        }

        return TipoDeMagia.Invalido;
    }

    public void CastearSpell()
    {
        if (!Valido())
        {
            Debug.Log("Spell no valido");
            return;
        }

        if (m_magiaRecibir.Count == 0)
            m_magiaRecibir.Add(TipoDeMagia.Temperatura);

        if (m_magiaDar.Count == 0)
            m_magiaDar.Add(TipoDeMagia.Temperatura);

        Debug.Log("Tiene que sacar: " + m_rangoDar);
        foreach (TipoDeMagia magia in m_magiaDar)
            Debug.Log("Con la magia: " + magia);

        Debug.Log("Tiene que agregar: " + m_rangoRecibir);
        foreach (TipoDeMagia magia in m_magiaRecibir)
            Debug.Log("Con la magia: " + magia);

        // llamamos spell system, tener en cuenta que el objeto magico puede ser null

        List<Grupo> darLista = new List<Grupo>();
        foreach (IObjetoMagico objetoMagico in ObjetosPorRango(m_rangoDar))
        {
            if (objetoMagico == null)
                continue;
            foreach (TipoDeMagia magia in m_magiaDar)
                darLista.Add(new Grupo
                {
                    elemento = objetoMagico,
                    tipoDeMagia = magia
                });
        }

        List<Grupo> recibirLista = new List<Grupo>();
        foreach (IObjetoMagico objetoMagico in ObjetosPorRango(m_rangoRecibir))
        {
            if (objetoMagico == null)
                continue;
            foreach (TipoDeMagia magia in m_magiaRecibir)
                recibirLista.Add(new Grupo
                {
                    elemento = objetoMagico,
                    tipoDeMagia = magia
                });
        }

        bool algo = SpellSystem.Spell(darLista, recibirLista, new EnergiaCoin(60));
        if (!algo)
            Debug.LogError("No funca");
    }

    private bool Valido()
    {
        return m_magiaRecibir.Count > 0 || m_magiaDar.Count > 0;
    }

    private List<IObjetoMagico> ObjetosPorRango(LugaresDeEfecto rango)
    {
        Vector3 posicion = transform.position;
        Vector3 direccion = transform.forward;

        switch (rango)
        {
            case LugaresDeEfecto.Punto:
                return new List<IObjetoMagico> { TargetSystem.ObjetoEnPunto(posicion, direccion, m_distancia) };
            case LugaresDeEfecto.Linea:
                return TargetSystem.ObjetoEnLinea(posicion, direccion, m_distancia);
            case LugaresDeEfecto.Area:
                return TargetSystem.ObjetosEnArea(posicion + Vector3.forward * m_distancia, direccion, new Vector2(m_extensionMedia, m_extensionMedia));
            case LugaresDeEfecto.Esfera:
                return TargetSystem.ObjetoEnEsfera(posicion, m_radio);
            case LugaresDeEfecto.Ambiente:
                return TargetSystem.ObjetoEnAmbiente(posicion, m_extensionMedia);
        }

        return null;
    }
}

public enum PosicionesSpell
{
    Dar,
    EfectoDar,
    EfectoRecibir,
    Recibir,
    Invalido
}

public enum LugaresDeEfecto
{
    Punto,
    Linea,
    Area,
    Esfera,
    Ambiente,
    Invalido
}

public enum TipoDeMagia
{
    Color,
    Alfa,
    Temperatura,
    Concentracion,
    Constitucion,
    Iluminacion,
    Invalido
};

