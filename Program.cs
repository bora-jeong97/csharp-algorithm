using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            Board board = new Board();
            Player player = new Player();
            board.Initialize(25, player);
            player.Initialize(1, 1, board);

            Console.CursorVisible = false; // 커서 안보이게
            const int WAIT_TICK = 1000 / 30; // 경과한 시간이 1/30초보다 작다면 1초가 1000ms

            int lastTick = 0;
            while (true)
            {
                #region 프레임 관리
                // 만약에 경과한 시간이 1/30초보다 작다면
                int currentTick = System.Environment.TickCount;
                if (currentTick - lastTick < WAIT_TICK)
                    continue;
                int deltaTick = currentTick - lastTick;
                lastTick = currentTick;
                #endregion

                // 입력

                // 로직(데이터가 변하는 부분)
                player.Update(deltaTick);


                // 렌더링
                Console.SetCursorPosition(0, 0);
                board.Render();


            }

        }
    }
}
