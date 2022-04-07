using UnityEngine;

namespace Configs
{
    public class Constants
    {
        public class Camera
        {
            public const float QuestDefaultZoom0 = 24;
            public const float QuestDefaultZoom1 = 14;
            public const float DragSpeed = 10;
            public const float DefaultZoom = 28;
            public const float ZoomRatio = 2f;
            public const float ZoomSpeed = 5;
            public const float MinZoom = 14;
            public const float MaxZoom = 36;
            public const float DefaultFOV = 60;
            public static readonly Vector3 DefaultRotation = new Vector3(30, 45, 0);
            public static readonly Vector3 DefaultPosition = new Vector3(-12.6f, 11.5f, -12.56f);
            public static readonly Vector3 DefaultClosePosition = new Vector3(-5.052f, 4.321f, -5.033f);
            public static readonly Vector3 DefaultMidPosition = new Vector3(-8.797f, 7.379f, -8.778f);
            public static readonly Vector3 DefaultFarPosition = new Vector3(-14.356f, 11.911f, -14.337f);
            public static readonly float MinZoomY = DefaultClosePosition.y;
            public static readonly float MaxZoomY = DefaultFarPosition.y;
            
        }
        public class Layers
        {
            public static string Default = "Default";
            public static string Ui = "Ui";
            public static string Fx = "MergeFx";
        }
    }
    
}