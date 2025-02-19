namespace Pudra
{
    public static class Offsets
    {
        // client.dll
        public static int dwEntityList = (int)CS2Dumper.Offsets.ClientDll.dwEntityList;
        public static int dwLocalPlayerPawn = (int)CS2Dumper.Offsets.ClientDll.dwLocalPlayerPawn;
        public static int dwViewMatrix = (int)CS2Dumper.Offsets.ClientDll.dwViewMatrix;

        // C_BaseEntity
        public static int m_iHealth = (int)CS2Dumper.Schemas.ClientDll.C_BaseEntity.m_iHealth;
        public static int m_lifeState = (int)CS2Dumper.Schemas.ClientDll.C_BaseEntity.m_lifeState;
        public static int m_fFlags = (int)CS2Dumper.Schemas.ClientDll.C_BaseEntity.m_fFlags;
        public static int m_iTeamNum = (int)CS2Dumper.Schemas.ClientDll.C_BaseEntity.m_iTeamNum;

        // CBasePlayerController
        public static int m_iszPlayerName = (int)CS2Dumper.Schemas.ClientDll.CBasePlayerController.m_iszPlayerName;

        // CCSPlayerController
        public static int m_hPlayerPawn = (int)CS2Dumper.Schemas.ClientDll.CCSPlayerController.m_hPlayerPawn;

        // CCSPlayerBase_CameraServices
        public static int m_iFOV = (int)CS2Dumper.Schemas.ClientDll.CCSPlayerBase_CameraServices.m_iFOV;

        // C_BasePlayerPawn
        public static int m_pCameraServices = (int)CS2Dumper.Schemas.ClientDll.C_BasePlayerPawn.m_pCameraServices;
        public static int m_vOldOrigin = (int)CS2Dumper.Schemas.ClientDll.C_BasePlayerPawn.m_vOldOrigin;

        // C_CSPlayerPawn
        public static int m_flViewmodelOffsetX = (int)CS2Dumper.Schemas.ClientDll.C_CSPlayerPawn.m_flViewmodelOffsetX;
        public static int m_flViewmodelOffsetY = (int)CS2Dumper.Schemas.ClientDll.C_CSPlayerPawn.m_flViewmodelOffsetY;
        public static int m_flViewmodelOffsetZ = (int)CS2Dumper.Schemas.ClientDll.C_CSPlayerPawn.m_flViewmodelOffsetZ;
        public static int m_flViewmodelFOV = (int)CS2Dumper.Schemas.ClientDll.C_CSPlayerPawn.m_flViewmodelFOV;

        // C_BaseModelEntity
        public static int m_vecViewOffset = (int)CS2Dumper.Schemas.ClientDll.C_BaseModelEntity.m_vecViewOffset;

        // buttons
        public static int jump = (int)CS2Dumper.Buttons.jump;
    }
}
