using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TipoTarget
{
    Punto, 
    //Circulo,
    //Esfera,
    Linea,
    Area,
    //Volumen
};

public enum Target
{
    Mundo,
    Entidades
};

public struct Spell
{
    Target target;
    TipoTarget tipoTarget;
    TipoDeMagia tipoDeMagia;
    EnergiaCoin energia;
};

[CreateAssetMenu(fileName = "SpellManager", menuName = "Manager/SpellManager", order = 2)]
public class SpellManager : ScriptableObject
{
    public static IContenedorGeneral m_mundo;

    private void OnEnable()
    {
        GameObject obj = GameObject.FindWithTag("Mundo");
        m_mundo = obj.GetComponent<IContenedorGeneral>();
    }

    public void Spell(List<Spell> dar, List<Spell> recibir)
    {

    }

    public void Spell(Spell dar, List<Spell> recibir)
    {
        Spell(new List<Spell> { dar }, recibir);
    }

    public void Spell(List<Spell> dar, Spell recibir)
    {
        Spell(dar, new List<Spell> { recibir });
    }

    public void Spell(Spell dar, Spell recibir)
    {
        Spell(new List<Spell> { dar }, new List<Spell> { recibir });
    }
}
