using TMPro;
using UnityEngine;
using VContainer;
using DG.Tweening;
using UnityEngine.UI;
using Extensions;
using System.Collections;

namespace Runner.Core
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _liveScoreText;
        [SerializeField] private TextMeshProUGUI _touchToStartText;
        [SerializeField] private Button _touchToStartButton;

        [SerializeField] private RectTransform _endGamePanel;
        [SerializeField] private RectTransform _winFlag;
        [SerializeField] private RectTransform _failFlag;
        [SerializeField] private Image _blackImage;
        [SerializeField] private TextMeshProUGUI _highScoreText;
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private RectTransform[] _endPanelItems;

        private AudioManager _audioManager;
        private GameData _gameData;

        [Inject]
        private void Construct(AudioManager audioManager, GameData gameData)
        {
            _audioManager = audioManager;
            _gameData = gameData;
        }

        private void Start()
        {
            _blackImage.transform.Open();
            _blackImage.DOFade(0, .5f);
            TouchToStartTextAnim();
        }

        public void UpdateScore(int value)
        {
            // Update score text
            _liveScoreText.text = value.ToString();
            _liveScoreText.transform.DOKill();
            _liveScoreText.DOKill();
            _liveScoreText.DOColor(Color.green, .25f);
            _liveScoreText.transform.DOScale(1.25f, .25f).SetEase(Ease.OutElastic).OnComplete(() =>
            {
                _liveScoreText.transform.DOScale(1f, .15f);
                _liveScoreText.DOColor(Color.white, .15f);
            });
        }

        public void OnFall(bool isWinGame)
        {
            StartCoroutine(EndPanelSequence(isWinGame));

            _scoreText.text = _liveScoreText.text;
            _highScoreText.text = _gameData.HighScore.ToString();

            // Score Text
            _liveScoreText.rectTransform.DOAnchorPosY(420f, .5f).SetDelay(.5f);
        }

        IEnumerator EndPanelSequence(bool isWinGame)
        {
            // Open End Game Panel
            _endGamePanel.Open();
            _endGamePanel.DOScale(1, .25f);
            yield return new WaitForSeconds(.25f);

            for (int i = 0; i < _endPanelItems.Length; i++)
            {
                _endPanelItems[i].Open();
                _endPanelItems[i].DOScale(1, .15f);
                yield return new WaitForSeconds(.15f);
            }

            if (isWinGame)
            {
                _winFlag.Open();
                _winFlag.DOScale(1, .25f);
                //yield return new WaitForSeconds(.25f);
                _audioManager.Play(SOUNDS.HIGHSCORE);
            }
            else
            {
                _failFlag.Open();
                _failFlag.DOScale(1, .25f);
            }
        }

        public void OnTouchToStartClick()
        {
            EventManagers.OnStartGame?.Invoke();
            _touchToStartButton.interactable = false;
            _touchToStartButton.transform.DOScale(0, .5f);

            _liveScoreText.rectTransform.DOAnchorPosY(-220f, .5f).SetDelay(.5f);
        }

        public void OnRetryClick()
        {
            _blackImage.DOFade(1, .5f).OnComplete(() =>
            {
                EventManagers.OnRetryGame?.Invoke();
            });
            //_touchToStartButton.transform.localScale = Vector3.one;
        }

        private void TouchToStartTextAnim()
        {
            _touchToStartText.transform.DOScale(1.25f, .25f).SetLoops(-1, LoopType.Yoyo);
        }
    }
}
