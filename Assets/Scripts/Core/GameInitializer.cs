using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Core
{
    public class GameInitializer : MonoBehaviour
    {
        [SerializeField] private UIContext _uiContext;
        [SerializeField] private GameField _gameField;
        [SerializeField] private Button _restartButton;
        
        private void Awake()
        {
            _uiContext.Initialize();
            _gameField.Initialize();
            
            _restartButton.onClick.AddListener(OnRestartButtonClick);
        }

        private void OnDestroy()
        {
            _uiContext.Complete();
            
            _restartButton.onClick.RemoveAllListeners();
        }

        private void OnRestartButtonClick()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}