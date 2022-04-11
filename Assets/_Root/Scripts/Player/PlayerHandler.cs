using Data;
using Game;
using System.Threading.Tasks;
using UnityEngine;

namespace Player
{
    internal class PlayerHandler
    {
        private Vector3 _playerInitialPosition;
        private Quaternion _playerInitialRotation;
        private PlayerView _playerView;
        private PlayerProfile _playerProfile;
        private CharacterAnimationControler _playerAnimController;
        private InfoBarHandler _infoHandler;
        private GameObject _infoBarPrefab;
        private PopupHandler _popupHandler;

        public PlayerView PlayerView => _playerView;
        public CharacterAnimationControler PlayerAnimController => _playerAnimController;

        public PlayerHandler(GameData gameData, PlayerProfile playerProfile)
        {
            _playerProfile = playerProfile;

            var cellViews = gameData.LevelData.CellsViews;
            _playerInitialPosition = cellViews[_playerProfile.Stats.CurrentCellID].transform.position;
            _playerInitialRotation = cellViews[playerProfile.Stats.CurrentCellID].transform.rotation;
            InitPlayer(gameData.PlayerData.PlayerPrefab, _playerInitialPosition, _playerInitialRotation);//TODO: insert position from cash

            _infoBarPrefab = gameData.PlayerData.InfoPrefab;
            _infoHandler = new InfoBarHandler(Camera.main);
            _popupHandler = new PopupHandler(_playerView.PopupPrefab, Camera.main);
        }

        public async Task SetDestinationAndMove(Vector3 position)
        {
            _playerView.NavMeshAgent.SetDestination(position);
            _playerView.GetAnimator().SetBool(AnimParameter.IsRunning, true);
            var transform = _playerView.transform;
            while(Vector3.SqrMagnitude(transform.position - position) > 0.2f * 0.2f)
                await Task.Yield();
        }

        public void OnGetHitEvent(int damageTakenAmount, int playerRemainingHealth)
        {
            _popupHandler.SpawnPopup(_playerView.transform.position, damageTakenAmount);

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

        public void SpawnPopup(int value, bool resourcePickup = false) => 
            _popupHandler.SpawnPopup(_playerView.transform.position, value, resourcePickup);

        public void PrepareToFight(int power, int health)
        {
            _infoHandler.SetInformationBar
                (_infoBarPrefab, _playerView.transform.position, power, health);
            _infoHandler.InitInformationBar();
        }

        internal void FinishFight()
        {
            _infoHandler.DestroyInformationBar();
        }

        private void InitPlayer(GameObject playerPrefab, Vector3 playerInitPosition, Quaternion playerInitialRotation)
        {
            var playerObject = GameObject.Instantiate(playerPrefab, playerInitPosition, playerInitialRotation);
            _playerView = playerObject.GetComponent<PlayerView>();
            _playerAnimController = playerObject.GetComponent<CharacterAnimationControler>();
        }
    }
}
