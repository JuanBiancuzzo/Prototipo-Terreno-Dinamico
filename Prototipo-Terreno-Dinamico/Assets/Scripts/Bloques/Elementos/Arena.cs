using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arena : Solido
{
    public Arena(Vector3Int posicion) : base(posicion)
    {
        m_densidad = 50;
        m_color = new Color(1, 0.88f, 0.29f, 1);
    }

    public override void ActuarEnOtro(Solido elemento, int dt)
    {
        /*Vector3Int direccion = elemento.m_posicion - m_posicion;
        direccion.Clamp(new Vector3Int(-1, -1, -1), new Vector3Int(1, 1, 1));

        // modificar su velocidad para q se caiga a los costados
        foreach (Vector3Int desfase in DesfaseAlrededor())
        {
            int producto = direccion.x * desfase.x + direccion.y * desfase.y + direccion.z * desfase.z;

            if (producto == 0)
            {
                AplicarAceleracion(desfase * 2);
                ActualizarVelocidad(dt);
                break;
            }
        }*/
    }

    private IEnumerable<Vector3Int> DesfaseAlrededor()
    {
        List<Vector3Int> posibilidades = AlrededoresDeDireccion(Vector3Int.zero);

        for (int i = 0; i < posibilidades.Count; i++)
        {
            int index = Random.Range(0, posibilidades.Count - 1);
            yield return posibilidades[index];
            posibilidades.RemoveAt(index);
        }
    }

    public override bool Reacciona(IContenedorConDatos mapa)
    {
        foreach (Vector3Int desfase in BuscarPosicionesDisponibles())
            if (ElementoDejaIntercambiarEn(mapa, m_posicion + desfase + Vector3Int.down, 1))
                return true;
        return false;
    }
}
