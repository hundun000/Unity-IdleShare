using Assets.Scripts.DemoGameCore;
using Assets.Scripts.DemoGameCore.logic;
using Assets.Scripts.DemoGameCore.ui.screen;
using hundun.unitygame.enginecorelib;
using hundun.unitygame.gamelib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace hundun.idleshare.enginecore
{


    public class IdleScreenBackgroundVM : MonoBehaviour, IGameAreaChangeListener
    {
        private BackgroundVM backgroundVM;
        private AbstractTextureManager textureManager;

        public void onGameAreaChange(string last, string current)
        {
            backgroundVM.update(textureManager.getBackgroundTexture(current));
        }

        internal void postPrefabInitialization(AbstractTextureManager textureManager)
        {
            this.textureManager = textureManager;
        }

        void Awake()
        {
            this.backgroundVM = this.transform.Find("BackgroundVM").GetComponent<BackgroundVM>();

        }

    }
}
