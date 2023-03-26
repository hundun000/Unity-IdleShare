using hundun.unitygame.adapters;
using hundun.unitygame.enginecorelib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace hundun.idleshare.enginecore
{
    public class AudioPlayManager<T_GAME, T_SAVE> where T_GAME : BaseHundunGame<T_GAME, T_SAVE>
    {
        // need Mono set
        private AudioSource music;

        AudioClip currentBgmSound;

        BaseHundunGame<T_GAME, T_SAVE> game;

        Dictionary<String, AudioClip> screenIdToSoundMap = new Dictionary<String, AudioClip>();

        public AudioPlayManager(BaseHundunGame<T_GAME, T_SAVE> game) {
            this.game = game;
        }

        public void intoScreen(AudioSource music, String screenId)
        {
            this.music = music;
            if (screenIdToSoundMap.containsKey(screenId))
            {
                setScreenBgm(screenIdToSoundMap.get(screenId));
            }
        }

        public void lazyInitOnGameCreate(Dictionary<String, String> screenIdToFilePathMap) {
            if (screenIdToFilePathMap != null) {
                foreach (var entry in screenIdToFilePathMap)
                {
                    var k = entry.Key;
                    var v = entry.Value;
                    v = v.Replace(".mp3", "");
                    v = v.Replace(".wav", "");
                    var resource = (AudioClip)Resources.Load("game/" + v);
                    screenIdToSoundMap.put(k, resource);
                }
            }
        }


        private void setScreenBgm(AudioClip bgmSound) {
            if (currentBgmSound != null) {
                music.Stop();
            }
            music.loop = true;
            music.clip = bgmSound;
            music.Play();
            currentBgmSound = bgmSound;
        }
    }


}
