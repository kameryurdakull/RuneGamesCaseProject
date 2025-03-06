using UnityEngine;
using VContainer;

namespace Runner.Core
{
    public class PlayerStepsController : MonoBehaviour
    {
        [Inject] private AudioManager _audioManager;

        // Animation Event
        private void OnStep()
        {
            _audioManager.Play(SOUNDS.STEP);
        }
    }
}
