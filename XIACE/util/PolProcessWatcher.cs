using System;
using System.Diagnostics;
using System.Management;
using System.Windows.Forms;

namespace FFXI.XIACE.util {

    public class PolProcessWatcherEventArgs : EventArgs {
        public Process _Process;
        public Process Process { get { return _Process; } }
        public PolProcessWatcherEventArgs(Process process) {
            this._Process = process;
        }
    }

    public class PolProcessWatcher {

        public event EventHandler<PolProcessWatcherEventArgs> ProcessAdded;
        public event EventHandler<PolProcessWatcherEventArgs> ProcessRemoved;

        private ManagementEventWatcher watcher;

        private const string POL_PROCESS_NAME = "pol";
        private const string POL_MODULE_NAME = "FFXiMain.dll";

        public PolProcessWatcher() {
            Application.ApplicationExit += new EventHandler(ApplicationExitEventHandler);
        }

        public void Start() {
            AddRunningProcesses();
            this.watcher = new ManagementEventWatcher(new WqlEventQuery("__InstanceCreationEvent", new TimeSpan(0, 0, 1), "TargetInstance isa \"Win32_Process\""));
            this.watcher.EventArrived += new EventArrivedEventHandler(ArrivedEventHandler);
            this.watcher.Start();
        }

        public void Stop() {
            if (this.watcher != null) {
                this.watcher.Stop();
            }
        }

        private void Add(Process process, IntPtr addr) {
            process.EnableRaisingEvents = true;
            process.Exited += new EventHandler(ProcessExitedEventHandler);
            ProcessAdded.Invoke(this, new PolProcessWatcherEventArgs(process));
        }

        private void Remove(Process process) {
            ProcessRemoved.Invoke(this, new PolProcessWatcherEventArgs(process));
        }

        private void AddRunningProcesses() {
            Process[] list = Process.GetProcessesByName(POL_PROCESS_NAME);
            foreach (Process process in list) {
                IntPtr addr = GetBaseAddress(process);
                if (addr != IntPtr.Zero) {
                    Add(process, addr);
                }
            }
        }

        private IntPtr GetBaseAddress(Process process) {
            foreach (ProcessModule module in process.Modules) {
                if (module.ModuleName == POL_MODULE_NAME) {
                    return module.BaseAddress;
                }
            }
            return IntPtr.Zero;
        }

        private void ArrivedEventHandler(object sender, EventArrivedEventArgs e) {
            ManagementBaseObject mbo = (ManagementBaseObject) e.NewEvent.Properties["TargetInstance"].Value;
            UInt32 pid = (UInt32) mbo.Properties["ProcessId"].Value;
            Process process = Process.GetProcessById((int) pid);
            if (process.ProcessName == POL_PROCESS_NAME) {
                IntPtr addr = GetBaseAddress(process);
                if (addr != IntPtr.Zero) {
                    Add(process, addr);
                }
            }
        }

        private void ProcessExitedEventHandler(object sender, EventArgs args) {
            Remove((Process) sender);
        }

        private void ApplicationExitEventHandler(object sender, EventArgs args) {
            Stop();
        }
    }
}
