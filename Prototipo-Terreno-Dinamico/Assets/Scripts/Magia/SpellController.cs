using System.Collections.Generic;
using UnityEngine;


public class SpellController : MonoBehaviour
{
    [SerializeField] Mundo m_mundo;
    [SerializeField] Camera m_camara;

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
        int energiaInicial = 50, energiaFinal = 100;
        Vector3 posicion = transform.position;
        float extensionMedia = 2f;
        TipoDeMagia inicio = TipoDeMagia.Color, final = TipoDeMagia.Alfa;


        List<Grupo> darLista = new List<Grupo>();

        foreach (IObjetoMagico objetoMagico in TargetSystem.ObjetoEnAmbiente(posicion, extensionMedia))
        {
            Grupo dar = new Grupo()
            {
                elemento = objetoMagico,
                energia = new EnergiaCoin(energiaInicial),
                tipoDeMagia = inicio
            };

            darLista.Add(dar);
        }

        Grupo recibir = new Grupo()
        {
            elemento = TargetSystem.ObjetoEnPunto(posicion, m_camara.transform.forward, 6f),
            energia = new EnergiaCoin(energiaFinal),
            tipoDeMagia = final
        };

        bool resultado = SpellSystem.Spell(darLista, recibir);

        if (!resultado)
            Debug.LogError("El spell no funciono");

    }
}
