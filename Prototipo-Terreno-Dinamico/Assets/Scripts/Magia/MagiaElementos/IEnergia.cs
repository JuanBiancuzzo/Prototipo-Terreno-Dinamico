
public interface IEnergia
{
    public EnergiaCoin Aumentar(EnergiaCoin energia);
    public EnergiaCoin EnergiaCapazDeDar(EnergiaCoin energiaDeseada = null);

    public EnergiaCoin Disminuir(EnergiaCoin energia);
    public EnergiaCoin EnergiaCapazDeRecibir(EnergiaCoin energiaDeseada = null);
}
