public class Wallet
{
    private int _wheat;
    private int _gold;
    private int _workerCount;

    private int WorkerCount => _workerCount;
    public int Wheat => _wheat;
    public int Gold => _gold;
    #region Gold
    public void AddGold(int gold)
    {
        _gold += gold;
    }
    public bool GetGold(int gold)
    {
        if (gold >= _gold)
        {
            _gold -= gold;
            return true;
        }
        return false;
    }
    public bool AmountGold(int gold) => _gold >= gold;
    public Wallet(int wheat, int gold)
    {
        _wheat = wheat;
        _gold = gold;
    }
    #endregion Gold
    #region Wheat
    public void AddWheat(int wheat)
    {
        _wheat += wheat;
    }

    public bool GetWheat(int wheat)
    {
        if (_wheat >= wheat)
        {
            _wheat -= wheat;
            return true;
        }
        return false;
    }

    public bool AmountWheat(int wheat) => _wheat >= wheat;
    #endregion Wheat
}