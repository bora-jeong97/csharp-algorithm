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

            BFS();
        }

        void BFS()
        {
            int[] deltaY = new int[] { -1, 0, 1, 0 };   // Dir방향을 기준으로 위왼아오
            int[] deltaX = new int[] { 0, -1, 0, 1 };

            bool[,] found = new bool[_board.Size, _board.Size]; // 좌표
            Pos[,] parent = new Pos[_board.Size, _board.Size];

            Queue<Pos> q = new Queue<Pos>(); // 선입 선출
            q.Enqueue(new Pos(PosY, PosX));  // 초기 시작점을 넣어준다.
            found[PosY, PosX] = true;        // 방문 체크
            parent[PosY, PosX] = new Pos(PosY, PosX);

            while (q.Count > 0)
            {
                // 맨 앞 제거 
                Pos pos = q.Dequeue();  
                int nowY = pos.Y;
                int nowX = pos.X;

                for (int i = 0; i < 4; i++) // 4 방향
                {
                    // next 좌표 뽑기 
                    int nextY = nowY + deltaY[i];   
                    int nextX = nowX + deltaX[i];


                    if (nextX < 0 || nextX >= _board.Size || nextY < 0 || nextY >= _board.Size)
                        continue;
                    // a.인접하지 않았으면 스킵
                    if (_board.Tile[nextY, nextX] == Board.TileType.Wall)
                        continue;
                    // b.이미 발견한 애라면 스킵
                    if (found[nextY, nextX])
                        continue;

                    // 맨 뒤 추가
                    q.Enqueue(new Pos(nextY, nextX));
                    found[nextY, nextX] = true;
                    parent[nextY, nextX] = new Pos(nowY, nowX);

                }
            }

            // 목적지 체크
            int y = _board.DestY;
            int x = _board.DestX;
            while(parent[y, x].Y != y || parent[y, x].X != x)   // 자식과 부모의 좌표가 같을 때까지 == 시작점까지 되짚어가기
            {
                _points.Add(new Pos(y, x)); // 내 좌표
                Pos pos = parent[y, x]; // 부모 좌표
                y = pos.Y;
                x = pos.X;
            }
            _points.Add(new Pos(y, x)); // 첫 좌표 넣어줌.
            _points.Reverse(); // 역으로 바꿔줌.

        }

        // 우수법
        void RightHand()
        {
            // 현재 바라보고 있는 방향을 기준으로, 좌표 변화를 나타낸다.
            int[] frontY = new int[] { -1, 0, 1, 0 };
            int[] frontX = new int[] { 0, -1, 0, 1 };
            int[] rightY = new int[] { 0, -1, 0, 1 }; // 오른쪽을 바라보고 있을 때 Y측 Dir
            int[] rightX = new int[] { 1, 0, -1, 0 }; // 오른쪽을 바라보고 있을 때 X츨 Dir

            _points.Add(new Pos(PosY, PosX));


            // 목적지 도착하기 전에는 계속 실행
            while (PosY != _board.DestY || PosX != _board.DestX)
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


        const int MOVE_TICK = 100; // 0.1초 100ms
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
