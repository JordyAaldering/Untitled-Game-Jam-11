#pragma warning disable 0649
using UnityEngine;
using UnityEngine.UI;

namespace Grid
{
    [RequireComponent(typeof(MeshRenderer))]
    public class SetQuadTexture : MonoBehaviour
    {
        [SerializeField] private GridSettings gridSettings;
        [SerializeField] private Material material;
        
        private MeshRenderer _meshRenderer;
        private MeshRenderer meshRenderer
        {
            get
            {
                if (!_meshRenderer)
                    _meshRenderer = GetComponent<MeshRenderer>();
                return _meshRenderer;
            }
        }

        public void SetTexture()
        {
            meshRenderer.sharedMaterial = material;
            meshRenderer.sharedMaterial.mainTexture = gridSettings.GetTexture();
        }
    }
}