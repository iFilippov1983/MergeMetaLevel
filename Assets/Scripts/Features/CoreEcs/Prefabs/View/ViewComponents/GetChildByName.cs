using System.Collections.Generic;
using UnityEngine;
using Utils;

public class GetChildByName 
{
    public Dictionary<string, Transform> childs;

    public Transform Get(string name) => childs.SaveGet(name);
}

public class GetRendererByName 
{
    public Dictionary<string, SpriteRenderer> childs;

    public SpriteRenderer Get(string name) => childs.SaveGet(name);
}

public class GetChildByIndex
{
    public List<Transform> childs;
    public Transform Get(int index) => childs.SaveGet(index);
}
public class GetChildByLives
{
    public List<Transform> childs;
    public Transform Get(int lives) => childs.SaveGet(lives);
}

public class GetChildByColor
{
    public Dictionary<string, Transform> childs;

    public Transform Get(string name) => childs.SaveGet(name);
}