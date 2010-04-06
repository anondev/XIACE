namespace FFXI.XIACE.util {
    partial class WindowerForm {
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナで生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.ProcessWatchTimer = new System.Windows.Forms.Timer(this.components);
            this.MainContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.WindowerMenuItemTopMost = new System.Windows.Forms.ToolStripMenuItem();
            this.WindowerMenuItemProcess = new System.Windows.Forms.ToolStripMenuItem();
            this.MainContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // ProcessWatchTimer
            // 
            this.ProcessWatchTimer.Tick += new System.EventHandler(this.ProcessWatchTimer_Tick);
            // 
            // MainContextMenu
            // 
            this.MainContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.WindowerMenuItemTopMost,
            this.WindowerMenuItemProcess});
            this.MainContextMenu.Name = "ProcessContextMenu";
            this.MainContextMenu.Size = new System.Drawing.Size(153, 70);
            // 
            // WindowerMenuItemTopMost
            // 
            this.WindowerMenuItemTopMost.CheckOnClick = true;
            this.WindowerMenuItemTopMost.Name = "WindowerMenuItemTopMost";
            this.WindowerMenuItemTopMost.Size = new System.Drawing.Size(152, 22);
            this.WindowerMenuItemTopMost.Text = "最前面";
            this.WindowerMenuItemTopMost.Click += new System.EventHandler(this.WindowerMenuItemTopMost_DropDownItemTopMost_Click);
            // 
            // WindowerMenuItemProcess
            // 
            this.WindowerMenuItemProcess.Name = "WindowerMenuItemProcess";
            this.WindowerMenuItemProcess.Size = new System.Drawing.Size(152, 22);
            this.WindowerMenuItemProcess.Text = "Windower";
            this.WindowerMenuItemProcess.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.WindowerMenuItemProcess_DropDownItemClicked);
            // 
            // WindowerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(207, 170);
            this.ContextMenuStrip = this.MainContextMenu;
            this.Name = "WindowerForm";
            this.Text = "WindowerForm";
            this.Load += new System.EventHandler(this.WindowerForm_Load);
            this.MainContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer ProcessWatchTimer;
        protected System.Windows.Forms.ContextMenuStrip MainContextMenu;
        private System.Windows.Forms.ToolStripMenuItem WindowerMenuItemProcess;
        private System.Windows.Forms.ToolStripMenuItem WindowerMenuItemTopMost;
    }
}
