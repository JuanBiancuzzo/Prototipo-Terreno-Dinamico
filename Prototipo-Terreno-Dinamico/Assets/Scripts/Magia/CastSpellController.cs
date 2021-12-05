using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastSpellController : MonoBehaviour
{
    [SerializeField] IContenedorGeneral m_mundo;
    [SerializeField] float m_distancia;

    public enum AccionMagica
    {
        Punto,
        //Circulo,
        //Esfera,
        Linea,
        Area,
        //Volumen
    };



}
