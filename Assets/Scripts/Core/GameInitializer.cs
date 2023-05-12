using System;
using DefaultNamespace;
using Model;
using UI;
using UnityEngine;

namespace Core
{
    public class GameInitializer : MonoBehaviour
    {
        [SerializeField] private UIContext _uiContext;
        [SerializeField] private GameField _gameField;
        
        private void Awake()
        {
            _uiContext.Initialize();
            _gameField.Initialize();
        }

        private void OnDestroy()
        {
            _uiContext.Complete();
        }
    }
}