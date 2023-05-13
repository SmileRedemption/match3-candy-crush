using System;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    [RequireComponent(typeof(Button))]
    public class CellView : MonoBehaviour
    {
        [SerializeField] private Image _candy;

        private Button _button;
        
        private Vector3 _normalScale;
        private Vector3 _selectScale;
        
        public Sprite Sprite => _candy.sprite;
        public Image Candy => _candy;
        
        public int X { get; private set; }
        public int Y { get; private set; }

        public event Action<int, int> OnButtonClicked;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _normalScale = transform.localScale;
            _selectScale += _normalScale * 1.5f;
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(() => OnButtonClicked?.Invoke(X, Y));
        }

        private void OnDisable()
        {
            _button.onClick.RemoveAllListeners();
        }

        public void InitializeCell(Sprite candy, int x, int y)
        {
            _candy.sprite = candy;
            SetPosition(x, y);
        }
        
        public void ChangeSprite(Sprite candy) => 
            _candy.sprite = candy;

        public void Select() => _candy.transform.localScale = _selectScale;
        
        public void UnSelect() => _candy.transform.localScale = _normalScale;

        private void SetPosition(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}