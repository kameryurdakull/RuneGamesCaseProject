using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

namespace Runner.Core
{
    public class VolumeController : MonoBehaviour
    {
        Volume _volume;

        static Bloom _bloom;
        static Vignette _vignette;

        private void Awake()
        {
            _volume = GetComponent<Volume>();

            _volume.profile.TryGet(out _bloom);
            _volume.profile.TryGet(out _vignette);
        }

        public void BloomEffect(float value, float timer, float completeTime = .15f)
        {
            DOTween.To(() => _bloom.intensity.value, x => _bloom.intensity.value = x, value, timer);
            DOTween.To(() => _bloom.intensity.value, x => _bloom.intensity.value = x, 0, completeTime).SetDelay(timer);
        }

        public void VignetteEffect(float value, float timer, float completeTime = .15f)
        {
            DOTween.To(() => _vignette.intensity.value, x => _vignette.intensity.value = x, value, timer);
            DOTween.To(() => _vignette.intensity.value, x => _vignette.intensity.value = x, 0, completeTime).SetDelay(timer);
        }
    }
}
