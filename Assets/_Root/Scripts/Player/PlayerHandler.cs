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
        private HeadAimHandler _headAimHandler;
        private Camera _camera;


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
            _camera = gameData.LevelData.CameraContainerView.MainCamera;
            _infoHandler = new InfoBarHandler(_camera);
            _popupHandler = new PopupHandler(_playerView.PopupPrefab, _camera, gameData.LevelData.CameraContainerView);
        }

        public async Task SetDestinationAndMove(Vector3 position)
        {
            _playerView.NavMeshAgent.SetDestination(position);
            while (Vector3.SqrMagnitude(_playerView.transform.position - position) > 0.8f * 0.8f)
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

        public void SpawnPopupAbovePlayer(int value, PopupType popupType = PopupType.Damage, bool firstPopup = false) =>
            _popupHandler.SpawnPopup(_playerView.PopupSpawnPoint, value, popupType, firstPopup);


        public void PrepareToFight(int power, int health)
        {
            _headAimHandler.StopLooking();
            _infoHandler.SetInformationBar
                (_infoBarPrefab, _playerView.transform.position, power, health);
            _infoHandler.InitInformationBar();
        }

        public void FinishFight()
        {
            _infoHandler.DestroyInformationBar();
        }

        public async Task LookAtCamera() => await _headAimHandler.LookAt(_camera.transform);
        public void StopLookingAtCamera() => _headAimHandler.StopLooking();

        public void InitPlayer()
        {
            var initialCellView = _cellViews[_playerProfile.Stats.CurrentCellID];
            _playerInitialPosition = initialCellView.transform.position;
            _playerInitialRotation = initialCellView.transform.rotation;
            var playerObject = GameObject.Instantiate(_playerPrefab, _playerInitialPosition, _playerInitialRotation);
            _playerView = playerObject.GetComponent<PlayerView>();
            _playerAnimController = playerObject.GetComponent<CharacterAnimationControler>();
            _headAimHandler = new HeadAimHandler(_playerView.HeadAimTarget, _playerView.HeadAim);
        }

        public void DestroyPlayer()
        {
            UnityEngine.Object.Destroy(_playerView.gameObject);
        }
    }
}
