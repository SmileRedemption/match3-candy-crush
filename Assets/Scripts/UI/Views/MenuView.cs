using System;
using UI.Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views
{
    public class MenuView : ScreenView
    {
        [SerializeField] private Button _gameButton;
        [SerializeField] private Button _dataButton;

        public event Action GameButtonClicked;
        public event Action DataButtonClicked;

        private void OnEnable()
        {
            _gameButton.onClick.AddListener(() => GameButtonClicked?.Invoke());
            _dataButton.onClick.AddListener(() => DataButtonClicked?.Invoke());
        }

        private void OnDisable()
        {
            _gameButton.onClick.RemoveAllListeners();
            _dataButton.onClick.RemoveAllListeners();
        }
    }
}