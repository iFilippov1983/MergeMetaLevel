using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarView : MonoBehaviour
{
    [SerializeField] private Button _progressButton;
    [SerializeField] private TextMeshProUGUI _progressValueTMP;
    [SerializeField] private Image _progressSprite;

    public Button ProgressButton => _progressButton;
    public TextMeshProUGUI ProgressValue => _progressValueTMP;
    public Image ProgressSprite => _progressSprite;
}
