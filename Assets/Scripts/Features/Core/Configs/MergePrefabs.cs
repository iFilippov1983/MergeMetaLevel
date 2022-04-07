using System.Collections.Generic;
using Components;
using Core;
using Features.Fx;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace Data
{
    public class MergePrefabs : SerializedScriptableObject
    {
        public Dictionary<string, AnimatorEvent> Tools; 
        public Dictionary<string, ParticleSystem> Particles; 
        public Dictionary<string, GameObject> ParticlesGo; 
        
        // public ParticleSystem AppearParticles; 
        // public ParticleSystem LifeExplosionParticles; 
        public MergeItemView Prefab; 
        public MergeItemLifeView LifePrefab; 
        public MergePlusOneView PlusOnePrefab;
        public List<FxTextGreat> TextsGreat;

        public AnimatorEvent GetTool(string name)
            => Tools.SaveGet(name);

        public ParticleSystem GetFx(string name)
            => Particles.SaveGet(name);

        public GameObject GetFxGo(string name)
            => ParticlesGo.SaveGet(name);

    }
}