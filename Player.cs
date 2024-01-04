using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm
{
    class Pos
    {
        public Pos(int y, int x) { Y = y; X = x; }
        public int Y;
        public int X;
    }


    class Player
    {
        public int PosX { get; private set; }
        public int PosY { get; private set; }
        Random _random = new Random();

        Board _board;

        enum Dir    // 반시계 방향
        {               // 오른쪽방향 // 왼쪽 방향
            Up = 0,     // 3 % 4 = 3 // 5 % 4 = 1
            Left = 1,   // 4 % 4 = 0 // 6 % 4 = 2
            Down = 2,   // 5 % 4 = 1 // 7 % 4 = 3
            Right = 3,  // 6 % 4 = 2 // 8 % 4 = 0
        }


        int _dir = (int)Dir.Up;
        List<Pos> _points = new List<Pos>();

        public void Initialize(int posY, int posX, Board board)  // 초기 좌표
        {
            PosY = posY;
            PosX = posX;
            _board = board; // 내부에서 저장

            // 현재 바라보고 있는 방향을 기준으로, 좌표 변화를 나타낸다.
            int[] frontY = new int[] { -1, 0, 1, 0 };
            int[] frontX = new int[] { 0, -1, 0, 1 };
            int[] rightY = new int[] { 0, -1, 0, 1 }; // 오른쪽을 바라보고 있을 때 Y측 Dir
            int[] rightX = new int[] { 1, 0, -1, 0 }; // 오른쪽을 바라보고 있을 때 X츨 Dir

            _points.Add(new Pos(PosY, PosX));


            // 목적지 도착하기 전에는 계속 실행
            while (PosY != board.DestY || PosX != board.DestX)
            {
                // 우수법
                // 1. 현재 바라보는 방향을 기준으로 오른쪽으로 갈 수 있는 지 확인.
                if (_board.Tile[PosY + rightY[_dir], PosX + rightX[_dir]] == Board.TileType.Empty)
                {
                    // 오른쪽 방향으로 90도 회전
                    _dir = (_dir - 1 + 4) % 4;

                    // 앞으로 한 보 전진
                    PosX = PosX + frontX[_dir];
                    PosY = PosY + frontY[_dir];
                    _points.Add(new Pos(PosY, PosX));

                }
                // 2. 현재 바라보는 방향을 기준으로 전진할 수 있는지 확인.
                else if (_board.Tile[PosY + frontY[_dir], PosX + frontX[_dir]] == Board.TileType.Empty) 
                {
                    // 앞으로 한 보 전진
                    PosX = PosX + frontX[_dir];
                    PosY = PosY + frontY[_dir];
                    _points.Add(new Pos(PosY, PosX));
                }
                else
                {
                    // 왼쪽 방향으로 90도 회전 전진은 안함. 몸을 돌려 빈공간을 찾기 위함
                    _dir = (_dir + 1 + 4) % 4;

                }

            }
        }


        const int MOVE_TICK = 10; // 0.1초 100ms
        int _sumTick = 0;
        int _lastIndex = 0;
        public void Update(int deltaTick)   // deltaTick : 이전 시간과 현재 시간의 차이
        {
            if (_lastIndex >= _points.Count)
                return;

            _sumTick += deltaTick;
            if(_sumTick > MOVE_TICK)
            {
                _sumTick = 0;

                PosY = _points[_lastIndex].Y;
                PosX = _points[_lastIndex].X;
                _lastIndex++;
            }
        }
    }
}
