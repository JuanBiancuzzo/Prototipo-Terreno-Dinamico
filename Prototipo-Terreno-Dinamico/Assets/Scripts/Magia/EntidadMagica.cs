using UnityEngine;

public class EntidadMagica : MonoBehaviour, IObjetoMagico
{
    [SerializeField] protected ElementoMagico m_elemento;

    public int IluminacionValor => m_elemento.IluminacionValor;
    public int TemperaturaValor => m_elemento.TemperaturaValor;
    public Color ColorValor => m_elemento.ColorValor;
    public float AlfaValor => m_elemento.AlfaValor;
    public int ConcentracionValor => m_elemento.ConcentracionValor;
    public int ConstitucionValor => m_elemento.ConstitucionValor;

    private void Start()
    {
        m_elemento.ActualizarColor();
    }

    public Constitucion ConstitucionActual()
    {
        return m_elemento.m_consitucion;
    }

    public void DarMagia()
    {
        m_elemento.DarMagia();
    }

    public void DejarDeDarMagia()
    {
        m_elemento.DejarDeDarMagia();
    }

    public virtual EnergiaCoin Dar(TipoDeMagia tipoDeMagia, EnergiaCoin cantidad)
    {
        return m_elemento.Dar(tipoDeMagia, cantidad);
    }

    public void RecibirMagia()
    {
        m_elemento.RecibirMagia();
    }

    public void DejarDeRecibirMagia()
    {
        m_elemento.DejarDeRecibirMagia();
    }

    public virtual EnergiaCoin Recibir(TipoDeMagia tipoDeMagia, EnergiaCoin cantidad)
    {
        return m_elemento.Recibir(tipoDeMagia, cantidad);
    }
}
