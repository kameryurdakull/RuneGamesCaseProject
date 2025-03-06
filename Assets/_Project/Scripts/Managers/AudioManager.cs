using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using VContainer;

namespace Runner.Core
{
    public class AudioManager : MonoBehaviour
    {
        [Inject] private SoundSettings _soundSettings;

        private Dictionary<string, Sound> _soundDictionary = new();
        private Coroutine _defaultPitchRoutine;

        private void Awake()
        {
            for (int i = 0; i < _soundSettings.Sounds.Length; i++)
            {
                _soundSettings.Sounds[i].Source = gameObject.AddComponent<AudioSource>();
            }
        }

        private void Start()
        {
            for (int i = 0; i < _soundSettings.Sounds.Length; i++)
            {
                _soundDictionary.Add(_soundSettings.Sounds[i].Name, _soundSettings.Sounds[i]);
            }
        }

        public void Play(string name, bool isPitch = false)
        {
            if (_soundDictionary.ContainsKey(name))
            {
                Sound sound = _soundDictionary[name];
                int randomClip = Random.Range(0, sound.Clips.Length);
                AudioClip clip = sound.Clips[randomClip];
                sound.Source.clip = clip;
                sound.Source.volume = sound.Volume;

                if (isPitch)
                {
                    if (sound.Source.pitch < 2.25f)
                    {
                        sound.Source.pitch += .05f;
                    }

                    if (_defaultPitchRoutine != null)
                    {
                        StopCoroutine(_defaultPitchRoutine);
                    }
                    _defaultPitchRoutine = StartCoroutine(DefaultPicth(sound.Source));
                }

                sound.Source.Play();
            }
        }

        IEnumerator DefaultPicth(AudioSource source)
        {
            yield return new WaitForSeconds(.8f);
            source.pitch = 1f;
        }
    }
}
