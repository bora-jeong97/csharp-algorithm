using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// 배열, 동적배열, 연결리스트 비교
namespace Algorithm
{

    // 연결리스트 구현 
    class Board
    {
        const char CIRCLE = '\u25cf';
        public TileType[,] _tile;   // 배열
        public int _size;

        public enum TileType
        {
            Empty,
            Wall,
        }

        public void Initialize(int size)
        {
            _tile = new TileType[size, size];
            _size = size;

            for (int y = 0; y < _size; y++)
            {
                for (int x = 0; x < _size; x++)
                {
                    if (x == 0 || x == _size - 1 || y == 0 || y == _size - 1)   // 외각인경우
                        _tile[y, x] = TileType.Wall;
                    else
                        _tile[y, x] = TileType.Empty;

                }
            }
        }
    
        public void Render()
        {
            ConsoleColor prevColor = Console.ForegroundColor;   // 초기 색상 저장

            for (int y = 0; y < _size; y++)
            {
                for (int x = 0; x < _size; x++)
                {
                    Console.ForegroundColor = GetTileColor(_tile[y, x]);
                    
                    Console.Write(CIRCLE);
                    Console.Write(' ');
                }
                Console.WriteLine();
            }

            Console.ForegroundColor = prevColor;    // 초기 색상 되돌림
        }
    
        ConsoleColor GetTileColor(TileType type)
        {
            switch (type)
            {
                case TileType.Empty:
                    return ConsoleColor.Green;
                case TileType.Wall:
                    return ConsoleColor.Red;
                default:
                    return ConsoleColor.Green;

            }
        }
    }
}
