using UnityEngine;

namespace Janegamedev.UI
{
    [CreateAssetMenu(menuName = "Data/SpriteSet")]
    public class SpriteSet : ScriptableObject
    {
        [SerializeField]
        private Sprite on;
        public Sprite On => on;
        [SerializeField]
        private Sprite off;
        public Sprite Off => off;
    }
}