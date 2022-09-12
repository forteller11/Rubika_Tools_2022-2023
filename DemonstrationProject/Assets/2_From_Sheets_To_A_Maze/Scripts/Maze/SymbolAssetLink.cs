using System;
using System.Collections.Generic;
using UnityEngine;

namespace Charly.SheetsToMaze
{
    [CreateAssetMenu(menuName = "Charly/SymbolAssetLinks", order = 0)]
    public class SymbolAssetLink : ScriptableObject
    {
        [SerializeField] public List<SymbolAsset> Links;

        public GameObject VisualFromName(string cellName)
        {
            foreach (var link in Links)
            {
                if (link.Name == cellName)
                    return link.Visual;
            }
            return null;
        }
    }

    [Serializable]
    public class SymbolAsset
    {
        public string Name;
        public GameObject Visual;
    }
}