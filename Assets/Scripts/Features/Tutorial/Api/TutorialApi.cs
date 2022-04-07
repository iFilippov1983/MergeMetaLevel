using System;
using System.Collections.Generic;
using Components.Services;
using Configs;
using Configs.Tutorial;
using Data;
using Features._Events;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Components.Api.Tutorial
{
    [Serializable]
    public class TutorialApi
    {
        public static TutorialApi instance;
        private UiTutorialApi _view;
        private DynamicData _dynamicData;
        private StaticData _configs;
        private List<TutorialStep> _steps;
        private int _stepIndex;
        private TutorialStepState _state;
        public TutorialStep _step;
        public Camera _camera;

        private int _indexToSave;
        private SaveApi _save;
        private RootEvents _events;


        public void SetCtx(CoreRoot root)
        {
            instance = this;
            _view = root.Ui.Tutorial;
            _dynamicData = root.Data;
            _configs = root.Configs;
            _camera = root.View.Merge.Player.Camera;
            _save = root.Save;
            _events = root.Events;
            
            _events.Tutorial.Check += CheckTutor;
        
            _events.App.OnDataLoaded += OnDataLoaded;
            _events.App.OnDataSave += OnDataSave;
        }

        private void OnDataLoaded(ProfileData data)
        {
            _indexToSave = data.TutorialIndex;
        }

        private void OnDataSave(ProfileData data)
        {
            data.TutorialIndex = _indexToSave;
        }

        private void CheckTutor(TutorialTriggerType type)
        {
            _dynamicData.Tutorial.Active = false;
            if(_configs.Debug.SkipTutors)
                return;
            
            Debug.Log($"TutorialApi:CheckTutor {type.ToString()}");

            var level = _dynamicData.Profile.LevelIndex;
            // Debug.Log($"conf {_configs} ,tut {_configs?.Tutorial}, steps {_configs.Tutorial.Steps}");
            // _steps = _configs.Tutorial.Steps.FindAll(v => IsTriggered(v, level, type));
            
            _steps = new List<TutorialStep>();

            for (int index = 0; index < _configs.Tutorial.Steps.Count; index++)
            {
                var step = _configs.Tutorial.Steps[index];
                if(step == null)
                    Debug.LogError($"Tutorial step == null index[{index}]");
                if(step != null && IsTriggered(step, level, type) && (_indexToSave == 0 || index > _indexToSave) )
                    _steps.Add(step);  
            }
            // _configs.Tutorial.Steps.FindAll(v => IsTriggered(v, level, type));
            
            Debug.Log($"TutorialApi:CheckTutor stepsCount {_steps.Count}");

            if (_steps.Count > 0)
                StartTutorial();
        }

        private bool IsTriggered(TutorialStep step, int level, TutorialTriggerType type)
            => step != null &&
               step.Trigger?.Level == level
               && step.Trigger?.Type == type;


        [Button]
        private void Editor_StartTutorial(TutorialStep step)
        {
            if(step != null)
                _step = step;
            OnStepEnter(_step);
        }
        
        private void StartTutorial()
        {
            _stepIndex = 0;
            RunStep();
        }

        private void RunStep()
        {
            Debug.Log($"TutorialApi:RunStep  {_stepIndex}");
            
            _step = _stepIndex < _steps.Count ? _steps[_stepIndex] : null;
            if(_step)
                OnStepEnter(_step);
        }

        [Button]
        public void Test_PlayTutor(TutorialStep step)
        {
            _state = new TutorialStepState(step, _dynamicData.Merge, _view, _events.Merge, _camera, OnStepExit);
            _state.OnEnter();
        }
        
        [Button]
        public void Test_StopTutor(TutorialStep step)
        {
            OnStepExit();
        }
        
        private void OnStepEnter(TutorialStep step)
        {
            _dynamicData.Tutorial.Active = true;
            _state = new TutorialStepState(step, _dynamicData.Merge, _view, _events.Merge, _camera, OnStepComplete);
            _state.OnEnter();
        }

        private void OnStepExit()
        {
            _dynamicData.Tutorial.Active = false;
            _state?.OnExit();
        }

        private void OnStepComplete()
        {
            Debug.Log($"TutorialApi:OnStepComplete  {_stepIndex}");
            
            if (_state.Step.SaveAfterComplete)
            {
                _indexToSave = _configs.Tutorial.Steps.IndexOf(_step);
                Debug.Log($"_indexToSave {_indexToSave}");
                _save.SaveProfile();
            }

            _stepIndex++;
            OnStepExit();
            RunStep();
        }
    }
}