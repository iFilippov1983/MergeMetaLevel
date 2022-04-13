using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    internal class InfoBarView : MonoBehaviour
    {
        [SerializeField] private TextMeshPro _healthText;
        [SerializeField] private TextMeshPro _powerText;
        [SerializeField] private Image _healthBarFront;
        [SerializeField] private Transform _popupSpawnPoint;

        public TextMeshPro HealthText => _healthText;
        public TextMeshPro PowerText => _powerText;
        public Image HealthBarFront => _healthBarFront;
        public Transform PopupSpawnPoint => _popupSpawnPoint;
    }
}
