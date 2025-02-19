using System.Collections.Concurrent;
using System.Numerics;
using System.Runtime.InteropServices;
using ClickableTransparentOverlay;
using ImGuiNET;

namespace Pudra
{
    public class Renderer : Overlay
    {
        [DllImport("user32.dll")]
        static extern short GetAsyncKeyState(int vKey);
        private bool isShow = true;
        private bool wasMenuPressed = false;
        public bool isBhop = false;
        private bool isEsp = false;
        private bool isEspTeam = false;

        private bool isBoxEsp = false;
        private bool isLineEsp = false;
        private bool isNameEsp = false;
        private bool isHealthEsp = false;

        private Vector4 enemyColor = new Vector4(1, 0, 0, 1);
        private Vector4 teamColor = new Vector4(0, 1, 0, 1);
        public Vector2 screenSize = new Vector2(1920, 1080);

        private ConcurrentQueue<Player> players = new ConcurrentQueue<Player>();
        private Player localPlayer = new Player();
        private readonly object playerLock = new object();

        ImDrawListPtr drawList;

        protected override void Render()
        {
            bool isDeletePressed = GetAsyncKeyState(0x2E) < 0;
            if (isDeletePressed && !wasMenuPressed)
            {
                isShow = !isShow;
            }
            wasMenuPressed = isDeletePressed;

            if (isShow)
            {
                ImGui.Begin("Pudra");
                ImGui.Checkbox("ESP", ref isEsp);
                if (isEsp)
                {
                    ImGui.Checkbox("TEAM ESP", ref isEspTeam);
                    ImGui.Checkbox("BOX ESP", ref isBoxEsp);
                    ImGui.Checkbox("LINE ESP", ref isLineEsp);
                    ImGui.Checkbox("NAME ESP", ref isNameEsp);
                    ImGui.Checkbox("HEALTH ESP", ref isHealthEsp);
                }
                ImGui.Checkbox("BHOP", ref isBhop);

                ImGui.End();
            }

            drawOverlay(screenSize);
            drawList = ImGui.GetWindowDrawList();

            if (isEsp)
            {
                foreach (Player player in players)
                {
                    bool isEnemy = localPlayer.team != player.team;
                    if (playerOnScreen(player) && player.position != localPlayer.position && (isEnemy || isEspTeam))
                    {
                        float playerHeight = player.position2D.Y - player.viewPosition2D.Y;
                        Vector2 rectTop = new Vector2(player.viewPosition2D.X - playerHeight / 3, player.viewPosition2D.Y);
                        Vector2 rectBottom = new Vector2(player.position2D.X + playerHeight / 3, player.position2D.Y);
                        Vector4 color = localPlayer.team == player.team ? teamColor : enemyColor;
                        Vector4 nameColor = new Vector4(1, 1, 1, 1);
                        Vector4 hpColor;
                        if (player.health > 70)
                            hpColor = new Vector4(0, 1, 0, 1); // green
                        else if (player.health > 40)
                            hpColor = new Vector4(1, 1, 0, 1); // yellow
                        else
                            hpColor = new Vector4(1, 0, 0, 1); // red

                        if (isBoxEsp)
                        {
                            drawList.AddRect(rectTop, rectBottom, ImGui.ColorConvertFloat4ToU32(color));
                        }

                        if (isLineEsp)
                        {
                            drawList.AddLine(new Vector2(screenSize.X / 2, screenSize.Y), player.position2D, ImGui.ColorConvertFloat4ToU32(color));
                        }

                        if (isNameEsp || isHealthEsp)
                        {
                            Vector2 infoPos = new Vector2(rectBottom.X, rectTop.Y);
                            if (isNameEsp)
                            {
                                drawList.AddText(infoPos, ImGui.ColorConvertFloat4ToU32(nameColor), player.name);
                                if (isHealthEsp)
                                {
                                    infoPos.Y += 15;
                                    drawList.AddText(infoPos, ImGui.ColorConvertFloat4ToU32(hpColor), player.health.ToString());
                                }
                            }
                            else if (isHealthEsp)
                            {
                                drawList.AddText(infoPos, ImGui.ColorConvertFloat4ToU32(hpColor), player.health.ToString());
                            }
                        }
                    }
                }
            }
        }

        bool playerOnScreen(Player player)
        {
            return player.position2D.X > 0 && player.position2D.X < screenSize.X &&
                   player.position2D.Y > 0 && player.position2D.Y < screenSize.Y;
        }

        public void updatePlayers(IEnumerable<Player> newPlayers)
        {
            players = new ConcurrentQueue<Player>(newPlayers);
        }

        public void updateLocalPlayer(Player newPlayer)
        {
            lock (playerLock)
                localPlayer = newPlayer;
        }

        public Player getLocalPlayer()
        {
            lock (playerLock)
                return localPlayer;
        }

        void drawOverlay(Vector2 screenSize)
        {
            ImGui.SetNextWindowSize(screenSize);
            ImGui.SetNextWindowPos(new Vector2(0, 0));
            ImGui.Begin("overlay", ImGuiWindowFlags.NoDecoration
                | ImGuiWindowFlags.NoBackground
                | ImGuiWindowFlags.NoBringToFrontOnFocus
                | ImGuiWindowFlags.NoMove
                | ImGuiWindowFlags.NoInputs
                | ImGuiWindowFlags.NoCollapse
                | ImGuiWindowFlags.NoScrollbar
                | ImGuiWindowFlags.NoScrollWithMouse
                );
        }
    }
}
