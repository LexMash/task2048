using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

public class Field
{
    public Action<bool> IsWon;
    public Action<int> ScoreAwarded;

    private bool _isMoved;

    public int size { get; private set; }

    private Tile[,] field;

    public Field(int size, Tile[] tiles, TileConfig tileConfig)
    {
        this.size = size;

        field = new Tile[size, size];

        for (int i = 0, c = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++, c++)
            {
                field[i, j] = tiles[c];
                field[i, j].SetConfig(tileConfig);
            }
        }
    }

    public int GetTileValue(int row, int column)
    {
        if (IsOnField(row, column))
        {
            return field[row, column].Value;
        }
        else return -1;
    }

    public void Refill()
    {
        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                SetTileValue(row, col, 0);
            }
        }

        GetRandomTile();
        GetRandomTile();
    }

    public bool CheckField()
    {
        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                if (GetTileValue(row, col) == 0)
                {
                    return false;
                }

                if (GetTileValue(row, col) == GetTileValue(row, col + 1)
                    || GetTileValue(row, col) == GetTileValue(row + 1, col))
                {
                    return false;
                }
            }
        }

        return true;
    }

    private void SetTileValue(int row, int column, int value)
    {
        if (IsOnField(row, column))
        {
            field[row, column].SetValue(value); 
        }
    }

    private bool IsOnField(int row, int column)
    {
        return row >= 0 && column >= 0 && row < size && column < size;
    }

    private void GetRandomTile()
    {
        List<Tile> zeroTiles = new List<Tile>();

        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                if (GetTileValue(row, col) == 0)
                {
                    zeroTiles.Add(GetTile(row, col));
                }
            }
        }

        var randomTile = zeroTiles[Random.Range(0, zeroTiles.Count)];

        randomTile.SetValue(SetRandomValue());
    }

    private Tile GetTile(int row, int column)
    {
        if (IsOnField(row, column))
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

    private void PushAll(int row, int column, int rowStep, int colStep)
    {
        if (GetTileValue(row, column) > 0)
        {
            while (GetTileValue(row + rowStep, column + colStep) == 0)
            {
                SetTileValue(row + rowStep, column + colStep, GetTileValue(row, column));
                SetTileValue(row, column, 0);

                row += rowStep;
                column += colStep;

                _isMoved = true;
            }
        }
    }

    private void Join(int row, int column, int rowStep, int colStep)
    {
        if (GetTileValue(row, column) > 0)
        {
            while (GetTileValue(row + rowStep, column + colStep) == GetTileValue(row, column))
            {
                SetTileValue(row + rowStep, column + colStep, GetTileValue(row, column) * 2);

                ScoreAwarded?.Invoke(GetTileValue(row + rowStep, column + colStep));
                
                if (GetTileValue(row + rowStep, column + colStep) == 2048)
                {
                    IsWon?.Invoke(true);
                }

                while (GetTileValue(row - rowStep, column - colStep) > 0)
                {
                    SetTileValue(row, column, GetTileValue(row - rowStep, column - colStep));
                    row -= rowStep;
                    column -= colStep;
                }

                SetTileValue(row, column, 0);

                _isMoved = true;
            }
        }
    }

    public void MoveUp()
    {
        _isMoved = false;

        for (int col = 0; col < size; col++)
        {
            for (int row = 1; row < size; row++)
            {
                PushAll(row, col, -1, 0);
            }

            for (int row = 1; row < size; row++)
            {
                Join(row, col, -1, 0);
            }
        }

        if (_isMoved)
        {
            GetRandomTile();
        }
    }

    public void MoveDown()
    {
        _isMoved = false;

        for (int col = 0; col < size; col++)
        {
            for (int row = size - 2; row >= 0; row--)
            {
                PushAll(row, col, 1, 0);
            }

            for (int row = size - 2; row >= 0; row--)
            {
                Join(row, col, 1, 0);
            }
        }

        if (_isMoved)
        {
            GetRandomTile();
        }
    }

    public void MoveRight()
    {
        _isMoved = false;

        for (int row = 0; row < size; row++)
        {
            for (int col = size - 2; col >= 0; col--)
            {
                PushAll(row, col, 0, 1);
            }

            for (int col = size - 2; col >= 0; col--)
            {
                Join(row, col, 0, 1);
            }
        }

        if (_isMoved)
        {
            GetRandomTile();
        }
    }

    public void MoveLeft()
    {
        _isMoved = false;

        for (int row = 0; row < size; row++)
        {
            for (int col = 1; col < size; col++)
            {
                PushAll(row, col, 0, -1);
            }

            for (int col = 1; col < size; col++)
            {
                Join(row, col, 0, -1);
            }
        }

        if (_isMoved)
        {
            GetRandomTile();
        }
    }
}
