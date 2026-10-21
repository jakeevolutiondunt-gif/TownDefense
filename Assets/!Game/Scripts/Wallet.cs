public class Wallet
{
    private int _wheat;
    private int _workerCount;
    private int _knightCount;
    private int _archerCount;
    private int _spearmanCount;
    private int _mageCount;

    public int Wheat => _wheat;
    public int WorkerCount => _workerCount;
    public int KnightCount => _knightCount;
    public int ArcherCount => _archerCount;
    public int SpearmanCount => _spearmanCount;
    public int MageCount => _mageCount;

    public Wallet(int wheat, int workers, int knights, int archers, int spearmans, int mages)
    {
        _wheat = wheat;
        _workerCount = workers;
        _knightCount = knights;
        _archerCount = archers;
        _spearmanCount = spearmans;
        _mageCount = mages;
    }

#region Wheat
    public void AddWheat(int wheat)
    {
        _wheat += wheat;
    }
    #endregion
#region Worker
    public void AddWorker(int worker)
    {
        _workerCount += worker;
    }
    #endregion
#region Knight
    public void AddKnight(int knight)
    {
        _knightCount += knight;
    }
    #endregion
#region Archer
    public void AddArcher(int archer)
    {
        _archerCount += archer;
    }
    #endregion
#region Spearman
    public void AddSpearman(int spearman)
    {
        _spearmanCount += spearman;
    }
    #endregion
#region Mage
    public void AddMage(int mage)
    {
        _mageCount += mage;
    }
    #endregion
}