using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UI _ui;
    [SerializeField] private ResourcesFabric _rFabric;
    [Header("Ballance")]
    [SerializeField] private int _startWheat;
    [SerializeField] private float _hireTime = 3f;
    [SerializeField] protected int _workerCost = 10;
    [SerializeField] protected int _knightCost = 40;
    [SerializeField] protected int _archerCost = 60;
    [SerializeField] protected int _spearmanCost = 120;
    [SerializeField] protected int _mageCost = 300;

    private Wallet _wallet;
    private Queue<UnitType> _hireQueue = new();
    private float _hireTimer = 0f;

    public int WheatCount => _wallet.Wheat;
    public int WorkerCount => _wallet.WorkerCount;
    public int WorkerCost => _workerCost;
    public int KnightCount => _wallet.KnightCount;
    public int KnightCost => _knightCost;
    public int ArcherCount => _wallet.ArcherCount;
    public int ArcherCost => _archerCost;
    public int SpearmanCount => _wallet.SpearmanCount;
    public int SpearmanCost => _spearmanCost;
    public int MageCount => _wallet.MageCount;
    public int MageCost => _mageCost;
    public float HireTimer => _hireTimer;
    public float HireTime => _hireTime;
    public float ResIncomeTimer => _rFabric.IncomeTimer;
    public float ResIncomeTime => _rFabric.TimeForIncome;

    private void Awake()
    {
        _wallet = new(_startWheat, 0, 0, 0, 0, 0);
        _rFabric.Init(_wallet);
    }

    private void Update()
    {
        _ui.UpdateResources();
        HireUnits();
    }

    private void HireUnits()
    {
        if (_hireQueue.Count > 0)
        {
            _hireTimer += Time.deltaTime;
            if (_hireTimer >= _hireTime)
            {
                UnitType unit = _hireQueue.Dequeue();
                if (unit == UnitType.Worker)
                {
                    _wallet.AddWorker(1);
                    _wallet.ReduceWheat(WorkerCost);
                }
                else if (unit == UnitType.Knight)
                {
                    _wallet.AddKnight(1);
                    _wallet.ReduceWheat(KnightCost);
                }
                else if (unit == UnitType.Archer)
                {
                    _wallet.AddArcher(1);
                    _wallet.ReduceWheat(ArcherCost);
                }
                else if (unit == UnitType.Spearman)
                {
                    _wallet.AddSpearman(1);
                    _wallet.ReduceWheat(SpearmanCost);
                }
                else if (unit == UnitType.Mage)
                {
                    _wallet.AddMage(1);
                    _wallet.ReduceWheat(MageCost);
                }
                _hireTimer = 0;
            }
        }
    }
    public void Hire(UnitType unit)
    {
        _hireQueue.Enqueue(unit);
    }

    public enum UnitType
    {
        Worker,
        Knight,
        Archer,
        Spearman,
        Mage
    }
}
public class Wallet: GameManager
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
    public void ReduceWheat(int wheat)
    {
        _wheat -= wheat;
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
