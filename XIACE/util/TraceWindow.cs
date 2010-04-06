using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace FFXI.XIACE.util {

    public partial class TraceWindow : Form {

        public int BufferSize { get; set; }

        public TraceWindow() {
            InitializeComponent();
            BufferSize = 30;
            Trace.Listeners.Add(new NotificationTraceListener(this));
        }

        public void Write(string message) {
            MessageView.Items.Add(message);
            if (MessageView.Items.Count > BufferSize)
                MessageView.Items.RemoveAt(0);
            MessageView.SelectedIndex = MessageView.Items.Count - 1;
        }
    }

    public class NotificationTraceListener : TraceListener {

        private TraceWindow form;

        public NotificationTraceListener(TraceWindow form) {
            this.form = form;
        }

        public override void Write(string message) {
            form.Write(message);
        }

        public override void WriteLine(string message) {
            Write(message);
        }
    }
}
