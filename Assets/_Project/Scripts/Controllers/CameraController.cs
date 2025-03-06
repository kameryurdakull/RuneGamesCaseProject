using UnityEngine;
using DG.Tweening;
using Unity.Cinemachine;

namespace Runner.Core
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private CinemachineCamera _camera;
        private CinemachineBasicMultiChannelPerlin _cinemachineBasic;

        private void Start()
        {
            _cinemachineBasic = _camera.GetComponent<CinemachineBasicMultiChannelPerlin>();
            _cinemachineBasic.AmplitudeGain = 0;
        }

        public void Shake(float amplitude, float timer)
        {
            DOTween.To(() => _cinemachineBasic.AmplitudeGain, x => _cinemachineBasic.AmplitudeGain = x, amplitude, timer / 2).OnComplete(() =>
            {
                DOTween.To(() => _cinemachineBasic.AmplitudeGain, x => _cinemachineBasic.AmplitudeGain = x, 0, timer / 2);
            });
        }
    }
}
