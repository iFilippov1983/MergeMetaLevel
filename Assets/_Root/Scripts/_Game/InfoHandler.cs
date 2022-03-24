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

        public InfoHandler(Camera cameraToWorkWith)
        {
            _camera = cameraToWorkWith;
        }

        public void InitInformation
            (
            GameObject infoRabPrefab, 
            Vector3 positionToSpawn,
            int power,
            int health
            )
        { 
            _infoBarObject = Object.Instantiate(infoRabPrefab, positionToSpawn, _camera.transform.rotation);
            _info = _infoBarObject.GetComponent<InfoView>();
            SetHealth(health, 1);
            SetPower(power);
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
