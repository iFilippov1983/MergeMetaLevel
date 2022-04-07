using System;
using System.Numerics;
using Components.Main;
using Data;
using Utils;
using Vector3 = UnityEngine.Vector3;

namespace Configs.Quests
{
    [Serializable]
    public class Resource
    {
        public static Resource Coins = new Resource(){Type = ResourceType.Coins, Count = 100};
        public static Resource Hearts = new Resource(){Type = ResourceType.Hearts, Count = 1};
        
        public ResourceType Type;
        public int Count;
        
        public override string ToString() => $"{Type} : {Count}";

        public string CountStr() => Type switch
        {
            ResourceType.Coins => $"{Count}",
            _ => $"x{Count}"
        };

        public ResourceView FindResourceView(CoreRoot root) => Type switch
        {
            // ResourceType.Xp => rootCtx.Ui.View.MainScreen.XpBtn,
            ResourceType.Hearts => root.Ui.View.MainScreen.HeartsBtn,
            ResourceType.Coins => root.Ui.View.MainScreen.CoinsBtn,
            // ResourceType.Diamonds => rootCtx.Ui.View.MainScreen.DiamondsBtn,
            _ => null,
        };
        
        public Vector3 FindFlyTarget(CoreRoot root) => Type switch
        {
            // ResourceType.Xp => rootCtx.Ui.View.MainScreen.XpBtn.Image.transform.UiCenter(),
            ResourceType.Hearts => root.Ui.View.MainScreen.HeartsBtn.Image.transform.UiCenter(),
            ResourceType.Coins => root.Ui.View.MainScreen.CoinsBtn.Image.transform.UiCenter(),
            // ResourceType.Diamonds => rootCtx.Ui.View.MainScreen.DiamondsBtn.Image.transform.UiCenter(),
            _ => root.Ui.View.MainScreen.PlayBtn.transform.UiCenter(),
        };
    }
}