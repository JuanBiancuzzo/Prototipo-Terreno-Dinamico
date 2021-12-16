using System.Collections.Generic;
using UnityEngine;


public class SpellController : MonoBehaviour
{
    [SerializeField] Mundo m_mundo;
    [SerializeField] Camera m_camara;

    [SerializeField] float m_extensionMedia;
    [SerializeField] float m_distancia;
    [SerializeField] float m_radio;
    [SerializeField] Transform pies;

    [Space]

    [SerializeField] TipoDeMagia m_tipoMagiaDar;
    [SerializeField] LugaresDeEfecto m_rangoDar;

    [Space]

    [SerializeField] TipoDeMagia m_tipoMagiaRecibir;
    [SerializeField] LugaresDeEfecto m_rangoRecibir;

    [Space]

    [SerializeField] int m_energiaDeseada;

    private void Awake()
    {
        TargetSystem.m_mundo = m_mundo;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
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
                tipoDeMagia = m_tipoMagiaRecibir
            };
            recibirLista.Add(recibir);
        }

        bool resultado = SpellSystem.Spell(darLista, recibirLista, new EnergiaCoin(m_energiaDeseada));

        if (!resultado)
            Debug.LogError("El spell no funciono");

    }

    private List<IObjetoMagico> ObjetosPorRango(LugaresDeEfecto rango)
    {
        Vector3 posicion = transform.position;
        Vector3 direccion = m_camara.transform.forward;

        switch (rango)
        {
            case LugaresDeEfecto.Punto:
                return new List<IObjetoMagico> { TargetSystem.ObjetoEnPunto(posicion, direccion, m_distancia) };
            case LugaresDeEfecto.Linea:
                return TargetSystem.ObjetoEnLinea(posicion, direccion, m_distancia);
            case LugaresDeEfecto.Area:
                return TargetSystem.ObjetosEnArea(posicion + Vector3.forward * direccion.z, direccion, new Vector2(m_extensionMedia, m_extensionMedia));
            case LugaresDeEfecto.Esfera:
                return TargetSystem.ObjetoEnEsfera(posicion, m_radio);
            //case Rangos.Volumen:
            //    return TargetSystem.ObjetoEnVolumen(posicion, direccion, new Vector3(m_extensionMedia, m_extensionMedia, m_extensionMedia));
            //case LugaresDeEfecto.Pies:
            //    return TargetSystem.ObjetosEnArea(pies.position, Vector3.up, new Vector3(m_extensionMedia, m_extensionMedia, m_extensionMedia));
            case LugaresDeEfecto.Ambiente:
                return TargetSystem.ObjetoEnAmbiente(posicion, m_extensionMedia);
        }

        return null;
    }
}
