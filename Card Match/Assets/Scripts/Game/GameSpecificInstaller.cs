using System;
using System.Collections.Generic;
using Game.Controller;
using Game.Grid;
using UnityEngine;

namespace Game.Installers
{
    public class GameSpecificInstaller : ConfigurationInstaller
    {
        public GameConfigVO GameConfigVO;
        public AudioMapConfigVO AudioMapConfigVO;

        public override void InstallBindings()
        {
            Container.Bind<GameConfigVO>().FromInstance(GameConfigVO).AsSingle();
            Container.Bind<AudioMapConfigVO>().FromInstance(AudioMapConfigVO).AsSingle();
            
            Container.Bind<GameController>().FromComponentInHierarchy().AsSingle();
            Container.Bind<GridManager>().AsSingle();
        }
    }

    [Serializable]
    public class GameConfigVO
    {
        public List<CardDataConfigVO> CardDataConfig;
    }
    
    [Serializable]
    public class AudioMapConfigVO
    {
        public List<AudioMapVO> AudioMap;

        public AudioClip GetAudioClip(string flipSoundId)
        {
            return AudioMap.Find(x => x.AudioId == flipSoundId)?.GetAudioClip();
        }
    }
    
    [Serializable]
    public class CardDataConfigVO
    {
        public int CardIndex;
        public Sprite CardImage;
    }
    
    [Serializable]
    public class AudioMapVO
    {
        public string AudioId;
        public AudioClip AudioClip;

        public AudioClip GetAudioClip()
        {
            return AudioClip;
        }
    }
}