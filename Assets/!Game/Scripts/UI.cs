using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class UI : MonoBehaviour
{
    private const string c_wheatText = "Пшеница: ";
    private const string c_workerText = "Рабы: ";
    private const string c_knightText = "Рыцари: ";
    private const string c_archerText = "Лучники: ";
    private const string c_spearmanText = "Копейщики: ";
    private const string c_mageText = "Маги: ";

    [SerializeField] private GameManager _gameManager;

    [Header("Info")]
    [SerializeField] private TMP_Text _wheatText;
    [SerializeField] private TMP_Text _workerText;
    [SerializeField] private TMP_Text _knightText;
    [SerializeField] private TMP_Text _archerText;
    [SerializeField] private TMP_Text _spearmanText;
    [SerializeField] private TMP_Text _mageText;

    [Header("Timers")]
    [SerializeField] private Image _resTimerImage;
    [SerializeField] private Image _hireTimerImage;

    [Header("Buttons")]
    [SerializeField] private Button _btnHireWorker;
    [SerializeField] private Button _btnHireKnight;
    [SerializeField] private Button _btnHireArcher;
    [SerializeField] private Button _btnHireSpearman;
    [SerializeField] private Button _btnHireMage;

    private int _wheat => _gameManager.WheatCount;

    private int _worker => _gameManager.WorkerCount;
    private int _workerCost => _gameManager.WorkerCost;

    private int _knight => _gameManager.KnightCount;
    private int _knightCost => _gameManager.KnightCost;

    private int _archer => _gameManager.ArcherCount;
    private int _archerCost => _gameManager.ArcherCost;

    private int _spearman => _gameManager.SpearmanCount;
    private int _spearmanCost => _gameManager.SpearmanCost;

    private int _mage => _gameManager.MageCount;
    private int _mageCost => _gameManager.MageCost;

    private float _resTimer => _gameManager.ResIncomeTimer;
    private float _resBaseTime => _gameManager.ResIncomeTime;

    private float _hireBaseTime => _gameManager.HireTime;
    private float _hireTimer => _gameManager.HireTimer;

    public void UpdateResources()
    {
        _wheatText.text = c_wheatText + _wheat;
        _workerText.text = c_workerText + _worker;
        _knightText.text = c_knightText + _knight;
        _archerText.text = c_archerText + _archer;
        _spearmanText.text = c_spearmanText + _spearman;
        _mageText.text = c_mageText + _mage;
    }

    private void Update()
    {
        UpdateHireWorkerButton();
        UpdateHireKnightButton();
        UpdateHireArcherButton();
        UpdateHireSpearmanButton();
        UpdateHireMageButton();
        UpdateTimers();
    }

    private void UpdateTimers()
    {
        if (_hireTimer > 0)
            _hireTimerImage.fillAmount = _hireTimer / _hireBaseTime;
        else
            _hireTimerImage.fillAmount = 1;

        _resTimerImage.fillAmount = 1 - _resTimer / _resBaseTime;
    }

    private void UpdateHireWorkerButton()
    {
        if (_workerCost <= _wheat)
            _btnHireWorker.interactable = true;
        else
            _btnHireWorker.interactable = false;
    }
    private void UpdateHireKnightButton()
    {
        if (_knightCost <= _wheat)
            _btnHireKnight.interactable = true;
        else
            _btnHireKnight.interactable = false;
    }
    private void UpdateHireArcherButton()
    {
        if (_archerCost <= _wheat)
            _btnHireArcher.interactable = true;
        else
            _btnHireArcher.interactable = false;
    }
    private void UpdateHireSpearmanButton()
    {
        if (_spearmanCost <= _wheat)
            _btnHireSpearman.interactable = true;
        else
            _btnHireSpearman.interactable = false;
    }
    private void UpdateHireMageButton()
    {
        if (_mageCost <= _wheat)
            _btnHireMage.interactable = true;
        else
            _btnHireMage.interactable = false;
    }

    public void HireWorker() => _gameManager.Hire(UnitType.Worker);
    public void HireKnight() => _gameManager.Hire(UnitType.Knight);
    public void HireArcher() => _gameManager.Hire(UnitType.Knight);
    public void HireSpearman() => _gameManager.Hire(UnitType.Knight);
    public void HireMage() => _gameManager.Hire(UnitType.Knight);
}
