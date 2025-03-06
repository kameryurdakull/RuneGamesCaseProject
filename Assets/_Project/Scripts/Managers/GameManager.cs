using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

namespace Runner.Core
{
    public class GameManager : MonoBehaviour
    {
        private PlayerStateMachine _playerController;
        private PlayerSettings _playerSettings;
        private DifficultySettings _difficultySettings;
        private UIController _uiController;
        private VolumeController _volumeController;
        private AudioManager _audioManager;
        private GameData _gameData;
        private int _currentScore;
        private int _currentDifficultyLevel;
        private float _levelCountdown;
        private bool _isGameStarted;
        private bool _isMaxLevel;

        [Inject]
        private void Construct(
            PlayerStateMachine playerController,
            PlayerSettings playerSettings,
            GameData gameData,
            AudioManager audioManager,
            DifficultySettings difficultySettings,
            UIController uiController,
            VolumeController volumeController)
        {
            _playerController = playerController;
            _playerSettings = playerSettings;
            _gameData = gameData;
            _audioManager = audioManager;
            _difficultySettings = difficultySettings;
            _uiController = uiController;
            _volumeController = volumeController;
        }

        // Class'lar birbirlerine inject olamadýklarýndan dolayý GameManager'a olaylarý event'lerle iletiyorum
        private void OnEnable()
        {
            EventManagers.OnStartGame += OnStartGame;
            EventManagers.OnRetryGame += OnRetryGame;
            EventManagers.OnFall += CheckHighScore;
        }

        private void OnDisable()
        {
            EventManagers.OnStartGame -= OnStartGame;
            EventManagers.OnRetryGame -= OnRetryGame;
            EventManagers.OnFall -= CheckHighScore;
        }

        private void Start()
        {
            SaveSystem.LoadData(_gameData);
            _uiController.UpdateScore(0);

            SetCurrentDataValues();
        }

        // Scriptable Objects üzerinden yaparak editor'de deðiþtirilmiþ deðerleri
        // tuttuðu için oyun baþlarken olmasý gereken deðerlerine geri çeviriyorum
        private void SetCurrentDataValues() 
        {
            _playerSettings.MoveSpeed = _playerSettings.CurrentMoveSpeed;
        }

        private void Update()
        {
            if (!_isMaxLevel && _isGameStarted)
            {
                _levelCountdown -= Time.deltaTime;
                if (_levelCountdown <= 0)
                {
                    NextLevel();
                }
            }
        }

        public void OnStartGame()
        {
            _playerController.OnGameStarted();

            // Start Timer To After Difficulty Level
            _levelCountdown = _difficultySettings.DifficultyLevelSettings[_currentDifficultyLevel].LevelCountdown;
            _isGameStarted = true;
        }

        private void OnRetryGame()
        {
            // Load this Scene
            SceneManager.LoadScene(0);
        }

        public void UpdateScore(int value)
        {
            _currentScore += value;
            _uiController.UpdateScore(_currentScore);
        }

        private void CheckHighScore()
        {
            // Eger highscore'u geçerse oyunu kazanmýþ olur, geçemezse fail durumuna düþer.
            if (_currentScore > _gameData.HighScore)
            {
                _gameData.HighScore = _currentScore;
                SaveSystem.SaveData(_gameData);
                _uiController.OnFall(true);
            }
            else
            {
                _uiController.OnFall(false);
            }
        }

        private void NextLevel()
        {
            EventManagers.OnNextLevel?.Invoke(_difficultySettings.DifficultyLevelSettings[_currentDifficultyLevel]);
            _currentDifficultyLevel++;
            _volumeController.BloomEffect(5, .3f);
            _audioManager.Play(SOUNDS.NEXTLEVEL);
            if (_currentDifficultyLevel >= _difficultySettings.DifficultyLevelSettings.Length)
            {
                _isMaxLevel = true;
            }
            else
            {
                _levelCountdown = _difficultySettings.DifficultyLevelSettings[_currentDifficultyLevel].LevelCountdown;
            }
        }

        [ContextMenu("DeleteData")]
        public void DeleteData()
        {
            SaveSystem.DeleteData();
        }
    }
}
