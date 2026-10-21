using UnityEngine;

public class ResourcesFabric : MonoBehaviour
{
    [Header("Base settings")]
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private float _timeForIncome = 3f;
    [SerializeField] private int _baseWheatIncome = 5;
    [SerializeField] private int _baseGoldIncome = 1;

    [Header("Balance")]
    [SerializeField] private int _wheatIncomePerWorker = 5;
    private bool _isInit = false;
    private Wallet _wallet;
    private int _goldIncome = 0;
    private int _wheatIncome = 0;

    private float _timer = 0;

    public void Init(Wallet wallet)
    {
        if (!_isInit)
        {
            _wallet = wallet;
            _isInit = true;
        }
    }
    private void Update()
    {
        _timer += Time.deltaTime;

        if ( _timer > _timeForIncome)
        {
            int wheatIncome = _baseWheatIncome + _wheatIncomePerWorker;

            _wallet.AddGold(_baseGoldIncome);
            _wallet.AddWheat(_baseWheatIncome);
            _timer -= _timeForIncome;
        }
    }
}
