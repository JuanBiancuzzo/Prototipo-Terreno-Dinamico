using System.Collections.Generic;
using UnityEngine;

public class SpellController : MonoBehaviour
{
    [SerializeField] MagiaManager m_magiaManager;
    [SerializeField] FallingSand m_mundo;

    private void Update()
    {
        if (m_magiaManager == null || m_mundo == null)
            return;

        if (Input.GetKeyDown("space"))
            HacerSpell();
    }

    private void HacerSpell()
    {
        List<Grupo> darLista = new List<Grupo>();

        for (int i = 0; i < 8; i++)
        {
            Grupo dar = new Grupo()
            {
                elemento = m_mundo.DarElementoMagico(new Vector3(-4 + i, -3, -3)),
                energia = new EnergiaCoin(20),
                tipoDeMagia = TipoDeMagia.Concentracion
            };

            darLista.Add(dar);
        }

        List<Grupo> recibirLista = new List<Grupo>()
        {
            new Grupo()
            {
                elemento = m_mundo.DarElementoMagico(new Vector3(3, -2, 3)),
                energia = new EnergiaCoin(100),
                tipoDeMagia = TipoDeMagia.Alfa
            },
            new Grupo()
            {
                elemento = m_mundo.DarElementoMagico(new Vector3(3, -2, 3)),
                energia = new EnergiaCoin(60),
                tipoDeMagia = TipoDeMagia.Color
            }
        }; 

        bool resultado = m_magiaManager.Spell(darLista, recibirLista);

        if (!resultado)
            Debug.LogError("El spell no funciono");

    }
}
