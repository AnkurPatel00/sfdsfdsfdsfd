using UnityEngine;

namespace Game.CommonModules.Audio
{
    public interface IAudioPlayer
    {
        /// <summary>
        /// Play particular clip, with given volume
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="volume"></param>
        void Play(AudioClip clip, float volume = 1);
        
        /// <summary>
        /// Pause Audio
        /// </summary>
        void Pause();
        
        /// <summary>
        /// UnPause Audio
        /// </summary>
        void UnPause();
    }
}