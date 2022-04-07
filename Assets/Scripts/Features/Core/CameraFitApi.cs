using System;
using Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Systems.Merge
{
    [Serializable]
    public class CameraFitApi
    {
        public Camera Camera;
        public Transform CameraTransform;
        public Transform bounds;

        public void SetCtx(Camera cameraMain, Transform cameraTransform)
        {
            Camera = cameraMain;
            CameraTransform = cameraTransform;
        }
        
        public float scaleRatio = 1.05f;
        public float FieldHeightRatio = 0.54f;
        private LevelConfig _lastLevel;


        public float orthographicSizeByHeight(float targetHeight) => targetHeight / 2f / FieldHeightRatio;
        public float orthographicSizeByWidth(float targetWidth) => targetWidth / Camera.aspect * 0.5f;

        public int w;
        public int h;
        
        [Button(ButtonSizes.Gigantic), HorizontalGroup()]
        public void SizeByW()
        {
            Camera.orthographicSize = orthographicSizeByWidth(w) * scaleRatio;
            bounds.position = Camera.transform.position = CameraCenter(w, h);
        }

        [Button(ButtonSizes.Gigantic), HorizontalGroup()]
        public void SizeByH()
        {
            Camera.orthographicSize = orthographicSizeByHeight(h) * scaleRatio;
            bounds.position = Camera.transform.position = CameraCenter(w, h);
        }

        [Button]
        public void LoadLevel(LevelConfig level)
        {
            _lastLevel = level;
            var asWidth = Mathf.Max(level.MaxWidth , level.MaxHeight) + 0.5f;
            Camera.orthographicSize = orthographicSizeByWidth(asWidth) * scaleRatio;
         
            // var clampedHeight = Screen.height * FieldHeightRatio;
            // if (clampedHeight < Screen.width)
                // Camera.orthographicSize = orthographicSizeByWidth(asWidth) * scaleRatio;
            // else
                // Camera.orthographicSize = orthographicSizeByHeight( asWidth * Camera.aspect ) * scaleRatio;
            
            CameraTransform.position = CameraCenter(level.MaxWidth, level.MaxHeight);
        }

        private Vector3 CameraCenter(int width, int height) 
            => new Vector3((width - 1f)/2, (height - 1f)/ 2, CameraTransform.position.z);

        // public float horizontalFoV = 90.0f;
        // void PerspectiveCamers()
        // {
        //     float halfWidth = Mathf.Tan(0.5f * horizontalFoV * Mathf.Deg2Rad);
        //     float halfHeight = halfWidth * Screen.height / Screen.width;
        //     float verticalFoV = 2.0f * Mathf.Atan(halfHeight) * Mathf.Rad2Deg;
        //     CameraMain.fieldOfView = verticalFoV;
        // }
    }
}