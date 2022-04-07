using Data;
using Tutorial.Game;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core
{
    public class GridGeneratorLinks : MonoBehaviour
    {
        public GameObject Gray;
        public GameObject White;
        public SpriteRenderer Bg;
        public Transform Container;
        [HideInInspector] 
        public LevelConfig MergeLevel;
        public Transform BgContainer;
        public BgController BgController;


        [Button]
        void Gen(LevelConfig level)
        { 
            var Api = new GridGeneratorApi(this);
            Api.LoadLevel(level);
        }
    }
}