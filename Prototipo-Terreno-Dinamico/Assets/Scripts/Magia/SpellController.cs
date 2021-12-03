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
        Grupo dar = new Grupo()
        {
            elemento = m_mundo.DarElementoMagico(new Vector3(-3, -3, -3)),
            energia = new EnergiaCoin(50),
            tipoDeMagia = TipoDeMagia.Iluminacion
        };

        Grupo recibir = new Grupo()
        {
            elemento = m_mundo.DarElementoMagico(new Vector3(3, -3, 3)),
            energia = new EnergiaCoin(50),
            tipoDeMagia = TipoDeMagia.Color
        };

        bool resultado = m_magiaManager.Spell(dar, recibir);

        if (!resultado)
            Debug.LogError("El spell no funciono");

    }
}
