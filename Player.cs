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

            AStar();
        }

        struct PQNode : IComparable<PQNode>
        {
            public int F;
            public int G;
            public int Y;
            public int X;

            public int CompareTo(PQNode other)
            {
                if (F == other.F)
                    return 0;
                return F < other.F ? 1 : -1; // 작으면 1 
            }
        }

        
        void AStar()
        {
            // U L D R + UL DL DR UR(대각선)
            int[] deltaY = new int[] { -1, 0, 1, 0, -1, 1, 1, -1 };   // Dir방향을 기준으로 위왼아오
            int[] deltaX = new int[] { 0, -1, 0, 1, -1, -1, 1, 1 };
            int[] cost = new int[] { 10, 10, 10, 10, 14, 14, 14, 14 };

            // 점수 매기기
            // F = G + H
            // F = 최종 점수 (작을 수록 좋음. 경로에 따라 달라짐)
            // G = 시작점에서 해당 좌표까지 이동하는데 드는 비용 (작을 수록 좋음. 경로에 따라서 달라짐)
            // H = 목적지에서 얼마나 가까운지(작을 수록 좋음, 고정)

            // (y, x) 이미 방문했는지 여부(방문 = closed 상태)
            bool[,] closed = new bool[_board.Size, _board.Size]; // CloseList

            // (y, x) 가는 길을 한 번이라도 발견했는지
            // 발견X => MaxValue
            // 발견O => F = G + H
            int[,] open = new int[_board.Size, _board.Size]; // OpenList
            for (int y = 0; y < _board.Size; y++)
                for (int x = 0; x < _board.Size; x++)
                    open[y, x] = Int32.MaxValue;

            // 부모 주소를 입력하여 경로 추적
            Pos[,] parent = new Pos[_board.Size, _board.Size];


            // PriorityQueue<>  // 가장 최상의 결과를 상단에 뽑는데 최적화
            // 오픈 리스트에 있는 정보들 중에서, 가장 좋은 후보를 빠르게 뽑아오기 위한 도구
            PriorityQueue<PQNode> pq = new PriorityQueue<PQNode>();

            // 시작점 발견 (예약 진행)
            open[PosY, PosX] = 10 * (Math.Abs(_board.DestY - PosY) + Math.Abs(_board.DestX - PosX) );   // F
            pq.Push(new PQNode() { F = 10 * (Math.Abs(_board.DestY - PosY) + Math.Abs(_board.DestX - PosX)), G = 0, Y = PosY, X = PosX });
            parent[PosY, PosX] = new Pos(PosY, PosX); // 시작점은 부모 위치에 자신을 넣어준다.

            while (pq.Count > 0)
            {
                // 제일 좋은 후보를 찾는다
                PQNode node = pq.Pop();
                // 동일한 죄표를 여러 경로로 찾아서, 더 빠른 경로로 인해서 이미 방문(closed)된 경우 스킵
                if (closed[node.Y, node.X])
                    continue;
                // 방문한다.
                closed[node.Y, node.X] = true;
                // 목적지 도착했으면 바로 종료
                if (node.Y == _board.DestY && node.X == _board.DestX)
                    break;

                // 상하좌우 등 이동할 수 있는 좌표인지 확인해서 예약(open)한다.
                for (int i = 0; i < deltaY.Length; i++)
                {
                    // next 좌표 뽑기 
                    int nextY = node.Y + deltaY[i];
                    int nextX = node.X + deltaX[i];

                    // 유효 범위를 벗어났으면 스킵
                    if (nextX < 0 || nextX >= _board.Size || nextY < 0 || nextY >= _board.Size)
                        continue;
                    // a.벽으로 막혀서 갈 수 없으면 스킵
                    if (_board.Tile[nextY, nextX] == Board.TileType.Wall)
                        continue;
                    // b.이미 방문한 곳이면 스킵
                    if (closed[nextY, nextX])
                        continue;

                    // 비용 계산
                    int g = node.G + cost[i]; // 이동에 쓰이는 비용
                    int h = 10 * (Math.Abs(_board.DestY - nextY) + Math.Abs(_board.DestX - nextX));
                    // 다른 경로에서 더 빠른 길 이미 찾았으면 스킵
                    if (open[nextY, nextX] < g + h)
                        continue;

                    // 예약 진행
                    open[nextY, nextX] = g + h;
                    pq.Push(new PQNode() { F = g + h, G = g, Y = nextY, X = nextX });
                    parent[nextY, nextX] = new Pos(node.Y, node.X); // 나를 가장 빠른 경로로 인도한 부모 노드 정보를 저장
                }
            }

            CalcPathFromParent(parent);
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
            CalcPathFromParent(parent);
        }


        void CalcPathFromParent(Pos[,] parent)
        {
            // 목적지 체크
            int y = _board.DestY;
            int x = _board.DestX;
            while (parent[y, x].Y != y || parent[y, x].X != x)   // 자식과 부모의 좌표가 같을 때까지 == 시작점까지 되짚어가기
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


        const int MOVE_TICK = 30; // 0.1초 100ms
        int _sumTick = 0;
        int _lastIndex = 0;
        public void Update(int deltaTick)   // deltaTick : 이전 시간과 현재 시간의 차이
        {
            if (_lastIndex >= _points.Count)    // 도착
            {   // 끝까지 도달 시 다시 재시동 하도록 만듬
                _lastIndex = 0;
                _points.Clear();
                _board.Initialize(_board.Size, this); 
                Initialize(1, 1, _board);

            }    
                // return;  // 최종


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
