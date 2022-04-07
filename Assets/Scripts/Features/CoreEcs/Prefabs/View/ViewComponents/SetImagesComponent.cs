using UnityEngine;

public abstract class SetImagesComponent
{
    public abstract void SetImage();
}

public class SetImageByName : SetImagesComponent
{
    public SpriteRenderer renderer;
    public Sprite image;

    public override void SetImage() => renderer.sprite = image;
}
