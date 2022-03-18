using System;
using UnityEngine;

namespace Enemy
{
    [Serializable]
    public struct EnemyAnimation
    {
        public EnemyAnimationType Type;
        public AnimationClip AnimationClip;
    }
    public enum EnemyAnimationType { Spawn, Idle, Run, Hit, GotHit, Die, Despawn }
}
