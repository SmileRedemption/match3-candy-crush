using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Extensions;
using Model.Candy;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.UI;
using Views;

namespace DefaultNamespace
{
    public class GameField : MonoBehaviour
    {
        [SerializeField] private Row[] _rows;
        [SerializeField] private CandyData[] _candiesData;
        
        private CellView[,] _board;
        private CellView _selectedCell;
        private CellView _nextSelectedCell;

        private int RowLength => _board.GetLength(0);
        private int ColumnLength => _board.GetLength(1);
        
        public void Initialize()
        {
            InitializeBoard();
            SetCandyInBoard();
        }

        private void InitializeBoard()
        {
            _board = new CellView[_rows.Length, _rows[0].CellViews.Length];

            for (int i = 0; i < RowLength; i++)
            {
                for (int j = 0; j < ColumnLength; j++)
                {
                    _board[i, j] = _rows[i].CellViews[j];
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
                  boardWithoutMatches = CheckHorizontal(true) != true && CheckVertical(true) != true;

            foreach (var row in _rows)
                row.InitializeCells(OnSelectCell);
        }

        private bool CheckHorizontal(bool isInitialize = false)
        {
            bool matchesFound = false;

            for (int row = 0; row < RowLength; row++)
            {
                int col = 0;

                while (col < ColumnLength)
                {
                    var currentPiece = _board[row, col].Sprite;

                    if (currentPiece != null)
                    {
                        int matchLength = 1;
                        int nextCol = col + 1;

                        while (nextCol < ColumnLength && _board[row, nextCol].Sprite == currentPiece)
                        {
                            matchLength++;
                            nextCol++;
                        }

                        if (matchLength >= 3)
                        {
                            matchesFound = true;
                            DestroyHorizontalMatches(row, col, matchLength, isInitialize);
                        }

                        col = nextCol;
                    }
                    else
                    {
                        col++;
                    }
                }
            }
            
            return matchesFound;
        }

        private bool CheckVertical(bool isInitialize = false)
        {
            bool matchesFound = false;

            for (int col = 0; col < ColumnLength; col++)
            {
                int row = 0;

                while (row < RowLength)
                {
                    var currentPiece = _board[row, col].Sprite;

                    if (currentPiece != null)
                    {
                        int matchLength = 1;
                        int nextRow = row + 1;

                        while (nextRow < RowLength && _board[nextRow, col].Sprite == currentPiece)
                        {
                            matchLength++;
                            nextRow++;
                        }

                        if (matchLength >= 3)
                        {
                            matchesFound = true;
                            DestroyVerticalMatches(row, col, matchLength, isInitialize);
                        }

                        row = nextRow;
                    }
                    else
                    {
                        row++;
                    }
                }
            }

            return matchesFound;
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
                
                AnimateDestroy(cellView.transform,
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
                
                AnimateDestroy(cellView.transform,
                    () => cellView.ChangeSprite(_candiesData.PickRandom().Sprite));
            }
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

            AnimateSpriteChange(_selectedCell.Candy, nextSelectedCellSprite, 0.2f);
            AnimateSpriteChange(_nextSelectedCell.Candy, selectedCellSprite, 0.2f, () => StartCoroutine(OnSwappingEnd()));
        }

        private IEnumerator OnSwappingEnd()
        {
            bool boardWithoutMatches = false;

            while (boardWithoutMatches == false)
            {
                boardWithoutMatches = CheckHorizontal() != true && CheckVertical() != true;
                yield return new WaitForSeconds(0.9f);
            }
            
            _selectedCell = null;
            _nextSelectedCell = null;
        }

        private void AnimateDestroy(Transform element, Action onCallback)
        {
            Sequence sequence = DOTween.Sequence();

            var normalScale = element.transform.localScale;
            sequence.Append(element.DOScale(Vector3.zero, 0.5f));
            sequence.AppendCallback(onCallback.Invoke);
            sequence.Append(element.DOScale(normalScale, 0.5f));

            sequence.Play()
                .OnComplete(() => sequence.Kill());
        }

        private void AnimateSpriteChange(Image image, Sprite newSprite, float animationDuration, Action onAction = null)
        {
            Sequence sequence = DOTween.Sequence();

            sequence.Append(image.DOFade(0f, animationDuration / 2f));

            sequence.AppendCallback(() => image.sprite = newSprite);

            sequence.Append(image.DOFade(1f, animationDuration / 2f));

            if (onAction != null)
                sequence.AppendCallback(onAction.Invoke);

            sequence.Play().OnComplete(() => sequence.Kill());
        }
    }
}