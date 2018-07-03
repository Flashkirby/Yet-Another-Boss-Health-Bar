using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;

//using FKTModSettings;

namespace FKBossHealthBar
{
    class FKBossHealthBar : Mod
    {

        public bool LoadedFKTModSettings = false;

        internal ModSimpleHealthBarType1 cacheHealthBarType1 = null;

        public FKBossHealthBar()
        {
            Properties = new ModProperties()
            {
                Autoload = true
            };
        }
        
        public override object Call(params object[] args)
        {
            // Servers don't bother
            if (Main.dedServ) return null;

            string methodName = args[0] as string; // cast to a string or null
            HealthBar hb;
            try
            {
                switch (methodName)
                {
                    #region HB methods
                    // Method to generate a healthbar
                    case "hbStart":
                        if(cacheHealthBarType1 != null)
                        { ErrorLogger.Log("FKBossHealthBar: A mod attempted to call 'hbStart' before calling 'hbFinishSingle' or 'hbFinishMultiple'. ");  }
                        cacheHealthBarType1 = new ModSimpleHealthBarType1();
                        break;
                    case "hbForceSmall":
                        if (cacheHealthBarType1 == null) { ErrorLogger.Log("FKBossHealthBar: A mod attempted to modify a healthbar before calling 'hbStart'. \n" + new System.Diagnostics.StackTrace()); break; }
                        if (args[1] != null) cacheHealthBarType1.ForceSmall = (bool)args[1];
                        break;
                    case "hbForceNoChip":
                        if (cacheHealthBarType1 == null) { ErrorLogger.Log("FKBossHealthBar: A mod attempted to modify a healthbar before calling 'hbStart'. \n" + new System.Diagnostics.StackTrace()); break; }
                        if (args[1] != null) cacheHealthBarType1.ForceNoChip = (bool)args[1];
                        break;
                    case "hbLoopMidBar":
                        if (cacheHealthBarType1 == null) { ErrorLogger.Log("FKBossHealthBar: A mod attempted to modify a healthbar before calling 'hbStart'. \n" + new System.Diagnostics.StackTrace()); break; }
                        if (args[1] != null) cacheHealthBarType1.LoopMidBar = (bool)args[1];
                        break;
                    case "hbSetTexture":
                        if (cacheHealthBarType1 == null) { ErrorLogger.Log("FKBossHealthBar: A mod attempted to modify a healthbar before calling 'hbStart'. \n" + new System.Diagnostics.StackTrace()); break; }
                        if (args[1] != null) cacheHealthBarType1.leftBar = args[1] as Texture2D;
                        if (args[2] != null) cacheHealthBarType1.midBar = args[2] as Texture2D;
                        if (args[3] != null) cacheHealthBarType1.rightBar = args[3] as Texture2D;
                        if (args[4] != null) cacheHealthBarType1.fillTexture = args[4] as Texture2D;
                        break;
                    case "hbSetTextureExpert":
                        if (cacheHealthBarType1 == null) { ErrorLogger.Log("FKBossHealthBar: A mod attempted to modify a healthbar before calling 'hbStart'. \n" + new System.Diagnostics.StackTrace()); break; }
                        if (args[1] != null) cacheHealthBarType1.leftBarEXP = args[1] as Texture2D;
                        if (args[2] != null) cacheHealthBarType1.midBarEXP = args[2] as Texture2D;
                        if (args[3] != null) cacheHealthBarType1.rightBarEXP = args[3] as Texture2D;
                        break;
                    case "hbSetTextureSmall":
                        if (cacheHealthBarType1 == null) { ErrorLogger.Log("FKBossHealthBar: A mod attempted to modify a healthbar before calling 'hbStart'. \n" + new System.Diagnostics.StackTrace()); break; }
                        if (args[1] != null) cacheHealthBarType1.leftBarSM = args[1] as Texture2D;
                        if (args[2] != null) cacheHealthBarType1.midBarSM = args[2] as Texture2D;
                        if (args[3] != null) cacheHealthBarType1.rightBarSM = args[3] as Texture2D;
                        if (args[4] != null) cacheHealthBarType1.fillTextureSM = args[4] as Texture2D;
                        break;
                    case "hbSetTextureSmallExpert":
                        if (cacheHealthBarType1 == null) { ErrorLogger.Log("FKBossHealthBar: A mod attempted to modify a healthbar before calling 'hbStart'. \n" + new System.Diagnostics.StackTrace()); break; }
                        if (args[1] != null) cacheHealthBarType1.leftBarSMEXP = args[1] as Texture2D;
                        if (args[2] != null) cacheHealthBarType1.midBarSMEXP = args[2] as Texture2D;
                        if (args[3] != null) cacheHealthBarType1.rightBarSMEXP = args[3] as Texture2D;
                        break;
                    case "hbSetMidBarOffsetY":
                        if (cacheHealthBarType1 == null) { ErrorLogger.Log("FKBossHealthBar: A mod attempted to modify a healthbar before calling 'hbStart'. \n" + new System.Diagnostics.StackTrace()); break; }
                        if (args[1] != null) cacheHealthBarType1.midBarOffsetY = (int)args[1];
                        break;
                    case "hbSetMidBarOffset":
                        if (cacheHealthBarType1 == null) { ErrorLogger.Log("FKBossHealthBar: A mod attempted to modify a healthbar before calling 'hbStart'. \n" + new System.Diagnostics.StackTrace()); break; }
                        if (args[1] != null) cacheHealthBarType1.midBarOffsetX = (int)args[1];
                        if (args[2] != null) cacheHealthBarType1.midBarOffsetY = (int)args[2];
                        break;
                    case "hbSetBossHeadCentre":
                        if (cacheHealthBarType1 == null) { ErrorLogger.Log("FKBossHealthBar: A mod attempted to modify a healthbar before calling 'hbStart'. \n" + new System.Diagnostics.StackTrace()); break; }
                        if (args[1] != null) cacheHealthBarType1.bossHeadCentreOffsetX = (int)args[1];
                        if (args[2] != null) cacheHealthBarType1.bossHeadCentreOffsetY = (int)args[2];
                        break;
                    case "hbSetBossHeadCentreSmall":
                        if (cacheHealthBarType1 == null) { ErrorLogger.Log("FKBossHealthBar: A mod attempted to modify a healthbar before calling 'hbStart'. \n" + new System.Diagnostics.StackTrace()); break; }
                        if (args[1] != null) cacheHealthBarType1.bossHeadCentreOffsetXSM = (int)args[1];
                        if (args[2] != null) cacheHealthBarType1.bossHeadCentreOffsetYSM = (int)args[2];
                        break;
                    case "hbSetColours":
                        if (cacheHealthBarType1 == null) { ErrorLogger.Log("FKBossHealthBar: A mod attempted to modify a healthbar before calling 'hbStart'. \n" + new System.Diagnostics.StackTrace()); break; }
                        if (args[1] != null) cacheHealthBarType1.barColourFull = (Color)args[1];
                        if (args[2] != null) cacheHealthBarType1.barColourMid = (Color)args[2];
                        if (args[3] != null) cacheHealthBarType1.barColourEmpty = (Color)args[3];
                        break;
                    case "hbSetBossHeadTexture":
                        if (cacheHealthBarType1 == null) { ErrorLogger.Log("FKBossHealthBar: A mod attempted to modify a healthbar before calling 'hbStart'. \n" + new System.Diagnostics.StackTrace()); break; }
                        if (args[1] != null) cacheHealthBarType1.customBossHeadIcon = args[1] as Texture2D;
                        break;
                    case "hbFinishSingle":
                        if (cacheHealthBarType1 == null) { ErrorLogger.Log("FKBossHealthBar: A mod attempted to call 'hbFinishSingle' before calling 'hbStart'. \n" + new System.Diagnostics.StackTrace()); break; }
                        BossDisplayInfo.SetCustomHealthBar(Convert.ToInt32(args[1]), cacheHealthBarType1);
                        break;
                    case "hbFinishMultiple":
                        if (cacheHealthBarType1 == null) { ErrorLogger.Log("FKBossHealthBar: A mod attempted to call 'hbFinishMultiple' before calling 'hbStart'. \n" + new System.Diagnostics.StackTrace()); break; }
                        int[] npcTypes1 = new int[args.Length - 1];
                        for (int i = 1; i < args.Length; i++)
                        {
                            npcTypes1[i - 1] = Convert.ToInt32(args[i]);
                        }
                        BossDisplayInfo.SetCustomHealthBarMultiple(cacheHealthBarType1, npcTypes1);
                        cacheHealthBarType1 = null;
                        break;
                    #endregion

                    #region RegisterHealthBar
                    case "RegisterHealthBar":
                        hb = new HealthBar();
                        BossDisplayInfo.SetCustomHealthBar(Convert.ToInt32(args[1]), hb);
                        break;
                    #endregion
                    #region RegisterHealthBarMulti
                    case "RegisterHealthBarMulti":
                        hb = new HealthBar();
                        int[] npcTypes2 = new int[args.Length - 1];
                        for (int i = 1; i < args.Length; i++)
                        {
                            npcTypes2[i - 1] = Convert.ToInt32(args[i]);
                        }
                        BossDisplayInfo.SetCustomHealthBarMultiple(hb, npcTypes2);
                        break;
                    #endregion

                    #region RegisterHealthBarMini
                    case "RegisterHealthBarMini":
                        hb = new HealthBar();
                        hb.ForceSmall = true;
                        BossDisplayInfo.SetCustomHealthBar(Convert.ToInt32(args[1]), hb);
                        break;
                    #endregion
                    #region RegisterHealthBarMultiMini
                    case "RegisterHealthBarMultiMini":
                        hb = new HealthBar();
                        hb.ForceSmall = true;
                        int[] npcSmTypes = new int[args.Length - 1];
                        for (int i = 1; i < args.Length; i++)
                        {
                            npcSmTypes[i - 1] = Convert.ToInt32(args[i]);
                        }
                        BossDisplayInfo.SetCustomHealthBarMultiple(hb, npcSmTypes);
                        break;
                    #endregion

                    #region RegisterDemonHealthBar
                    case "RegisterDemonHealthBar":
                        hb = new DemonHealthBar();
                        BossDisplayInfo.SetCustomHealthBar(Convert.ToInt32(args[1]), hb);
                        break;
                    #endregion
                    #region RegisterDemonHealthBarMulti
                    case "RegisterDemonHealthBarMulti":
                        hb = new DemonHealthBar();
                        int[] demonNpcTypes = new int[args.Length - 1];
                        for (int i = 0; i < args.Length - 1; i++)
                        {
                            demonNpcTypes[i] = Convert.ToInt32(args[i + 1]);
                        }
                        BossDisplayInfo.SetCustomHealthBarMultiple(hb, demonNpcTypes);
                        break;
                    #endregion

                    #region RegisterMechHealthBar
                    case "RegisterMechHealthBar":
                        hb = new MechHealthBar();
                        BossDisplayInfo.SetCustomHealthBar(Convert.ToInt32(args[1]), hb);
                        break;
                    #endregion
                    #region RegisterMechHealthBarMulti
                    case "RegisterMechHealthBarMulti":
                        hb = new MechHealthBar();
                        int[] mechNpcTypes = new int[args.Length - 1];
                        for (int i = 0; i < args.Length - 1; i++)
                        {
                            mechNpcTypes[i] = Convert.ToInt32(args[i + 1]);
                        }
                        BossDisplayInfo.SetCustomHealthBarMultiple(hb, mechNpcTypes);
                        break;
                    #endregion

                    #region RegisterDD2HealthBar
                    case "RegisterDD2HealthBar":
                        hb = new DD2HealthBar();
                        BossDisplayInfo.SetCustomHealthBar(Convert.ToInt32(args[1]), hb);
                        break;
                    #endregion
                    #region RegisterDD2HealthBarMini
                    case "RegisterDD2HealthBarMini":
                        hb = new DD2HealthBar();
                        hb.ForceSmall = true;
                        BossDisplayInfo.SetCustomHealthBar(Convert.ToInt32(args[1]), hb);
                        break;
                    #endregion
                    #region RegisterDD2hHealthBarMulti
                    case "RegisterDD2hHealthBarMulti":
                        hb = new DD2HealthBar();
                        int[] dd2hNpcTypes = new int[args.Length - 1];
                        for (int i = 0; i < args.Length - 1; i++)
                        {
                            dd2hNpcTypes[i] = Convert.ToInt32(args[i + 1]);
                        }
                        BossDisplayInfo.SetCustomHealthBarMultiple(hb, dd2hNpcTypes);
                        break;
                    #endregion

                    #region RegisterCustomHealthBar
                    // RegisterCustomHealthBar(bool ForceSmall, nullable string DisplayName, 
                    // Texture2D fillTexture, LeftBar, midBar, rightBar
                    // int midBarOffsetX, int midBarOffsetY, int fillDecoOffsetX
                    // int bossHeadCentreOffsetY, int bossHeadCentreOffsetX
                    // Texture2D, fillTextureSM, leftBarSM, midBarSM, rightBarSM
                    // int fillDecoOffsetXSM, int bossHeadCentreOffsetXSM, int bossHeadCentreOffsetYSM
                    // bool LoopMidBar
                    case "RegisterCustomHealthBar":
                        ModTextureHealthBar thb = new ModTextureHealthBar();
                        if (args[2] != null) thb.ForceSmall = (bool)args[2];
                        if (args[3] != null) thb.displayName = args[3] as string;
                        if (args[4] != null) thb.fillTexture = args[4] as Texture2D;
                        if (args[5] != null) thb.leftBar = args[5] as Texture2D;
                        if (args[6] != null) thb.midBar = args[6] as Texture2D;
                        if (args[7] != null) thb.rightBar = args[7] as Texture2D;

                        if (args[8] != null) thb.midBarOffsetX = (int)args[8];
                        if (args[9] != null) thb.midBarOffsetY = (int)args[9];
                        if (args[10] != null) thb.fillDecoOffsetX = (int)args[10];
                        if (args[11] != null) thb.bossHeadCentreOffsetX = (int)args[11];
                        if (args[12] != null) thb.bossHeadCentreOffsetY = (int)args[12];

                        if (args[13] != null) thb.fillTextureSM = args[13] as Texture2D;
                        if (args[14] != null) thb.leftBarSM = args[14] as Texture2D;
                        if (args[15] != null) thb.midBarSM = args[15] as Texture2D;
                        if (args[16] != null) thb.rightBarSM = args[16] as Texture2D;

                        if (args[17] != null) thb.fillDecoOffsetXSM = (int)args[17];
                        if (args[18] != null) thb.bossHeadCentreOffsetXSM = (int)args[18];
                        if (args[19] != null) thb.bossHeadCentreOffsetYSM = (int)args[19];

                        if (args[20] != null) thb.LoopMidBar = (bool)args[20];

                        BossDisplayInfo.SetCustomHealthBar(Convert.ToInt32(args[1]), thb);
                        break;
                    #endregion
                    #region RegisterCustomHealthBarMulti
                    case "RegisterCustomHealthBarMulti":
                        ModTextureHealthBar thbm = new ModTextureHealthBar();
                        if (args[2] != null) thbm.ForceSmall = (bool)args[2];
                        if (args[3] != null) thbm.displayName = args[3] as string;
                        if (args[4] != null) thbm.fillTexture = args[4] as Texture2D;
                        if (args[5] != null) thbm.leftBar = args[5] as Texture2D;
                        if (args[6] != null) thbm.midBar = args[6] as Texture2D;
                        if (args[7] != null) thbm.rightBar = args[7] as Texture2D;

                        if (args[8] != null) thbm.midBarOffsetX = (int)args[8];
                        if (args[9] != null) thbm.midBarOffsetY = (int)args[9];
                        if (args[10] != null) thbm.fillDecoOffsetX = (int)args[10];
                        if (args[11] != null) thbm.bossHeadCentreOffsetX = (int)args[11];
                        if (args[12] != null) thbm.bossHeadCentreOffsetY = (int)args[12];

                        if (args[13] != null) thbm.fillTextureSM = args[13] as Texture2D;
                        if (args[14] != null) thbm.leftBarSM = args[14] as Texture2D;
                        if (args[15] != null) thbm.midBarSM = args[15] as Texture2D;
                        if (args[16] != null) thbm.rightBarSM = args[16] as Texture2D;

                        if (args[17] != null) thbm.fillDecoOffsetXSM = (int)args[17];
                        if (args[18] != null) thbm.bossHeadCentreOffsetXSM = (int)args[18];
                        if (args[19] != null) thbm.bossHeadCentreOffsetYSM = (int)args[19];

                        if (args[20] != null) thbm.LoopMidBar = (bool)args[20];

                        BossDisplayInfo.SetCustomHealthBarMultiple(thbm, args[1] as int[]);
                        break;
                    #endregion

                    #region RegisterCustomMethodHealthBar
                    case "RegisterCustomMethodHealthBar":
                        ModMethodHealthBar mhb = new ModMethodHealthBar();
                        if (args[2] != null) mhb.ForceSmall = (bool)args[2];
                        if (args[3] != null) mhb.getBossDisplayNameNPC = args[3] as Func<NPC, string>;
                        if (args[4] != null) mhb.getFillTexture = args[4] as Func<Texture2D>;
                        if (args[5] != null) mhb.getLeftBar = args[5] as Func<Texture2D>;
                        if (args[6] != null) mhb.getMidBar = args[6] as Func<Texture2D>;
                        if (args[7] != null) mhb.getRightBar = args[7] as Func<Texture2D>;

                        if (args[8] != null) mhb.midBarOffsetX = (int)args[8];
                        if (args[9] != null) mhb.midBarOffsetY = (int)args[9];
                        if (args[10] != null) mhb.fillDecoOffsetX = (int)args[10];
                        if (args[11] != null) mhb.bossHeadCentreOffsetX = (int)args[11];
                        if (args[12] != null) mhb.bossHeadCentreOffsetY = (int)args[12];

                        if (args[13] != null) mhb.getSmallFillTexture = args[13] as Func<Texture2D>;
                        if (args[14] != null) mhb.getSmallLeftBar = args[14] as Func<Texture2D>;
                        if (args[15] != null) mhb.getSmallMidBar = args[15] as Func<Texture2D>;
                        if (args[16] != null) mhb.getSmallRightBar = args[16] as Func<Texture2D>;

                        if (args[17] != null) mhb.fillDecoOffsetXSM = (int)args[17];
                        if (args[18] != null) mhb.bossHeadCentreOffsetXSM = (int)args[18];
                        if (args[19] != null) mhb.bossHeadCentreOffsetYSM = (int)args[19];

                        if (args[20] != null) mhb.getHealthColour = args[20] as Func<NPC, int, int, Color>;
                        BossDisplayInfo.SetCustomHealthBar(Convert.ToInt32(args[1]), mhb);
                        break;
                    #endregion
                    #region RegisterCustomMethodHealthBarMulti
                    case "RegisterCustomMethodHealthBarMulti":
                        ModMethodHealthBar mhbm = new ModMethodHealthBar();
                        if (args[2] != null) mhbm.ForceSmall = (bool)args[2];
                        if (args[3] != null) mhbm.getBossDisplayNameNPC = args[3] as Func<NPC, string>;
                        if (args[4] != null) mhbm.getFillTexture = args[4] as Func<Texture2D>;
                        if (args[5] != null) mhbm.getLeftBar = args[5] as Func<Texture2D>;
                        if (args[6] != null) mhbm.getMidBar = args[6] as Func<Texture2D>;
                        if (args[7] != null) mhbm.getRightBar = args[7] as Func<Texture2D>;

                        if (args[8] != null) mhbm.midBarOffsetX = (int)args[8];
                        if (args[9] != null) mhbm.midBarOffsetY = (int)args[9];
                        if (args[10] != null) mhbm.fillDecoOffsetX = (int)args[10];
                        if (args[11] != null) mhbm.bossHeadCentreOffsetX = (int)args[11];
                        if (args[12] != null) mhbm.bossHeadCentreOffsetY = (int)args[12];

                        if (args[13] != null) mhbm.getSmallFillTexture = args[13] as Func<Texture2D>;
                        if (args[14] != null) mhbm.getSmallLeftBar = args[14] as Func<Texture2D>;
                        if (args[15] != null) mhbm.getSmallMidBar = args[15] as Func<Texture2D>;
                        if (args[16] != null) mhbm.getSmallRightBar = args[16] as Func<Texture2D>;

                        if (args[17] != null) mhbm.fillDecoOffsetXSM = (int)args[17];
                        if (args[18] != null) mhbm.bossHeadCentreOffsetXSM = (int)args[18];
                        if (args[19] != null) mhbm.bossHeadCentreOffsetYSM = (int)args[19];

                        if (args[20] != null) mhbm.getHealthColour = args[20] as Func<NPC, int, int, Color>;
                        BossDisplayInfo.SetCustomHealthBarMultiple(mhbm, args[1] as int[]);
                        break;
                        #endregion
                }
            }
            catch { }
            return null;
        }

        public override void Load()
        {
            // Servers don't bother
            if (Main.dedServ) return;

            DemonHealthBar.DemonBarL = GetTexture("UI/DemonBarStart");
            DemonHealthBar.DemonBarM = GetTexture("UI/DemonBarMiddle");
            DemonHealthBar.DemonBarR = GetTexture("UI/DemonBarEnd");

            MechHealthBar.MechBarL = GetTexture("UI/MechBarStart");
            MechHealthBar.MechBarM = GetTexture("UI/MechBarMiddle");
            MechHealthBar.MechBarR = GetTexture("UI/MechBarEnd");

            DD2HealthBar.BarL = GetTexture("UI/DD2BarStart");
            DD2HealthBar.BarM = GetTexture("UI/DD2BarMiddle");
            DD2HealthBar.BarR = GetTexture("UI/DD2BarEnd");
            DD2HealthBar.BarF = GetTexture("UI/DD2BarFill");
            DD2HealthBar.BarFsm = GetTexture("UI/DD2SmBarFill");

            Config.LoadConfig();

            LoadedFKTModSettings = ModLoader.GetMod("FKTModSettings") != null;
            try
            {
                if (LoadedFKTModSettings)
                {
                    // Needs to be in a method otherwise it throws a namespace error
                    LoadModSettings();
                }
            }
            catch (Exception e) { throw e; }

            HealthBar hb;
            
            Call("RegisterDemonHealthBar", NPCID.WallofFlesh);

            Call("RegisterMechHealthBar", NPCID.TheDestroyer);
            Call("RegisterMechHealthBar", NPCID.Spazmatism);
            Call("RegisterMechHealthBar", NPCID.Retinazer);
            Call("RegisterMechHealthBar", NPCID.SkeletronPrime);
            
            Call("RegisterHealthBarMulti", 
                NPCID.EaterofWorldsHead,
                NPCID.EaterofWorldsBody,
                NPCID.EaterofWorldsTail);

            Call("RegisterHealthBarMulti", 
                NPCID.BrainofCthulhu, NPCID.Creeper);

            Call("RegisterHealthBarMulti", 
                NPCID.Golem, NPCID.GolemHead, NPCID.GolemFistLeft, NPCID.GolemFistRight);

            Call("RegisterHealthBarMulti",
                NPCID.MartianSaucerCore, NPCID.MartianSaucerCannon, NPCID.MartianSaucerTurret);

            Call("RegisterCustomHealthBar",
                NPCID.Plantera,
                null, //ForceSmall
                null, //displayName
                GetTexture("UI/PlantBarFill"), //fillTexture
                GetTexture("UI/PlantBarStart"), //leftBar
                GetTexture("UI/PlantBarMiddle"), //midBar
                GetTexture("UI/PlantBarEnd"), //rightBar
                null, //midBarOffsetX
                0, //midBarOffsetY
                null, //fillDecoOffsetX
                34, //bossHeadCentreOffsetX
                null, //bossHeadCentreOffsetY
                null, //fillTextureSM
                null, //leftBarSM
                null, //midBarSM
                null, //rightBarSM
                null, //fillDecoOffsetXSM
                null, //bossHeadCentreOffsetXSM
                null, //bossHeadCentreOffsetYSM
                true); //LoopMidBar

            #region Invasions
            // Minibosses use small bars
            Call("RegisterHealthBarMini", NPCID.GoblinSummoner);
            Call("RegisterHealthBarMulti",
                NPCID.PirateShip, NPCID.PirateShipCannon);
            Call("RegisterHealthBarMini", NPCID.PirateCaptain);
            Call("RegisterHealthBar", NPCID.Mothron);

            // Pumpkin Moon
            Call("RegisterHealthBarMini", NPCID.MourningWood);
            Call("RegisterHealthBar", NPCID.Pumpking);

            // Frost Moon
            Call("RegisterHealthBarMini", NPCID.Everscream);
            Call("RegisterHealthBarMini", NPCID.SantaNK1);
            Call("RegisterHealthBar", NPCID.IceQueen);
            #endregion

            // Pillars
            hb = new CelestialTowerHealthBar();
            BossDisplayInfo.SetCustomHealthBar(NPCID.LunarTowerSolar, hb);
            hb = new CelestialTowerHealthBar();
            BossDisplayInfo.SetCustomHealthBar(NPCID.LunarTowerVortex, hb);
            hb = new CelestialTowerHealthBar();
            BossDisplayInfo.SetCustomHealthBar(NPCID.LunarTowerNebula, hb);
            hb = new CelestialTowerHealthBar();
            BossDisplayInfo.SetCustomHealthBar(NPCID.LunarTowerStardust, hb);

            #region Moon Lord
            // Moon Lord custom example
            Call("hbStart");
            Call("hbForceSmall", true);
            Call("hbSetColours", new Color(0f, 0f, 1f), new Color(0f, 1f, 1f), new Color(0f, 1f, 0f));
            Call("hbFinishSingle", NPCID.MoonLordHead);

            Call("hbStart");
            Call("hbForceSmall", true);
            Call("hbSetColours", new Color(0f, 0f, 1f), new Color(0f, 1f, 1f), new Color(0f, 1f, 0f));
            Call("hbSetBossHeadTexture", GetTexture("BossHeads/8_1"));
            Call("hbFinishSingle", NPCID.MoonLordHand);

            Call("hbStart");
            Call("hbSetColours", new Color(0f, 1f, 0.6f), new Color(0.8f, 1f, 0f), new Color(1f, 0f, 0f));
            //Call("hbSetBossHeadTexture", Main.npcHeadBossTexture[8]);
            Call("hbSetBossHeadTexture", GetTexture("BossHeads/8_2"));
            Call("hbSetTexture", 
                GetTexture("UI/MoonLordBarStart"), null, 
                GetTexture("UI/MoonLordBarEnd"));
            Call("hbSetTextureExpert", 
                GetTexture("UI/MoonLordBarStart_Exp"), null, 
                GetTexture("UI/MoonLordBarEnd_Exp"));
            Call("hbFinishSingle", NPCID.MoonLordCore);
            #endregion

            #region DD2
            Call("RegisterDD2HealthBar", NPCID.DD2DarkMageT1);
            Call("RegisterDD2HealthBarMini", NPCID.DD2DarkMageT3);

            Call("RegisterDD2HealthBar", NPCID.DD2OgreT2);
            Call("RegisterDD2HealthBarMini", NPCID.DD2OgreT3);

            Call("RegisterDD2HealthBar", NPCID.DD2Betsy);

            Call("RegisterCustomMethodHealthBar", NPCID.DD2EterniaCrystal,
                true, null, null, null, null, null,
                null, null, null, null, null,
                null, null, null, null,
                null, null, null,
                DD2CrystalHealthBar.GetHealthColour
                );
            #endregion

            if (Config.SlimeRainBar)
            {
                hb = new SlimeRainHealthBar();
                BossDisplayInfo.SetCustomHealthBarMultiple(hb, NPCID.BlueSlime);
            }

            HealthBar.Initialise(this);
        }

        public override void Unload()
        {
            BossDisplayInfo.ResetNPCHealthBars();
        }

        public override void PreSaveAndQuit()
        {
            Config.SaveValues();
        }

        public override void PostUpdateInput()
        {
            try
            {
                if (LoadedFKTModSettings && !Main.gameMenu)
                {
                    // Needs to be in a method otherwise it throws a namespace error
                    UpdateModSettings();
                }
            }
            catch { }
        }

        #region Mod Settings Integration
        private void LoadModSettings()
        {
            /*
            ModSetting setting = ModSettingsAPI.CreateModSettingConfig(this);
            setting.AddBool("ShowBossHealthBars", "Enable Health Bars", false);
            setting.AddBool("SmallHealthBars", "Force Small Health Bars", false);

            setting.AddComment("TRANSPARENCY", 1.1f);

            setting.AddFloat("HealthBarUIDefaultAlpha", "Default Transparency", 0f, 1f, false);
            setting.AddFloat("HealthBarUIFadeHover", "Mouse Over Modifier", 0, 1f, false);
            setting.AddInt("HealthBarUIFadeTime", "Fade Time (seconds/60)", 0, 180, false);

            setting.AddComment("FANCY FX", 1.1f);

            setting.AddBool("HealthBarFXFillUp", "Fill bar on Entry", false);
            setting.AddBool("HealthBarFXShake", "Shake bar with damage", false);
            setting.AddBool("HealthBarFXShakeHorizontal", "Shake bar horizontally", false);
            setting.AddInt("HealthBarFXShakeIntensity", "Shake bar intensity", 1, 10, false);
            setting.AddBool("HealthBarFXChip", "Chip damage display", false);
            setting.AddBool("HealthBarFXChipNumbers", "Chip damage numbers", false);
            setting.AddInt("HealthBarFXChipWaitTime", "Chip drain delay", 0, 180, false);
            setting.AddFloat("HealthBarFXChipSpeed", "Chip drain speed%", 0.001f, 1f, false);

            setting.AddComment("POSITIONING", 1.1f);

            setting.AddInt("HealthBarUIScreenOffset", "Distance from bottom", 0, 100, false);
            setting.AddInt("HealthBarUIStackOffset", "Distance between bars", 0, 100, false);
            setting.AddFloat("HealthBarUIScreenLength", "Screen width:bar scaling", 0f, 0.85f, false);
            setting.AddFloat("HealthBarUIMaxStackSize", "Screen height:bar threshold", 0f, 1f, false);
            */
        }
        private void UpdateModSettings()
        {
            /*
            ModSetting setting;
            if (ModSettingsAPI.TryGetModSetting(this, out setting))
            {
                setting.Get("ShowBossHealthBars", ref Config.ShowBossHealthBars);
                setting.Get("SmallHealthBars", ref Config.SmallHealthBars);

                setting.Get("HealthBarUIScreenOffset", ref Config.HealthBarUIScreenOffset);
                setting.Get("HealthBarUIScreenLength", ref Config.HealthBarUIScreenLength);
                setting.Get("HealthBarUIStackOffset", ref Config.HealthBarUIStackOffset);

                setting.Get("HealthBarUIDefaultAlpha", ref Config.HealthBarUIDefaultAlpha);
                setting.Get("HealthBarUIFadeHover", ref Config.HealthBarUIFadeHover);
                setting.Get("HealthBarUIFadeTime", ref Config.HealthBarUIFadeTimeINT);
                setting.Get("HealthBarUIMaxStackSize", ref Config.HealthBarUIMaxStackSize);

                setting.Get("HealthBarFXFillUp", ref Config.HealthBarFXFillUp);
                setting.Get("HealthBarFXShake", ref Config.HealthBarFXShake);
                setting.Get("HealthBarFXShakeIntensity", ref Config.HealthBarFXShakeIntensity);
                setting.Get("HealthBarFXShakeHorizontal", ref Config.HealthBarFXShakeHorizontal);

                setting.Get("HealthBarFXChip", ref Config.HealthBarFXChip);
                setting.Get("HealthBarFXChipWaitTime", ref Config.HealthBarFXChipWaitTime);
                setting.Get("HealthBarFXChipSpeed", ref Config.HealthBarFXChipSpeed);
                setting.Get("HealthBarFXChipNumbers", ref Config.HealthBarFXChipNumbers);

            }
            */
        }
        #endregion

        public override void UpdateMusic(ref int music)
        {
            if (Main.gameMenu)
            {
                // Consider moving this to PreSaveAndQuit?
                BossBarTracker.ResetTracker();
            }
        }

        public override void PostDrawInterface(SpriteBatch spriteBatch)
        {
            try
            {
                if (!Main.gameInactive) BossBarTracker.UpdateNPCTracker();
            }
            catch (System.Exception e) { Main.NewTextMultiline(e.ToString()); }

            if (!Config.ShowBossHealthBars) return;
            BossBarTracker.DrawHealthBars(spriteBatch);
        }
    }
}
