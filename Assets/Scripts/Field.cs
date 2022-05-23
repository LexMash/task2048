using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

public class Field
{
    private bool _isMoved;

    public int size { get; private set; }

    private Tile[,] field;

    public Field(int size, Tile[] tiles)
    {
        this.size = size;

        field = new Tile[size, size];

        //перевод из одномерного массива в двумерный
        for (int i = 0, c = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++, c++)
            {
                if (c == tiles.Length) break ;
                field[i, j] = tiles[c];
            }
        }
    }

    public int GetTileValue(int row, int column)
    {
        if (OnField(row, column))
        {
            return field[row, column].Value;
        }
        else return -1;
    }

    public void Refill(Field field)
    {
        for (int row = 0; row < field.size; row++)
        {
            for (int col = 0; col < field.size; col++)
            {
                field.SetTileValue(row, col, 0);
            }
        }

        field.GetRandomTile(field);
        field.GetRandomTile(field);
    }

    public bool CheckField(Field field)
    {
        for (int row = 0; row < field.size; row++)
        {
            for (int col = 0; col < field.size; col++)
            {
                if (field.GetTileValue(row, col) == 0)
                {
                    return false;
                }

                if (field.GetTileValue(row, col) == field.GetTileValue(row, col + 1)
                    || field.GetTileValue(row, col) == field.GetTileValue(row + 1, col))
                {
                    return false;
                }
            }
        }

        return true;
    }

    private void SetTileValue(int row, int column, int value)
    {
        if (OnField(row, column))
        {
            field[row, column].SetValue(value); 
        }
    }

    private bool OnField(int row, int column)
    {
        return row >= 0 && column >= 0 && row < size && column < size;
    }

    private void GetRandomTile(Field field)
    {
        List<Tile> zeroTiles = new List<Tile>();

        for (int row = 0; row < field.size; row++)
        {
            for (int col = 0; col < field.size; col++)
            {
                if (field.GetTileValue(row, col) == 0)
                {
                    zeroTiles.Add(field.GetTile(row, col));
                }
            }
        }

        var randomTile = zeroTiles[Random.Range(0, zeroTiles.Count)];

        randomTile.SetValue(SetRandomValue());
    }

    private Tile GetTile(int row, int column)
    {
        if (OnField(row, column))
        {
            return field[row, column];
        }
        else return null;
    }

    private int SetRandomValue()
    {
        var random = Random.Range(0, 100);

        return random <= 89 ? 2 : 4;
    }

    private void PushAll(Field field,int row, int column, int rowStep, int colStep)
    {
        if (field.GetTileValue(row, column) > 0)
        {
            while (field.GetTileValue(row + rowStep, column + colStep) == 0)
            {
                field.SetTileValue(row + rowStep, column + colStep, field.GetTileValue(row, column));
                field.SetTileValue(row, column, 0);

                row += rowStep;
                column += colStep;

                _isMoved = true;
            }
        }
    }

    public Action<bool> IsWon;
    public Action<int> ScoreAwarded;

    private void Join(Field field,int row, int column, int rowStep, int colStep)
    {
        if (field.GetTileValue(row, column) > 0)
        {
            while (field.GetTileValue(row + rowStep, column + colStep) == field.GetTileValue(row, column))
            {
                field.SetTileValue(row + rowStep, column + colStep, field.GetTileValue(row, column) * 2);

                //начисление очков
                ScoreAwarded?.Invoke(field.GetTileValue(row + rowStep, column + colStep));
                
                //проверка на победу
                if (field.GetTileValue(row + rowStep, column + colStep) == 2048)
                {
                    IsWon?.Invoke(true);
                }

                while (field.GetTileValue(row - rowStep, column - colStep) > 0)
                {
                    field.SetTileValue(row, column, field.GetTileValue(row - rowStep, column - colStep));
                    row -= rowStep;
                    column -= colStep;
                }

                field.SetTileValue(row, column, 0);

                _isMoved = true;
            }
        }
    }

    public void MoveUp(Field field)
    {
        _isMoved = false;

        for (int col = 0; col < field.size; col++)
        {
            for (int row = 1; row < field.size; row++)
            {
                PushAll(field, row, col, -1, 0);
            }

            for (int row = 1; row < field.size; row++)
            {
                Join(field, row, col, -1, 0);
            }
        }

        if (_isMoved)
        {
            field.GetRandomTile(field);
        }
    }

    public void MoveDown(Field field)
    {
        _isMoved = false;

        for (int col = 0; col < field.size; col++)
        {
            for (int row = field.size - 2; row >= 0; row--)
            {
                PushAll(field, row, col, 1, 0);
            }

            for (int row = field.size - 2; row >= 0; row--)
            {
                Join(field, row, col, 1, 0);
            }
        }

        if (_isMoved)
        {
            field.GetRandomTile(field);
        }
    }

    public void MoveRight(Field field)
    {
        _isMoved = false;

        for (int row = 0; row < field.size; row++)
        {
            for (int col = field.size - 2; col >= 0; col--)
            {
                PushAll(field, row, col, 0, 1);
            }

            for (int col = field.size - 2; col >= 0; col--)
            {
                Join(field, row, col, 0, 1);
            }
        }

        if (_isMoved)
        {
            field.GetRandomTile(field);
        }
    }

    public void MoveLeft(Field field)
    {
        _isMoved = false;

        for (int row = 0; row < field.size; row++)
        {
            for (int col = 1; col < field.size; col++)
            {
                PushAll(field, row, col, 0, -1);
            }

            for (int col = 1; col < field.size; col++)
            {
                Join(field, row, col, 0, -1);
            }
        }

        if (_isMoved)
        {
            field.GetRandomTile(field);
        }
    }
}
