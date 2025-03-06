using UnityEngine;
using VContainer;
using DG.Tweening;

namespace Runner.Core
{
    public class MapGenerator : MonoBehaviour
    {
        [SerializeField] private Transform _roadParent;

        private ObjectPool _pool;
        private MapSettings _mapSettings;
        private Transform _currentRoad;
        private RoadController _currentRoadController;

        private float _currentXPos = 20f;
        private int _itemCurrentZPos;
        private const float _laneWidth = 2f;
        private string[] _obstacles;

        [Inject]
        private void Construct(ObjectPool pool, MapSettings mapSettings)
        {
            _pool = pool;
            _mapSettings = mapSettings;
        }

        private void OnEnable()
        {
            EventManagers.OnNextLevel += OnNextLevel;
        }

        private void OnDisable()
        {
            EventManagers.OnNextLevel -= OnNextLevel;
        }

        private void Start()
        {
            CreateObstacleArray();
            SetCurrentDataValues();
        }

        // Obstacle'lar için bir dizi oluþturup aralarýndan random seçtireceðim
        private void CreateObstacleArray()
        {
            _obstacles = new string[4];
            _obstacles[0] = POOL.OBSTACLE_1;
            _obstacles[1] = POOL.OBSTACLE_2;
            _obstacles[2] = POOL.OBSTACLE_3;
            _obstacles[3] = POOL.OBSTACLE_4;
        }

        // Scriptable Objects üzerinden yaparak editor'de deðiþtirilmiþ deðerleri
        // tuttuðu için oyun baþlarken olmasý gereken deðerlerine geri çeviriyorum
        private void SetCurrentDataValues()
        {
            _mapSettings.RateOfDropObstacle = _mapSettings.CurrentRateOfDropObstacle;
            _mapSettings.RateOfDropCoin = _mapSettings.CurrentRateOfDropCoin;
            _mapSettings.RateOfDropEmptySpace = _mapSettings.CurrentRateOfDropEmptySpace;

            _mapSettings.CoinValue = _mapSettings.CurrentCoinValue;
        }

        public void GetNewRoad()
        {
            _itemCurrentZPos = 0;
            Vector3 randomPos = new(-10, Random.Range(-10f, 10f), _currentXPos + _mapSettings.LaneLength);
            Vector3 newRoadPos = new(0, 0, _currentXPos + _mapSettings.LaneLength);
            GameObject road = _pool.Get(POOL.ROAD, randomPos, Quaternion.identity, _roadParent);

            if (road.TryGetComponent(out RoadController roadController))
            {
                roadController.Construct(_pool);
                _currentRoadController = roadController;
            }

            _currentRoad = road.transform;
            _currentXPos += _mapSettings.LaneLength;

            road.transform.DOMove(newRoadPos, 1f).SetEase(Ease.OutQuart).OnComplete(() =>
            {
                OnDropItems();
            });
        }

        private void OnDropItems()
        {
            float totalValue = _mapSettings.RateOfDropObstacle + _mapSettings.RateOfDropCoin + _mapSettings.RateOfDropEmptySpace;
            float randomItem = Random.Range(0, totalValue); // Coin mi, Obstacle mý yoksa boþ alan mý düþüceðini random belirle

            if (randomItem < _mapSettings.RateOfDropObstacle)
            {
                DropObstacles();
            }
            else if (randomItem <= _mapSettings.RateOfDropObstacle + _mapSettings.RateOfDropCoin)
            {
                DropCoins();
            }
            else
            {
                DropEmptySpace();
            }
        }

        private void DropCoins()
        {
            int lane = Random.Range(0, _mapSettings.LaneCount);
            int randomGroupSize = Random.Range(_mapSettings.CoinGroupMinSize, _mapSettings.CoinGroupMaxSize);

            // Coin grubunu oluþtur
            for (int j = 0; j < randomGroupSize; j++)
            {
                Vector3 spawnPosition = GetLanePosition(lane, _itemCurrentZPos);
                if (_itemCurrentZPos < _mapSettings.LaneLength)
                {
                    GameObject coin = _pool.Get(POOL.COIN);
                    _currentRoadController.GetDroppedItem(coin, POOL.COIN);
                    coin.transform.SetParent(_currentRoad);
                    coin.transform.localPosition = spawnPosition;
                    coin.transform.DOLocalMoveY(0.5f, _mapSettings.DropDuration).SetDelay(_mapSettings.CoinDropDelay * j);
                    _itemCurrentZPos++;

                    Invoke(nameof(OnDropItems), .1f);
                }
            }
        }

        private void DropObstacles()
        {
            int lane = Random.Range(0, _mapSettings.LaneCount);
            Vector3 spawnPosition = GetLanePosition(lane, _itemCurrentZPos);

            if (_itemCurrentZPos < _mapSettings.LaneLength)
            {
                int randomObstacle = Random.Range(0, _obstacles.Length);
                GameObject obstacle = _pool.Get(_obstacles[randomObstacle]);
                _currentRoadController.GetDroppedItem(obstacle, _obstacles[randomObstacle]);

                obstacle.transform.SetParent(_currentRoad);
                obstacle.transform.localPosition = spawnPosition;
                obstacle.transform.DOLocalMoveY(0f, _mapSettings.DropDuration).SetDelay(_mapSettings.ObstacleDropDelay);
                _itemCurrentZPos++;

                Invoke(nameof(OnDropItems), .1f);
            }
        }

        private void DropEmptySpace()
        {
            if (_itemCurrentZPos < _mapSettings.LaneLength)
            {
                _itemCurrentZPos++;
                Invoke(nameof(OnDropItems), .1f);
            }
        }

        private Vector3 GetLanePosition(int lane, int zPosition)
        {
            // Yol pozisyonlarýný hesapla
            float xPosition = (lane - (_mapSettings.LaneCount - 1) / 2f) * _laneWidth;
            return new Vector3(xPosition, _mapSettings.DropHeight, zPosition);
        }

        private void OnNextLevel(DifficultyLevelSettings settings)
        {
            _mapSettings.RateOfDropObstacle += settings.ObstacleDropRateToBeUpdated;
            _mapSettings.RateOfDropCoin += settings.CoinDropRateToBeUpdated;
            _mapSettings.RateOfDropEmptySpace += settings.EmptySpaceDropRateToBeUpdated;

            _mapSettings.CoinValue += settings.CoinValueToBeIncreased;


        }
    }
}

