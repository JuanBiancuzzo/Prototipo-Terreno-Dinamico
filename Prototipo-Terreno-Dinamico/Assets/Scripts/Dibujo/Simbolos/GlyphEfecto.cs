using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Glyph/Efecto")]
public class GlyphEfecto : Glyph
{
    public enum TipoEfecto
    {
        Recibir,
        Dar
    };

    [SerializeField] TipoEfecto tipoEfecto;
}
