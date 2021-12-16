using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Glyph/Efecto")]
public class GlyphEfecto : GlyphElemento
{
    private string Dar => m_nombre + " dar";
    private string Recibir => m_nombre + " recibir";

    public override string[] GetNombres()
    {
        return new string[2] { Dar, Recibir };
    }

    public override bool ConNombre(string nombre)
    {
        return (nombre == Dar || nombre == Recibir);
    }

    public override PosicionesSpell Posicion(Vector3 punto, string nombre)
    {
        if (nombre == Dar)
            return PosicionesSpell.EfectoDar;
        if (nombre == Recibir)
            return PosicionesSpell.EfectoRecibir;

        return PosicionesSpell.Invalido;
    }
}
