using System.Linq;
using UnityEngine;

namespace Game.Skins
{
    [CreateAssetMenu(menuName = "Game/Skin Catalog", fileName = "SkinCatalog")]
    public class SkinCatalog : ScriptableObject
    {
        public SkinDef[] Items;

        public SkinDef GetDefault()
        {
            if (Items == null) return null;
            return Items.FirstOrDefault(i => i != null && i.IsDefault) ?? Items.FirstOrDefault(i => i != null);
        }

        public SkinDef GetById(string id)
        {
            if (string.IsNullOrEmpty(id) || Items == null) return null;
            return Items.FirstOrDefault(i => i != null && i.Id == id);
        }
    }
}