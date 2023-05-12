using System;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    [RequireComponent(typeof(Button))]
    public class CellView : MonoBehaviour
    {
        [SerializeField] private Image _candy;

        private Vector3 _normalScale;
        private Vector3 _selectScale;
        
        public Button Button;

        public Sprite Sprite => _candy.sprite;

        public Image Candy => _candy;

        public int X { get; private set; }
        public int Y { get; private set; }

        private void Awake()
        {
            Button = GetComponent<Button>();
            _normalScale = transform.localScale;
            _selectScale += _normalScale * 1.5f;
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