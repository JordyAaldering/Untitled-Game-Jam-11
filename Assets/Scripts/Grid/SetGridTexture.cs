#pragma warning disable 0649
using UnityEngine;
using UnityEngine.UI;

namespace Grid
{
    [RequireComponent(typeof(RawImage))]
    public class SetGridTexture : MonoBehaviour
    {
        [SerializeField] private GridSettings gridSettings;
        
        private RawImage _image;
        private RawImage image
        {
            get
            {
                if (!_image)
                    _image = GetComponent<RawImage>();
                return _image;
            }
        }

        public void SetTexture()
        {
            image.texture = gridSettings.GetTexture();
        }
    }
}
