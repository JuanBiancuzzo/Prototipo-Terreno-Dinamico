
public interface IEnergia
{
    public EnergiaCoin Aumentar(EnergiaCoin energia);

    public EnergiaCoin EnergiaCapazDeDar();

    public EnergiaCoin Disminuir(EnergiaCoin energia);
    public EnergiaCoin EnergiaCapazDeRecibir();
}
