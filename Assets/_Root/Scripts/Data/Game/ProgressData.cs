using Sirenix.OdinInspector;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "GameData/ProgressData", fileName = "ProgressData")]
    public sealed class ProgressData : ScriptableObject
    {
        [SerializeField] 
        private int _priceConstant = 25;
        [SerializeField] 
        private int _calculationFactor = 2;

        [SerializeField]
        [InlineButton("CalculateValues", "Calculate")]
        private int _currentPowerProgressLevel;

        [ShowInInspector, ReadOnly]
        private static int _currentUpgradePrice;

        [ShowInInspector, ReadOnly]
        private int _currentPowerGain;

        public bool UpgradeUnlocked = true;
        [SerializeField] 
        private int _powerConstantForMergeWin = 50;

        public int PowerConstantForMergeWin => _powerConstantForMergeWin;
        public int CurrentPowerProgressLevel => _currentPowerProgressLevel;
        public int GetCurrentPowerGain()
        { 
            CalculateValues();
            return _currentPowerGain;
        }

        public int GetCurrentUpgradePrice()
        {
            CalculateValues();
            return _currentUpgradePrice;
        }

        public void SetPowerProgressLevel(int level = 1)
        { 
            _currentPowerProgressLevel = level;
            CalculateValues();
        }

        public void SetNextUpgradeLevel()
        {
            _currentPowerProgressLevel++;
            CalculateValues();
        }

        private int CalculateCurrentUpgradePrice() => 
            _currentPowerProgressLevel * _priceConstant;

        private int CalculateCurrentPowerGain() => 
            Mathf.RoundToInt(_currentUpgradePrice * _calculationFactor * Mathf.Sqrt(1f / _currentPowerProgressLevel));

        private void CalculateValues()
        {
            _currentUpgradePrice = CalculateCurrentUpgradePrice();
            _currentPowerGain = CalculateCurrentPowerGain();
        }
    }
}