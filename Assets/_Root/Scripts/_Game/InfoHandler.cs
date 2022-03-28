using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game
{
    internal class InfoHandler
    {
        private GameObject _infoBarObject;
        private InfoView _info;
        private Camera _camera;

        private GameObject _infoBarPrefab;
        private Vector3 _positionToSpawn;
        private int _health;
        private int _power;

        public InfoHandler(Camera cameraToWorkWith)
        {
            _camera = cameraToWorkWith;
        }

        public void InitInformation()
        {
            _infoBarObject = Object.Instantiate(_infoBarPrefab, _positionToSpawn, _camera.transform.rotation);
            _info = _infoBarObject.GetComponent<InfoView>();
            SetHealth(_health, 1);
            SetPower(_power);
        }

        public void SetInformation
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
            //_infoBarObject = Object.Instantiate(infoBarPrefab, positionToSpawn, _camera.transform.rotation);
            //_info = _infoBarObject.GetComponent<InfoView>();
            //SetHealth(health, 1);
            //SetPower(power);
        }

        public void DestroyInformation()
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
