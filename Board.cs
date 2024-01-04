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
        public TileType[,] Tile { get; private set; }  // 배열
        public int Size { get; private set; }

        Player _player;

        public enum TileType
        {
            Empty,
            Wall,
        }

        public void Initialize(int size, Player player)
        {
            if (size % 2 == 0)  // 벽으로 싸여있기 위해서는 홀수여야함.
                return;

            _player = player;

            Tile = new TileType[size, size];
            Size = size;

            // Mazes for Programmers 책
            //GenerateByBinaryTree();
            GenerateBySideWinder();

        }

        void GenerateByBinaryTree()
        {
            // Mazes for Programmers
            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    if (x % 2 == 0 || y % 2 == 0)
                        Tile[y, x] = TileType.Wall;
                    else
                        Tile[y, x] = TileType.Empty;

                }
            }

            // 랜덤으로 우측 혹은 아래로 길을 뚫는 작업
            // Binary Tree Algorithm
            Random rand = new Random();
            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    if (x % 2 == 0 || y % 2 == 0)
                        continue;

                    if (y == Size - 2 && x == Size - 2)
                        continue;

                    if (y == Size - 2) // 아래 외각 바로 전에 위치할 때는
                    {
                        Tile[y, x + 1] = TileType.Empty;   // 무조건 우측으로 가도록 함.
                        continue;
                    }

                    if (x == Size - 2) // 오른쪽 외각 바로 전에 위치할 때는
                    {
                        Tile[y + 1, x] = TileType.Empty;   // 무조건 아래측으로 가도록 함.
                        continue;
                    }

                    if (rand.Next(0, 2) == 0)   // 0~1
                    {
                        Tile[y, x + 1] = TileType.Empty;   // 우측
                    }
                    else
                    {
                        Tile[y + 1, x] = TileType.Empty;   // 아래측

                    }

                }
            }
        }

        void GenerateBySideWinder()
        {
            // 먼저 길을 다 막아버린다.
            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    if (x % 2 == 0 || y % 2 == 0)
                        Tile[y, x] = TileType.Wall;
                    else
                        Tile[y, x] = TileType.Empty;

                }
            }

            // 랜덤으로 우측 혹은 아래로 길을 뚫는 작업
            // Binary Tree Algorithm
            Random rand = new Random();
            for (int y = 0; y < Size; y++)
            {
                int count = 1;  // 연속된 점의 개수

                for (int x = 0; x < Size; x++)
                {
                    if (x % 2 == 0 || y % 2 == 0)
                        continue;


                    if (y == Size - 2 && x == Size - 2)
                        continue;

                    if (y == Size - 2) // 아래 외각 바로 전에 위치할 때는
                    {
                        Tile[y, x + 1] = TileType.Empty;   // 무조건 우측으로 가도록 함.
                        continue;
                    }

                    if (x == Size - 2) // 오른쪽 외각 바로 전에 위치할 때는
                    {
                        Tile[y + 1, x] = TileType.Empty;   // 무조건 아래측으로 가도록 함.
                        continue;
                    }



                    // 연속해서 뚫린 길 중 하나를 선택해서 길을 뚫는다.


                    if (rand.Next(0, 2) == 0)   // 0~1
                    {
                        Tile[y, x + 1] = TileType.Empty;   // 우측
                        count++;
                    }
                    else
                    {
                        int randomIndex = rand.Next(0, count);
                        Tile[y + 1, x - randomIndex * 2] = TileType.Empty;   // 아래측 *2를 해주는 이유는 2칸에 1칸이 벽이기 때문
                        count = 1; // 다시 초기화

                    }

                }
            }
        }


        public void Render()
        {
            ConsoleColor prevColor = Console.ForegroundColor;   // 초기 색상 저장

            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    // 플레이어 좌표를 가져 와서, 그 좌표랑 현재 y, x가 일치하면 플레이어 전용 색상으로 표시.
                    if (x == _player.PosX && y == _player.PosY)
                        Console.ForegroundColor = ConsoleColor.Blue;
                    else
                        Console.ForegroundColor = GetTileColor(Tile[y, x]);


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
