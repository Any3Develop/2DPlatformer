using UnityEngine;

namespace UI.Service
{
    public abstract class UIWindow : MonoBehaviour
    {
        public bool IsOpened { get; private set; }

        public virtual void Open()
        {
            IsOpened = true;
            gameObject.SetActive(true);
        }

        public virtual void Close()
        {
            IsOpened = false;
            gameObject.SetActive(false);
        }
    }
}