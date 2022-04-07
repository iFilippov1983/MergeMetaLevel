using System;
using Components;
using Core;
using Data;
using UnityEngine;
using Utils;
using Object = UnityEngine.Object;

[Serializable]
public class ViewFactoryService
{
    private Transform _container;
    private MergeConfig _configs;
    private MergePrefabs _prefabs;
    private Camera _camera;

    public void SetCtx(Transform container, Camera camera, MergeConfig mergeConfig)
    {
        _container = container;
        _configs = mergeConfig;
        _prefabs = mergeConfig.Prefabs;
        _camera = camera;
    }

    public MergeItemView CreateItemView(MergeItemProfileData data)
    {
        var pos = new Vector3Int(data.x, data.y, 0);
        var view = Object.Instantiate(_prefabs.Prefab, pos, Quaternion.identity, _container);
        view.SetCtx(data, Destroy);
        view.ApplyFromData();
        return view;
    }

    // public MergeItemView CreateItem(MergeItemConfig config, int posX, int posY)
    // {
    // var data = new MergeItemProfileData(config, posX, posY, 0);
    // return CreateItem(data);
    // }
        
    public MergeItemLifeView CreateLifeFlyParticles(int posX, int posY)
    {
        var fx = Object.Instantiate(_prefabs.LifePrefab, new Vector3(posX, posY, 0), Quaternion.identity);
        fx.SetCtx(Destroy);
        return fx;
    }  
    public AnimatorEvent CreateTool(string name, int posX, int posY)
    {
        var fx = Object.Instantiate(_prefabs.GetTool(name), new Vector3(posX, posY, 0), Quaternion.identity);
        // fx.SetCtx(Destroy);
        return fx;
    }

    public void CreateLifeExplosionParticles(int posX, int posY)
    {
        var fx = Object.Instantiate(_prefabs.GetFx("life_explosion"), new Vector3(posX, posY, 0), Quaternion.identity);
    }

    public void CreateFx(string name, int posX, int posY)
    {
        var fx = Object.Instantiate(_prefabs.GetFx(name), new Vector3(posX, posY, 0), Quaternion.identity);
    }
    public void CreateFx2(string name, int posX, int posY)
    {
        var fx = Object.Instantiate(_prefabs.GetFxGo(name), new Vector3(posX, posY, 0), Quaternion.identity);
    }
    
    public void FxTextGreat(int posX, int posY, string text = "Greate")
    {
        var fx = Object.Instantiate(_prefabs.TextsGreat.Random(), new Vector3(posX, posY, 0), Quaternion.identity);
        fx.Text.text = text;
        fx.Canvas.worldCamera = _camera;
    }

    public void CreateUnlockParticles(int posX, int posY)
    {
        var fx = Object.Instantiate(_prefabs.GetFx("unlock"), new Vector3(posX, posY, 0), Quaternion.identity);
    }

    public void CreateDisappearParticles(int posX, int posY)
    {
        var fx = Object.Instantiate(_prefabs.GetFx("appear"), new Vector3(posX, posY, 0), Quaternion.identity);
    }
    
    public void CreateAppearParticles(int posX, int posY)
    {
        var fx = Object.Instantiate(_prefabs.GetFx("appear"), new Vector3(posX, posY, 0), Quaternion.identity);
    }
    public GameObject CreateParticles(string name, int posX, int posY) 
        => Object.Instantiate(_prefabs.GetFx(name), new Vector3(posX, posY, 0), Quaternion.identity).gameObject;

    public void Destroy(ParticleSystem view)
        => Object.Destroy(view.gameObject);
    
    public void Destroy(GameObject go)
        => Object.Destroy(go);
        
    public void Destroy(MergeItemView view)
    {
        view.isDestroyed = true;
        Object.Destroy(view.gameObject);
    }

    public void Destroy(MergeItemLifeView view)
    {
        view.isDestroyed = true;
        Object.Destroy(view.gameObject);
    }

    public void Destroy(MergePlusOneView view)
    {
        view.isDestroyed = true;
        Object.Destroy(view.gameObject);
    }

    public void CreatePlusOne(int posX, int posY)
    {
        var plusOne = Object.Instantiate(_prefabs.PlusOnePrefab, new Vector3(posX, posY, 0), Quaternion.identity);
        plusOne.SetCtx(Destroy);
    }

    public void ClearContainer()
    {
        _container.Clear();
    }


}

namespace Systems
{
}