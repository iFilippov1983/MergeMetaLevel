using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Enemy
{
    public class EnemyInfoView : MonoBehaviour
    {
        [SerializeField] private TextMeshPro _healthText;
        [SerializeField] private TextMeshPro _powerText;
        [SerializeField] private Image _healthBarFront;

        public TextMeshPro HealthText => _healthText;
        public TextMeshPro PowerText => _powerText;
        public Image HealthBarFront => _healthBarFront;
    }
}
