using System;
using UnityEngine;

namespace Components
{
    public class ManagedMonobeh<TDerived> : MonoBehaviour, IManagedMonobeh, IDisposable where TDerived : class
    {
        private event Action<TDerived> _doDispose;
        public bool isDestroyed;

        protected void SetCtxBase(Action<TDerived> dispose)
        {
            isDestroyed = false;
            _doDispose = dispose;
        }

        public void Dispose()
        {
            if(isDestroyed )
                return;
            
            isDestroyed = true;
            _doDispose?.Invoke(this as TDerived);
        }

        public bool IsDestroyed()
        {
            return isDestroyed;
        }
    }
}