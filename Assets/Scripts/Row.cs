using System;
using Extensions;
using Model.Candy;
using UnityEngine;
using Views;

namespace DefaultNamespace
{
    public class Row : MonoBehaviour
    {
        [SerializeField] private CellView[] _cellViews;

        public CellView[] CellViews => _cellViews;

        public void InitializeCells(Action<int, int> onClicked)
        {
            foreach (var cellView in _cellViews)
                cellView.Button.onClick.AddListener(() => onClicked.Invoke(cellView.X, cellView.Y));
        }
    }
}