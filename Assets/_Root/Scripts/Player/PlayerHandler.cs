using Data;
using Game;
using Level;
using System.Threading.Tasks;
using UnityEngine;

namespace Player
{
    internal class PlayerHandler
    {
        private GameObject _playerPrefab;
        private Vector3 _playerInitialPosition;
        private Quaternion _playerInitialRotation;
        private CellView[] _cellViews;
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
            _playerPrefab = gameData.PlayerData.PlayerPrefab;

            _cellViews = gameData.LevelData.CellsViews;
            _playerInitialPosition = _cellViews[_playerProfile.Stats.CurrentCellID].transform.position;
            _playerInitialRotation = _cellViews[_playerProfile.Stats.CurrentCellID].transform.rotation;
            
            InitPlayer();

            _infoBarPrefab = gameData.PlayerData.InfoPrefab;
            _infoHandler = new InfoBarHandler(Camera.main);
            _popupHandler = new PopupHandler(_playerView.PopupPrefab, Camera.main);
        }

        public async Task SetDestinationAndMove(Vector3 position)
        {
            _playerView.NavMeshAgent.SetDestination(position);
            _playerView.GetAnimator().SetBool(AnimParameter.IsRunning, true);
            var transform = _playerView.transform;
            while(Vector3.SqrMagnitude(transform.position - position) > 0.8f * 0.8f)
                await Task.Yield();
        }

        public void OnGetHitEvent(int damageTakenAmount, int playerRemainingHealth)
        {
            _popupHandler.SpawnPopup(_infoHandler.PopupSpawnPoint, damageTakenAmount);

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

        public void SpawnPopupAbovePlayer(int value, PopupType popupType = PopupType.Damage) =>
            _popupHandler.SpawnPopup(_playerView.PopupSpawnPoint, value, popupType);


        public void PrepareToFight(int power, int health)
        {
            _infoHandler.SetInformationBar
                (_infoBarPrefab, _playerView.transform.position, power, health);
            _infoHandler.InitInformationBar();
        }

        public void FinishFight()
        {
            _infoHandler.DestroyInformationBar();
        }

        //private void InitPlayer(GameObject playerPrefab, Vector3 playerInitPosition, Quaternion playerInitialRotation)
        public void InitPlayer()
        {
            _playerInitialPosition = _cellViews[_playerProfile.Stats.CurrentCellID].transform.position;
            _playerInitialRotation = _cellViews[_playerProfile.Stats.CurrentCellID].transform.rotation;
            var playerObject = GameObject.Instantiate(_playerPrefab, _playerInitialPosition, _playerInitialRotation);
            _playerView = playerObject.GetComponent<PlayerView>();
            _playerAnimController = playerObject.GetComponent<CharacterAnimationControler>();
        }

        public void DestroyPlayer()
        { 
            Object.Destroy(_playerView.gameObject);
        }
    }
}
