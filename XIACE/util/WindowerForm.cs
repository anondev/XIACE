using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace FFXI.XIACE.util {

    public partial class WindowerForm : Form {

        protected event EventHandler WindowerInstanceChanged = (object sender, EventArgs e) => { };
        protected event EventHandler ProcessWatchTimerTick = (object sender, EventArgs e) => { };

        private XIWindower _windower;
        private PolProcessWatcher ppw;
        private Queue<Process> qadd;
        private Queue<Process> qdel;
        private string TextOriginal;

        protected XIWindower Windower {
            get { return _windower; }
        }

        public WindowerForm() {
            InitializeComponent();
            InitProcessHandler();
            WindowerInstanceChanged += new EventHandler(WindowerForm_WindowerInstanceChanged);
        }

        private void WindowerForm_Load(object sender, EventArgs e) {
            WindowerMenuItemTopMost.Checked = TopMost;
            ProcessWatchTimer_Tick(null, null);
            ProcessWatchTimer.Start();
            TextOriginal = Text;
        }

        private void InitProcessHandler() {
            _windower = null;
            qadd = new Queue<Process>();
            qdel = new Queue<Process>();
            ppw = new PolProcessWatcher();
            ppw.ProcessAdded += new EventHandler<PolProcessWatcherEventArgs>(ProcessAddedEventHandler);
            ppw.ProcessRemoved += new EventHandler<PolProcessWatcherEventArgs>(ProcessRemovedEventHandler);
            ppw.Start();
        }

        private void ProcessAddedEventHandler(object sender, PolProcessWatcherEventArgs e) {
            qadd.Enqueue(e.Process);
        }

        private void ProcessRemovedEventHandler(object sender, PolProcessWatcherEventArgs e) {
            qdel.Enqueue(e.Process);
        }

        private void ProcessWatchTimer_Tick(object sender, EventArgs e) {

            while (qadd.Count > 0) {
                PolProcess pol = new PolProcess(qadd.Dequeue());
                ToolStripMenuItem menuItem = new ToolStripMenuItem(pol.Title);
                menuItem.Tag = pol;
                WindowerMenuItemProcess.DropDownItems.Add(menuItem);
            }

            while (qdel.Count > 0) {
                Process p = qdel.Dequeue();
                foreach (ToolStripMenuItem menuItem in WindowerMenuItemProcess.DropDownItems) {
                    PolProcess pol = (PolProcess) menuItem.Tag;
                    if (pol != null && pol.Pid == p.Id) {
                        WindowerMenuItemProcess.DropDownItems.Remove(menuItem);
                        break;
                    }
                }
            }

            if (WindowerMenuItemProcess.DropDownItems.Count == 1) {
                ToolStripMenuItem menuItem = (ToolStripMenuItem) WindowerMenuItemProcess.DropDownItems[0];
                if (_windower == null || !menuItem.Checked)
                    WindowerMenuItemProcess_DropDownItemClicked(this, new ToolStripItemClickedEventArgs(menuItem));
            }

            if (_windower != null && WindowerMenuItemProcess.DropDownItems.Count == 0) {
                _windower = null;
                WindowerInstanceChanged.Invoke(this, new EventArgs());
            }

            ProcessWatchTimerTick.Invoke(this, new EventArgs());
        }

        private void WindowerMenuItemTopMost_DropDownItemTopMost_Click(object sender, EventArgs e) {
            TopMost = WindowerMenuItemTopMost.Checked;
        }

        private void WindowerMenuItemProcess_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e) {

            foreach (ToolStripMenuItem item in WindowerMenuItemProcess.DropDownItems)
                item.Checked = false;

            ToolStripMenuItem menuItem = (ToolStripMenuItem) e.ClickedItem;
            menuItem.Checked = true;

            PolProcess pol = (PolProcess) menuItem.Tag;

            if (pol == null || (_windower != null && _windower.Pid == pol.Pid))
                return;

            _windower = new XIWindower(pol.Pid);

            WindowerInstanceChanged.Invoke(this, new EventArgs());
        }

        private void WindowerForm_TextChanged(object sender, EventArgs e) {
            TextOriginal = Text;
        }

        private void WindowerForm_WindowerInstanceChanged(object sender, EventArgs e) {
            string tmp = TextOriginal;
            Text = (_windower == null) ? TextOriginal : string.Format("{0} [ {1} ]", TextOriginal, _windower.pol.Title);
            TextOriginal = tmp;
        }

        private void WindowerMenuItemAbout_Click(object sender, EventArgs e) {
            MessageBox.Show(this, GetAboutString(), "Copyright");
        }

        protected virtual string GetAboutString() {
            return @"
このプログラムは、

  XIACE ( http://ff11rcm.googlecode.com/ )

を使用しています。


XIACE Copyright (C) 2009 FFXI RCM Project. 
";
        }
    }
}
