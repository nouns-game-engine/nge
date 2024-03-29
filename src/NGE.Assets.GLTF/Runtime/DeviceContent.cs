﻿namespace NGE.Assets.GLTF.Runtime
{
    /// <summary>
    /// A wrapper that contains an object with disposable resources.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DeviceContent<T> : IDisposable
        where T:class
    {
        #region lifecycle

        internal DeviceContent(T instance, IDisposable[] disposables)
        {
            _Instance = instance;
            _Disposables = disposables;
        }

        public void Dispose()
        {
            _Instance = null;
            if (_Disposables == null) return;

            foreach (var d in _Disposables) d.Dispose();
            _Disposables = null;
        }

        ~DeviceContent()
        {
            System.Diagnostics.Debug.Assert(_Disposables == null, "Not disposed correctly");
        }

        #endregion

        #region data

        /// <summary>
        /// The actual object.
        /// </summary>
        private T _Instance;

        /// <summary>
        /// The disposable resources associated with <see cref="_Instance"/>.
        /// </summary>
        private IDisposable[] _Disposables;

        #endregion

        #region properties

        public static implicit operator T(DeviceContent<T> value) { return value?.Instance; }

        public T Instance => _Instance;

        #endregion       
    }
}
