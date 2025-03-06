using UnityEngine;
using DG.Tweening;

namespace Runner.Core
{
    public class CoinController : MonoBehaviour
    {
        private void OnEnable()
        {
            RotateAnim();
        }

        private void OnDisable()
        {
            transform.DOComplete();
        }

        private void RotateAnim()
        {
            transform.DORotate(new Vector3(0, 360, 0), 1f, RotateMode.WorldAxisAdd)
                .SetLoops(-1,LoopType.Restart)
                .SetEase(Ease.Linear);
        }
    }
}
