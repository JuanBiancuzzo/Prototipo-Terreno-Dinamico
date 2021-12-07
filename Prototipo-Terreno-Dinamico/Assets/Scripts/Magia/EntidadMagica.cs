using UnityEngine;

public class EntidadMagica : MonoBehaviour, IObjetoMagico
{
    [SerializeField] protected Iluminacion m_iluminacion;
    [SerializeField] protected RGB m_rgb;
    [SerializeField] protected Alfa m_alfa;

    [SerializeField] protected Temperatura m_temperatura;
    [SerializeField] protected Concentracion m_concentracion;
    [SerializeField] public Constitucion m_constitucion;

    private Color m_color;

    public int IluminacionValor => m_iluminacion.IluminacionValor;
    public int TemperaturaValor => m_temperatura.TemperaturaValor;
    public Color ColorValor => m_color;
    public float AlfaValor => m_alfa.AlfaValor;
    public int ConcentracionValor => m_concentracion.ConcentracionValor;
    public int ConstitucionValor => m_constitucion.ConstitucionValor;

    private void Start()
    {
        ActualizarColor();
    }

    public void DarMagia()
    {
        EventHandlerMagia.current.darEnergia += Dar;
    }

    public void DejarDeDarMagia()
    {
        EventHandlerMagia.current.darEnergia -= Dar;
    }

    public virtual EnergiaCoin Dar(TipoDeMagia tipoDeMagia, EnergiaCoin cantidad)
    {
        IEnergia energia = TipoAEnergia(tipoDeMagia);
        return (energia == null) ? null : energia.Disminuir(cantidad);
    }

    public void RecibirMagia()
    {
        EventHandlerMagia.current.recibirEnergia += Recibir;
    }

    public void DejarDeRecibirMagia()
    {
        EventHandlerMagia.current.recibirEnergia -= Recibir;
    }

    public virtual EnergiaCoin Recibir(TipoDeMagia tipoDeMagia, EnergiaCoin cantidad)
    {
        IEnergia energia = TipoAEnergia(tipoDeMagia);
        return (energia == null) ? null : energia.Aumentar(cantidad);
    }

    protected IEnergia TipoAEnergia(TipoDeMagia tipoDeMagia)
    {
        switch (tipoDeMagia)
        {
            case TipoDeMagia.Iluminacion:
                return m_iluminacion;
            case TipoDeMagia.Color:
                ActualizarColor();
                return m_rgb;
            case TipoDeMagia.Alfa:
                ActualizarColor();
                return m_alfa;
            case TipoDeMagia.Temperatura:
                return m_temperatura;
            case TipoDeMagia.Concentracion:
                return m_concentracion;
            case TipoDeMagia.Consitucion:
                return m_constitucion;
        }

        return null;
    }

    public void ActualizarColor()
    {
        Vector3 rgb = m_rgb.RGBValor;
        for (int i = 0; i < 3; i++)
            m_color[i] = rgb[i];
        m_color.a = m_alfa.AlfaValor;
    }
}
