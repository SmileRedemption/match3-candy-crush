

using System;
using System.Collections.Generic;
using UI.Core;
using UI.Views;
using UnityEngine;

namespace UI
{
    public class UIContext : MonoBehaviour
    {
        [SerializeField] private MenuView _menuView;
        [SerializeField] private GameView _gameView;
        [SerializeField] private DataView _dataView;

        private ScreenView _currentScreenView;

        public void Initialize()
        {
            _currentScreenView = _menuView;
            _menuView.GameButtonClicked += OnGameButtonClicked;
            _menuView.DataButtonClicked += OnDataButtonClicked;
        }

        public void Complete()
        {
            _menuView.GameButtonClicked -= OnGameButtonClicked;
            _menuView.DataButtonClicked -= OnDataButtonClicked;
        }
        
        private void OnGameButtonClicked()
        {
            _currentScreenView.Hide();
            _gameView.Show();
            _currentScreenView = _gameView;
        }

        private void OnDataButtonClicked()
        {
            _currentScreenView.Hide();
            _dataView.Show();
            _currentScreenView = _dataView;
        }
    }
}