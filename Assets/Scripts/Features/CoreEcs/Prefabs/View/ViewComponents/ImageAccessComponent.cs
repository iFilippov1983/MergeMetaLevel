using UnityEngine;

public class ImageAccessComponent
{
    public SpriteRenderer value;
    public GetRendererByName rendererByName;
    
    public void Set(Sprite sprite)
    {
        Log.NullWarning(value, nameof(ImageAccessComponent));
        
        if(value != null) 
            value.sprite = sprite;
    }
    
    public void Set(string name, Sprite sprite)
    {
        var spriteRenderer = rendererByName?.Get(name);
        Log.NullWarning(spriteRenderer, $"{nameof(ImageAccessComponent)} {name}" );
        
        if(spriteRenderer != null)
            spriteRenderer.sprite = sprite;
    }
}