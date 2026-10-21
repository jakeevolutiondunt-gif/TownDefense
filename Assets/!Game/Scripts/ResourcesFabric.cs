using UnityEngine;

public class ResourcesFabric : MonoBehaviour
{
    [Header("Base settings")]
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private float _timeForIncome = 3f;
    [SerializeField] private int _baseWheatIncome = 5;

    [Header("Balance")]
    [SerializeField] private int _wheatIncomePerWorker = 5;

    private bool _isInit = false;
    private Wallet _wallet;

    private float _incomeTimer = 0;

    public float IncomeTimer => _incomeTimer;
    public float TimeForIncome => _timeForIncome;
    
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
        _incomeTimer += Time.deltaTime;

        if ( _incomeTimer > _timeForIncome)
        {
            int wheatIncome = _baseWheatIncome + _wheatIncomePerWorker * _wallet.WorkerCount;

            _wallet.AddWheat(wheatIncome);
            _incomeTimer -= _timeForIncome;
        }
    }
}
