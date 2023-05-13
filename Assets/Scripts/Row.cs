using System;
using System.Collections.Generic;
using UnityEngine;
using Views;

public class Row : MonoBehaviour
{
    [SerializeField] private CellView[] _cellViews;

    public IReadOnlyCollection<CellView> CellViews => _cellViews;

    public void InitializeCells(Action<int, int> onClicked)
    {
        foreach (var cellView in _cellViews)
            cellView.OnButtonClicked += onClicked;
    }
}