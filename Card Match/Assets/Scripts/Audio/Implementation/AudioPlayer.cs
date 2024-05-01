using UnityEngine;

namespace Game.CommonModules.Audio
{
    public class AudioPlayer : IAudioPlayer
    {
        private AudioSource audioSource;

        private AudioSource pAudioSource
        {
            get
            {
                if (audioSource == null)
                    audioSource = Camera.main.GetComponent<AudioSource>();
                return audioSource;
            }
        }

        public void Play(AudioClip clip, float volume = 1)
        {
            pAudioSource.PlayOneShot(clip, volume);
        }

        public void Pause()
        {
            pAudioSource.Pause();
        }

        public void UnPause()
        {
            pAudioSource.UnPause();
        }
    }
}