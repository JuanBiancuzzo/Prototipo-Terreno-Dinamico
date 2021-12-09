
public interface IEnergia
{
    public EnergiaCoin Recibir(EnergiaCoin energia);
    public EnergiaCoin EnergiaCapazDeRecibir(EnergiaCoin energiaDeseada = null);

    public EnergiaCoin Dar(EnergiaCoin energia);
    public EnergiaCoin EnergiaCapazDeDar(EnergiaCoin energiaDeseada = null);
}
