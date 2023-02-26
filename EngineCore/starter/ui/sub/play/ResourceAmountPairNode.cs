using hundun.unitygame.enginecorelib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace hundun.idleshare.enginecore
{
    public class ResourceAmountPairNode : MonoBehaviour
    {



        String resourceType;
        // ------ replace-lombok ------
        public String getResourceType()
        {
            return resourceType;
        }

        Image image;
        Text label;

        void Awake()
        {
            this.image = this.transform.Find("image").GetComponent<Image>();
            this.label = this.transform.Find("label").GetComponent<Text>();
        }

        public void postPrefabInitialization(AbstractTextureManager textureManager, String resourceType)
        {

            this.resourceType = resourceType;
            Sprite textureRegion = textureManager.getResourceIcon(resourceType);
            this.image.sprite = textureRegion;
            this.label.text = "";
        }

        public void update(long amout)
        {
            label.text = (
                    amout + ""
                    );
        }
    }
}
