using System.Numerics;
using System.Runtime.InteropServices;
using Swed64;

namespace Pudra
{
    public class Program
    {
        // globals
        public static List<Player> players = new List<Player>();
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
            const int LALT = 0xA4; // vKey
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

                if (renderer.isAntiFlash)
                {
                    Utils.antiFlash(game, client);
                }

                if (renderer.isAim && GetAsyncKeyState(LALT) < 0)
                {
                    if (renderer.isAimByDistance)
                    {
                        Utils.aimByDistance(game, client, players, localPlayer, renderer.isAimTeam);
                    }
                }

                Thread.Sleep(1);
            }
        }
    }
}
