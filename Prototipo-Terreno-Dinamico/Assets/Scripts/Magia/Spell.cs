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
    LugaresDeEfecto m_rangoDar, m_rangoRecibir;
    List<TipoDeMagia> m_magiaDar = new List<TipoDeMagia>(), m_magiaRecibir = new List<TipoDeMagia>();

    private void Awake()
    {
        TargetSystem.m_mundo = m_mundo;
    }

    private void Start()
    {
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
            return;

        GlyphElemento basePosible = null;
        foreach (GlyphElemento baseActual in m_bases)
            if (baseActual.ConNombre(respuesta.GestureClass))
            {
                basePosible = baseActual;
                break;
            }

        if (basePosible == null)
            return;

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
            return;

        GlyphElemento caracteristicaPosible = null;
        foreach (GlyphElemento caracteristica in m_caracteristicas)
            if (caracteristica.ConNombre(respuesta.GestureClass))
            {
                caracteristicaPosible = caracteristica;
                break;
            }

        if (caracteristicaPosible == null || !caracteristicaPosible.EnRango(posicion))
            return;

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
    }

    private LugaresDeEfecto LugarPorNombre(string nombre)
    {
        switch (nombre)
        {
            case "Punto": return LugaresDeEfecto.Punto;
            case "Linea": return LugaresDeEfecto.Linea;
            case "Area": return LugaresDeEfecto.Area;
            case "Ambiente": return LugaresDeEfecto.Ambiente;
            case "Esfera": return LugaresDeEfecto.Esfera;
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
            case "Iluminacion": return TipoDeMagia.Iluminacion;
        }

        return TipoDeMagia.Invalido;
    }

    public void CrearSpell()
    {
        // llamamos spell system, tener en cuenta que el objeto magico puede ser null
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

