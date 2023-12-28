using UnityEngine;
using UnityEngine.Rendering;

namespace General.Components.PostEffects
{
    public class SetPostEffectProfile : MonoBehaviour
    {
        [SerializeField] private VolumeProfile _profile;
        [SerializeField] private VolumeProfile _defaultProfile;


        public void Set()
        {
            var volumes = FindObjectsOfType<Volume>();
            foreach (var volume in volumes)
            {
                if (!volume.isGlobal) continue;
                volume.profile = _profile;
                break;
            }
        }


        public void SetToDefault()
        {
            var volumes = FindObjectsOfType<Volume>();
            foreach (var volume in volumes)
            {
                if (!volume.isGlobal) continue;
                volume.profile = _defaultProfile;
                break;
            }
        }
    }
}

