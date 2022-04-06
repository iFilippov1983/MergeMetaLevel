﻿using Data;
using UnityEngine;

namespace Game
{
    internal class ProgressHandler
    {
        private ProgressData _progressData;
        private PlayerProfile _playerProfile;

        public bool UpgradeUnlocked
        {
            get { return _progressData.UpgradeUnlocked; }
            set { _progressData.UpgradeUnlocked = value; }
        }

        public int UpgradeLevel => _progressData.CurrentPowerProgressLevel;
        public int UpgradePrice => _progressData.GetCurrentUpgradePrice();

        public ProgressHandler(ProgressData progressData, PlayerProfile playerProfile)
        {
            _progressData = progressData;
            _playerProfile = playerProfile;

            SetUpgradeLevel(_playerProfile.Stats.CurrentPowerUpgradeLevel);
        }

        public void HandleMergeLevelComplete(bool levelComplete = true)
        {
            if (levelComplete)
                SetMergeLevelComplete();
            else
                SetMergeLevelNotComplete();
        }

        public void SetUpgradeLevel(int level) => _progressData.SetPowerProgressLevel(level);

        public int GetPowerGain(bool mergeWin = false) =>
            mergeWin == true
            ? _progressData.PowerConstantForMergeWin
            : _progressData.GetCurrentPowerGain();

        public bool CheckPlayerFunds() =>
            _playerProfile.Stats.Gold >= UpgradePrice ? true : false;

        public void MakePowerUpgrade()
        {
            _playerProfile.Stats.Power += GetPowerGain();
            _playerProfile.Stats.Gold -= UpgradePrice;
            _playerProfile.Stats.CurrentPowerUpgradeLevel++;
            _progressData.SetNextUpgradeLevel();
        }

        private void SetMergeLevelComplete()
        {
            _playerProfile.Stats.CurrentMergeLevel++;
            _playerProfile.Stats.DiceRolls++;
            _playerProfile.Stats.Power += _progressData.PowerConstantForMergeWin;
        }

        private void SetMergeLevelNotComplete()
        {
            //do something
            return;
        }
    }
}
