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

        [Header("Visuals")]
        public Sprite Sprite;
        public RuntimeAnimatorController AnimatorController;
        public Sprite Icon;
    }
}