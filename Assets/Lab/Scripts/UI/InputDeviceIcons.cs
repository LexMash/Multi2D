
using System;
using UnityEngine;

namespace Multi2D
{
    [CreateAssetMenu(fileName = "ControllDefinitionScreenResources", menuName = "Input/ControllDefinitionScreenResources")]
    public class ControllDefinitionScreenResources : ScriptableObject
    {
        [Serializable]
        public struct InputDeviceTypesIcon
        {
            public InputDeviceType Type;
            public Sprite Icon;
        }

        public RowOfCells RowPrefab;
        public CellForElement CellPrefab;
        public InputDeviceDefinitionView DefinitionPrefab;
        public InputDeviceTypesIcon[] InputDeviceTypesIcons;
    }
}
