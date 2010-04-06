/* XIACE - Memory Access Provider for FFXI.
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

/*
 * Player - to get Player's (own) HP/HPP/MP/MPP/TP/Zone(Area) 
 */
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace FFXI.XIACE {

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe internal struct PlayerStatus {
        internal fixed byte name[20];
        internal fixed byte unknown[10];
        internal int HP;
        internal int MP;
        internal int TP;
        internal byte HPP;
        internal byte MPP;
        internal byte Area;
    }

    unsafe internal struct Max {
        internal int HP;
        internal int MP;
    }

    unsafe internal struct JobInfo {
        internal byte Main;
        internal byte MainLv;
        internal byte Sub;
        internal byte SubLv;
        internal ushort Exp;
        internal ushort ExpNext;
    }

    public class Player : XIWindowerSubBase {

        private PlayerStatus stat;
        private Max max;
        private eActivity act;
        private short[] buffs = new short[32];
        private JobInfo job;

        public Player(XIWindower windower)
            : base(windower) {
            Read();
        }

        unsafe public bool isLoggedIn() {
            int val;
            MemoryProvider.ReadProcessMemory(windower.pol.Handle, (IntPtr) ((int) windower.pol.BaseAddress + Offset.Get("LOGGED_IN")), &val, 4, null);
            if (val == 0)
                return true;
            return false;
        }

        unsafe private void Read() {
            PlayerStatus status = new PlayerStatus();
            MemoryProvider.ReadProcessMemory(windower.pol.Handle, (IntPtr) ((int) windower.pol.BaseAddress + Offset.Get("PLAYER_INFO")), &status, (uint) Marshal.SizeOf(stat), null);
            stat = status;
        }

        unsafe private void ReadMax() {
            Max maxstat = new Max();
            MemoryProvider.ReadProcessMemory(windower.pol.Handle, (IntPtr) ((int) windower.pol.BaseAddress + Offset.Get("MAXHPMP_INFO")), &maxstat, (uint) Marshal.SizeOf(maxstat), null);
            max = maxstat;
        }

        unsafe private void ReadActivity() {
            byte pAct;
            MemoryProvider.ReadProcessMemory(windower.pol.Handle, (IntPtr) ((int) windower.pol.BaseAddress + Offset.Get("ACTIVITY_INFO")), &pAct, 1, null);
            act = (eActivity) pAct;
        }

        /// <summary>
        /// ステータスエフェクトを取得
        /// </summary>
        unsafe private void ReadBuffs() {
            int addr;
            // Buffs b = new Buffs();
            fixed (short* b = buffs) {
                MemoryProvider.ReadProcessMemory(windower.pol.Handle, (IntPtr) ((int) windower.pol.BaseAddress + Offset.Get("BUFFS_INFO")), &addr, 4, null);
                MemoryProvider.ReadProcessMemory(windower.pol.Handle, (IntPtr) (addr), b, 64, null);
            }
        }

        unsafe private string ReadName() {
            int count = 18;
            byte[] name = new byte[count];
            Read();
            fixed (byte* src = stat.name, dst = name) {
                MemoryProvider.memcpy(src, dst, count);
            }
            return Encoding.Default.GetString(name).Trim('\0');
        }

        unsafe private void ReadJob() {
            JobInfo ji = new JobInfo();
            MemoryProvider.ReadProcessMemory(windower.pol.Handle, (IntPtr) ((int) windower.pol.BaseAddress + (int) Offset.Get("JOB_INFO")), &ji, 8, null);
            job = ji;
        }

        public string Name {
            get {
                return ReadName();
            }
        }

        public int HP {
            get {
                Read();
                return stat.HP;
            }
        }

        public int HPMax {
            get {
                ReadMax();
                return max.HP;
            }
        }

        public int MP {
            get {
                Read();
                return stat.MP;
            }
        }

        public int MPMax {
            get {
                ReadMax();
                return max.MP;
            }
        }

        public int TP {
            get {
                Read();
                return stat.TP;
            }
        }

        public int HPP {
            get {
                Read();
                return Convert.ToInt32(stat.HPP);
            }
        }

        public int MPP {
            get {
                Read();
                return Convert.ToInt32(stat.MPP);
            }
        }

        public eActivity Activity {
            get {
                ReadActivity();
                return act;
            }
        }

        public int Area {
            get {
                Read();
                return Convert.ToInt32(stat.Area);
            }
        }

        public string AreaName {
            get {
                return AreaToName(Area);
            }
        }

        public eJob MainJob {
            get {
                ReadJob();
                return (eJob) job.Main;
            }
        }

        public byte MainJobLevel {
            get {
                ReadJob();
                return job.MainLv;
            }
        }

        public eJob SubJob {
            get {
                ReadJob();
                return (eJob) job.Sub;
            }
        }

        public byte SubJobLevel {
            get {
                ReadJob();
                return job.SubLv;
            }
        }

        public ushort Exp {
            get {
                ReadJob();
                return job.Exp;
            }
        }

        public ushort ExpNext {
            get {
                ReadJob();
                return job.ExpNext;
            }
        }

        unsafe public bool isBuffed(eBuff buff) {
            ReadBuffs();
            fixed (short* ba = buffs) {
                for (short s = 0; s < 32; s++) {
                    if ((short) buff == ba[s])
                        return true;
                    if (ba[s] < 0)
                        break;
                }
            }
            return false;
        }

        unsafe public eBuff[] ListBuffs() {
            ReadBuffs();
            eBuff[] list = new eBuff[32];
            fixed (short* ba = buffs) {
                for (short s = 0; s < 32; s++) {
                    if (ba[s] >= 0)
                        list[s] = (eBuff) ba[s];
                    else {
                        list[s] = eBuff.Undefined;
                        break;
                    }
                }
            }
            return list;
        }

        #region "エリアIDからエリア名に変換"
        private string AreaToName(int id) {
            switch (id) {
                case 0:
                    return "unknown";
                case 1:
                    return "Phanauet Channel";
                case 2:
                    return "Carpenters' Landing";
                case 3:
                    return "Manaclipper";
                case 4:
                    return "Bibiki Bay";
                case 5:
                    return "Uleguerand Range";
                case 6:
                    return "Bearclaw Pinnacle";
                case 7:
                    return "Attohwa Chasm";
                case 8:
                    return "Boneyard Gully";
                case 9:
                    return "Pso'Xja";
                case 10:
                    return "The Shrouded Maw";
                case 11:
                    return "Oldton Movalpolos";
                case 12:
                    return "Newton Movalpolos";
                case 13:
                    return "Mine Shaft #2716";
                case 14:
                    return "Hall of Transference";
                case 16:
                    return "Promyvion - Holla";
                case 17:
                    return "Spire of Holla";
                case 18:
                    return "Promyvion - Dem";
                case 19:
                    return "Spire of Dem";
                case 20:
                    return "Promyvion - Mea";
                case 21:
                    return "Spire of Mea";
                case 22:
                    return "Promyvion - Vahzl";
                case 23:
                    return "Spire of Vahzl";
                case 24:
                    return "Lufaise Meadows";
                case 25:
                    return "Misareaux Coast";
                case 26:
                    return "Tavnazian Safehold";
                case 27:
                    return "Phomiuna Aqueducts";
                case 28:
                    return "Sacrarium";
                case 29:
                    return "Riverne - Site #B01";
                case 30:
                    return "Riverne - Site #A01";
                case 31:
                    return "Monarch Linn";
                case 32:
                    return "Sealion's Den";
                case 33:
                    return "Al'Taieu";
                case 34:
                    return "Grand Palace of Hu'Xzoi";
                case 35:
                    return "The Garden of Ru'Hmet";
                case 36:
                    return "Empyreal Paradox";
                case 37:
                    return "Temenos";
                case 38:
                    return "Apollyon";
                case 39:
                    return "Dynamis - Valkurm";
                case 40:
                    return "Dynamis - Buburimu";
                case 41:
                    return "Dynamis - Qufim";
                case 42:
                    return "Dynamis - Tavnazia";
                case 43:
                    return "Diorama Abdhaljs-Ghelsba";
                case 44:
                    return "Abdhaljs Isle-Purgonorgo";
                case 46:
                    return "Open sea route to Al Zahbi";
                case 47:
                    return "Open sea route to Mhaura";
                case 48:
                    return "Al Zahbi";
                case 50:
                    return "Aht Urhgan Whitegate";
                case 51:
                    return "Wajaom Woodlands";
                case 52:
                    return "Bhaflau Thickets";
                case 53:
                    return "Nashmau";
                case 54:
                    return "Arrapago Reef";
                case 55:
                    return "Ilrusi Atoll";
                case 56:
                    return "Periqia";
                case 57:
                    return "Talacca Cove";
                case 58:
                    return "Silver Sea route to Nashmau";
                case 59:
                    return "Silver Sea route to Al Zahbi";
                case 60:
                    return "The Ashu Talif";
                case 61:
                    return "Mount Zhayolm";
                case 62:
                    return "Halvung";
                case 63:
                    return "Lebros Cavern";
                case 64:
                    return "Navukgo Execution Chamber";
                case 65:
                    return "Mamook";
                case 66:
                    return "Mamool Ja Training Grounds";
                case 67:
                    return "Jade Sepulcher";
                case 68:
                    return "Aydeewa Subterrane";
                case 69:
                    return "Leujaoam Sanctum";
                case 70:
                    return "Chocobo Circuit";
                case 71:
                    return "The Colosseum";
                case 72:
                    return "Alzadaal Undersea Ruins";
                case 73:
                    return "Zhayolm Remnants";
                case 74:
                    return "Arrapago Remnants";
                case 75:
                    return "Bhaflau Remnants";
                case 76:
                    return "Silver Sea Remnants";
                case 77:
                    return "Nyzul Isle";
                case 78:
                    return "Hazhalm Testing Grounds";
                case 79:
                    return "Caedarva Mire";
                case 80:
                    return "Southern San d'Oria [S]";
                case 81:
                    return "East Ronfaure [S]";
                case 82:
                    return "Jugner Forest [S]";
                case 83:
                    return "Vunkerl Inlet [S]";
                case 84:
                    return "Batallia Downs [S]";
                case 85:
                    return "La Vaule [S]";
                case 86:
                    return "Everbloom Hollow";
                case 87:
                    return "Bastok Markets [S]";
                case 88:
                    return "North Gustaberg [S]";
                case 89:
                    return "Grauberg [S]";
                case 90:
                    return "Pashhow Marshlands [S]";
                case 91:
                    return "Rolanberry Fields [S]";
                case 92:
                    return "Beadeaux [S]";
                case 93:
                    return "Ruhotz Silvermines";
                case 94:
                    return "Windurst Waters [S]";
                case 95:
                    return "West Sarutabaruta [S]";
                case 96:
                    return "Fort Karugo-Narugo [S]";
                case 97:
                    return "Meriphataud Mountains [S]";
                case 98:
                    return "Sauromugue Champaign [S]";
                case 99:
                    return "Oztroja Castle [S]";
                case 100:
                    return "West Ronfaure";
                case 101:
                    return "East Ronfaure";
                case 102:
                    return "La Theine Plateau";
                case 103:
                    return "Valkurm Dunes";
                case 104:
                    return "Jugner Forest";
                case 105:
                    return "Batallia Downs";
                case 106:
                    return "North Gustaberg";
                case 107:
                    return "South Gustaberg";
                case 108:
                    return "Konschtat Highlands";
                case 109:
                    return "Pashhow Marshlands";
                case 110:
                    return "Rolanberry Fields";
                case 111:
                    return "Beaucedine Glacier";
                case 112:
                    return "Xarcabard";
                case 113:
                    return "Cape Teriggan";
                case 114:
                    return "Eastern Altepa Desert";
                case 115:
                    return "West Sarutabaruta";
                case 116:
                    return "East Sarutabaruta";
                case 117:
                    return "Tahrongi Canyon";
                case 118:
                    return "Buburimu Peninsula";
                case 119:
                    return "Meriphataud Mountains";
                case 120:
                    return "Sauromugue Champaign";
                case 121:
                    return "The Sanctuary of Zi'Tah";
                case 122:
                    return "Ro'Maeve";
                case 123:
                    return "Yuhtunga Jungle";
                case 124:
                    return "Yhoator Jungle";
                case 125:
                    return "Western Altepa Desert";
                case 126:
                    return "Qufim Island";
                case 127:
                    return "Behemoth's Dominion";
                case 128:
                    return "Valley of Sorrows";
                case 129:
                    return "Ghoyu's Reverie";
                case 130:
                    return "Ru'Aun Gardens";
                case 131:
                    return "Mordion Gaol";
                case 134:
                    return "Dynamis - Beaucedine";
                case 135:
                    return "Dynamis - Xarcabard";
                case 136:
                    return "Beaucedine Glacier [S]";
                case 137:
                    return "Xarcabard [S]";
                case 138:
                    return "Castle Zvahl Baileys [S]";
                case 139:
                    return "Horlais Peak";
                case 140:
                    return "Ghelsba Outpost";
                case 141:
                    return "Fort Ghelsba";
                case 142:
                    return "Yughott Grotto";
                case 143:
                    return "Palborough Mines";
                case 144:
                    return "Waughroon Shrine";
                case 145:
                    return "Giddeus";
                case 146:
                    return "Balga's Dais";
                case 147:
                    return "Beadeaux";
                case 148:
                    return "Qulun Dome";
                case 149:
                    return "Davoi";
                case 150:
                    return "Monastic Cavern";
                case 151:
                    return "Castle Oztroja";
                case 152:
                    return "Altar Room";
                case 153:
                    return "The Boyahda Tree";
                case 154:
                    return "Dragon's Aery";
                case 155:
                    return "Castle Zvahl Keep [S]";
                case 156:
                    return "Throne Room [S]";
                case 157:
                    return "Middle Delkfutt's Tower";
                case 158:
                    return "Upper Delkfutt's Tower";
                case 159:
                    return "Temple of Uggalepih";
                case 160:
                    return "Den of Rancor";
                case 161:
                    return "Castle Zvahl Baileys";
                case 162:
                    return "Castle Zvahl Keep";
                case 163:
                    return "Sacrificial Chamber";
                case 164:
                    return "Garlaige Citadel [S]";
                case 165:
                    return "Throne Room";
                case 166:
                    return "Ranguemont Pass";
                case 167:
                    return "Bostaunieux Oubliette";
                case 168:
                    return "Chamber of Oracles";
                case 169:
                    return "Toraimarai Canal";
                case 170:
                    return "Full Moon Fountain";
                case 171:
                    return "Crawlers' Nest [S]";
                case 172:
                    return "Zeruhn Mines";
                case 173:
                    return "Korroloka Tunnel";
                case 174:
                    return "Kuftal Tunnel";
                case 175:
                    return "The Eldieme Necropolis [S]";
                case 176:
                    return "Sea Serpent Grotto";
                case 177:
                    return "Ve'Lugannon Palace";
                case 178:
                    return "The Shrine of Ru'Avitau";
                case 179:
                    return "Stellar Fulcrum";
                case 180:
                    return "La'Loff Amphitheater";
                case 181:
                    return "The Celestial Nexus";
                case 182:
                    return "Walk of Echoes";
                case 183:
                    return "The Last Stand";
                case 184:
                    return "Lower Delkfutt's Tower";
                case 185:
                    return "Dynamis - San d'Oria";
                case 186:
                    return "Dynamis - Bastok";
                case 187:
                    return "Dynamis - Windurst";
                case 188:
                    return "Dynamis - Jeuno";
                case 190:
                    return "King Ranperre's Tomb";
                case 191:
                    return "Dangruf Wadi";
                case 192:
                    return "Inner Horutoto Ruins";
                case 193:
                    return "Ordelle's Caves";
                case 194:
                    return "Outer Horutoto Ruins";
                case 195:
                    return "The Eldieme Necropolis";
                case 196:
                    return "Gusgen Mines";
                case 197:
                    return "Crawlers' Nest";
                case 198:
                    return "Maze of Shakhrami";
                case 200:
                    return "Garlaige Citadel";
                case 201:
                    return "Cloister of Gales";
                case 202:
                    return "Cloister of Storms";
                case 203:
                    return "Cloister of Frost";
                case 204:
                    return "Fei'Yin";
                case 205:
                    return "Ifrit's Cauldron";
                case 206:
                    return "Qu'Bia Arena";
                case 207:
                    return "Cloister of Flames";
                case 208:
                    return "Quicksand Caves";
                case 209:
                    return "Cloister of Tremors";
                case 211:
                    return "Cloister of Tides";
                case 212:
                    return "Gustav Tunnel";
                case 213:
                    return "Labyrinth of Onzozo";
                case 215:
                    return "Jeuno Residential Area";
                case 216:
                    return "San d'Oria Residential Area";
                case 217:
                    return "Bastok Residential Area";
                case 218:
                    return "Windurst Residential Area";
                case 220:
                    return "Ship bound for Selbina";
                case 221:
                    return "Ship bound for Mhaura";
                case 223:
                    return "San d'Oria-Jeuno Airship";
                case 224:
                    return "Bastok-Jeuno Airship";
                case 225:
                    return "Windurst-Jeuno Airship";
                case 226:
                    return "Kazham-Jeuno Airship";
                case 227:
                    return "Ship bound for Selbina(Pirates)";
                case 228:
                    return "Ship bound for Mhaura(Pirates)";
                case 230:
                    return "Southern San d'Oria";
                case 231:
                    return "Northern San d'Oria";
                case 232:
                    return "Port San d'Oria";
                case 233:
                    return "Chateau d'Oraguille";
                case 234:
                    return "Bastok Mines";
                case 235:
                    return "Bastok Markets";
                case 236:
                    return "Port Bastok";
                case 237:
                    return "Metalworks";
                case 238:
                    return "Windurst Waters";
                case 239:
                    return "Windurst Walls";
                case 240:
                    return "Port Windurst";
                case 241:
                    return "Windurst Woods";
                case 242:
                    return "Heavens Tower";
                case 243:
                    return "Ru'Lude Gardens";
                case 244:
                    return "Upper Jeuno";
                case 245:
                    return "Lower Jeuno";
                case 246:
                    return "Port Jeuno";
                case 247:
                    return "Rabao";
                case 248:
                    return "Selbina";
                case 249:
                    return "Mhaura";
                case 250:
                    return "Kazham";
                case 251:
                    return "Hall of the Gods";
                case 252:
                    return "Norg";
                case 253:
                    return "San d'Oria Residential Area";
                case 254:
                    return "Bastok Residential Area";
                case 255:
                    return "Windurst Residential Area";
                default:
                    return "Error/Area unknown";
            }
        }
        #endregion
    }
}
