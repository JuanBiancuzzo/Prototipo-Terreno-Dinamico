
public class ElementoMagico
{
    public Iluminacion m_iluminacion;
    public RGB m_rgb;
    public Alfa m_alfa;
    public Temperatura m_temperatura;

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
                return m_rgb;
            case TipoDeMagia.Alfa:
                return m_alfa;
            case TipoDeMagia.Temperatura:
                return m_temperatura;
        }

        return null;
    }
}
