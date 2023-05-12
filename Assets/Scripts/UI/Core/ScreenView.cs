using UnityEngine;

namespace UI.Core
{
    public abstract class ScreenView : MonoBehaviour
    {
        [SerializeField] private Canvas _root;

        public void Show() => _root.enabled = true;
        public void Hide() => _root.enabled = false;
    }
}