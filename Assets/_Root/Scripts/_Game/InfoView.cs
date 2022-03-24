using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    internal class InfoView : MonoBehaviour
    {
        [SerializeField] private TextMeshPro _healthText;
        [SerializeField] private TextMeshPro _powerText;
        [SerializeField] private Image _healthBarFront;

        public TextMeshPro HealthText => _healthText;
        public TextMeshPro PowerText => _powerText;
        public Image HealthBarFront => _healthBarFront;
    }
}
