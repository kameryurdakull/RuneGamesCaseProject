using System.Collections.Generic;
using UnityEngine;

namespace Runner.Core
{
    [System.Serializable]
    public struct Pool
    {
        public string ID;
        public GameObject Prefab;
        public int Size;
    }

    public class ObjectPool : MonoBehaviour
    {
        [SerializeField] private List<Pool> pools = new();

        private readonly Dictionary<string, Queue<GameObject>> poolDictionary = new();
        private readonly Dictionary<string, GameObject> backupDictionary = new();

        private void Awake()
        {
            ExecutePool();
        }

        private void ExecutePool()
        {
            foreach (var pool in pools)
            {
                Queue<GameObject> objectPool = new();

                for (int i = 0; i < pool.Size; i++)
                {
                    GameObject obj = Instantiate(pool.Prefab);
                    obj.SetActive(false);
                    obj.transform.SetParent(transform);
                    objectPool.Enqueue(obj);
                }
                poolDictionary.Add(pool.ID, objectPool);
                backupDictionary.Add(pool.ID, pool.Prefab);
            }
        }

        public GameObject Get(string id)
        {
            if (!poolDictionary.ContainsKey(id))
            {
                Debug.LogWarning($"This {id} is not found ObjectPool");
                return null;
            }

            GameObject gameObj;

            if (PooledCount(id) > 0)
            {
                gameObj = poolDictionary[id].Dequeue();
            }
            else
            {
                gameObj = Instantiate(backupDictionary[id]);
            }

            gameObj.SetActive(true);

            return gameObj;
        }

        public GameObject Get(string id, Vector3 position, Quaternion quaternion)
        {
            GameObject gameObj;

            if (PooledCount(id) > 0)
            {
                gameObj = Get(id);
            }
            else
            {
                gameObj = Instantiate(backupDictionary[id]);
            }

            gameObj.transform.position = position;
            gameObj.transform.rotation = quaternion;

            return gameObj;
        }

        public GameObject Get(string id, Vector3 position, Quaternion quaternion, Transform parent)
        {
            GameObject gameObj;

            if (PooledCount(id) > 0)
            {
                gameObj = Get(id);
            }
            else
            {
                gameObj = Instantiate(backupDictionary[id]);
            }

            gameObj.transform.position = position;
            gameObj.transform.rotation = quaternion;
            gameObj.transform.SetParent(parent);

            return gameObj;
        }

        public void Give(GameObject gameObject, string id)
        {
            gameObject.SetActive(false);
            gameObject.transform.position = Vector3.zero;
            gameObject.transform.rotation = Quaternion.identity;
            poolDictionary[id].Enqueue(gameObject);
            gameObject.transform.SetParent(transform);
        }

        private int PooledCount(string id)
        {
            return poolDictionary[id].Count;
        }
    }
}
