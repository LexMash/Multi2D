using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

namespace Multi2D
{
    public class PlayerControllDefinitionScreenView : MonoBehaviour
    { 
        [SerializeField] private Transform root;
        [SerializeField] private ControllDefinitionScreenResources configuration;

        private readonly Dictionary<string, InputDeviceDefinitionView> inputDeviceViews = new();
        private readonly Dictionary<string, (int row, int cell)> viewGridPositions = new();
        private readonly Dictionary<(int row, int cell), CellForElement> viewGridCells = new();
        private readonly Dictionary<int, RowOfCells> rowViews = new();
        private readonly Queue<int> freeRows = new();
        private ObjectPool<InputDeviceDefinitionView> inputDeviceViewPool;
        private int minRows;

        public void Initialize()
        {
            inputDeviceViewPool = 
                new ObjectPool<InputDeviceDefinitionView>(
                    createFunc: () => Instantiate(configuration.DefinitionPrefab, root),
                    actionOnRelease: (instance) => instance.Hide());
        }

        public void BuildView(int minInputs, int maxInputs)
        {
            CleanUp();
            
            minRows = minInputs;
            var maxRows = maxInputs;
            var cells = minInputs + 1;

            for(int i = 0; i < maxRows; i++)
            {
                RowOfCells row = Instantiate(configuration.RowPrefab, root);
                row.transform.SetSiblingIndex(i);
                rowViews.Add(i, row);
                
                for (int j = 0; j < cells; j++)
                {
                    CellForElement cell = Instantiate(configuration.CellPrefab, row.transform);
                    viewGridCells.Add((i, j), cell);
                }

                if (i > minInputs)
                    row.Hide();

                freeRows.Enqueue(i);
            }
        }

        public void AddInputDeviceDefinition(PlayerInputEntryDefinition definition, InputDeviceType deviceType)
        {
            Sprite icon = configuration.InputDeviceTypesIcons.First(icon => icon.Type == deviceType).Icon;

            InputDeviceDefinitionView view = inputDeviceViewPool.Get();          
            view.Setup(icon, definition.DeviceName);

            int targetRow = freeRows.Dequeue();
            (int row, int cell) cellPosition = (row: targetRow, cell: definition.PlayerNumber);
            viewGridPositions.Add(definition.DeviceName, cellPosition);
            CellForElement cell = viewGridCells[cellPosition];
            cell.SetElement(view.transform);

            inputDeviceViews.Add(definition.DeviceName, view);

            if (targetRow > minRows)
            {
                RowOfCells rowView = rowViews[targetRow];             
                rowView.Show();
                rowView.transform.SetAsLastSibling();
            }              
        }

        public void RemoveInputDevice(string deviceName)
        {
            InputDeviceDefinitionView view = inputDeviceViews[deviceName];           
            inputDeviceViewPool.Release(view);
            inputDeviceViews.Remove(deviceName);
            var row = viewGridPositions[deviceName].row;
            RowOfCells rowView = rowViews[row];

            if (row > minRows)
                rowView.Hide();

            freeRows.Enqueue(row);
        }

        public void UpdateDeviceDefinition(PlayerInputEntryDefinition definition)
        {
            string deviceName = definition.DeviceName;
            (int row, int cell) cellPosition = viewGridPositions[deviceName];
            CellForElement cell = viewGridCells[(cellPosition.row, definition.PlayerNumber)];

            InputDeviceDefinitionView view = inputDeviceViews[deviceName];
            view.Define(definition.IsDefined);
            cell.SetElement(view.transform);
        }

        private void CleanUp()
        {
            foreach (var view in inputDeviceViews.Values)
                inputDeviceViewPool.Release(view);
                
            CleanUpCollections();
        }

        private void CleanUpCollections()
        {
            inputDeviceViews.Clear();
            viewGridPositions.Clear();
            viewGridCells.Clear();
            rowViews.Clear();
            freeRows.Clear();
        }

        private void OnDestroy()
        {
            CleanUpCollections();
            
            inputDeviceViewPool.Clear();
            inputDeviceViewPool.Dispose();
        }
    }
}
