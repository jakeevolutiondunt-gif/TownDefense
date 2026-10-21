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
    [SerializeField] private int _workerCost = 10;
    [SerializeField] private int _knightCost = 40;
    [SerializeField] private int _archerCost = 60;
    [SerializeField] private int _spearmanCost = 120;
    [SerializeField] private int _mageCost = 300;

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
                    _wallet.AddWorker(1);
                else if (unit == UnitType.Knight)
                    _wallet.AddKnight(1);
                else if (unit == UnitType.Archer)
                    _wallet.AddArcher(1);
                else if (unit == UnitType.Spearman)
                    _wallet.AddSpearman(1);
                else if (unit == UnitType.Mage)
                    _wallet.AddMage(1);
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
