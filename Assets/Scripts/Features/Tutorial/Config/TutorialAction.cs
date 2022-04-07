using System;
using System.Linq;
using Api.Ui;
using Components;
using Tutorial.UI;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Configs.Tutorial
{
    public class TutorialAction
    {
        [OnValueChanged("UiRedraw")]
        public TutorialActionType Type;
        
        [ShowIf("@Type==TutorialActionType.Ui")]
        public string UiGuid;

        [ShowIf("@Type==TutorialActionType.Merge")]
        public bool clickOnly;
        
        [ShowIf("@Type==TutorialActionType.Merge")]
        public Vector2Int fromCell;
        
        [HideIf("@clickOnly")]
        [ShowIf("@Type==TutorialActionType.Merge")]
        public Vector2Int toCell;

        public int AutoCompleteByTime;
        
        void UiRedraw()
        {
            var tutUI = GameObject.FindObjectOfType<UiTutorialView>(true);
            tutUI.Api.Editor_Redraw();
        }

    
#if UNITY_EDITOR      
        
        // [Button]
        // private void ClearTarget()
        // {
        //     var targets = Object.FindObjectsOfType<UiTutorialItem>().ToList();
        //     var target = targets.Find(x => x.Guid == TargetGuid);
        //     if (target == null)
        //     {
        //         Debug.Log($"Can not find tutorial target with Guid: {TargetGuid}");
        //     }
        //     else
        //     {
        //         Debug.Log($"Tutorial target with Guid: {TargetGuid} destroyed");
        //         EditorUtility.SetDirty(target);
        //         Object.DestroyImmediate(target);
        //     }
        //     TargetGuid = string.Empty;
        // }
        [ShowIf("@Type==TutorialActionType.Ui")]
        [Button(ButtonSizes.Medium), HorizontalGroup("btn")]
        private void SetTarget()
        {
            if (!(Selection.activeObject && Selection.activeObject is GameObject &&
                  (Selection.activeObject as GameObject)?.GetComponent<Graphic>() != null))
            {
                return;
            }
                
            var go = Selection.activeObject as GameObject;
            var targetComp = go.GetComponent<UiTutorialItem>();
            if (targetComp == null)
            {
                targetComp = go.AddComponent<UiTutorialItem>();
                UiGuid = Guid.NewGuid().ToString();
                targetComp.Guid = UiGuid;
            }
            else
            {
                UiGuid = targetComp.Guid;
            }
            EditorUtility.SetDirty(go);
        }
        
        [ShowIf("@Type==TutorialActionType.Ui")]
        [Button(ButtonSizes.Medium), HorizontalGroup("btn")]
        void FindTarget()
        {
            var tar = TutorialExtensions.FindTarget(UiGuid, true);
            if (tar == null)
            {
                Debug.LogWarning($"NotFound {UiGuid}");
            }
            else
            {
                Selection.activeGameObject = tar.gameObject;
            // Selection.activeObject = tar.gameObject;
            }
        }
#endif
    }

    public enum TutorialActionType
    {
        None,
        Merge,
        Ui,
    }
}