using System;
using System.Threading.Tasks;
using Api.Ui;
using Configs.Tutorial;
using Data;
using DG.Tweening;
using Tutorial.UI;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Components
{
    [Serializable]
    public class UiTutorialApi : UiBaseApi
    {
        public static UiTutorialApi instance;
        private const string tutorSortingLayer = "UiTutorial";
        private const string tutorTopSortingLayer = "UiTutorialTop";
        
        public Camera _camera;
        public UiTutorialView _view;
        [InlineEditor(InlineEditorObjectFieldModes.Boxed)]
        public TutorialStep _step;
        
        private Sequence _tw;
        private Sequence _twHand;
        private Sequence _twAutoComplete;
        private Action _onComplete;
        private RectTransform _tutorialElementsContainer;
        private Transform _origParent;
        private UiTutorialItem _uiTarget;
        private int _origOrder;
        private Button _button;

        public void SetCtx(UiTutorialView view, RectTransform tutorialElementsContainer, Camera camera)
        {
            base.SetCtxBase(view);
            view.CanvasGroup.interactable = true;
            view.CanvasGroup.blocksRaycasts = true;
            
            _view = view;
            _camera = camera;
            _tutorialElementsContainer = tutorialElementsContainer;
            
            instance = this;
        }

        [Button]
        public void Test_Show(TutorialStep step)
        {
            Show(step, () =>
            {
                Debug.Log("COMPLETE");
                Hide();
            });
        }
        
        public void Show(TutorialStep step, Action onComplete)
        {
            // UiRelease();
            
            _step = step;
            _onComplete = onComplete;
            
                
            _tw?.Kill(false);
            _twHand?.Kill(false);
            _view.DOKill(false);
            
            _tw = DOTween.Sequence();
            _tw.Pause();

            // bool closeByClick = step.UiData.ShowArrow == false && step.UiData.ShowHand == false;
            // _view.CanvasGroup.interactable = closeByClick;
            // _view.CanvasGroup.blocksRaycasts = closeByClick;

            if (_view.Canvas.enabled)
                _tw
                    .AppendCallback(() => _view.TweenFadeOut(null))
                    .AppendInterval(_view.FadeOutDuration / 1000f + 0.1f);
                
            _tw
                .AppendCallback(() => UiRelease())
                .AppendCallback(() => UiPrepare())
                .AppendCallback(() => Redraw())
                .AppendCallback(() => _view.TweenFadeIn())
                .AppendInterval(_view.FadeInDuration / 1000f + 0.1f)
                .AppendCallback(AnimateHand);
                
            if(_step.Action.AutoCompleteByTime != 0)
                _tw
                    .AppendInterval(_step.Action.AutoCompleteByTime / 1000f)
                    .AppendCallback(Autocomplete);
                
            _tw.Play();
        }

        public void Hide()
        {
            RemoveListeners();
            UiRelease();
            
            _tw?.Kill(false);
            _twHand?.Kill(false);
            _view.DOKill(false);
            _view.TweenFadeOut(() =>
            {
            });
        }

        void UiPrepare()
        {
            _view.ClickReceiver.OnMouseClick = null;
            
            if(_step.Action.Type == TutorialActionType.Ui)
                _uiTarget = TutorialExtensions.FindTarget(_step.Action.UiGuid, false);
            
            ChangeUiTargetLayer();
            ListenUiClick();
        }

        void UiRelease()
        {
            RestoreUiTargetLayer();
        }

        private void RemoveListeners()
        {
            if (_button)
                _button.onClick.RemoveListener(ClickButton);
            _button = null;
            _view.ClickReceiver.OnMouseClick = null;
        }

        private void ListenUiClick()
        {
            if(_step.Action.Type != TutorialActionType.Ui && _step.Action.Type != TutorialActionType.None)
                return;
            
            if(_uiTarget)
                _button = _uiTarget.GetComponentInChildren<Button>();
            
            if (_button)
                _button.onClick.AddListener(ClickButton);
            else
                _view.ClickReceiver.OnMouseClick = ClickButton;
        }

        public void Test_ChageUiTargetLayer() => ChangeUiTargetLayer();
        public void Test_RestoreUiTargetLayer() => RestoreUiTargetLayer()
        ;
        private void ChangeUiTargetLayer()
        {
            if(_uiTarget == null)
                return;
            
            _origParent = _uiTarget.transform.parent;
            _origOrder = _origParent.GetSiblingIndex();
            _uiTarget.transform.SetParent(_tutorialElementsContainer, true);
        }
        
        private void RestoreUiTargetLayer()
        {
            if(_uiTarget == null)
                return;

            _uiTarget.transform.SetParent(_origParent, true);
            _uiTarget.transform.SetSiblingIndex(_origOrder);
            _uiTarget = null;
        }


        private void ClickButton()
        {
            Debug.Log(">>> ClickButton");
            _button?.onClick.RemoveListener(ClickButton); 
            OnComplete();
        }


        private void Autocomplete()
        {
            if(_step.Action.AutoCompleteByTime == 0)
                return;
            OnComplete();
        }

        public void Editor_Redraw()
        {
            if (_step.Action.Type == TutorialActionType.Ui)
            {
                _uiTarget = TutorialExtensions.FindTarget(_step.Action.UiGuid, true);
                Debug.Log($"Editor_Redraw _uiTarget:{_uiTarget}");
            }
            Redraw();
            _view.HandContainer.gameObject.SetActive(_step.UiData.ShowHand);
            // AnimateHand();
        }
        
        private void Redraw()
        {
            RedrawBg();
            RedrawTextCenter();
            RedrawHandsAndArrows();
        }

        private void RedrawHandsAndArrows( )
        {
            var uiData = _step.UiData;
            var targetCenter = TargetCenter();

            _view.HandContainer.position = targetCenter;
            _view.HandContainer.localPosition += uiData.HandOffset.ToVector3();
            _view.HandContainer.rotation = Quaternion.Euler(0,0,uiData.HandAngle);
            _view.HandContainer.gameObject.SetActive(false);

            _view.ArrowContainer.position = targetCenter;
            _view.ArrowContainer.localPosition += uiData.ArrowOffset.ToVector3();
            _view.ArrowContainer.rotation = Quaternion.Euler(0,0,uiData.ArrowAngle);
            _view.ArrowContainer.gameObject.SetActive(uiData.ShowArrow);
        }

        private Vector3 TargetCenter()
        {
            var targetCenter = _step.Action.Type switch
            {
                TutorialActionType.None => ScreenCenter(),
                TutorialActionType.Merge => ScreenPos(_step.Action.fromCell),
                TutorialActionType.Ui => _uiTarget ? _uiTarget.transform.position : ScreenCenter(),
            };
            return targetCenter;
        }

        private void RedrawTextCenter()
        {
            var uiData = _step.UiData;
            var targetCenter = TargetCenter();
            
            var textCenter = uiData.TextOffsetType switch
            {
                TutorialUiData.OffsetType.FromCenter => ScreenCenter(),
                TutorialUiData.OffsetType.FromTarget => targetCenter,
            };
            _view.TextContainer.anchoredPosition = Vector2.zero;
            textCenter.Set(_view.TextContainer.position.x, textCenter.y, 0);

            _view.TextContainer.position = textCenter;
            _view.TextContainer.localPosition += uiData.TextOffset.ToVector3();
            _view.TextContainer.gameObject.SetActive(uiData.ShowText);
            _view.Text.text = uiData.Text.Loc("tutors");
            // _view.TextLayoutGroup.SetLayoutHorizontal();
            // LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform) _view.TextLayoutGroup.transform );
            
            // ActivateTextLayout();
        }

        private async void ActivateTextLayout()
        {
            await Task.Yield();
            _view.TextLayoutGroup.gameObject.SetActive(false);
            // _view.TextLayoutGroup.enabled = true;
            await Task.Yield();
            _view.TextLayoutGroup.gameObject.SetActive(true);
        }

        public void RedrawBg()
        {
            var actionType = _step.Action.Type;
            _view.Bg.gameObject.SetActive(actionType == TutorialActionType.Ui || actionType == TutorialActionType.None);
            _view.BgMini.GetComponent<Image>().enabled  = (actionType == TutorialActionType.Merge); // Hack
        }

        private void OnComplete()
        {
            _onComplete?.Invoke();
        }

        private Vector3 ScreenCenter() 
            => new Vector3(_camera.pixelWidth * 0.5f, _camera.pixelHeight * 0.5f);


        private Vector3 ScreenPos(Vector2Int pos)
            => _camera.WorldToScreenPoint(pos.ToVector3());
        
        private void AnimateHand()
        {
            _view.HandContainer.gameObject.SetActive(_step.UiData.ShowHand);
            if(!_step.UiData.ShowHand)
                return;
            
            _view.HandContainer.gameObject.SetActive(false);
            
            _view.HandContainer.position = ScreenPos(_step.Action.fromCell) + _step.UiData.HandOffset.ToVector3();

            _twHand?.Kill();
            _twHand = DOTween.Sequence()
                .AppendInterval(0.1f)
                .AppendCallback(() => _view.HandContainer.gameObject.SetActive(true))
                
                .AppendInterval(0.5f)
                .Append(_view.HandContainer.DOMove(ScreenPos(_step.Action.toCell) + _step.UiData.HandOffset.ToVector3(), 1))
                .AppendInterval(1)
                .AppendCallback(() => _view.HandContainer.gameObject.SetActive(false))
                
                // Repeat
                .AppendInterval(1)
                .AppendCallback(AnimateHand);
        }
    }
}