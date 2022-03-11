using System;

namespace SingletonUpdateManagement
{
    internal abstract class RootController : IDisposable
    {
        private const bool AddInConstructor = true;
        private const bool RemoveOnDispose = true;

        public RootController()
        {
            if (AddInConstructor) UpdateManager.AddItem(this);
        }

        /// <summary>
        /// Substitutes Awake
        /// </summary>
        public virtual void Setup() { }

        /// <summary>
        /// Substitutes Start
        /// </summary>
        public virtual void Initialize() { }

        /// <summary>
        /// Substitutes Update
        /// </summary>
        public virtual void Execute() { }

        /// <summary>
        /// Substitutes FixedUpdate
        /// </summary>
        public virtual void FixedExecute() { }

        /// <summary>
        /// Substitutes LateUpdate
        /// </summary>
        public virtual void LateExecute() { }

        /// <summary>
        /// Substitutes OnDestroy
        /// </summary>
        public virtual void Cleanup() { }

        public void Dispose()
        {
            if (RemoveOnDispose) UpdateManager.RemoveSpecificItem(this);
            Cleanup();
        }
    }

    public static class ControllersMethod
    {
        public const string Setup = "Setup";
        public const string Initiaize = "Initialize";
        public const string Execute = "Execute";
        public const string FixedExecute = "FixedExecute";
        public const string LateExecute = "LateExecute";
        public const string Cleanup = "Cleanup";
    }
}

