using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    private const string c_wheatText = "Пшеница: ";
    private const string c_goldText = "Золото: ";


    [SerializeField] private TMP_Text _wheatText;
    [SerializeField] private TMP_Text _goldText;
    [SerializeField] private Button _btnHireWorker;

    public void UpdateResources(int wheat, int gold)
    {
        _wheatText.text = c_wheatText + wheat;
        _goldText.text = c_goldText + gold;
    }
}
