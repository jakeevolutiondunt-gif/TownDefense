using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UI _ui;
    [SerializeField] private ResourcesFabric _rFabric;
    [Header("Ballance")]
    [SerializeField] private int _startWheat;
    [SerializeField] private int _startGold;
    [SerializeField] private int _workerCost = 10;

    private int _workerCount = 0;
    private Wallet _wallet;
    private void Awake()
    {
        _wallet = new(_startWheat, _startGold);
        _rFabric.Init(_wallet);
    }

    private void Update()
    {
        _ui.UpdateResources(_wallet.Wheat, _wallet.Gold);
    }
}
