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
        public AudioSource music;

        AudioClip currentBgmSound;

        BaseHundunGame<T_GAME, T_SAVE> game;

        Dictionary<String, AudioClip> screenIdToSoundMap = new Dictionary<String, AudioClip>();

        public AudioPlayManager(BaseHundunGame<T_GAME, T_SAVE> game) {
            this.game = game;
        }

        public void lazyInit(Dictionary<String, String> screenIdToFilePathMap) {
            if (screenIdToFilePathMap != null) {
                foreach (var entry in screenIdToFilePathMap)
                {
                    var k = entry.Key;
                    var v = entry.Value;
                    screenIdToSoundMap.put(k, (AudioClip)Resources.Load(v));
                }
            }
        }


        public void intoScreen(String screenId) {
            if (screenIdToSoundMap.containsKey(screenId)) {
                setScreenBgm(screenIdToSoundMap.get(screenId));
            }
        }


        private void setScreenBgm(AudioClip bgmSound) {
            if (currentBgmSound != null) {
                music.Stop();
            }
            music.PlayOneShot(bgmSound);
            currentBgmSound = bgmSound;
        }
    }


}
