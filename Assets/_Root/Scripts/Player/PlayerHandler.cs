using Data;
using Game;
using System.Threading.Tasks;
using UnityEngine;

namespace Player
{
    internal class PlayerHandler
    {
        private Vector3 _playerInitialPosition;
        private GameObject _playerPrefab;
        private PlayerView _playerView;
        private PlayerProfile _playerProfile;
        private InfoHandler _infoHandler;
        private CharacterAnimationControler _playerAnimController;
        private GameObject _infoPrefab;


        public PlayerView PlayerView => _playerView;
        public CharacterAnimationControler PlayerAnimController => _playerAnimController;

        public PlayerHandler(GameData gameData, PlayerProfile playerProfile)
        {
            _playerProfile = playerProfile;
            _playerPrefab = gameData.PlayerData.PlayerPrefab;
            var cellViews = gameData.LevelData.CellsViews;
            _playerInitialPosition = cellViews[_playerProfile.Stats.CurrentCellID].transform.position;
            InitPlayer(_playerInitialPosition);//TODO: insert position from cash
            _infoHandler = new InfoHandler(Camera.main);
            _infoPrefab = gameData.PlayerData.InfoPrefab;
        }

        public async Task SetDestinationAndMove(Vector3 position)
        {
            _playerView.NavMeshAgent.SetDestination(position);
            _playerView.GetAnimator().SetBool(CharState.IsRunning, true);
            var transform = _playerView.transform;
            while(Vector3.SqrMagnitude(transform.position - position) > 0.2f * 0.2f)
                await Task.Yield();
        }

        public void OnGetHitEvent(int playerRemainingHealth)
        {
            if (playerRemainingHealth <= 0)
            {
                _infoHandler.SetHealth(0, 0f);
            }  
            else
            {
                float fillAmount = (float)playerRemainingHealth / (float)_playerProfile.Stats.Health;
                _infoHandler.SetHealth(playerRemainingHealth, fillAmount);
            }
        }

        public void PrepareToFight(int power, int health)
        {
            _infoHandler.SetInformation
                (_infoPrefab, _playerView.transform.position, power, health);
            _infoHandler.InitInformation();
        }

        internal void FinishFight()
        {
            _infoHandler.DestroyInformation();
        }

        private void InitPlayer(Vector3 playerInitPosition)
        {
            var playerObject = GameObject.Instantiate(_playerPrefab, playerInitPosition, Quaternion.identity);
            _playerView = playerObject.GetComponent<PlayerView>();
            _playerAnimController = playerObject.GetComponent<CharacterAnimationControler>();
        }
    }
}
