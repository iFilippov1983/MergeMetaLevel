using UnityEngine;
using Object = UnityEngine.Object;

namespace Game
{
    internal class InfoBarHandler
    {
        private GameObject _infoBarObject;
        private InfoBarView _info;
        private Camera _camera;

        private GameObject _infoBarPrefab;
        private Vector3 _positionToSpawn;
        private int _health;
        private int _power;

        public InfoBarHandler(Camera cameraToWorkWith)
        {
            _camera = cameraToWorkWith;
        }

        public void InitInformationBar()
        {
            _infoBarObject = Object.Instantiate(_infoBarPrefab, _positionToSpawn, _camera.transform.rotation);
            _info = _infoBarObject.GetComponent<InfoBarView>();
            SetHealth(_health, 1);
            SetPower(_power);
        }

        public void SetInformationBar
            (
            GameObject infoBarPrefab, 
            Vector3 positionToSpawn,
            int power,
            int health
            )
        {
            _infoBarPrefab = infoBarPrefab;
            _positionToSpawn = positionToSpawn;
            _power = power;
            _health = health;
        }

        public void DestroyInformationBar()
        {
            _info = null;
            Object.Destroy(_infoBarObject);
        }

        public void SetHealth(int amount, float fillAmount)
        { 
            _info.HealthText.text = amount.ToString();
            _info.HealthBarFront.fillAmount = fillAmount;
        }

        private void SetPower(int amount)
        { 
            _info.PowerText.text = amount.ToString();
        }
    }
}
