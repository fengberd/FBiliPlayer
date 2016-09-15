namespace FBiliPlayer
{
	partial class MainForm
	{
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		/// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
		protected override void Dispose(bool disposing)
		{
			if(disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows 窗体设计器生成的代码

		/// <summary>
		/// 设计器支持所需的方法 - 不要修改
		/// 使用代码编辑器修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.trackBar1 = new System.Windows.Forms.TrackBar();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.vlcControl1 = new Vlc.DotNet.Forms.VlcControl();
			this.button1 = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
			((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.vlcControl1)).BeginInit();
			this.contextMenuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// trackBar1
			// 
			this.trackBar1.AutoSize = false;
			this.trackBar1.Location = new System.Drawing.Point(0, 496);
			this.trackBar1.Maximum = 0;
			this.trackBar1.Name = "trackBar1";
			this.trackBar1.Size = new System.Drawing.Size(834, 15);
			this.trackBar1.TabIndex = 1;
			this.trackBar1.TickStyle = System.Windows.Forms.TickStyle.None;
			this.trackBar1.Visible = false;
			this.trackBar1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.trackBar1_MouseDown);
			this.trackBar1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.trackBar1_MouseUp);
			// 
			// timer1
			// 
			this.timer1.Interval = 10;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// vlcControl1
			// 
			this.vlcControl1.BackColor = System.Drawing.Color.Black;
			this.vlcControl1.Location = new System.Drawing.Point(0, 0);
			this.vlcControl1.Name = "vlcControl1";
			this.vlcControl1.Size = new System.Drawing.Size(834, 496);
			this.vlcControl1.Spu = -1;
			this.vlcControl1.TabIndex = 2;
			this.vlcControl1.Visible = false;
			this.vlcControl1.VlcLibDirectory = null;
			this.vlcControl1.VlcMediaplayerOptions = null;
			this.vlcControl1.VlcLibDirectoryNeeded += new System.EventHandler<Vlc.DotNet.Forms.VlcLibDirectoryNeededEventArgs>(this.vlcControl1_VlcLibDirectoryNeeded);
			this.vlcControl1.Playing += new System.EventHandler<Vlc.DotNet.Core.VlcMediaPlayerPlayingEventArgs>(this.vlcControl1_Playing);
			this.vlcControl1.PositionChanged += new System.EventHandler<Vlc.DotNet.Core.VlcMediaPlayerPositionChangedEventArgs>(this.vlcControl1_PositionChanged);
			this.vlcControl1.Stopped += new System.EventHandler<Vlc.DotNet.Core.VlcMediaPlayerStoppedEventArgs>(this.vlcControl1_Stopped);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(8, 27);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(50, 23);
			this.button1.TabIndex = 3;
			this.button1.Text = "To 99%";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// label1
			// 
			this.label1.ContextMenuStrip = this.contextMenuStrip1;
			this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label1.Font = new System.Drawing.Font("微软雅黑", 25F);
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(834, 511);
			this.label1.TabIndex = 4;
			this.label1.Text = "Bilibili Player\r\nRight click to show menu.";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(105, 26);
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(104, 22);
			this.toolStripMenuItem2.Text = "2333";
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(834, 511);
			this.ContextMenuStrip = this.contextMenuStrip1;
			this.Controls.Add(this.label1);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.vlcControl1);
			this.Controls.Add(this.trackBar1);
			this.Name = "MainForm";
			this.ShowIcon = false;
			this.Text = "Bilibili Player";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.SizeChanged += new System.EventHandler(this.MainForm_SizeChanged);
			((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.vlcControl1)).EndInit();
			this.contextMenuStrip1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.TrackBar trackBar1;
		private System.Windows.Forms.Timer timer1;
		private Vlc.DotNet.Forms.VlcControl vlcControl1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
	}
}

