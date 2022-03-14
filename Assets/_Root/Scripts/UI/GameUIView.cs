using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameUIView : MonoBehaviour
{
    [SerializeField] private Button _rollButton;

    public void Init(UnityAction rollButtonClicked)
    {
        _rollButton.onClick.AddListener(rollButtonClicked);
    }

    private void OnDestroy()
    {
        _rollButton.onClick.RemoveAllListeners();
    }
}
