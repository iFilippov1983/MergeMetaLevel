using DG.Tweening;
using UnityEngine;

namespace Data
{
    public class MergeVisualConfig : ScriptableObject
    {
        [Header("Appear")]
        public GameObject AppearParticles;
        public int AppearDuraion = 400;
        public AnimationCurve AppearEasing ;
        public AnimationCurve LifeFlyScaleEasing ;
        
        [Header("GeneratedFly")]
        public int GeneratedFlySpeed = 150;
        public int GeneratedFlyTime = 150;
        public AnimationCurve GeneratedFlyX ;
        public AnimationCurve GeneratedFlyY ;
        public AnimationCurve GeneratedScale ;
        
        public float ScaleRatio = 0.4f;
        
        [Header("Collapse")]
        public int CollapseFlyDuration = 500;
        public int CollapseScaleDelay = 300;
        public AnimationCurve CollapseEasing ;
        public int CollapseScaleDuration = 300;
        public AnimationCurve CollapseScaleEasing ;
        public float CollapseScaleRatio = 0.8f;
        public int DelayBeforeCreate = 200;
        
        [Header("Create")]

        [Header("Cancel Drag")]
        public Ease CancelDragFlyEase = Ease.OutCubic;
        public float CancelDragFlySpeed = 0.2f;
        
        [Header("CoinsFly")]
        public AnimationCurve CoinsFromMovesX ;
        public AnimationCurve CoinsFromMovesY ;
        public AnimationCurve CoinsScale ;
        public int CoinsStarDestroyDelay = 3800 ;
        public int CoinsFromMovesDelay = 80 ;
        public float CoinsFromMovesHeight = 7 ;
        
        [Header("Layers")]
        public string NormalLayer;
        public string TopLayer;
        public Ease DragFlyEasing = Ease.OutFlash;
        public float DragFlySpeed = 0.4f;
        
    }
}