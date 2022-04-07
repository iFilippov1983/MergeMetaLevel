using Api.Merge;
using Data;
using UnityEngine;

namespace Core
{
    public class MergePlayerLinks : MonoBehaviour
    {
        public Transform Container;
        public LevelConfig Level;
        public MergeItemView Prefab;
        
        public Camera Camera;
        public Transform CameraTransform;
        public GridGeneratorLinks GridGeneratorLinks;

        // public MergePlayerApi Api;
    }
}