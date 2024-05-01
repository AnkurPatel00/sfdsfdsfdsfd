using UnityEngine;

namespace Game.CommonModules.Pooling.Audio
{
    public class AudioPlayer : IAudioPlayer
    {
        private AudioSource pAudioSource;

        private AudioSource _audioSource
        {
            get
            {
                if (pAudioSource == null)
                    pAudioSource = Camera.main.GetComponent<AudioSource>();
                return pAudioSource;
            }
        }

        public void Play(AudioClip clip, float volume = 1)
        {
            _audioSource.PlayOneShot(clip, volume);
        }

        public void Pause()
        {
            _audioSource.Pause();
        }

        public void UnPause()
        {
            _audioSource.UnPause();
        }
    }
}