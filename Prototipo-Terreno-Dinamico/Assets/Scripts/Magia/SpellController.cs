using System.Collections.Generic;
using UnityEngine;

public enum AccionMagica
{
    Punto,
    //Circulo,
    //Esfera,
    Linea,
    Area,
    //Volumen
};

public class SpellController : MonoBehaviour
{
    [SerializeField] FallingSand m_mundo;

    private void Update()
    {
        if (m_mundo == null)
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

        bool resultado = SpellSystem.Spell(darLista, recibirLista);

        if (!resultado)
            Debug.LogError("El spell no funciono");

    }
}
