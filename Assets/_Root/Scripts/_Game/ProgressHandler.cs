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

        public ProgressHandler(ProgressData progressData, PlayerProfile playerProfile)
        {
            _progressData = progressData;
            _playerProfile = playerProfile;

            SetUpgradeLevel(_playerProfile.Stats.CurrentPowerUpgradeLevel);
        }

        public int PowerGain(bool mergeWin = false) =>
        mergeWin == true
            ? _progressData.PowerConstantForMergeWin
            : _progressData.GetCurrentPowerGain();

        public void SetUpgradeLevel(int level) => _progressData.SetPowerProgressLevel(level);

        public bool CheckPlayerFunds() =>
            _playerProfile.Stats.Gold >= UpgradePrice ? true : false;

        public void MakePowerUpgrade()
        {
            _playerProfile.Stats.Power += PowerGain();
            _playerProfile.Stats.Gold -= UpgradePrice;
            _progressData.SetNextUpgradeLevel();
        }

        public void AddPowerForMergeRound()
        {
            _playerProfile.Stats.Power += _progressData.PowerConstantForMergeWin;
        }
    }
}
