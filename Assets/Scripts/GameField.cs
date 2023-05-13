using System;
using System.Collections;
using System.Linq;
using Animations;
using Extensions;
using Model.Candy;
using UnityEngine;
using Views;

public class GameField : MonoBehaviour
{
    [SerializeField] private Row[] _rows;
    [SerializeField] private CandyData[] _candiesData;
        
    private CellView[,] _board;
    
    private CellView _selectedCell;
    private CellView _nextSelectedCell;

    private readonly CandyAnimation _candyAnimation = new CandyAnimation();
    private MatchChecker _matchChecker;

    private int RowLength => _board.GetLength(0);
    private int ColumnLength => _board.GetLength(1);

    public void Initialize()
    {
        InitializeBoard();
        InitializeMatchChecker();
        SetCandyInBoard();
    }

    private void OnDestroy()
    {
        _matchChecker.DestroyedHorizontal -= DestroyHorizontalMatches;
        _matchChecker.DestroyedVertical -= DestroyVerticalMatches;
    }

    private void InitializeBoard()
    {
        _board = new CellView[_rows.Length, _rows[0].CellViews.Count];

        for (int i = 0; i < RowLength; i++)
        {
            for (int j = 0; j < ColumnLength; j++)
            {
                _board[i, j] = _rows[i].CellViews.ElementAt(j);
            }
        }
    }

    private void SetCandyInBoard()
    {
        for (int i = 0; i < RowLength; i++)
        {
            for (int j = 0; j < ColumnLength; j++)
            {
                var candySprite = _candiesData.PickRandom().Sprite;
                _board[i, j].InitializeCell(candySprite, i, j);
            }
        }

        var boardWithoutMatches = false;

        while (boardWithoutMatches == false)
            boardWithoutMatches = _matchChecker.CheckHorizontal(true) != true && _matchChecker.CheckVertical(true) != true;

        foreach (var row in _rows)
            row.InitializeCells(OnSelectCell);
    }
    
    private void InitializeMatchChecker()
    {
        _matchChecker = new MatchChecker(_board, RowLength, ColumnLength);
        _matchChecker.DestroyedHorizontal += DestroyHorizontalMatches;
        _matchChecker.DestroyedVertical += DestroyVerticalMatches;
    }

    private void OnSelectCell(int x, int y)
    {
        if (_selectedCell == null )
        {
            _selectedCell = _board[x, y];
            _selectedCell.Select();
            return;
        }

        if (_selectedCell.X == x && _selectedCell.Y == y)
        {
            _selectedCell.UnSelect();
            _selectedCell = null;
            return;
        }

        if (_nextSelectedCell == null)
        {
            if (Math.Abs(x - _selectedCell.X) == 1 && Math.Abs(y - _selectedCell.Y) == 0
                || Math.Abs(y - _selectedCell.Y) == 1 && Math.Abs(x - _selectedCell.X) == 0){
                _nextSelectedCell = _board[x, y];
                _selectedCell.UnSelect();
                Swapping();
            }
        }
    }

    private void Swapping()
    {
        var selectedCellSprite = _selectedCell.Sprite;
        var nextSelectedCellSprite = _nextSelectedCell.Sprite;

        _candyAnimation.AnimateSpriteChange(_selectedCell.Candy, nextSelectedCellSprite, 0.2f);
        _candyAnimation.AnimateSpriteChange(_nextSelectedCell.Candy, selectedCellSprite, 0.2f, () => StartCoroutine(OnSwappingEnd()));
    }

    private IEnumerator OnSwappingEnd()
    {
        bool boardWithoutMatches = false;

        while (boardWithoutMatches == false)
        {
            boardWithoutMatches = _matchChecker.CheckHorizontal() != true && _matchChecker.CheckVertical() != true;
            yield return new WaitForSeconds(0.9f);
        }
            
        _selectedCell = null;
        _nextSelectedCell = null;
    }
    
    private void DestroyHorizontalMatches(int row, int col, int matchLength, bool isInitialize)
    {
        for (int j = col; j < col + matchLength; j++)
        {
            var cellView = _board[row, j];

            if (isInitialize)
            {
                cellView.ChangeSprite(_candiesData.PickRandom().Sprite);
                return;
            }

            _candyAnimation.AnimateDestroy(cellView.transform,
                () => cellView.ChangeSprite(_candiesData.PickRandom().Sprite));
        }
    }

    private void DestroyVerticalMatches(int row, int col, int matchLength, bool isInitialize)
    {
        for (int i = row; i < row + matchLength; i++)
        {
            var cellView = _board[i, col];

            if (isInitialize)
            {
                cellView.ChangeSprite(_candiesData.PickRandom().Sprite);
                return;
            }

            _candyAnimation.AnimateDestroy(cellView.transform,
                () => cellView.ChangeSprite(_candiesData.PickRandom().Sprite));
        }
    }
}