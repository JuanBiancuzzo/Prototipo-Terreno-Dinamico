using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastSpellController : MonoBehaviour
{
    [SerializeField] Mundo m_mundo;
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
