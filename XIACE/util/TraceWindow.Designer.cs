namespace FFXI.XIACE.util {
    partial class TraceWindow {
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
            this.MessageView = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // MessageView
            // 
            this.MessageView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MessageView.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (128)));
            this.MessageView.FormattingEnabled = true;
            this.MessageView.ItemHeight = 12;
            this.MessageView.Location = new System.Drawing.Point(0, 0);
            this.MessageView.Name = "MessageView";
            this.MessageView.Size = new System.Drawing.Size(406, 172);
            this.MessageView.TabIndex = 0;
            // 
            // TraceWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(406, 177);
            this.Controls.Add(this.MessageView);
            this.Name = "TraceWindow";
            this.Text = "TraceForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox MessageView;
    }
}