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
        private Sprite[] _currentAnimationSprites;
        [SerializeField] private Animation _animation;
        private int _currentFrame;
        private float _timer;
        private float _framerate = 0.05f;
        
        public bool _isPlaying;
        

        private void Awake()
        {
            _animation.Play();
            _isPlaying = true;
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
            _currentFrame = 0;
            _isPlaying = true;
        }

        public async Task AnimateDice(int animationIndex)
        { 
            _currentAnimationSprites = _animations[animationIndex].Sprites;
            _isPlaying = true;
            while(_isPlaying)
                await Task.Yield();
        }
    }

    [Serializable]
    public struct AnimationDictionary
    {
        public int Number;
        public Sprite[] Sprites;
    }
}

