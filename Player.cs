using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm
{
    class Player
    {
        public int PosX { get; private set; }
        public int PosY { get; private set; }
        Random _random = new Random();

        Board _board;

        public void Initialize(int posY, int posX, int destY, int destX, Board board)  // 초기 좌표
        {
            PosY = posY;
            PosX = posX;

            _board = board; // 내부에서 저장
        }


        const int MOVE_TICK = 100; // 0.1초 100ms
        int _sumTick = 0;
        public void Update(int deltaTick)   // deltaTick : 이전 시간과 현재 시간의 차이
        {
            _sumTick += deltaTick;
            if(_sumTick > MOVE_TICK)
            {
                _sumTick = 0;

                // 0.1초마다 실행될 로직을 넣어준다.
                int randValue = _random.Next(0, 4);
                switch (randValue)
                {
                    case 0: // 상
                        if (PosY-1 >= 0 && _board.Tile[PosY - 1, PosX] == Board.TileType.Empty)    // 위쪽 타일이 빈칸이면 플레이어 좌표를 위로 바꾼다.
                            PosY = PosY - 1;
                        break;
                    case 1: // 하
                        if (PosY + 1 < _board.Size && _board.Tile[PosY + 1, PosX] == Board.TileType.Empty)   
                            PosY = PosY + 1;
                        break;
                    case 2: // 좌
                        if (PosX - 1 >= 0 && _board.Tile[PosY, PosX - 1] == Board.TileType.Empty)
                            PosX = PosX - 1;
                        break;
                    case 3: // 우
                        if (PosX + 1 < _board.Size && _board.Tile[PosY, PosX + 1] == Board.TileType.Empty)
                            PosX = PosX + 1;
                        break;
                }
            }
        }
    }
}
