using System.Collections.Generic;
using UnityEngine;


public enum Rangos
{
    Punto,
    Linea,
    Area,
    Ambiente
}

public class SpellController : MonoBehaviour
{
    [SerializeField] Mundo m_mundo;
    [SerializeField] Camera m_camara;

    [SerializeField] float m_extensionMedia;
    [SerializeField] float m_distancia;

    [Space]

    [SerializeField] TipoDeMagia m_tipoMagiaDar;
    [SerializeField] Rangos m_rangoDar;
    [SerializeField] int m_energiaDar;

    [Space]

    [SerializeField] TipoDeMagia m_tipoMagiaRecibir;
    [SerializeField] Rangos m_rangoRecibir;
    [SerializeField] int m_energiaRecibir;

    private void Awake()
    {
        TargetSystem.m_mundo = m_mundo;
    }

    private void Update()
    {
        if (Input.GetKeyDown("space"))
            HacerSpell();
    }

    private void HacerSpell()
    {
        List<Grupo> darLista = new List<Grupo>();
        foreach (IObjetoMagico objetoMagico in ObjetosPorRango(m_rangoDar))
        {
            Grupo dar = new Grupo()
            {
                elemento = objetoMagico,
                energia = new EnergiaCoin(m_energiaDar),
                tipoDeMagia = m_tipoMagiaDar
            };

            darLista.Add(dar);
        }

        List<Grupo> recibirLista = new List<Grupo>();
        foreach (IObjetoMagico objetoMagico in ObjetosPorRango(m_rangoRecibir))
        {
            Grupo recibir = new Grupo()
            {
                elemento = objetoMagico,
                energia = new EnergiaCoin(m_energiaRecibir),
                tipoDeMagia = m_tipoMagiaRecibir
            };
            recibirLista.Add(recibir);
        }

        bool resultado = SpellSystem.Spell(darLista, recibirLista);

        if (!resultado)
            Debug.LogError("El spell no funciono");

    }

    private List<IObjetoMagico> ObjetosPorRango(Rangos rango)
    {
        Vector3 posicion = transform.position;
        Vector3 direccion = m_camara.transform.forward;

        switch (rango)
        {
            case Rangos.Punto:
                return new List<IObjetoMagico> { TargetSystem.ObjetoEnPunto(posicion, direccion, m_distancia) };
            case Rangos.Linea:
                return TargetSystem.ObjetoEnLinea(posicion, direccion, m_distancia);
            case Rangos.Area:
                return TargetSystem.ObjetosEnArea(posicion, direccion, new Vector2(m_extensionMedia, m_extensionMedia));
            case Rangos.Ambiente:
                return TargetSystem.ObjetoEnAmbiente(posicion, m_extensionMedia);
        }

        return null;
    }
}
