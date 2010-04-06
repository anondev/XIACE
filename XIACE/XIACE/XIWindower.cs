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
using System;
using System.Diagnostics;

namespace FFXI.XIACE {

    public class XIWindower : FFXI.Windower {

        public Player Player;
        public Inventory Inventory;
        public Fishing Fishing;
        public Craft Craft;
        public Menu Menu;

        private int _Pid;
        private PolProcess _pol;

        public XIWindower(int pid)
            : base(pid) {

            this._Pid = pid;

            Process p = Process.GetProcessById((int) pid);
            p.Exited += new EventHandler(ProcessExitedEventHandler);

            this._pol = new PolProcess(p);

            this.Player = new Player(this);
            this.Inventory = new Inventory(this);
            this.Fishing = new Fishing(this);
            this.Craft = new Craft(this);
            this.Menu = new Menu(this);
        }

        public new int Pid {
            get { return (this._Pid == 0) ? this._Pid : base.Pid; }
        }

        public PolProcess pol {
            get { return this._pol; }
        }

        private void ProcessExitedEventHandler(object sender, EventArgs args) {
            this._Pid = 0;
        }
    }

    public class XIWindowerSubBase {
        public XIWindower windower;
        public XIWindowerSubBase(XIWindower windower) {
            this.windower = windower;
        }
    }
}
