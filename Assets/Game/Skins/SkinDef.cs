using UnityEngine;

namespace Game.Skins
{
    [CreateAssetMenu(menuName = "Game/Skin", fileName = "Skin_")]
    public class SkinDef : ScriptableObject
    {
        [Header("Identity")]
        public string Id;              
        public string DisplayName;

        [Header("Economy")]
        public int Price;
        public bool IsDefault;

        [Header("Visuals")]
        public RuntimeAnimatorController AnimatorController;
        public Sprite Icon;
        
        [Header("Collider")]
        public Vector2 ColliderOffset;
        public Vector2 ColliderSize;
    }
}