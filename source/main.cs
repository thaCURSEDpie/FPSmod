using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Media;
using GTA;

// weapon numbers 395 t/m 418

namespace killNotifications
{
    public class FPSmod : Script
    {
        #region variable declaration
        killNotification[] killNotificationArray;

        RectangleF radar;

        killEvent[] killEventArray;

        SoundPlayer headshotSoundPlayer,
                    godlikeSound,
                    firstBloodSound,
                    doubleKillSound,
                    multiKillSound,
                    killingSpreeSound,
                    dominatingSound,
                    humiliationSound,
                    megaKillSound,
                    unstoppableSound,
                    wickedSickSound,
                    holyShitSound,
                    ownageSound,
                    rampageSound,
                    comboWhoreSound;

        float iconHeight = 40f, iconWidth = 395f, iconX = 40f, iconYdiff = 5f,
                              attackerX = 50f,
                              victimX = 300f,
                              weaponX = 220f;

        int RAMPAGE_TIME = 110,
            KILLS_FOR_RAMPAGE = 5,
            GODLIKE_TIME = 2400,
            UNSTOPPABLE_TIME = 110,
            killEventRemoveTime = 150,
            MULTIKILL_TIMELIMIT = 30,
            OWNAGE_TIMELIMIT = 110,
            KILLS_FOR_OWNAGE = 5,
            MEGAKILL_TIMELIMIT = 5,
            KILLS_FOR_UNSTOPPABLE = 6,
            KILLS_FOR_MULTIKILL = 4,
            KILLS_FOR_COMBOWHORE = 8,
            KILLS_FOR_KILLINGSPREE = 20,
            KILLS_FOR_DOMINATING = 40,
            KILLS_FOR_WICKEDSICK = 70,
            KILLS_FOR_HOLYSHIT = 20;

        string[] alphabetArray = {"A",
                                  "B",
                                  "C",
                                  "D",
                                  "E",
                                  "F",
                                  "G",
                                  "H",
                                  "I",
                                  "J",
                                  "K",
                                  "L", 
                                  "M",
                                  "N",
                                  "O",
                                  "P",
                                  "Q",
                                  "R",
                                  "S",
                                  "T",
                                  "U",
                                  "V",
                                  "W",
                                  "X",
                                  "Y",
                                  "Z"};


        //Texture testTexture;
        Texture aa12,
                pistol44,
                advanced_sniper,
                assault_shotgun,
                auto_pistol,
                golden_smg,
                grenadelauncher,
                lmg,
                p90,
                pipebomb,
                poolcue,
                sawnoff_shotgun,
                sticky,
                headshot,
                runover,
                unarmed,
                baseball,
                knife,
                grenade,
                molotov,
                deagle,
                shotgun,
                combat_shotgun,
                micro_smg,
                mp5,
                pistol,
                sniper,
                combat_sniper,
                rpg,
                m4,
                ak47,
                vehicle_explosion,
                headshotModifier;

        int gameMode = 0,
            unstoppableExclusionNumber = 0,
            rampageExclusionNumber = 0,
            godlikeTimer = 0,
            ownageExclusionNumber = 0,
            multiKillExclusionNumber = 0,
            currentEventArraySize = 50,
            numberOfKillEvents = 0,
            globalCounter = 0,
            comboWhoreCounter = 0,
            numberOfHeadshotsSinceLastDeath = 0,
            numberOfKillsSinceLastDeath = 0,
            numberOfkillsStart,
            numberOfSurnames = 77,
            aa12regularStart,
            aa12explosiveStart,
            pool_cueStart,
            pistol44Start,
            advanced_sniperStart,
            assault_shotgunStart,
            auto_pistolStart,
            golden_smgStart,
            grenadelauncherStart,
            lmgStart,
            p90Start,
            pipebombStart,
            sawnoff_shotgunStart,
            stickyStart,
            hsStart,
            runoverStart,
            unarmedStart,
            baseballStart,
            poolcueStart,
            knifeStart,
            pistolStart,
            grenadeStart,
            molotovStart,
            rocketStart,
            deagleStart,
            shotgunStart,
            shotgun2Start,
            smgStart,
            mp5Start,
            m4Start,
            ak47Start,
            sniperStart,
            sniper2Start,
            rpgStart,
            tickCounter,
            maxKillNotifications = 8,
            tempBool1 = 0;
        
        string[] surnameArray;
        float killStayTime = 10f;

        bool showDbgTexts = true,
             showIcons = true, 
             killingSpreePlayed = false,
             dominatingPlayed = false,
             wickedSickPlayed = false,
             modEnabled = true,
             startModAtStartup = true,
             playSound = true,
             useHsModifier = true,
             deathEventBool = false;

        GTA.Timer multiKillTimer;
        GameEpisode currentEpisode = GameEpisode.GTAIV;

        #endregion

        #region constructor
        public FPSmod()
        {
            gameMode = GTA.Native.Function.Call<int>("NETWORK_GET_GAME_MODE");
            if (gameMode != -1)
            {
                Game.Console.Print("[FPSmod]: not in singleplayer, thus disabled!");
            }
            currentEpisode = Game.CurrentEpisode;
            if (currentEpisode != GameEpisode.GTAIV)
            {
                Game.Console.Print("[FPSmod]: not in standard GTA:IV!");
                Game.DisplayText("Be advised: Kill-Icons is untested for EFLC!");
            }
            multiKillTimer = new GTA.Timer(MULTIKILL_TIMELIMIT, false);
            killEventArray = new killEvent[50];
            ownageSound = new SoundPlayer();
            ownageSound.Stream = new System.IO.MemoryStream(loadFile("./scripts/sounds/ownage.wav"));
            comboWhoreSound = new SoundPlayer();
            comboWhoreSound.Stream = new System.IO.MemoryStream(loadFile("./scripts/sounds/combowhore.wav"));
            headshotSoundPlayer = new SoundPlayer();
            headshotSoundPlayer.Stream = new System.IO.MemoryStream(loadFile("./scripts/sounds/headshot.wav"));
            firstBloodSound = new SoundPlayer();
            firstBloodSound.Stream = new System.IO.MemoryStream(loadFile("./scripts/sounds/firstblood.wav"));
            multiKillSound = new SoundPlayer();
            multiKillSound.Stream = new System.IO.MemoryStream(loadFile("./scripts/sounds/multikill.wav"));
            killingSpreeSound = new SoundPlayer();
            killingSpreeSound.Stream = new System.IO.MemoryStream(loadFile("./scripts/sounds/killingspree.wav"));
            dominatingSound = new SoundPlayer();
            dominatingSound.Stream = new System.IO.MemoryStream(loadFile("./scripts/sounds/dominating.wav"));
            humiliationSound = new SoundPlayer();
            humiliationSound.Stream = new System.IO.MemoryStream(loadFile("./scripts/sounds/humiliation.wav"));
            megaKillSound = new SoundPlayer();
            megaKillSound.Stream = new System.IO.MemoryStream(loadFile("./scripts/sounds/megakill.wav"));
            unstoppableSound = new SoundPlayer();
            unstoppableSound.Stream = new System.IO.MemoryStream(loadFile("./scripts/sounds/unstoppable.wav"));
            wickedSickSound = new SoundPlayer();
            wickedSickSound.Stream = new System.IO.MemoryStream(loadFile("./scripts/sounds/wickedsick.wav"));
            holyShitSound = new SoundPlayer();
            holyShitSound.Stream = new System.IO.MemoryStream(loadFile("./scripts/sounds/holyshit.wav"));
            godlikeSound = new SoundPlayer();
            godlikeSound.Stream = new System.IO.MemoryStream(loadFile("./scripts/sounds/godlike.wav"));
            rampageSound = new SoundPlayer();
            rampageSound.Stream = new System.IO.MemoryStream(loadFile("./scripts/sounds/rampage.wav"));
            GeneralInfo = "kill announcer by thaCURSEDpie";
            Interval = 100; // Interval is the time between two Ticks (in milliseconds)
            this.Tick += new EventHandler(this.TickEvent);
            this.PerFrameDrawing += new GraphicsEventHandler(this.OnPerFrameDrawing);
            BindConsoleCommand("fpsmod_mute", new ConsoleCommandDelegate(mute_console), "- disable killsounds");
            BindConsoleCommand("fpsmod_unmute", new ConsoleCommandDelegate(unmute_console), "- disable killsounds");
            BindConsoleCommand("fpsmod_on", new ConsoleCommandDelegate(modOnConsole), "- Enable FPSmod.");
            BindConsoleCommand("fpsmod_off", new ConsoleCommandDelegate(modOffConsole), "- Disable FPSmod.");
            BindConsoleCommand("fpsmod_hidedebugtexts", new ConsoleCommandDelegate(hidedbgtexts_Console), "- Dont show debug texts.");
            BindConsoleCommand("fpsmod_showdebugtexts", new ConsoleCommandDelegate(showdbgtexts_Console), "- Do show debug texts.");
            BindConsoleCommand("fpsmod_reload", new ConsoleCommandDelegate(reloadSettingsConsole), "- Reload FPSmod settings.");
            BindConsoleCommand("fpsmod_hideicons", new ConsoleCommandDelegate(hideicons_console), "- Dont show kill icons");
            BindConsoleCommand("fpsmod_showicons", new ConsoleCommandDelegate(showicons_console), "- Do show kill icons");
            startUp();
        }

        #endregion

        #region mod control
        private void reloadSettingsConsole(ParameterCollection Parameter)
        {
            reloadSettings();
            
        }

        private void hidedbgtexts_Console(ParameterCollection Parameter)
        {
            if (showDbgTexts)
            {
                showDbgTexts = !showDbgTexts;
                Game.Console.Print("[FPSmod]: Debug texts are hidden");
            }
            else
            {
                Game.Console.Print("[FPSmod]: Debug texts are already hidden!");
            }
        }

        private void showdbgtexts_Console(ParameterCollection Parameter)
        {
            if (!showDbgTexts)
            {
                showDbgTexts = !showDbgTexts;
                Game.Console.Print("[FPSmod]: Debug texts are being show");
            }
            else
            {
                Game.Console.Print("[FPSmod]: Debug texts are already being shown");
            }
        }


        private void hideicons_console(ParameterCollection Parameter)
        {
            if (showIcons)
            {
                showIcons = !showIcons;
                Game.Console.Print("[FPSmod]: Icons are hidden");
            }
            else
            {
                Game.Console.Print("[FPSmod]: Icons are already hidden!");
            }
        }

        private void showicons_console(ParameterCollection Parameter)
        {
            if (!showIcons)
            {
                showIcons = !showIcons;
                Game.Console.Print("[FPSmod]: Icons are shown");
            }
            else
            {
                Game.Console.Print("[FPSmod]: Icons are already shown!");
            }
        }

        private void mute_console(ParameterCollection Parameter)
        {
            if (playSound)
            {
                playSound = !playSound;
                Game.Console.Print("[FPSmod]: Mod has been muted");
            }
            else
            {
                Game.Console.Print("[FPSmod]: Mod is already muted!");
            }
        }

        private void unmute_console(ParameterCollection Parameter)
        {
            if (!playSound)
            {
                playSound = !playSound;
                Game.Console.Print("[FPSmod]: Mod has been unmuted");
            }
            else
            {
                Game.Console.Print("[FPSmod]: Mod is already unmuted!");
            }
        }

        private void modOnConsole(ParameterCollection Parameter)
        {
            if (!modEnabled)
            {
                modEnabled = !modEnabled;
                numberOfkillsStart = getIntStat(289);
                Game.Console.Print("[FPSmod]: Mod enabled");
            }
            else
            {
                Game.Console.Print("[FPSmod]: Mod is already active!");
            }
        }

        private void modOffConsole(ParameterCollection Parameter)
        {
            if (modEnabled)
            {
                modEnabled = !modEnabled;
                Game.Console.Print("[FPSmod]: Mod disabled");
            }
            else
            {
                Game.Console.Print("[FPSmod]: Mod is already disabled!");
            }
        }

        #endregion

        #region loading
        private bool reloadSettings()
        {
            Settings.Load();
            tempBool1 = Settings.GetValueInteger("loadConfirmation", "advanced", 1);

            for (int i = 0; i < numberOfSurnames; i++)
            {
                surnameArray[i] = Settings.GetValueString(i.ToString(), "names", "");
            }

            startModAtStartup = Settings.GetValueBool("startModOnStartup", "variables", true);
            playSound = Settings.GetValueBool("playSound", "variables", true);
            showIcons = Settings.GetValueBool("showIcons", "variables", true);
            killStayTime = Settings.GetValueFloat("textStayTime", "advanced", 30.0f);
            showDbgTexts = Settings.GetValueBool("showDbgTexts", "advanced", true);

            Settings.Load();

            if (tempBool1 != 0)
            {
                Game.Console.Print("[FPSmod]: settings file not found!");
                return false;
            }

            Game.Console.Print("[FPSmod]: settings successfully (re)loaded");
            return true;
        }

        private void loadTextures()
        {
            pistol44 = new Texture(loadFile("./scripts/png/pistol44.png"));
            aa12 = new Texture(loadFile("./scripts/png/aa12.png"));
            assault_shotgun = new Texture(loadFile("./scripts/png/assault_shotgun.png"));
            p90 = new Texture(loadFile("./scripts/png/p90.png"));
            grenadelauncher = new Texture(loadFile("./scripts/png/grenadelauncher.png"));
            auto_pistol = new Texture(loadFile("./scripts/png/auto_pistol.png"));
            advanced_sniper = new Texture(loadFile("./scripts/png/advanced_sniper.png"));
            poolcue = new Texture(loadFile("./scripts/png/poolcue.png"));
            sticky = new Texture(loadFile("./scripts/png/sticky.png"));
            sawnoff_shotgun = new Texture(loadFile("./scripts/png/sawnoff_shotgun.png"));
            golden_smg = new Texture(loadFile("./scripts/png/golden_smg.png"));
            lmg = new Texture(loadFile("./scripts/png/lmg.png"));
            pipebomb = new Texture(loadFile("./scripts/png/pipebomb.png"));

            headshotModifier = new Texture(loadFile("./scripts/png/headshot_modifier.png"));
            vehicle_explosion = new Texture(loadFile("./scripts/png/vehicle_explosion.png"));
            pistol = new Texture(loadFile("./scripts/png/pistol.png"));
            headshot = new Texture(loadFile("./scripts/png/headshot.png"));
            rpg = new Texture(loadFile("./scripts/png/rpg.png"));
            runover = new Texture(loadFile("./scripts/png/runover.png"));
            unarmed = new Texture(loadFile("./scripts/png/unarmed.png"));
            baseball = new Texture(loadFile("./scripts/png/baseball.png"));
            knife = new Texture(loadFile("./scripts/png/knife.png"));
            grenade = new Texture(loadFile("./scripts/png/grenade.png"));
            molotov = new Texture(loadFile("./scripts/png/molotov.png"));
            deagle = new Texture(loadFile("./scripts/png/deagle.png"));
            shotgun = new Texture(loadFile("./scripts/png/shotgun.png"));
            combat_shotgun = new Texture(loadFile("./scripts/png/combat_shotgun.png"));
            micro_smg = new Texture(loadFile("./scripts/png/micro_smg.png"));
            mp5 = new Texture(loadFile("./scripts/png/mp5.png"));
            sniper = new Texture(loadFile("./scripts/png/sniper.png"));
            combat_sniper = new Texture(loadFile("./scripts/png/combat_sniper.png"));
            ak47 = new Texture(loadFile("./scripts/png/ak47.png"));
            m4 = new Texture(loadFile("./scripts/png/m4.png"));
        }

        #endregion

        #region mod logic
        private void startUp()
        {
            killNotificationArray = new killNotification[maxKillNotifications];
            surnameArray = new string[numberOfSurnames];
            for (int i = 0; i < killNotificationArray.Length; i++)
            {
                killNotificationArray[i] = new killNotification();
            }
            loadTextures();
            //killTexture = Resources.GetTexture("kill.png");
            reloadSettings();
            modEnabled = startModAtStartup;
            getStatsStart();

            
            //testTexture = new Texture(loadFile("./scripts/HSfiles/chef_3.BMP"));
        }

        private int getAvailableKillNotification()
        {
            for (int i = 0; i < killNotificationArray.Length; i++)
            {
                if (killNotificationArray[i].isVisible == false)
                {
                    return i;
                }
            }
            // all spots are taken. So we shuffle everything one up:
            shuffleNotifications();
            
            // and we return the highest i

            return (killNotificationArray.Length - 1);
        }

        private void shuffleNotifications()
        {
            for (int i = 0; i < killNotificationArray.Length - 1; i++)
            {
                killNotificationArray[i] = killNotificationArray[i + 1];
            }
            killNotificationArray[killNotificationArray.Length - 1].isVisible = false;
        }

        private string getKillType()
        {
            int hsTemp = getIntStat((int)gtaStats.STAT_KILLS_BY_HEADSHOTS);
            int runoverTemp = getIntStat((int)gtaStats.STAT_PEOPLE_RUN_DOWN);
            int unarmedTemp = getIntStat((int)gtaStats.STAT_KILLS_WITH_UNARMED);
            int baseballTemp = getIntStat((int)gtaStats.STAT_KILLS_WITH_BASEBALL_BAT);
            int poolcueTemp = getIntStat((int)gtaStats.STAT_KILLS_WITH_POOLCUE);
            int knifeTemp = getIntStat((int)gtaStats.STAT_KILLS_WITH_KNIFE);
            int pistolTemp = getIntStat((int)gtaStats.STAT_KILLS_WITH_PISTOL);
            int grenadeTemp = getIntStat((int)gtaStats.STAT_KILLS_WITH_GRENADE);
            int molotovTemp = getIntStat((int)gtaStats.STAT_KILLS_WITH_MOLOTOV);
            int rocketTemp = getIntStat((int)gtaStats.STAT_KILLS_WITH_ROCKET);
            int deagleTemp = getIntStat((int)gtaStats.STAT_KILLS_WITH_COMBAT_PISTOL);
            int shotgunTemp = getIntStat((int)gtaStats.STAT_KILLS_WITH_PUMP_SHOTGUN);
            int shotgun2Temp = getIntStat((int)gtaStats.STAT_KILLS_WITH_COMBAT_SHOTGUN);
            int smgTemp = getIntStat((int)gtaStats.STAT_KILLS_WITH_MICRO_SMG);
            int mp5Temp = getIntStat((int)gtaStats.STAT_KILLS_WITH_SMG);
            int m4Temp = getIntStat((int)gtaStats.STAT_KILLS_WITH_CARBINE_RIFLE);
            int ak47Temp = getIntStat((int)gtaStats.STAT_KILLS_WITH_ASSAULT_RIFLE);
            int sniperTemp = getIntStat((int)gtaStats.STAT_KILLS_WITH_SNIPER_RIFLE);
            int sniper2Temp = getIntStat((int)gtaStats.STAT_KILLS_WITH_COMBAT_SNIPER);
            int rpgTemp = getIntStat((int)gtaStats.STAT_KILLS_WITH_RPG);

            int aa12regularTemp = getIntStat((int)gtaStats.STAT_KILLS_WITH_AA12_REGULAR);
            int aa12explosiveTemp = getIntStat((int)gtaStats.STAT_KILLS_WITH_AA12_EXPLOSIVE);
            int auto_pistolTemp = getIntStat((int)gtaStats.STAT_KILLS_WITH_AUTO_PISTOL);
            int assault_shotgunTemp = getIntStat((int)gtaStats.STAT_KILLS_WITH_ASSAULT_SHOTGUN);
            int p90Temp = getIntStat((int)gtaStats.STAT_KILLS_WITH_P90);
            int pipebombTemp = getIntStat((int)gtaStats.STAT_KILLS_WITH_PIPEBOMB);
            int pistol44Temp = getIntStat((int)gtaStats.STAT_KILLS_WITH_PISTOL44);
            int pool_cueTemp = getIntStat((int)gtaStats.STAT_KILLS_WITH_POOL_CUE);
            int lmgTemp = getIntStat((int)gtaStats.STAT_KILLS_WITH_LMG);
            int golden_smgTemp = getIntStat((int)gtaStats.STAT_KILLS_WITH_GOLDEN_SMG);
            int advanced_sniperTemp = getIntStat((int)gtaStats.STAT_KILLS_WITH_ADVANCED_SNIPER);
            int grenadelauncherTemp = getIntStat((int)gtaStats.STAT_KILLS_WITH_GRENADELAUNCHER);
            int sawnoff_shotgunTemp = getIntStat((int)gtaStats.STAT_KILLS_WITH_SAWNOFF_SHOTGUN);
            int stickyTemp = getIntStat((int)gtaStats.STAT_KILLS_WITH_STICKY);

            if (showDbgTexts)
            {
                if (hsTemp > hsStart)
                {
                    /*
                    pistolStart = pistolTemp;
                    deagleStart = deagleTemp;
                    smgStart = smgTemp;
                    mp5Start = mp5Temp;
                    m4Start = m4Temp;
                    ak47Start = ak47Temp;
                    sniperStart = sniperTemp;
                    sniper2Start = sniper2Temp;
                     */
                    hsStart += 1;
                    return "headshot";
                }
                if (runoverTemp > runoverStart)
                {
                    if (Player.Character.isInVehicle())
                    {
                        //runoverStart = runoverTemp;
                        Game.Console.Print("[FPSmod-dbg]: runover kill: current stat: " + runoverTemp.ToString() + " runoverStart: " + runoverStart.ToString());
                        runoverStart += 1;
                        return "runover";
                    }

                }

                if (unarmedTemp > unarmedStart)
                {
                    //unarmedStart = unarmedTemp;
                    Game.Console.Print("[FPSmod-dbg]: unarmed kill: current stat: " + unarmedTemp.ToString() + " unarmedStart: " + unarmedStart.ToString());
                    unarmedStart += 1;
                    return "unarmed";
                }
                if (baseballTemp > baseballStart)
                {
                    //baseballStart = baseballTemp;
                    //Game.Console.Print("[FPSdbg]: \nnot.#\n " + notificationNumber.ToString() + " \nktype:\n " + killNotificationArray[notificationNumber].Weapon + " \nvictim:\n " + killNotificationArray[notificationNumber].Victim + " \nattacker:\n " + killNotificationArray[notificationNumber].Attacker);
                    Game.Console.Print("[FPSmod-dbg]: baseball kill: current stat: " + baseballTemp.ToString() + " baseballStart: " + baseballStart.ToString());
                    baseballStart += 1;
                    return "baseball";
                }
                if (aa12explosiveTemp > aa12explosiveStart)
                {
                    Game.Console.Print("[FPSmod-dbg]: aa12expl kill: current stat: " + aa12explosiveTemp.ToString() + " aa12explosiveStart: " + aa12explosiveStart.ToString());
                    aa12explosiveStart += 1;
                    return "aa12explosive";
                }
                if (aa12regularTemp > aa12regularStart)
                {
                    Game.Console.Print("[FPSmod-dbg]: aa12reg kill: current stat: " + aa12regularTemp.ToString() + " aa12regularStart: " + aa12regularStart.ToString());
                    aa12regularStart += 1;
                    return "aa12regular";
                }
                if (auto_pistolTemp > auto_pistolStart)
                {
                    Game.Console.Print("[FPSmod-dbg]: auto pistol kill: current stat: " + auto_pistolTemp.ToString() + " auto_pistolStart: " + auto_pistolStart.ToString());
                    auto_pistolStart += 1;
                    return "auto_pistol";
                }
                if (assault_shotgunTemp > assault_shotgunStart)
                {
                    Game.Console.Print("[FPSmod-dbg]: assault_shotgun kill: current stat: " + assault_shotgunTemp.ToString() + " assaultshotgunStart: " + assault_shotgunStart.ToString());
                    assault_shotgunStart += 1;
                    return "assault_shotgun";
                }
                if (p90Temp > p90Start)
                {
                    Game.Console.Print("[FPSmod-dbg]: p90 kill: current stat: " + p90Temp.ToString() + " p90Start: " + p90Start.ToString());
                    p90Start += 1;
                    return "p90";
                }
                if (pipebombTemp > pipebombStart)
                {
                    Game.Console.Print("[FPSmod-dbg]: pipebomb kill: current stat: " + pipebombTemp.ToString() + " pipebombStart: " + pipebombStart.ToString());
                    pipebombStart += 1;
                    return "pipebomb";
                }
                if (pistol44Temp > pistol44Start)
                {
                    Game.Console.Print("[FPSmod-dbg]: pistol44 kill: current stat: " + pistol44Temp.ToString() + " pistol44Start: " + pistol44Start.ToString());
                    pistol44Start += 1;
                    return "pistol44";
                }
                if (pool_cueTemp > pool_cueStart)
                {
                    Game.Console.Print("[FPSmod-dbg]: pool_cue kill: current stat: " + pool_cueTemp.ToString() + " pool_cueStart: " + pool_cueStart.ToString());
                    pool_cueStart += 1;
                    return "pool_cue";
                }
                if (lmgTemp > lmgStart)
                {
                    Game.Console.Print("[FPSmod-dbg]: lmg kill: current stat: " + lmgTemp.ToString() + " lmgStart: " + lmgStart.ToString());
                    lmgStart += 1;
                    return "lmg";
                }
                if (golden_smgTemp > golden_smgStart)
                {
                    Game.Console.Print("[FPSmod-dbg]: golden_smg kill: current stat: " + golden_smgTemp.ToString() + " golden_smgStart: " + golden_smgStart.ToString());
                    golden_smgStart += 1;
                    return "golden_smg";
                }
                if (advanced_sniperTemp > advanced_sniperStart)
                {
                    Game.Console.Print("[FPSmod-dbg]: advanced_sniper kill: current stat: " + advanced_sniperTemp.ToString() + " advanced_sniperStart: " + advanced_sniperStart.ToString());
                    advanced_sniperStart += 1;
                    return "advanced_sniper";
                }
                if (grenadelauncherTemp > grenadelauncherStart)
                {
                    Game.Console.Print("[FPSmod-dbg]: glauncher kill: current stat: " + grenadelauncherTemp.ToString() + " glauncherStart: " + grenadelauncherStart.ToString());
                    grenadelauncherStart += 1;
                    return "grenadelauncher";
                }
                if (sawnoff_shotgunTemp > sawnoff_shotgunStart)
                {
                    Game.Console.Print("[FPSmod-dbg]: sawnoff_shotgun kill: current stat: " + sawnoff_shotgunTemp.ToString() + " sawnoff_shotgunStart: " + sawnoff_shotgunStart.ToString());
                    sawnoff_shotgunStart += 1;
                    return "sawnoff_shotgun";
                }
                if (stickyTemp > stickyStart)
                {
                    Game.Console.Print("[FPSmod-dbg]: sticky kill: current stat: " + stickyTemp.ToString() + " stickyrStart: " + stickyStart.ToString());
                    stickyStart += 1;
                    return "sticky";
                }
                if (poolcueTemp > poolcueStart)
                {
                    Game.Console.Print("[FPSmod-dbg]: poolcue kill: current stat: " + poolcueTemp.ToString() + " poolcueStart: " + poolcueStart.ToString());
                    //poolcueStart = poolcueTemp;
                    poolcueStart += 1;
                    return "poolcue";
                }
                if (knifeTemp > knifeStart)
                {
                    //knifeStart = knifeTemp;
                    Game.Console.Print("[FPSmod-dbg]: knife kill: current stat: " + knifeTemp.ToString() + " knifeStart: " + knifeStart.ToString());
                    knifeStart += 1;
                    return "knife";
                }
                if (pistolTemp > pistolStart)
                {
                    //pistolStart = pistolTemp;
                    Game.Console.Print("[FPSmod-dbg]: pistol kill: current stat: " + pistolTemp.ToString() + " pistolStart: " + pistolStart.ToString());
                    pistolStart += 1;
                    return "pistol";
                }
                if (grenadeTemp > grenadeStart)
                {
                    Game.Console.Print("[FPSmod-dbg]: grenade kill: current stat: " + grenadeTemp.ToString() + " grenadeStart: " + grenadeStart.ToString());
                    //grenadeStart = grenadeTemp;
                    grenadeStart += 1;
                    return "grenade";
                }
                if (molotovTemp > molotovStart)
                {
                    Game.Console.Print("[FPSmod-dbg]: molotov kill: current stat: " + molotovTemp.ToString() + " molotovStart: " + molotovStart.ToString());
                    //molotovStart = molotovTemp;
                    molotovStart += 1;
                    return "molotov";
                }
                if (rocketTemp > rocketStart)
                {
                    Game.Console.Print("[FPSmod-dbg]: rocket kill: current stat: " + rocketTemp.ToString() + " rocketStart: " + rocketStart.ToString());
                    //rocketStart = rocketTemp;
                    rocketStart += 1;
                    return "rocket";
                }
                if (deagleTemp > deagleStart)
                {
                    Game.Console.Print("[FPSmod-dbg]: deagle kill: current stat: " + grenadeTemp.ToString() + " deagleStart: " + deagleStart.ToString());
                    //deagleStart = deagleTemp;
                    deagleStart += 1;
                    return "deagle";
                }
                if (shotgunTemp > shotgunStart)
                {
                    Game.Console.Print("[FPSmod-dbg]: shotgun kill: current stat: " + shotgunTemp.ToString() + " shotgunStart: " + shotgunStart.ToString());
                    //shotgunStart = shotgunTemp;
                    shotgunStart += 1;
                    return "shotgun";
                }
                if (shotgun2Temp > shotgun2Start)
                {
                    Game.Console.Print("[FPSmod-dbg]: shotgun2 kill: current stat: " + shotgun2Temp.ToString() + " shotgun2Start: " + shotgun2Start.ToString());
                    //shotgun2Start = shotgun2Temp;
                    shotgun2Start += 1;
                    return "combat_shotgun";
                }
                if (smgTemp > smgStart)
                {
                    Game.Console.Print("[FPSmod-dbg]: smg kill: current stat: " + smgTemp.ToString() + " smgStart: " + smgStart.ToString());
                    //smgStart = smgTemp;
                    smgStart += 1;
                    return "micro_smg";
                }
                if (mp5Temp > mp5Start)
                {
                    Game.Console.Print("[FPSmod-dbg]: mp5 kill: current stat: " + mp5Temp.ToString() + " mp5Start: " + mp5Start.ToString());
                    //mp5Start = mp5Temp;
                    mp5Start += 1;
                    return "mp5";
                }
                if (m4Temp > m4Start)
                {
                    Game.Console.Print("[FPSmod-dbg]: m4 kill: current stat: " + m4Temp.ToString() + " m4Start: " + m4Start.ToString());
                    //m4Start = m4Temp;
                    m4Start += 1;
                    return "m4";
                }
                if (ak47Temp > ak47Start)
                {
                    Game.Console.Print("[FPSmod-dbg]: ak47 kill: current stat: " + ak47Temp.ToString() + " ak47Start: " + ak47Start.ToString());
                    //ak47Start = ak47Temp;
                    ak47Start += 1;
                    return "ak47";
                }
                if (sniperTemp > sniperStart)
                {
                    Game.Console.Print("[FPSmod-dbg]: sniper kill: current stat: " + sniperTemp.ToString() + " sniperStart: " + sniperStart.ToString());
                    //sniperStart = sniperTemp;
                    sniperStart += 1;
                    return "sniper";
                }
                if (sniper2Temp > sniper2Start)
                {
                    Game.Console.Print("[FPSmod-dbg]: sniper2 kill: current stat: " + sniper2Temp.ToString() + " sniper2Start: " + sniper2Start.ToString());
                    //sniper2Start = sniper2Temp;
                    sniper2Start += 1;
                    return "combat_sniper";
                }
                if (rpgTemp > rpgStart)
                {
                    Game.Console.Print("[FPSmod-dbg]: rpg kill: current stat: " + rpgTemp.ToString() + " rpgStart: " + rpgStart.ToString());
                    //rpgStart = rpgTemp;
                    rpgStart += 1;
                    return "rpg";
                }

                if (Player.Character.isInVehicle())
                {
                    Game.Console.Print("[FPSmod-dbg]: fake runover kill: current stat: " + runoverTemp.ToString() + " runoverStart: " + runoverStart.ToString());
                    return "runover";
                }
                else
                {
                    return "vehicle_explosion";
                }
            }
            else
            {
                if (hsTemp > hsStart)
                {
                    /*
                    pistolStart = pistolTemp;
                    deagleStart = deagleTemp;
                    smgStart = smgTemp;
                    mp5Start = mp5Temp;
                    m4Start = m4Temp;
                    ak47Start = ak47Temp;
                    sniperStart = sniperTemp;
                    sniper2Start = sniper2Temp;
                     */
                    hsStart += 1;
                    return "headshot";
                }
                if (runoverTemp > runoverStart)
                {
                    if (Player.Character.isInVehicle())
                    {
                        //runoverStart = runoverTemp;
                       // Game.Console.Print("[FPSmod-dbg]: runover kill: current stat: " + runoverTemp.ToString() + " runoverStart: " + runoverStart.ToString());
                        runoverStart += 1;
                        return "runover";
                    }

                }

                if (unarmedTemp > unarmedStart)
                {
                    //unarmedStart = unarmedTemp;
                    //Game.Console.Print("[FPSmod-dbg]: unarmed kill: current stat: " + unarmedTemp.ToString() + " unarmedStart: " + unarmedStart.ToString());
                    unarmedStart += 1;
                    return "unarmed";
                }
                if (baseballTemp > baseballStart)
                {
                    //baseballStart = baseballTemp;
                    //Game.Console.Print("[FPSdbg]: \nnot.#\n " + notificationNumber.ToString() + " \nktype:\n " + killNotificationArray[notificationNumber].Weapon + " \nvictim:\n " + killNotificationArray[notificationNumber].Victim + " \nattacker:\n " + killNotificationArray[notificationNumber].Attacker);
                    //Game.Console.Print("[FPSmod-dbg]: baseball kill: current stat: " + baseballTemp.ToString() + " baseballStart: " + baseballStart.ToString());
                    baseballStart += 1;
                    return "baseball";
                }
                if (aa12explosiveTemp > aa12explosiveStart)
                {
                    //Game.Console.Print("[FPSmod-dbg]: aa12expl kill: current stat: " + aa12explosiveTemp.ToString() + " aa12explosiveStart: " + aa12explosiveStart.ToString());
                    aa12explosiveStart += 1;
                    return "aa12explosive";
                }
                if (aa12regularTemp > aa12regularStart)
                {
                    //Game.Console.Print("[FPSmod-dbg]: aa12reg kill: current stat: " + aa12regularTemp.ToString() + " aa12regularStart: " + aa12regularStart.ToString());
                    aa12regularStart += 1;
                    return "aa12regular";
                }
                if (auto_pistolTemp > auto_pistolStart)
                {
                    //Game.Console.Print("[FPSmod-dbg]: auto pistol kill: current stat: " + auto_pistolTemp.ToString() + " auto_pistolStart: " + auto_pistolStart.ToString());
                    auto_pistolStart += 1;
                    return "auto_pistol";
                }
                if (assault_shotgunTemp > assault_shotgunStart)
                {
                    //Game.Console.Print("[FPSmod-dbg]: assault_shotgun kill: current stat: " + assault_shotgunTemp.ToString() + " assaultshotgunStart: " + assault_shotgunStart.ToString());
                    assault_shotgunStart += 1;
                    return "assault_shotgun";
                }
                if (p90Temp > p90Start)
                {
                    //Game.Console.Print("[FPSmod-dbg]: p90 kill: current stat: " + p90Temp.ToString() + " p90Start: " + p90Start.ToString());
                    p90Start += 1;
                    return "p90";
                }
                if (pipebombTemp > pipebombStart)
                {
                    //Game.Console.Print("[FPSmod-dbg]: pipebomb kill: current stat: " + pipebombTemp.ToString() + " pipebombStart: " + pipebombStart.ToString());
                    pipebombStart += 1;
                    return "pipebomb";
                }
                if (pistol44Temp > pistol44Start)
                {
                    //Game.Console.Print("[FPSmod-dbg]: pistol44 kill: current stat: " + pistol44Temp.ToString() + " pistol44Start: " + pistol44Start.ToString());
                    pistol44Start += 1;
                    return "pistol44";
                }
                if (pool_cueTemp > pool_cueStart)
                {
                    //Game.Console.Print("[FPSmod-dbg]: pool_cue kill: current stat: " + pool_cueTemp.ToString() + " pool_cueStart: " + pool_cueStart.ToString());
                    pool_cueStart += 1;
                    return "pool_cue";
                }
                if (lmgTemp > lmgStart)
                {
                    //Game.Console.Print("[FPSmod-dbg]: lmg kill: current stat: " + lmgTemp.ToString() + " lmgStart: " + lmgStart.ToString());
                    lmgStart += 1;
                    return "lmg";
                }
                if (golden_smgTemp > golden_smgStart)
                {
                    //Game.Console.Print("[FPSmod-dbg]: golden_smg kill: current stat: " + golden_smgTemp.ToString() + " golden_smgStart: " + golden_smgStart.ToString());
                    golden_smgStart += 1;
                    return "golden_smg";
                }
                if (advanced_sniperTemp > advanced_sniperStart)
                {
                    //Game.Console.Print("[FPSmod-dbg]: advanced_sniper kill: current stat: " + advanced_sniperTemp.ToString() + " advanced_sniperStart: " + advanced_sniperStart.ToString());
                    advanced_sniperStart += 1;
                    return "advanced_sniper";
                }
                if (grenadelauncherTemp > grenadelauncherStart)
                {
                    //Game.Console.Print("[FPSmod-dbg]: glauncher kill: current stat: " + grenadelauncherTemp.ToString() + " glauncherStart: " + grenadelauncherStart.ToString());
                    grenadelauncherStart += 1;
                    return "grenadelauncher";
                }
                if (sawnoff_shotgunTemp > sawnoff_shotgunStart)
                {
                    //Game.Console.Print("[FPSmod-dbg]: sawnoff_shotgun kill: current stat: " + sawnoff_shotgunTemp.ToString() + " sawnoff_shotgunStart: " + sawnoff_shotgunStart.ToString());
                    sawnoff_shotgunStart += 1;
                    return "sawnoff_shotgun";
                }
                if (stickyTemp > stickyStart)
                {
                    //Game.Console.Print("[FPSmod-dbg]: sticky kill: current stat: " + stickyTemp.ToString() + " stickyrStart: " + stickyStart.ToString());
                    stickyStart += 1;
                    return "sticky";
                }
                if (poolcueTemp > poolcueStart)
                {
                    //Game.Console.Print("[FPSmod-dbg]: poolcue kill: current stat: " + poolcueTemp.ToString() + " poolcueStart: " + poolcueStart.ToString());
                    //poolcueStart = poolcueTemp;
                    poolcueStart += 1;
                    return "poolcue";
                }
                if (knifeTemp > knifeStart)
                {
                    //knifeStart = knifeTemp;
                    //Game.Console.Print("[FPSmod-dbg]: knife kill: current stat: " + knifeTemp.ToString() + " knifeStart: " + knifeStart.ToString());
                    knifeStart += 1;
                    return "knife";
                }
                if (pistolTemp > pistolStart)
                {
                    //pistolStart = pistolTemp;
                    //Game.Console.Print("[FPSmod-dbg]: pistol kill: current stat: " + pistolTemp.ToString() + " pistolStart: " + pistolStart.ToString());
                    pistolStart += 1;
                    return "pistol";
                }
                if (grenadeTemp > grenadeStart)
                {
                    //Game.Console.Print("[FPSmod-dbg]: grenade kill: current stat: " + grenadeTemp.ToString() + " grenadeStart: " + grenadeStart.ToString());
                    //grenadeStart = grenadeTemp;
                    grenadeStart += 1;
                    return "grenade";
                }
                if (molotovTemp > molotovStart)
                {
                    //Game.Console.Print("[FPSmod-dbg]: molotov kill: current stat: " + molotovTemp.ToString() + " molotovStart: " + molotovStart.ToString());
                    //molotovStart = molotovTemp;
                    molotovStart += 1;
                    return "molotov";
                }
                if (rocketTemp > rocketStart)
                {
                    //Game.Console.Print("[FPSmod-dbg]: rocket kill: current stat: " + rocketTemp.ToString() + " rocketStart: " + rocketStart.ToString());
                    //rocketStart = rocketTemp;
                    rocketStart += 1;
                    return "rocket";
                }
                if (deagleTemp > deagleStart)
                {
                    //Game.Console.Print("[FPSmod-dbg]: deagle kill: current stat: " + grenadeTemp.ToString() + " deagleStart: " + deagleStart.ToString());
                    //deagleStart = deagleTemp;
                    deagleStart += 1;
                    return "deagle";
                }
                if (shotgunTemp > shotgunStart)
                {
                    //Game.Console.Print("[FPSmod-dbg]: shotgun kill: current stat: " + shotgunTemp.ToString() + " shotgunStart: " + shotgunStart.ToString());
                    //shotgunStart = shotgunTemp;
                    shotgunStart += 1;
                    return "shotgun";
                }
                if (shotgun2Temp > shotgun2Start)
                {
                    //Game.Console.Print("[FPSmod-dbg]: shotgun2 kill: current stat: " + shotgun2Temp.ToString() + " shotgun2Start: " + shotgun2Start.ToString());
                    //shotgun2Start = shotgun2Temp;
                    shotgun2Start += 1;
                    return "combat_shotgun";
                }
                if (smgTemp > smgStart)
                {
                    //Game.Console.Print("[FPSmod-dbg]: smg kill: current stat: " + smgTemp.ToString() + " smgStart: " + smgStart.ToString());
                    //smgStart = smgTemp;
                    smgStart += 1;
                    return "micro_smg";
                }
                if (mp5Temp > mp5Start)
                {
                    //Game.Console.Print("[FPSmod-dbg]: mp5 kill: current stat: " + mp5Temp.ToString() + " mp5Start: " + mp5Start.ToString());
                    //mp5Start = mp5Temp;
                    mp5Start += 1;
                    return "mp5";
                }
                if (m4Temp > m4Start)
                {
                    //Game.Console.Print("[FPSmod-dbg]: m4 kill: current stat: " + m4Temp.ToString() + " m4Start: " + m4Start.ToString());
                    //m4Start = m4Temp;
                    m4Start += 1;
                    return "m4";
                }
                if (ak47Temp > ak47Start)
                {
                    //Game.Console.Print("[FPSmod-dbg]: ak47 kill: current stat: " + ak47Temp.ToString() + " ak47Start: " + ak47Start.ToString());
                    //ak47Start = ak47Temp;
                    ak47Start += 1;
                    return "ak47";
                }
                if (sniperTemp > sniperStart)
                {
                    //Game.Console.Print("[FPSmod-dbg]: sniper kill: current stat: " + sniperTemp.ToString() + " sniperStart: " + sniperStart.ToString());
                    //sniperStart = sniperTemp;
                    sniperStart += 1;
                    return "sniper";
                }
                if (sniper2Temp > sniper2Start)
                {
                    //Game.Console.Print("[FPSmod-dbg]: sniper2 kill: current stat: " + sniper2Temp.ToString() + " sniper2Start: " + sniper2Start.ToString());
                    //sniper2Start = sniper2Temp;
                    sniper2Start += 1;
                    return "combat_sniper";
                }
                if (rpgTemp > rpgStart)
                {
                    //Game.Console.Print("[FPSmod-dbg]: rpg kill: current stat: " + rpgTemp.ToString() + " rpgStart: " + rpgStart.ToString());
                    //rpgStart = rpgTemp;
                    rpgStart += 1;
                    return "rpg";
                }

                if (Player.Character.isInVehicle())
                {
                    //Game.Console.Print("[FPSmod-dbg]: fake runover kill: current stat: " + runoverTemp.ToString() + " runoverStart: " + runoverStart.ToString());
                    return "runover";
                }
                else
                {
                    return "vehicle_explosion";
                }
            }
        }

        private void getStatsStart()
        {

            aa12regularStart = getIntStat((int)gtaStats.STAT_KILLS_WITH_AA12_REGULAR);
            aa12explosiveStart = getIntStat((int)gtaStats.STAT_KILLS_WITH_AA12_EXPLOSIVE);
            auto_pistolStart = getIntStat((int)gtaStats.STAT_KILLS_WITH_AUTO_PISTOL);
            assault_shotgunStart = getIntStat((int)gtaStats.STAT_KILLS_WITH_ASSAULT_SHOTGUN);
            p90Start = getIntStat((int)gtaStats.STAT_KILLS_WITH_P90);
            pipebombStart = getIntStat((int)gtaStats.STAT_KILLS_WITH_PIPEBOMB);
            pistol44Start = getIntStat((int)gtaStats.STAT_KILLS_WITH_PISTOL44);
            pool_cueStart = getIntStat((int)gtaStats.STAT_KILLS_WITH_POOL_CUE);
            lmgStart = getIntStat((int)gtaStats.STAT_KILLS_WITH_LMG);
            golden_smgStart = getIntStat((int)gtaStats.STAT_KILLS_WITH_GOLDEN_SMG);
            advanced_sniperStart = getIntStat((int)gtaStats.STAT_KILLS_WITH_ADVANCED_SNIPER);
            grenadelauncherStart = getIntStat((int)gtaStats.STAT_KILLS_WITH_GRENADELAUNCHER);
            sawnoff_shotgunStart = getIntStat((int)gtaStats.STAT_KILLS_WITH_SAWNOFF_SHOTGUN);
            stickyStart = getIntStat((int)gtaStats.STAT_KILLS_WITH_STICKY);

            numberOfkillsStart = getIntStat((int)gtaStats.STAT_PEOPLE_KILLED);
            hsStart = getIntStat((int)gtaStats.STAT_KILLS_BY_HEADSHOTS);
            runoverStart = getIntStat((int)gtaStats.STAT_PEOPLE_RUN_DOWN);
            unarmedStart = getIntStat((int)gtaStats.STAT_KILLS_WITH_UNARMED);
            baseballStart = getIntStat((int)gtaStats.STAT_KILLS_WITH_BASEBALL_BAT);
            poolcueStart = getIntStat((int)gtaStats.STAT_KILLS_WITH_POOLCUE);
            knifeStart = getIntStat((int)gtaStats.STAT_KILLS_WITH_KNIFE);
            pistolStart = getIntStat((int)gtaStats.STAT_KILLS_WITH_PISTOL);
            grenadeStart = getIntStat((int)gtaStats.STAT_KILLS_WITH_GRENADE);
            molotovStart = getIntStat((int)gtaStats.STAT_KILLS_WITH_MOLOTOV);
            rocketStart = getIntStat((int)gtaStats.STAT_KILLS_WITH_ROCKET);
            deagleStart = getIntStat((int)gtaStats.STAT_KILLS_WITH_COMBAT_PISTOL);
            shotgunStart = getIntStat((int)gtaStats.STAT_KILLS_WITH_PUMP_SHOTGUN);
            shotgun2Start = getIntStat((int)gtaStats.STAT_KILLS_WITH_COMBAT_SHOTGUN);
            smgStart = getIntStat((int)gtaStats.STAT_KILLS_WITH_MICRO_SMG);
            mp5Start = getIntStat((int)gtaStats.STAT_KILLS_WITH_SMG);
            m4Start = getIntStat((int)gtaStats.STAT_KILLS_WITH_CARBINE_RIFLE);
            ak47Start = getIntStat((int)gtaStats.STAT_KILLS_WITH_ASSAULT_RIFLE);
            sniperStart = getIntStat((int)gtaStats.STAT_KILLS_WITH_SNIPER_RIFLE);
            sniper2Start = getIntStat((int)gtaStats.STAT_KILLS_WITH_COMBAT_SNIPER);
            rpgStart = getIntStat((int)gtaStats.STAT_KILLS_WITH_RPG);
        }

        #endregion

        #region time-based events
        private void TickEvent(object sender, EventArgs e) // each and every tick (interval is specified above)
        {
            if (modEnabled && (gameMode == -1))
            {
#if DEBUG
               // Game.Console.Print("kill event array size: " + killEventArray.Length.ToString() +"\ntime: " + globalCounter.ToString());
#endif
                globalCounter += 1;
                tickCounter += 1;
                if (playSound)
                {
                    int highestI = -1;
                    for (int i = 0; i < killEventArray.Length; i++)
                    {
                        if (killEventArray[i].eventTime != 0)
                        {
                            ///  Game.Console.Print("KeA|" + i.ToString() + "|" + killEventArray[i].eventTime.ToString() + "|" + killEventArray[i].eventType);
                            if (killEventArray[i].eventTime + killEventRemoveTime < globalCounter)
                            {
#if DEBUG
                                //       Game.Console.Print("R: " + killEventArray[i].eventType + "|" + killEventArray[i].eventTime.ToString() + "|" + globalCounter.ToString());
#endif
                                highestI = i;
                                numberOfKillEvents -= 1;
                            }
                        }
                    }
                    if (highestI != -1)
                    {
                        killEvent[] tempArray = killEventArray;
                        killEventArray = new killEvent[tempArray.Length - highestI - 1];

                        for (int i = 0; i < killEventArray.Length; i++)
                        {
                            killEventArray[i] = new killEvent();
                            killEventArray[i] = tempArray[i + highestI + 1];
                        }
                    }
                }
                if (tickCounter == 200)
                {
                    tickCounter = 100;
                }              

                if ((tickCounter % 10) == 0) // every second
                {
                    for (int i = 0; i < killNotificationArray.Length; i++)
                    {
                        if (killNotificationArray[i].isVisible)
                        {
                            killNotificationArray[i].timeLimit -= 1;
                            if (killNotificationArray[i].timeLimit < 1)
                            {
                                killNotificationArray[i].isVisible = false;
                                shuffleNotifications();
                            }
                        }
                    }
                }
            }
        }        

        private void OnPerFrameDrawing(object sender, GraphicsEventArgs e) // Executes every GTA-frame
        {
            e.Graphics.Scaling = FontScaling.ScreenUnits;
            if (modEnabled && (gameMode == -1))
            {
                radar = e.Graphics.GetRadarRectangle(FontScaling.ScreenUnits);
                //e.Graphics.DrawText("tick-counter:" + tickCounter.ToString(), 0.5f, 0.5f);
               // e.Graphics.DrawText("no1. timelim:" + killNotificationArray[0].timeLimit.ToString(), 0.5f, 0.75f);
               // e.Graphics.DrawText("Test", 0.1f, 0.1f);
               // e.Graphics.DrawText("HS-start:" + hsStart.ToString(), 0.5f, 0.1f);
                if ((Player.Character.Health == -100) && (!deathEventBool))
                {
                    deathEventBool = true;
                    numberOfKillsSinceLastDeath = 0;
                    numberOfHeadshotsSinceLastDeath = 0;
                    dominatingPlayed = false;
                    wickedSickPlayed = false;
                    killingSpreePlayed = false;
                }
                else if ((Player.Character.Health != -100) && (deathEventBool))
                {
                    deathEventBool = false;
                }

                int tempkills = getIntStat((int)gtaStats.STAT_PEOPLE_KILLED);
                if (tempkills > numberOfkillsStart) // the player has made a kill
                {


                    int amountOfKills = tempkills - numberOfkillsStart; // this is the amount of kills the player made THIS FRAME. (is correct!!)
                    //Game.DisplayText("Amount of kills:" + amountOfKills.ToString()); // if this is > 1, problems occur =/. (double names)

                    // what to do? delay the 2nd, 3rd, etc kills to next frames! How?
                    //int[] randomIntArray = new int[amountOfKills],
                    //      randomIntArray2 = new int[amountOfKills];
                    //Random[] randomNumberArray = new Random[amountOfKills],
                    //        randomNumberArray2 = new Random[amountOfKills];

                    //for (int i = 0; i < randomIntArray.Length; i++)
                    //{
                    //  randomNumberArray[i] = new Random();
                    //  randomNumberArray2[i] = new Random();
                    //  randomIntArray[i] = randomNumberArray[i].Next(0, 26);
                    //  randomIntArray2[i] = randomNumberArray2[i].Next(0, numberOfSurnames - 1);
                    //}

                    //for (int i = 0; i < amountOfKills; i++) // loop through all made kills and assign stuff
                    //{
                    int notificationNumber = getAvailableKillNotification();
                    string killType = getKillType();
                    int tempHScounter = numberOfHeadshotsSinceLastDeath;
                    if (string.Compare(killType, "headshot") == 0)
                    {
                        numberOfHeadshotsSinceLastDeath += 1;
                        comboWhoreCounter += 1;
                        string killType2 = getKillType();
                        killType = (killType + "_" + killType2);
                    }
                    else
                    {
                        comboWhoreCounter = 0;
                    }
                    

                    if (playSound)
                    {
                        int tempMultiKillCounter = 1,
                            tempRampageCounter = 1,
                            tempRampageExclusionNumber = 0,
                            tempExclusionNumber = 0,
                            tempOwnageExclusionNumber = 0,
                            tempOwnageCounter = 1,
                            tempMegaKillCounter = 1,
                            tempUnstoppableCounter = 1,
                            tempUnstoppableExclusionNumber = 0;
                        for (int i = 0; i < killEventArray.Length; i++)
                        {
                            if ((killEventArray[i].eventTime > globalCounter - MULTIKILL_TIMELIMIT) && (killEventArray[i].eventTime > multiKillExclusionNumber))
                            {
                                if (killEventArray[i].eventTime > tempExclusionNumber)
                                {
                                    tempExclusionNumber = killEventArray[i].eventTime;
                                }
                                tempMultiKillCounter += 1;
                            }
                            if ((killEventArray[i].eventTime > globalCounter - UNSTOPPABLE_TIME) && (killEventArray[i].eventTime > unstoppableExclusionNumber) && (killEventArray[i].eventInVehicle == true))
                            {
                                if (killEventArray[i].eventTime > tempUnstoppableExclusionNumber)
                                {
                                    tempUnstoppableExclusionNumber = killEventArray[i].eventTime;
                                }
                                tempUnstoppableCounter += 1;
                            }
                            if ((killEventArray[i].eventTime > globalCounter - RAMPAGE_TIME) && (killEventArray[i].eventTime > rampageExclusionNumber) && (killEventArray[i].eventType.Contains("grenadelauncher") || killEventArray[i].eventType.Contains("pipebomb") || killEventArray[i].eventType.Contains("rpg") || killEventArray[i].eventType.Contains("grenade") || killEventArray[i].eventType.Contains("sticky")))
                            {
                                if (killEventArray[i].eventTime > tempRampageExclusionNumber)
                                {
                                    tempRampageExclusionNumber = killEventArray[i].eventTime;
                                }
                                tempRampageCounter += 1;
                            }
                            if ((killEventArray[i].eventTime > globalCounter - OWNAGE_TIMELIMIT) && (killEventArray[i].eventTime > ownageExclusionNumber))
                            {
                                if (killEventArray[i].eventTime > tempOwnageExclusionNumber)
                                {
                                    tempOwnageExclusionNumber = killEventArray[i].eventTime;
                                }
                                tempOwnageCounter += 1;
                            }
                            if (killEventArray[i].eventTime > globalCounter - MEGAKILL_TIMELIMIT)
                            {
                                tempMegaKillCounter += 1;
                            }
                        }
#if DEBUG
                        Game.Console.Print("rampage counter: " + tempRampageCounter.ToString());
#endif
                        numberOfKillsSinceLastDeath += 1;

                        if (numberOfKillsSinceLastDeath == 1)
                        {
                            firstBloodSound.Play();
                        }
                        else if ((tempUnstoppableCounter >= KILLS_FOR_UNSTOPPABLE) && Player.Character.isInVehicle())
                        {
                            unstoppableSound.Play();
                            unstoppableExclusionNumber = tempUnstoppableExclusionNumber;
                        }
                        else if ((killType.CompareTo("pool_cue") == 0) || (killType.CompareTo("knife") == 0) || (killType.CompareTo("unarmed") == 0) || (killType.CompareTo("baseball") == 0))
                        {
                            humiliationSound.Play();
                        }
                        else if (tempRampageCounter >= KILLS_FOR_RAMPAGE)
                        {
                            rampageExclusionNumber = tempRampageExclusionNumber;
                            rampageSound.Play();
                        }
                        else if ((tempOwnageCounter >= KILLS_FOR_OWNAGE) && !(killType.Contains("grenadelauncher") || killType.Contains("pipebomb") || killType.Contains("rpg") || killType.Contains("grenade") || killType.Contains("sticky")) && !(Player.Character.isInVehicle()))
                        {
                            ownageExclusionNumber = tempOwnageExclusionNumber;
                            ownageSound.Play();
                        }
                        else if ((tempMegaKillCounter > 3) && (killType.Contains("grenadelauncher") || killType.Contains("pipebomb") || killType.Contains("rpg") || killType.Contains("grenade") || killType.Contains("sticky")))
                        {
                            megaKillSound.Play();
                        }
                        else if (tempMultiKillCounter >= KILLS_FOR_MULTIKILL)
                        {
                            multiKillExclusionNumber = tempExclusionNumber;
                            multiKillSound.Play();
                        }
                        else if (tempHScounter < numberOfHeadshotsSinceLastDeath)
                        {
                            if (comboWhoreCounter == KILLS_FOR_HOLYSHIT)
                            {
                                holyShitSound.Play();
                            }
                            else if (comboWhoreCounter == KILLS_FOR_COMBOWHORE)
                            {
                                //comboWhoreCounter = 0;
                                comboWhoreSound.Play();
                            }
                            else
                            {
                                headshotSoundPlayer.Play();
                            }
                        }
                        else if ((globalCounter > GODLIKE_TIME + godlikeTimer) && (Player.WantedLevel > 4))
                        {
                            godlikeSound.Play();
                            godlikeTimer = globalCounter;
                        }
                        else if ((numberOfKillsSinceLastDeath >= KILLS_FOR_DOMINATING) && (!dominatingPlayed))
                        {
                            dominatingPlayed = true;
                            dominatingSound.Play();
                        }
                        else if ((numberOfKillsSinceLastDeath >= KILLS_FOR_KILLINGSPREE) && (!killingSpreePlayed))
                        {
                            killingSpreePlayed = true;
                            killingSpreeSound.Play();
                        }
                        else if ((numberOfKillsSinceLastDeath >= KILLS_FOR_WICKEDSICK) && (!wickedSickPlayed))
                        {
                            wickedSickPlayed = true;
                            wickedSickSound.Play();
                        }
                    }

                    int randomNumber,
                        randomNumber2;
                    Random randomInt = new Random(),
                           randomInt2 = new Random();

                    randomNumber = randomInt.Next(0, 26);
                    randomNumber2 = randomInt2.Next(0, numberOfSurnames);

                    if (currentEpisode == GameEpisode.GTAIV)
                    {
                        killNotificationArray[notificationNumber].Attacker = "N. Bellic";
                    }
                    else if (currentEpisode == GameEpisode.TBOGT)
                    {
                        killNotificationArray[notificationNumber].Attacker = "L. Lopez";
                    }
                    else if (currentEpisode == GameEpisode.TLAD)
                    {
                        killNotificationArray[notificationNumber].Attacker = "J. Klebitz";
                    }
                    else
                    {
                        killNotificationArray[notificationNumber].Attacker = "You";
                    }

                    killNotificationArray[notificationNumber].Weapon = killType;
                    killNotificationArray[notificationNumber].Victim = (alphabetArray[randomNumber] + ". " + surnameArray[randomNumber2]);
                    killNotificationArray[notificationNumber].isVisible = true;
                    killNotificationArray[notificationNumber].timeLimit = killStayTime;

                    
                    numberOfKillEvents += 1;

                    if (numberOfKillEvents >= killEventArray.Length - 2)
                    {
                        killEvent[] tempArray = killEventArray;
                        killEventArray = new killEvent[killEventArray.Length + 10];
                        currentEventArraySize += 10;
                        for (int i = 0; i < killEventArray.Length; i++)
                        {

                            killEventArray[i] = new killEvent();
                            if (i < tempArray.Length)
                            {
                                killEventArray[i] = tempArray[i];

                            }
                        }

                    }
                    killEventArray[numberOfKillEvents] = new killEvent();
                    killEventArray[numberOfKillEvents].eventType = killType;
                    killEventArray[numberOfKillEvents].eventTime = globalCounter;
                    killEventArray[numberOfKillEvents].eventInVehicle = false;
                    if (Player.Character.isInVehicle())
                    {
                        killEventArray[numberOfKillEvents].eventInVehicle = true;
                    }

                    
#if DEBUG
                        Game.Console.Print("KeA: " + killEventArray[numberOfKillEvents].eventType + "|" + killEventArray[numberOfKillEvents].eventTime + "|" + killEventArray[numberOfKillEvents].eventInVehicle.ToString());
#endif


#if DEBUG
                        Game.Console.Print("[DEBUG]: Notification No: " + notificationNumber.ToString());
                        Game.Console.Print("[DEBUG]: Victim: " + (alphabetArray[randomNumber] + ". " + surnameArray[randomNumber2]));

#endif
                    numberOfkillsStart += 1;

                    //}
                }
                
                int tempRunOver = getIntStat((int)gtaStats.STAT_PEOPLE_RUN_DOWN);
                if (tempRunOver > runoverStart)
                {
                    runoverStart = tempRunOver;
                }
                //e.Graphics.DrawSprite(testTexture, 0.5f, 0.5f, 0.3f, 0.3f, 0);
                if (showIcons)
                {
                    for (int i = 0; i < killNotificationArray.Length; i++)
                    {
                        if (killNotificationArray[i].isVisible) // Dubbele....!
                        {
                            //e.Graphics.Scaling = FontScaling.ScreenUnits;
                            e.Graphics.Scaling = FontScaling.Pixel;
                            RectangleF radarRect = e.Graphics.GetRadarRectangle(FontScaling.Pixel);
                            float scaler = radarRect.Width / 210; // at 1680x1050 this is 210

                            //e.Graphics.DrawText(killNotificationArray[i].Attacker + "-" + killNotificationArray[i].Weapon + "-" + killNotificationArray[i].Victim, 0.033f, 0.1f * (0.3f*i));
                            if ((i % 2) == 0)
                            {
                                e.Graphics.DrawRectangle(new RectangleF(iconX * scaler, (iconHeight + iconYdiff) * i * scaler, iconWidth * scaler, iconHeight * scaler), Color.FromArgb(100, Color.White));
                                e.Graphics.DrawText(killNotificationArray[i].Attacker, attackerX * scaler, (iconHeight + iconYdiff) * i * scaler + 0.25f * iconHeight * scaler, Color.FromArgb(255, 120, 120, 120));
                                e.Graphics.DrawText(killNotificationArray[i].Victim, victimX * scaler, (iconHeight + iconYdiff) * i * scaler + 0.25f * iconHeight * scaler, Color.FromArgb(255, 120, 120, 120));
                                //e.Graphics.DrawRectangle(new RectangleF(0.032f, 0.098f * (0.35f * i), 0.25f, 0.032f), Color.FromArgb(100, Color.White));
                                //e.Graphics.DrawText(killNotificationArray[i].Attacker + "                         " + killNotificationArray[i].Victim, 0.033f, 0.1f * (0.35f * i), Color.Gray);
                            }
                            else
                            {
                                e.Graphics.DrawRectangle(new RectangleF(iconHeight * scaler, (iconHeight + iconYdiff) * i * scaler, iconWidth * scaler, iconHeight * scaler), Color.FromArgb(100, Color.Gray));
                                e.Graphics.DrawText(killNotificationArray[i].Attacker, attackerX * scaler, (iconHeight + iconYdiff) * i * scaler + 0.25f * iconHeight * scaler, Color.White);
                                e.Graphics.DrawText(killNotificationArray[i].Victim, victimX * scaler, (iconHeight + iconYdiff) * i * scaler + 0.25f * iconHeight * scaler, Color.White);
                                //e.Graphics.DrawRectangle(new RectangleF(0.032f, 0.098f * (0.35f * i), 0.25f, 0.032f), Color.FromArgb(100, Color.Gray));
                                // e.Graphics.DrawText(killNotificationArray[i].Attacker + "                         " + killNotificationArray[i].Victim, 0.033f, 0.1f * (0.35f * i), Color.White);
                            }

                            // e.Graphics.Scaling = FontScaling.Pixel;

                            /*
                            float scaler = radarRect.Width; // at 1680x1050 this is 210
                            RectangleF placeRect = new RectangleF(
                                                            new PointF(
                                                                radarRect.X + (0.35f * scaler),
                                                                radarRect.Y - (3.74f * scaler) + (i * 0.17f * scaler)),
                                                                    new SizeF(
                                                                        (256 * imageSizeMultiplier) * (scaler / 210),
                                                                        (128 * imageSizeMultiplier) * (scaler / 210)));
                            */

                            //RectangleF placeRect = new RectangleF(new PointF((int)180 * scaler, (int)(17 * scaler) * i), new SizeF(256 * scaler * 0.7f , 128 * scaler * 0.7f));
                            Vector4 placeVect = new Vector4(weaponX * scaler,                               // X    
                                                            (iconHeight + iconYdiff) * i * scaler + 0.5f * iconHeight * scaler,        // Y
                                                            256 * scaler * 0.6f,                        // width
                                                            128 * scaler * 0.6f);                       // height
                            /*
                            RectangleF placeRect = new RectangleF(
                                                            new PointF(
                                                                0.033f + 0.06f,
                                                                0.035f * i - 0.0165f),
                                                                    new SizeF(
                                                                        0.07f,
                                                                        0.07f));
                             * */

                            if (killNotificationArray[i].Weapon.Contains("headshot") && useHsModifier)
                            {
                                e.Graphics.DrawSprite(headshotModifier, placeVect.X, placeVect.Y, placeVect.Z, placeVect.W, 0);
                                //e.Graphics.DrawSprite(headshot, placeRect);
                            }
                            if (killNotificationArray[i].Weapon.Contains("unarmed"))
                            {
                                e.Graphics.DrawSprite(unarmed, placeVect.X, placeVect.Y, placeVect.Z, placeVect.W, 0);
                                // e.Graphics.DrawSprite(unarmed, placeRect);
                            }
                            else if (killNotificationArray[i].Weapon.Contains("aa12regular"))
                            {
                                e.Graphics.DrawSprite(aa12, placeVect.X, placeVect.Y, placeVect.Z, placeVect.W, 0);
                                //e.Graphics.DrawSprite(baseball, placeRect);
                            }
                            else if (killNotificationArray[i].Weapon.Contains("aa12explosive"))
                            {
                                e.Graphics.DrawSprite(aa12, placeVect.X, placeVect.Y, placeVect.Z, placeVect.W, 0);
                                //e.Graphics.DrawSprite(baseball, placeRect);
                            }
                            else if (killNotificationArray[i].Weapon.Contains("auto_pistol"))
                            {
                                e.Graphics.DrawSprite(auto_pistol, placeVect.X, placeVect.Y, placeVect.Z, placeVect.W, 0);
                                //e.Graphics.DrawSprite(baseball, placeRect);
                            }
                            else if (killNotificationArray[i].Weapon.Contains("assault_shotgun"))
                            {
                                e.Graphics.DrawSprite(assault_shotgun, placeVect.X, placeVect.Y, placeVect.Z, placeVect.W, 0);
                                //e.Graphics.DrawSprite(baseball, placeRect);
                            }
                            else if (killNotificationArray[i].Weapon.Contains("lmg"))
                            {
                                e.Graphics.DrawSprite(lmg, placeVect.X, placeVect.Y, placeVect.Z, placeVect.W, 0);
                                //e.Graphics.DrawSprite(baseball, placeRect);
                            }
                            else if (killNotificationArray[i].Weapon.Contains("sawnoff_shotgun"))
                            {
                                e.Graphics.DrawSprite(sawnoff_shotgun, placeVect.X, placeVect.Y, placeVect.Z, placeVect.W, 0);
                                //e.Graphics.DrawSprite(baseball, placeRect);
                            }
                            else if (killNotificationArray[i].Weapon.Contains("pipebomb"))
                            {
                                e.Graphics.DrawSprite(pipebomb, placeVect.X, placeVect.Y, placeVect.Z, placeVect.W, 0);
                                //e.Graphics.DrawSprite(baseball, placeRect);
                            }
                            else if (killNotificationArray[i].Weapon.Contains("advanced_sniper"))
                            {
                                e.Graphics.DrawSprite(advanced_sniper, placeVect.X, placeVect.Y, placeVect.Z, placeVect.W, 0);
                                //e.Graphics.DrawSprite(baseball, placeRect);
                            }
                            else if (killNotificationArray[i].Weapon.Contains("pool_cue"))
                            {
                                e.Graphics.DrawSprite(poolcue, placeVect.X, placeVect.Y, placeVect.Z, placeVect.W, 0);
                                //e.Graphics.DrawSprite(baseball, placeRect);
                            }
                            else if (killNotificationArray[i].Weapon.Contains("sticky"))
                            {
                                e.Graphics.DrawSprite(sticky, placeVect.X, placeVect.Y, placeVect.Z, placeVect.W, 0);
                                //e.Graphics.DrawSprite(baseball, placeRect);
                            }
                            else if (killNotificationArray[i].Weapon.Contains("grenadelauncher"))
                            {
                                e.Graphics.DrawSprite(grenadelauncher, placeVect.X, placeVect.Y, placeVect.Z, placeVect.W, 0);
                                //e.Graphics.DrawSprite(baseball, placeRect);
                            }
                            else if (killNotificationArray[i].Weapon.Contains("p90"))
                            {
                                e.Graphics.DrawSprite(p90, placeVect.X, placeVect.Y, placeVect.Z, placeVect.W, 0);
                            }
                            else if (killNotificationArray[i].Weapon.Contains("golden_smg"))
                            {
                                e.Graphics.DrawSprite(golden_smg, placeVect.X, placeVect.Y, placeVect.Z, placeVect.W, 0);
                                //e.Graphics.DrawSprite(baseball, placeRect);
                            }
                            else if (killNotificationArray[i].Weapon.Contains("pistol44"))
                            {
                                e.Graphics.DrawSprite(pistol44, placeVect.X, placeVect.Y, placeVect.Z, placeVect.W, 0);
                                //e.Graphics.DrawSprite(baseball, placeRect);
                            }
                            else if (killNotificationArray[i].Weapon.Contains("baseball"))
                            {
                                e.Graphics.DrawSprite(baseball, placeVect.X, placeVect.Y, placeVect.Z, placeVect.W, 0);
                                //e.Graphics.DrawSprite(baseball, placeRect);
                            }
                            else if (killNotificationArray[i].Weapon.Contains("knife"))
                            {
                                e.Graphics.DrawSprite(knife, placeVect.X, placeVect.Y, placeVect.Z, placeVect.W, 0);
                                //e.Graphics.DrawSprite(knife, placeRect);
                            }
                            else if (killNotificationArray[i].Weapon.Contains("pistol"))
                            {
                                e.Graphics.DrawSprite(pistol, placeVect.X, placeVect.Y, placeVect.Z, placeVect.W, 0);
                                //e.Graphics.DrawSprite(pistol, placeRect);
                            }
                            else if (killNotificationArray[i].Weapon.Contains("grenade"))
                            {
                                e.Graphics.DrawSprite(grenade, placeVect.X, placeVect.Y, placeVect.Z, placeVect.W, 0);
                                // e.Graphics.DrawSprite(grenade, placeRect);
                            }
                            else if (killNotificationArray[i].Weapon.Contains("molotov"))
                            {
                                e.Graphics.DrawSprite(molotov, placeVect.X, placeVect.Y, placeVect.Z, placeVect.W, 0);
                                //e.Graphics.DrawSprite(molotov, placeRect);
                            }
                            else if (killNotificationArray[i].Weapon.Contains("deagle"))
                            {
                                e.Graphics.DrawSprite(deagle, placeVect.X, placeVect.Y, placeVect.Z, placeVect.W, 0);
                                // e.Graphics.DrawSprite(deagle, placeRect);
                            }
                            else if (killNotificationArray[i].Weapon.Contains("vehicle_explosion"))
                            {
                                e.Graphics.DrawSprite(vehicle_explosion, placeVect.X, placeVect.Y, placeVect.Z, placeVect.W, 0);
                                //e.Graphics.DrawSprite(vehicle_explosion, placeRect);
                            }
                            else if (killNotificationArray[i].Weapon.Contains("combat_shotgun"))
                            {
                                e.Graphics.DrawSprite(combat_shotgun, placeVect.X, placeVect.Y, placeVect.Z, placeVect.W, 0);
                                //e.Graphics.DrawSprite(combat_shotgun, placeRect);
                            }
                            else if (killNotificationArray[i].Weapon.Contains("shotgun"))
                            {
                                e.Graphics.DrawSprite(shotgun, placeVect.X, placeVect.Y, placeVect.Z, placeVect.W, 0);
                                //e.Graphics.DrawSprite(shotgun, placeRect);
                            }
                            else if (killNotificationArray[i].Weapon.Contains("micro_smg"))
                            {
                                e.Graphics.DrawSprite(micro_smg, placeVect.X, placeVect.Y, placeVect.Z, placeVect.W, 0);
                                //e.Graphics.DrawSprite(micro_smg, placeRect);
                            }
                            else if (killNotificationArray[i].Weapon.Contains("mp5"))
                            {
                                e.Graphics.DrawSprite(mp5, placeVect.X, placeVect.Y, placeVect.Z, placeVect.W, 0);
                                //e.Graphics.DrawSprite(mp5, placeRect);
                            }
                            /*else if (killNotificationArray[i].Weapon.Contains("m4"))
                            {
                                e.Graphics.DrawSprite(m4, 0.5f, (0.1f * (0.3f * i)) + 0.03f, 0.08f, (0.08f * (103f / 245f)) * (radar.Height / radar.Width), 0f);
                            }*/
                            else if (killNotificationArray[i].Weapon.Contains("m4"))
                            {
                                e.Graphics.DrawSprite(m4, placeVect.X, placeVect.Y, placeVect.Z, placeVect.W, 0);
                                //e.Graphics.DrawSprite(m4, placeRect);
                            }
                            else if (killNotificationArray[i].Weapon.Contains("ak47"))
                            {
                                e.Graphics.DrawSprite(ak47, placeVect.X, placeVect.Y, placeVect.Z, placeVect.W, 0);
                                //e.Graphics.DrawSprite(ak47, placeRect);
                            }
                            else if (killNotificationArray[i].Weapon.Contains("rpg"))
                            {
                                e.Graphics.DrawSprite(rpg, placeVect.X, placeVect.Y, placeVect.Z, placeVect.W, 0);
                                //e.Graphics.DrawSprite(rpg, placeRect);
                            }
                            else if (killNotificationArray[i].Weapon.Contains("combat_sniper"))
                            {
                                e.Graphics.DrawSprite(combat_sniper, placeVect.X, placeVect.Y, placeVect.Z, placeVect.W, 0);
                                //e.Graphics.DrawSprite(combat_sniper, placeRect);
                            }
                            else if (killNotificationArray[i].Weapon.Contains("sniper"))
                            {
                                e.Graphics.DrawSprite(sniper, placeVect.X, placeVect.Y, placeVect.Z, placeVect.W, 0);
                                //e.Graphics.DrawSprite(sniper, placeRect);
                            }
                            else if (killNotificationArray[i].Weapon.Contains("runover"))
                            {
                                e.Graphics.DrawSprite(runover, placeVect.X, placeVect.Y, placeVect.Z, placeVect.W, 0);
                                //e.Graphics.DrawSprite(runover, placeRect);
                            }
                            e.Graphics.Scaling = FontScaling.ScreenUnits;

                        }
                    }
                }
                if ((tempBool1 != 0) && (tempBool1 != 1))
                {
                    e.Graphics.DrawText("I told you not to mess with that. \n I explicitely told you so: \n \n'Definitely don't touch this. You'll screw up everything.' \n\n But noooo, you just had to be stupid. \n Now be a good boy/girl and change that value back! \n \n \n Thank you.", 0.3f, 0.3f, Color.Yellow);
                }                
                //e.Graphics.DrawText("Enabled", 0.5f, 0.5f);
#if DEBUG
                //Game.Console.Print("x " + Game.Mouse.PositionPixel.X.ToString() + " y " + Game.Mouse.PositionPixel.Y.ToString());
#endif
            }
        }

        #endregion

        #region helper functions
        private int getIntStat(int number) // function used to get stats
        {
            return GTA.Native.Function.Call<int>("GET_INT_STAT", number);
        }

        private byte[] loadFile(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);

            // Create a byte array of file stream length
            byte[] ImageData = new byte[fs.Length];

            //Read block of bytes from stream into the byte array
            fs.Read(ImageData, 0, System.Convert.ToInt32(fs.Length));

            //Close the File Stream
            fs.Close();
            return ImageData; //return the byte data
        }

        #endregion
    }
}