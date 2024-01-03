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
            board.Initialize();

            Console.CursorVisible = false; // 커서 안보이게
            const char CIRCLE = '\u25cf';


            const int WAIT_TICK = 1000 / 30; // 경과한 시간이 1/30초보다 작다면 1초가 1000ms

            int lastTick = 0;
            while (true)
            {
                #region 프레임 관리
                // FPS 프레임( 60프레임이 pc기본 30프레임 이하로 No)
                int currentTick = System.Environment.TickCount;
                if (currentTick - lastTick < WAIT_TICK)
                    continue;
                lastTick = currentTick;
                #endregion

                // 입력

                // 로직

                // 렌더링
                Console.SetCursorPosition(0, 0);

                for (int i = 0; i < 25; i++)
                {
                    for (int j = 0; j < 25; j++)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(CIRCLE);
                        Console.Write(' ');
                    }
                    Console.WriteLine();
                }
            }

        }
    }
}
