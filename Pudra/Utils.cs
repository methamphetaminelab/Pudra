using System.Numerics;
using Swed64;

namespace Pudra
{
    public static class Utils
    {
        public static (Player[], Player) getPlayers(Swed game, IntPtr client, Vector2 screenSize)
        {
            IntPtr entityList = game.ReadPointer(client, Offsets.dwEntityList);
            IntPtr listEntry = game.ReadPointer(entityList, 0x10);
            IntPtr localPlayerPawn = game.ReadPointer(client, Offsets.dwLocalPlayerPawn);
            Player localPlayer = new Player();
            List<Player> players = new List<Player>();
            localPlayer.team = game.ReadInt(localPlayerPawn, Offsets.m_iTeamNum);
            localPlayer.pawnAddress = localPlayerPawn;

            for (int i = 0; i < 64; i++)
            {
                if (listEntry == IntPtr.Zero) 
                    continue;

                Player player = new Player();

                IntPtr currentController = game.ReadPointer(listEntry, i * 0x78);
                if (currentController == IntPtr.Zero) 
                    continue;

                int pawnHandle = game.ReadInt(currentController, Offsets.m_hPlayerPawn);
                if (pawnHandle == 0) 
                    continue;

                IntPtr listEntry2 = game.ReadPointer(entityList, 0x8 * ((pawnHandle & 0x7FFF) >> 9) + 0x10);
                IntPtr currentPawn = game.ReadPointer(listEntry2, 0x78 * (pawnHandle & 0x1FF));

                string name = game.ReadString(currentController, Offsets.m_iszPlayerName, 16);
                uint health = game.ReadUInt(currentPawn, Offsets.m_iHealth);
                uint lifeState = game.ReadUInt(currentPawn, Offsets.m_lifeState);

                if (lifeState != 256)
                    continue;

                float[] viewMatrix = game.ReadMatrix(client + Offsets.dwViewMatrix);

                player.position = game.ReadVec(currentPawn, Offsets.m_vOldOrigin);
                player.viewOffset = game.ReadVec(currentPawn, Offsets.m_vecViewOffset);
                player.position2D = Calculate.worldToScreen(viewMatrix, player.position, screenSize);
                player.viewPosition2D = Calculate.worldToScreen(viewMatrix, Vector3.Add(player.position, player.viewOffset), screenSize);
                player.name = name;
                player.health = health;
                player.lifeState = lifeState;
                player.team = game.ReadInt(currentPawn, Offsets.m_iTeamNum);
                player.pawnAddress = currentPawn;
                players.Add(player);
            }

            return (players.ToArray(), localPlayer);
        }

        public static uint getFov(Swed game, IntPtr localPlayerPawn)
        {
            IntPtr cameraServices = game.ReadPointer(localPlayerPawn, Offsets.m_pCameraServices);
            uint fov = game.ReadUInt(cameraServices + Offsets.m_iFOV);

            return fov;
        }

        public static float[] getViewModel(Swed game, IntPtr localPlayerPawn)
        {
            float m_flViewmodelX = game.ReadFloat(localPlayerPawn + Offsets.m_flViewmodelOffsetX);
            float m_flViewmodelY = game.ReadFloat(localPlayerPawn + Offsets.m_flViewmodelOffsetY);
            float m_flViewmodelZ = game.ReadFloat(localPlayerPawn + Offsets.m_flViewmodelOffsetZ);
            float m_flViewmodelFOV = game.ReadFloat(localPlayerPawn + Offsets.m_flViewmodelFOV);

            return new float[] { m_flViewmodelX, m_flViewmodelY, m_flViewmodelZ, m_flViewmodelFOV };
        }

        public static string getfFlag(Swed game, IntPtr localPlayerPawn)
        {
            string flag = "";
            uint fFlags = game.ReadUInt(localPlayerPawn, Offsets.m_fFlags);

            if (fFlags == 65665) flag = "standing";
            else if (fFlags == 65664) flag = "jumping";
            else if (fFlags == 65667) flag = "crouching";

            return flag;
        }

        public static void makeBunny(Swed game, IntPtr client)
        {
            IntPtr localPlayerPawn = game.ReadPointer(client, Offsets.dwLocalPlayerPawn);
            string fFlag = getfFlag(game, localPlayerPawn);

            if (fFlag == "standing" || fFlag == "crouching")
            {
                Thread.Sleep(1);
                game.WriteUInt(client + Offsets.jump, 65537);
            }
            else
            {
                game.WriteUInt(client + Offsets.jump, 256);
            }
        }

        public static void antiFlash(Swed game, IntPtr client)
        {
            IntPtr localPlayerPawn = game.ReadPointer(client, Offsets.dwLocalPlayerPawn);
            float flashBangTime = game.ReadFloat(localPlayerPawn, Offsets.m_flFlashBangTime);

            if (flashBangTime > 0)
                game.WriteFloat(localPlayerPawn, Offsets.m_flFlashBangTime, 0);
        }
    }
}
