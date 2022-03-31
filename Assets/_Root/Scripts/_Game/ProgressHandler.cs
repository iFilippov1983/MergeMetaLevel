using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;

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
        public int PowerGain => _progressData.GetCurrentPowerGain();

        public ProgressHandler(ProgressData progressData, PlayerProfile playerProfile)
        {
            _progressData = progressData;
            _playerProfile = playerProfile;

            SetUpgradeLevel(_playerProfile.Stats.CurrentPowerUpgradeLevel);
        }

        public void SetUpgradeLevel(int level) => _progressData.SetPowerProgressLevel(level);

        public bool CheckPlayerFunds() =>
            _playerProfile.Stats.Gold >= UpgradePrice ? true : false;

        public void MakePowerUpgrade(bool mergeWin = false)
        {
            if (mergeWin)
            {
                _playerProfile.Stats.Power += _progressData.PowerConstantForMergeWin;
                _progressData.SetNextUpgradeLevel();
            }
            else
            {
                _playerProfile.Stats.Power += PowerGain;
                _playerProfile.Stats.Gold -= UpgradePrice;
                _progressData.SetNextUpgradeLevel();
            }
                
        }
    }
}
