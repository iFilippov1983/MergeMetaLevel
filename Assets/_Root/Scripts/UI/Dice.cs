using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{
    public class Dice : MonoBehaviour
    {
        [SerializeField] private AnimationDictionary[] _animations;
        [SerializeField] private Image _image;
        [SerializeField] private Animation _animation;
        [SerializeField] private float _framerate = 0.05f;
        private Sprite[] _currentAnimationSprites;
        private int _currentFrame;
        private float _timer;
        private bool _isPlaying;

        public async Task AnimateDice(int animationIndex)
        {
            _currentAnimationSprites = _animations[animationIndex].Sprites;
            _isPlaying = true;
            while (_isPlaying)
                await Task.Yield();
        }

        private void Awake()
        {
            //_animation.Play();
        }

        private void Update()
        {
            _timer += Time.deltaTime;

            if (_timer >= _framerate)
            {
                _timer -= _framerate;
                _currentFrame++;
                if (_currentFrame >= _currentAnimationSprites.Length)
                {
                    _isPlaying = false;
                    return;
                } 
                _image.sprite = _currentAnimationSprites[_currentFrame];
            }
        }

        private void OnDisable()
        {
            _isPlaying = true;
            _currentFrame = 0;
            _timer = 0;
        }
    }

    [Serializable]
    public struct AnimationDictionary
    {
        public int Number;
        public Sprite[] Sprites;
    }
}

