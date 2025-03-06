using Extensions;
using UnityEngine;
using VContainer;

namespace Runner.Core
{
    public class PlayerTriggerController : MonoBehaviour
    {
        [SerializeField] private ParticleSystem[] _sparkParticle;

        private GameManager _gameManager;
        private MapGenerator _roadManager;
        private AudioManager _audioManager;
        private PlayerStateMachine _player;
        private MapSettings _mapSettings;
        private BoxCollider _collider;
        private int _currentParticle;

        [Inject]
        public void Construct(
            MapGenerator roadManager, 
            PlayerStateMachine player, 
            GameManager gameManager,
            AudioManager audioManager,
            MapSettings mapSettings)
        {
            _roadManager = roadManager;
            _player = player;
            _gameManager = gameManager;
            _audioManager = audioManager;
            _mapSettings = mapSettings;
            _collider = GetComponent<BoxCollider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(TAGS.NEW_ROAD))
            {
                _roadManager.GetNewRoad();
            }
            else if (other.CompareTag(TAGS.END_ROAD))
            {
                if (other.TryGetComponent(out RoadController roadController))
                {
                    roadController.GoPool();
                }
            }
            else if (other.CompareTag(TAGS.COIN))
            {
                other.gameObject.Close();

                // Play particle
                // Ufak çaplý bir pool mantýðý kullandým. Kullandýðýmýz pool'dan direkt olarak
                // particlesystem componentine eriþmen daha costlu olacaðýndan bu þekilde bir yöntem izledim
                ParticleSystem spark = GetNextParticle();
                spark.transform.position = other.transform.position;
                spark.Clear();
                spark.Play();
                _currentParticle++;

                // Update highscore
                _gameManager.UpdateScore(_mapSettings.CoinValue);

                // play sound
                _audioManager.Play(SOUNDS.COIN, true);
            }
            else if (other.CompareTag(TAGS.OBSTACLE))
            {
                _collider.enabled = false;
                _player.OnFall();
            }
        }

        private ParticleSystem GetNextParticle()
        {
            if (_currentParticle >= _sparkParticle.Length)
            {
                _currentParticle = 0;
            }

            return _sparkParticle[_currentParticle];
        }
    }
}
