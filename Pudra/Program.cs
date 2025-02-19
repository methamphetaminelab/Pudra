using System.Numerics;
using System.Runtime.InteropServices;
using Swed64;

namespace Pudra
{
    public class Program
    {
        // globals
        public static Player[] players = { };
        public static Player localPlayer = new Player();
        [DllImport("user32.dll")]
        static extern short GetAsyncKeyState(int vKey);
        public static void Main()
        {
            // renderer
            var renderer = new Renderer();
            var renderThread = new Thread(async () => await renderer.Start())
            {
                IsBackground = true
            };
            renderThread.Start();

            // modules and globals
            Swed game = new Swed("cs2");
            IntPtr client = game.GetModuleBase("client.dll");
            const int SPACEBAR = 0x20; // vKey
            Vector2 screenSize = renderer.screenSize;
            Console.CursorVisible = false;
            

            // main loop
            while (true)
            {
                
                
                (players, localPlayer) = Utils.getPlayers(game, client, screenSize);
                renderer.updateLocalPlayer(localPlayer);
                renderer.updatePlayers(players);

                if (renderer.isBhop && GetAsyncKeyState(SPACEBAR) < 0)
                {
                    Utils.makeBunny(game, client);
                }

                Thread.Sleep(1);
            }
        }
    }
}
