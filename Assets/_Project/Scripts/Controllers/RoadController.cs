using System.Collections.Generic;
using UnityEngine;

namespace Runner.Core
{
    public class RoadController : MonoBehaviour
    {
        private ObjectPool _pool;
        private List<RoadItem> _items = new();

        public void Construct(ObjectPool pool)
        {
            _pool = pool;
        }

        public void GoPool()
        {
            for (int i = 0; i < _items.Count; i++)
            {
                _pool.Give(_items[i].Item, _items[i].Name);
            }
            _items.Clear();
            _pool.Give(gameObject, POOL.ROAD);
        }

        public void GetDroppedItem(GameObject item, string name)
        {
            RoadItem roadItem = new(item, name);

            _items.Add(roadItem);
        }


    }
}
