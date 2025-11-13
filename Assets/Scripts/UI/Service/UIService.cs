using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Service
{
    public class UIService : MonoBehaviour
    {
        [SerializeField] private List<UIWindow> windowPrefabs;
        private readonly Dictionary<Type, UIWindow> _windows = new();
        private Transform _uiRoot;

        private void Awake()
        {
            _uiRoot = transform;
            foreach (var win in GetComponentsInChildren<UIWindow>(true))
                _windows[win.GetType()] = win;
        }

        public T Open<T>() where T : UIWindow
        {
            var window = Get<T>();
            window.Open();
            return window;
        }

        public void Close<T>() where T : UIWindow
        {
            if (_windows.TryGetValue(typeof(T), out var window))
                window.Close();
        }

        public T Get<T>() where T : UIWindow
        {
            var type = typeof(T);

            if (_windows.TryGetValue(type, out var wnd))
                return (T) wnd;

            foreach (var prefab in windowPrefabs)
            {
                if (prefab.GetType() == type)
                {
                    var instance = Instantiate(prefab, _uiRoot).GetComponent<T>();
                    instance.gameObject.SetActive(false);
                    _windows[type] = instance;
                    return instance;
                }
            }

            Debug.LogError($"[UIService] Prefab for window {type.Name} not found!");
            return null;
        }

        public bool IsOpen<T>() where T : UIWindow
        {
            return _windows.TryGetValue(typeof(T), out var wnd) && wnd.IsOpened;
        }
    }
}