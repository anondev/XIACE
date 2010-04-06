﻿/* XIACE - Memory Access Provider for FFXI.
 * Copyright (C) 2009 FFXI RCM Project <ff11rcm@gmail.com>
 * 
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the
 * Free Software Foundation, Inc., 59 Temple Place - Suite 330,
 * Boston, MA 02111-1307, USA.
 */

namespace FFXI.XIACE {

    /// <summary>
    /// 各種オフセット
    /// </summary>
    internal enum OFFSET : int {
        TARGET_INFO = 0x4d992c,  // 
        PLAYER_INFO = 0x57ea46, // 名前,HPMPTP,エリアなど (FIX: 2009.04.22)
        JOB_INFO = 0x3ef070, // ジョブ情報(FIX: 2009.04.22)
        MAXHPMP_INFO = 0x3ef068, // なんか適当 (FIXME: 2009.04.22) 
        ACTIVITY_INFO = 0x53d220,  // 行動状態 enum Activity を参照 (FIX: 2009.04.22)
        BUFFS_INFO = 0x84d2b, // 各種ステータス状態アイコン (FIX: 2009.04.22)
        FISH_INFO = 0x3f1660, // 釣り(uncomfirmed FIX: 2009.04.09)
        EQUIP_INFO = 0x8dad58, // 装備 (FIX: 2009.04.22)
        INVENTORY_INFO = 0x8d52d0, // かばん (FIX: 2009.04.22)
        SAFEBOX_INFO = INVENTORY_INFO + (81 * 0x2c), // 金庫 (FIX: 2009.04.22)
        STORAGE_INFO = SAFEBOX_INFO + (81 * 0x2c), // 収納 (FIX: 2009.04.22)
        LOCKER_INFO = STORAGE_INFO + (162 * 0x2c), // ロッカー (FIX: 2009.04.22)
        SATCHEL_INFO = LOCKER_INFO + (81 * 0x2c), // サッチェル (Added: 2009.04.22)s
        ITEM_INFO = 0x52d2b0, // アイテム (maybe FIX: 2009.04.22)
        INVENTORY_COUNT = 0x584018, // カバン所持数 (+ 0x24 size 1) (maybe FIX: 2009.04.22)
        INVENTORY_MAX = 0x8dabbd, // カバンMAX (maybe FIX: 2009.04.22)
        LOGGED_IN = 0x585AE0, // ログインしてるかどうか(int, INで0 OUTで5075****)(2009.04.24)
    }

    /// <summary>
    /// 行動状態
    /// </summary>
    public enum eActivity : byte {
        Standing = 0,
        Fighting = 1,
        Dead1 = 2,
        Dead2 = 3,
        CutScene = 4,
        Chocobo = 5,
        Fishing = 6,
        Healing = 33,
        FishBite = 38,
        Obtained = 39,
        RodBreak = 40,
        LineBreak = 41,
        LostCatch = 43,
        CatchMonster = 42,
        Synthing = 44,
        Sitting = 47
    }

    #region "ステータスエフェクト"
    public enum eBuff : short {
        Undefined = -1,
        KO = 0,
        Weakness = 1,
        Sleep = 2,
        Poison = 3,
        Paralysis = 4,
        Blindness = 5,
        Silence = 6,
        Petrification = 7,
        Disease = 8,
        Curse = 9,
        Stun = 10,
        Bind = 11,
        Weight = 12,
        Slow = 13,
        Charm1 = 14,
        Doom = 15,
        Amnesia = 16,
        Charm2 = 17,
        Terror = 28,
        Mute = 29,
        Bane = 30,
        Plague = 31,
        Flee = 32,
        Haste = 33,
        Blaze_Spikes = 34,
        Ice_Spikes = 35,
        Blink = 36,
        Stoneskin = 37,
        Shock_Spikes = 38,
        Aquaveil = 39,
        Protect = 40,
        Shell = 41,
        Regen = 42,
        Refresh = 43,
        Mighty_Strikes = 44,
        Boost = 45,
        Hundred_Fists = 46,
        Manafont = 47,
        Chainspell = 48,
        Perfect_Dodge = 49,
        Invincible = 50,
        Blood_Weapon = 51,
        Soul_Voice = 52,
        Eagle_Eye_Shot = 53,
        Meikyo_Shisui = 54,
        Astral_Flow = 55,
        Berserk = 56,
        Defender = 57,
        Aggressor = 58,
        Focus = 59,
        Dodge = 60,
        Counterstance = 61,
        Sentinel = 62,
        Souleater = 63,
        Last_Resort = 64,
        Sneak_Attack = 65,
        Copy_Image = 66,
        Third_Eye = 67,
        Warcry = 68,
        Invisible = 69,
        Deodorize = 70,
        Sneak = 71,
        Sharpshot = 72,
        Barrage = 73,
        Holy_Circle = 74,
        Arcane_Circle = 75,
        Hide = 76,
        Camouflage = 77,
        Divine_Seal = 78,
        Elemental_Seal = 79,
        STR_Boost1 = 80,
        DEX_Boost1 = 81,
        VIT_Boost1 = 82,
        AGI_Boost1 = 83,
        INT_Boost1 = 84,
        MND_Boost1 = 85,
        CHR_Boost1 = 86,
        Trick_Attack = 87,
        Max_HP_Boost = 88,
        Max_MP_Boost = 89,
        Accuracy_Boost = 90,
        Attack_Boost = 91,
        Evasion_Boost = 92,
        Defense_Boost = 93,
        Enfire = 94,
        Enblizzard = 95,
        Enaero = 96,
        Enstone = 97,
        Enthunder = 98,
        Enwater = 99,
        Barfire = 100,
        Barblizzard = 101,
        Baraero = 102,
        Barstone = 103,
        Barthunder = 104,
        Barwater = 105,
        Barsleep = 106,
        Barpoison = 107,
        Barparalyze = 108,
        Barblind = 109,
        Barsilence = 110,
        Barpetrify = 111,
        Barvirus = 112,
        Reraise = 113,
        Cover = 114,
        Unlimited_Shot = 115,
        Phalanx = 116,
        Warding_Circle = 117,
        Ancient_Circle = 118,
        STR_Boost2 = 119,
        DEX_Boost2 = 120,
        VIT_Boost2 = 121,
        AGI_Boost2 = 122,
        INT_Boost2 = 123,
        MND_Boost2 = 124,
        CHR_Boost2 = 125,
        Spirit_Surge = 126,
        Costume = 127,
        Burn = 128,
        Frost = 129,
        Choke = 130,
        Rasp = 131,
        Shock = 132,
        Drown = 133,
        Dia = 134,
        Bio = 135,
        STR_Down = 136,
        DEX_Down = 137,
        VIT_Down = 138,
        AGI_Down = 139,
        INT_Down = 140,
        MND_Down = 141,
        CHR_Down = 142,
        Level_Restriction = 143,
        Max_HP_Down = 144,
        Max_MP_Down = 145,
        Accuracy_Down = 146,
        Attack_Down = 147,
        Evasion_Down = 148,
        Defense_Down = 149,
        Physical_Shield = 150,
        Arrow_Shield = 151,
        Magic_Shield1 = 152,
        Damage_Spikes = 153,
        Shining_Ruby = 154,
        Medicine = 155,
        Flash = 156,
        Subjob_Restriction = 157,
        Provoke = 158,
        Penalty = 159,
        Preparations = 160,
        Sprint = 161,
        Enchantment = 162,
        Azure_Lore = 163,
        Chain_Affinity = 164,
        Burst_Affinity = 165,
        Overdrive = 166,
        Magic_Def_Down = 167,
        Inhibit_TP = 168,
        Potency = 169,
        Regain = 170,
        Pax = 171,
        Magic_Shield2 = 189,
        Magic_Atk_Boost = 190,
        Magic_Def_Boost = 191,
        Requiem = 192,
        Lullaby = 193,
        Elegy = 194,
        Paeon = 195,
        Ballad = 196,
        Minne = 197,
        Minuet = 198,
        Madrigal = 199,
        Prelude = 200,
        Mambo = 201,
        Aubade = 202,
        Pastoral = 203,
        Hum = 204,
        Fantasia = 205,
        Operetta = 206,
        Capriccio = 207,
        Serenade = 208,
        Round = 209,
        Gavotte = 210,
        Fugue = 211,
        Rhapsody = 212,
        Aria = 213,
        March = 214,
        Etude = 215,
        Carol = 216,
        Threnody = 217,
        Hymnus = 218,
        Mazurka = 219,
        Sirvente = 220,
        Auto_Regen = 233,
        Auto_Refresh = 234,
        Fishing_Imagery = 235,
        Woodworking = 236,
        Smithing = 237,
        Goldsmithing = 238,
        Clothcraft = 239,
        Leathercraft = 240,
        Bonecraft = 241,
        Alchemy = 242,
        Cooking = 243,
        Dedication = 249,
        Ef_Badge = 250,
        Food = 251,
        Chocobo = 252,
        Signet = 253,
        Battlefield = 254,
        Sanction = 256,
        Besieged = 257,
        Illusion = 258,
        No_Weapons_Armor = 259,
        No_Support_Job = 260,
        No_Job_Abilities = 261,
        No_Magic_Casting = 262,
        Penalty_to_Attribute_s_ = 263,
        Overload = 299,
        Fire_Maneuver = 300,
        Ice_Maneuver = 301,
        Wind_Maneuver = 302,
        Earth_Maneuver = 303,
        Thunder_Maneuver = 304,
        Water_Maneuver = 305,
        Light_Maneuver = 306,
        Dark_Maneuver = 307,
        Doubleup_Chance = 308,
        Bust = 309,
        Fighters_Roll = 310,
        Monks_Roll = 311,
        Healers_Roll = 312,
        Wizards_Roll = 313,
        Warlocks_Roll = 314,
        Rogues_Roll = 315,
        Gallants_Roll = 316,
        Chaos_Roll = 317,
        Beast_Roll = 318,
        Choral_Roll = 319,
        Hunters_Roll = 320,
        Samurai_Roll = 321,
        Ninja_Roll = 322,
        Drachen_Roll = 323,
        Evokers_Roll = 324,
        Maguss_Roll = 325,
        Corsairs_Roll = 326,
        Puppet_Roll = 327,
        Warriors_Charge = 340,
        Formless_Strikes = 341,
        Assassins_Charge = 342,
        Feint = 343,
        Fealty = 344,
        Dark_Seal = 345,
        Diabolic_Eye = 346,
        Nightingale = 347,
        Troubadour = 348,
        Killer_Instinct = 349,
        Stealth_Shot = 350,
        Flashy_Shot = 351,
        Sange = 352,
        Hasso = 353,
        Seigan = 354
    }
    #endregion

    public enum eRodPosition : short {
        Error = 0,
        Center = 1,
        Left = 2,
        Right = 3
    }

    public enum eEquipSlot : short {
        Main = 0,
        Sub = 1,
        Range = 2,
        Ammo = 3,
        Head = 4,
        Body = 5,
        Hands = 6,
        Legs = 7,
        Feet = 8,
        Neck = 9,
        Waist = 10,
        EarLeft = 11,
        EarRight = 12,
        RingLeft = 13,
        RingRight = 14,
        Back = 15
    }

    public enum eJob : byte {
        NA = 0,
        WAR = 1,
        MNK = 2,
        WHM = 3,
        BLM = 4,
        RDM = 5,
        THF = 6,
        PLD = 7,
        DRK = 8,
        BST = 9,
        BRD = 10,
        RNG = 11,
        SAM = 12,
        NIN = 13,
        DRG = 14,
        SMN = 15,
        BLU = 16,
        COR = 17,
        PUP = 18,
        DNC = 19,
        SCH = 20
    }
}