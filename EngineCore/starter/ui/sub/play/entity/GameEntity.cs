using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace hundun.idleshare.enginecore
{
    public class GameEntity : MonoBehaviour
    {
        public RectTransform rectTransform;
        public Image image;

        public Sprite texture;
        public float x;
        public float y;
        public int drawWidth;
        public int drawHeight;
        public Boolean moveable;
        public Boolean hiden;
        public float speedX;
        public float speedY;
        public Boolean textureFlipX;

        void Awake()
        {
            this.rectTransform = this.GetComponent<RectTransform>(); 
            this.image = this.GetComponent<Image>();
        }

        virtual public void frameLogic() { }
        virtual public Boolean checkRemove()
        {
            return false;
        }
    }
}
