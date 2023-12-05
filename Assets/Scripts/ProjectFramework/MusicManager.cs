using System;
using UnityEngine;

namespace FrameWork
{
    public class MusicManager : SingletonManager<MusicManager>
    {
        AudioSource _backgroundMusic;
        AudioSource SafeBackgroundMusic
        {
            get
            {
                if (_backgroundMusic == null)
                {
                    _backgroundMusic = Extension.GetSingletonComponent<AudioSource>();
                }
                return _backgroundMusic;
            }
        }
        AudioSource _soundEffect;
        AudioSource SafeSoundEffect
        {
            get
            {
                if (_soundEffect == null)
                {
                    _soundEffect = SafeBackgroundMusic.gameObject.AddComponent<AudioSource>();
                }
                return _soundEffect;
            }
        }

        // 背景音乐
#region 

        public void PlayBackgroundMusic(string BGM, bool loop = true)
        {
            SafeBackgroundMusic.clip = ResourceManager.Instance.Load<AudioClip>("Music/BGM/" + BGM);
            SafeBackgroundMusic.loop = loop;
            SafeBackgroundMusic.Play();
        }

        public void StopBackgroundMusic()
        {
            SafeBackgroundMusic.Stop();
        }

        public void PauseBackgroundMusic()
        {
            SafeBackgroundMusic.Pause();
        }

        public void ChangeBackgroundMusicVolume(float volume)
        {
            SafeBackgroundMusic.volume = volume;
        }

#endregion

        // 音效 
#region 

        public void PlaySoundEffect(string soundEffect)
        {
            var clip = ResourceManager.Instance.Load<AudioClip>("Music/SoundEffect/" + soundEffect);
            SafeSoundEffect.PlayOneShot(clip);
        }

#endregion
    }
}