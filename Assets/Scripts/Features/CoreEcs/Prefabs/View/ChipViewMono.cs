using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Core.Blocks.Base;
using UnityEngine;
using Utils;


public class ChipViewMono : SerializedMonoBehaviour
{
    public Transform Transform;
    [OdinSerialize] public ImageAccessComponent image;
    [OdinSerialize] public GetChildByName childByName;
    [OdinSerialize] public GetChildByIndex childByIndex;
    [OdinSerialize] public GetChildByColor childByColor;
    [OdinSerialize] public GetChildByLives childByLive;
    [OdinSerialize] public Dictionary<string, AnimationComponent> animationAccess;
    [OdinSerialize] public Dictionary<string, SetImageByName> setImageAccess;
    [OdinSerialize] public List<SortingBy> sortBy;


    [Button]
    public void Init()
    {
        if (Transform == null)
            Transform = transform;
        if (image == null)
            image = new ImageAccessComponent() {value = gameObject.GetComponentInChildren<SpriteRenderer>()};
    }

    [Button]
    public void StopAnimate(string name)
    {
        var animation = animationAccess?.SaveGet(name);
        Log.NullWarning(animation, $"AnimationAccess[{name}]");
        if(animation == null)
            return;
        
        animation.Stop();
    }

    [Button]
    public async Task Animate(string name)
    {
        var animation = animationAccess?.SaveGet(name);
        Log.NullWarning(animation, $"AnimationAccess[{name}]");
        if(animation == null)
            return;
        
        animation.Animate();
        await animation.WaitComplete();
    }

    [Button]
    public void SetImage(Sprite sprite) 
        => Image().sprite = sprite; 
     
    [Button]
    public void SetImage(string name)
    {
        var imageByName = setImageAccess?.SaveGet(name);
        Log.NullWarning(imageByName, $"setImageAccess[{name}]");
        imageByName?.SetImage();
    }

    public void SetImageActive(bool active)
        => Image()?.gameObject.SetActive(active);
    
    public SpriteRenderer Image()
    {
        Log.NullWarning(image?.value, "ImageAccess.Value");
        return image?.value;
    }

    public Transform ChildByName(string name)
    {
        var res = childByName.Get(name);
        Log.NullWarning(res, $"ChildByName[{name}]");
        return childByName.Get(name);
    }

    public Transform ChildByLives(int lives) => childByIndex.Get(lives);
    public Transform ChildByColor(string color) => childByColor.Get(color);
    public Transform ChildByIndex(int index) => childByLive.Get(index);

    public void SetSortingOrder(int y) 
        => sortBy.ForEach(it => it.Order(y));
}

