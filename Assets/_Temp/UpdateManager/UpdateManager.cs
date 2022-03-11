using System;

namespace SingletonUpdateManagement
{
    public class UpdateManager : SingletonBehaviour<UpdateManager>
    {
        protected override bool DoNotDestroyOnLoad { get { return false; } } // Need to add proper cleanup support in case this is set to "true"

        private static event Action OnAwakeEvent;
        private static event Action OnStartEvent;
        private static event Action OnUpdateEvent;
        private static event Action OnFixedUpdateEvent;
        private static event Action OnLateUpdateEvent;
        private static event Action OnDestroyEvent;

        private static readonly Type BaseControllerType = typeof(RootController);

        public static void SubscribeToAwake(Action callback)
        {
            if (Instance == null) return;

            OnAwakeEvent += callback;
        }

        public static void SubscribeToStart(Action callback)
        {
            if (Instance == null) return;

            OnStartEvent += callback;
        }

        public static void SubscribeToUpdate(Action callback)
        {
            if (Instance == null) return;

            OnUpdateEvent += callback;
        }

        public static void SubscribeToFixedUpdate(Action callback)
        {
            if (Instance == null) return;

            OnFixedUpdateEvent += callback;
        }

        public static void SubscribeToLateUpdate(Action callback)
        {
            if (Instance == null) return;

            OnLateUpdateEvent += callback;
        }

        public static void SubscribeToOnDestroy(Action callback)
        {
            if (Instance == null) return;

            OnDestroyEvent += callback;
        }

        public static void UnsubscribeFromAwake(Action callback)
        {
            OnAwakeEvent -= callback;
        }

        public static void UnsubscribeFromStart(Action callback)
        {
            OnStartEvent -= callback;
        }

        public static void UnsubscribeFromUpdate(Action callback)
        {
            OnUpdateEvent -= callback;
        }

        public static void UnsubscribeFromFixedUpdate(Action callback)
        {
            OnFixedUpdateEvent -= callback;
        }

        public static void UnsubscribeFromLateUpdate(Action callback)
        {
            OnLateUpdateEvent -= callback;
        }

        public static void UnsubscribeFromOnDestroy(Action callback)
        {
            OnDestroyEvent -= callback;
        }

        internal static void AddItem(RootController controller)
        {
            if (controller == null) throw new NullReferenceException("The behaviour you've tried to add is null!");

            if (isShuttingDown) return;

            AddItemToStream(controller);
        }

        internal static void RemoveSpecificItem(RootController behaviour)
        {
            if (behaviour == null) throw new NullReferenceException("The behaviour you've tried to remove is null!");

            if (isShuttingDown) return;

            if (Instance != null) RemoveSpecificItemFromStream(behaviour);
        }

        internal static void RemoveSpecificItemAndDestroyComponent(RootController behaviour)
        {
            if (behaviour == null) throw new NullReferenceException("The behaviour you've tried to remove is null!");

            if (isShuttingDown) return;

            if (Instance != null) RemoveSpecificItemFromStream(behaviour);

            behaviour.Dispose();
        }

        private static void AddItemToStream(RootController behaviour)
        {
            Type behaviourType = behaviour.GetType();

            if (behaviourType.GetMethod(ControllersMethod.Setup).DeclaringType != BaseControllerType)
                SubscribeToAwake(behaviour.Setup);

            if (behaviourType.GetMethod(ControllersMethod.Initiaize).DeclaringType != BaseControllerType)
                SubscribeToStart(behaviour.Initialize);

            if (behaviourType.GetMethod(ControllersMethod.Execute).DeclaringType != BaseControllerType)
                SubscribeToUpdate(behaviour.Execute);

            if (behaviourType.GetMethod(ControllersMethod.FixedExecute).DeclaringType != BaseControllerType)
                SubscribeToFixedUpdate(behaviour.FixedExecute);

            if (behaviourType.GetMethod(ControllersMethod.LateExecute).DeclaringType != BaseControllerType)
                SubscribeToLateUpdate(behaviour.LateExecute);

            if (behaviourType.GetMethod(ControllersMethod.Cleanup).DeclaringType != BaseControllerType)
                SubscribeToOnDestroy(behaviour.Cleanup);
        }

        private static void RemoveSpecificItemFromStream(RootController behaviour)
        {
            UnsubscribeFromAwake(behaviour.Setup);
            UnsubscribeFromStart(behaviour.Initialize);
            UnsubscribeFromUpdate(behaviour.Execute);
            UnsubscribeFromFixedUpdate(behaviour.FixedExecute);
            UnsubscribeFromLateUpdate(behaviour.LateExecute);
            UnsubscribeFromOnDestroy(behaviour.Cleanup);
        }

        private void Awake()
        {
            OnAwakeEvent?.Invoke();
        }

        private void Start()
        {
            OnStartEvent?.Invoke();
        }

        private void Update()
        {
            OnUpdateEvent?.Invoke();
        }

        private void FixedUpdate()
        {
            OnFixedUpdateEvent?.Invoke();
        }

        private void LateUpdate()
        {
            OnLateUpdateEvent?.Invoke();
        }

        private void OnDestroy()
        {
            OnDestroyEvent?.Invoke();
        }
    }
}
