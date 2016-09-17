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
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.vlcControl1 = new Vlc.DotNet.Forms.VlcControl();
			this.label1 = new System.Windows.Forms.Label();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.rightMenu_OpenFolder = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.rightMenu_Pause = new System.Windows.Forms.ToolStripMenuItem();
			this.rightMenu_Stop = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.rightMenu_Fullscreen = new System.Windows.Forms.ToolStripMenuItem();
			this.rightMenu_Video = new System.Windows.Forms.ToolStripMenuItem();
			this.rightMenu_Video_Speed = new System.Windows.Forms.ToolStripMenuItem();
			this.rightMenu_Video_Speed_02 = new System.Windows.Forms.ToolStripMenuItem();
			this.rightMenu_Video_Speed_05 = new System.Windows.Forms.ToolStripMenuItem();
			this.rightMenu_Video_Speed_08 = new System.Windows.Forms.ToolStripMenuItem();
			this.rightMenu_Video_Speed_10 = new System.Windows.Forms.ToolStripMenuItem();
			this.rightMenu_Video_Speed_15 = new System.Windows.Forms.ToolStripMenuItem();
			this.rightMenu_Video_Speed_20 = new System.Windows.Forms.ToolStripMenuItem();
			this.rightMenu_Video_Speed_50 = new System.Windows.Forms.ToolStripMenuItem();
			this.rightMenu_Video_Tracks = new System.Windows.Forms.ToolStripMenuItem();
			this.rightMenu_DEV = new System.Windows.Forms.ToolStripMenuItem();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.videobar = new System.Windows.Forms.Panel();
			((System.ComponentModel.ISupportInitialize)(this.vlcControl1)).BeginInit();
			this.contextMenuStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.videobar.SuspendLayout();
			this.SuspendLayout();
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
			this.vlcControl1.Size = new System.Drawing.Size(834, 510);
			this.vlcControl1.Spu = -1;
			this.vlcControl1.TabIndex = 2;
			this.vlcControl1.Visible = false;
			this.vlcControl1.VlcLibDirectory = null;
			this.vlcControl1.VlcMediaplayerOptions = null;
			this.vlcControl1.VlcLibDirectoryNeeded += new System.EventHandler<Vlc.DotNet.Forms.VlcLibDirectoryNeededEventArgs>(this.vlcControl1_VlcLibDirectoryNeeded);
			this.vlcControl1.Paused += new System.EventHandler<Vlc.DotNet.Core.VlcMediaPlayerPausedEventArgs>(this.vlcControl1_Paused);
			this.vlcControl1.Playing += new System.EventHandler<Vlc.DotNet.Core.VlcMediaPlayerPlayingEventArgs>(this.vlcControl1_Playing);
			this.vlcControl1.PositionChanged += new System.EventHandler<Vlc.DotNet.Core.VlcMediaPlayerPositionChangedEventArgs>(this.vlcControl1_PositionChanged);
			this.vlcControl1.Stopped += new System.EventHandler<Vlc.DotNet.Core.VlcMediaPlayerStoppedEventArgs>(this.vlcControl1_Stopped);
			// 
			// label1
			// 
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
            this.rightMenu_OpenFolder,
            this.toolStripMenuItem2,
            this.rightMenu_Pause,
            this.rightMenu_Stop,
            this.toolStripMenuItem1,
            this.rightMenu_Fullscreen,
            this.rightMenu_Video,
            this.rightMenu_DEV});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(150, 148);
			// 
			// rightMenu_OpenFolder
			// 
			this.rightMenu_OpenFolder.Name = "rightMenu_OpenFolder";
			this.rightMenu_OpenFolder.Size = new System.Drawing.Size(149, 22);
			this.rightMenu_OpenFolder.Text = "Open Folder";
			this.rightMenu_OpenFolder.Click += new System.EventHandler(this.rightMenu_OpenFolder_Click);
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(146, 6);
			// 
			// rightMenu_Pause
			// 
			this.rightMenu_Pause.Name = "rightMenu_Pause";
			this.rightMenu_Pause.Size = new System.Drawing.Size(149, 22);
			this.rightMenu_Pause.Text = "Pause";
			this.rightMenu_Pause.Click += new System.EventHandler(this.rightMenu_Pause_Click);
			// 
			// rightMenu_Stop
			// 
			this.rightMenu_Stop.Name = "rightMenu_Stop";
			this.rightMenu_Stop.Size = new System.Drawing.Size(149, 22);
			this.rightMenu_Stop.Text = "Stop";
			this.rightMenu_Stop.Click += new System.EventHandler(this.rightMenu_Stop_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(146, 6);
			// 
			// rightMenu_Fullscreen
			// 
			this.rightMenu_Fullscreen.Name = "rightMenu_Fullscreen";
			this.rightMenu_Fullscreen.Size = new System.Drawing.Size(149, 22);
			this.rightMenu_Fullscreen.Text = "Fullscreen";
			this.rightMenu_Fullscreen.Click += new System.EventHandler(this.rightMenu_Fullscreen_Click);
			// 
			// rightMenu_Video
			// 
			this.rightMenu_Video.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rightMenu_Video_Speed,
            this.rightMenu_Video_Tracks});
			this.rightMenu_Video.Name = "rightMenu_Video";
			this.rightMenu_Video.Size = new System.Drawing.Size(149, 22);
			this.rightMenu_Video.Text = "Video";
			// 
			// rightMenu_Video_Speed
			// 
			this.rightMenu_Video_Speed.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rightMenu_Video_Speed_02,
            this.rightMenu_Video_Speed_05,
            this.rightMenu_Video_Speed_08,
            this.rightMenu_Video_Speed_10,
            this.rightMenu_Video_Speed_15,
            this.rightMenu_Video_Speed_20,
            this.rightMenu_Video_Speed_50});
			this.rightMenu_Video_Speed.Name = "rightMenu_Video_Speed";
			this.rightMenu_Video_Speed.Size = new System.Drawing.Size(152, 22);
			this.rightMenu_Video_Speed.Text = "Speed";
			// 
			// rightMenu_Video_Speed_02
			// 
			this.rightMenu_Video_Speed_02.Name = "rightMenu_Video_Speed_02";
			this.rightMenu_Video_Speed_02.Size = new System.Drawing.Size(152, 22);
			this.rightMenu_Video_Speed_02.Tag = "0.2";
			this.rightMenu_Video_Speed_02.Text = "x0.2";
			this.rightMenu_Video_Speed_02.Click += new System.EventHandler(this.rightMenu_Video_Speed_Item_Click);
			// 
			// rightMenu_Video_Speed_05
			// 
			this.rightMenu_Video_Speed_05.Name = "rightMenu_Video_Speed_05";
			this.rightMenu_Video_Speed_05.Size = new System.Drawing.Size(152, 22);
			this.rightMenu_Video_Speed_05.Tag = "0.5";
			this.rightMenu_Video_Speed_05.Text = "x0.5";
			this.rightMenu_Video_Speed_05.Click += new System.EventHandler(this.rightMenu_Video_Speed_Item_Click);
			// 
			// rightMenu_Video_Speed_08
			// 
			this.rightMenu_Video_Speed_08.Name = "rightMenu_Video_Speed_08";
			this.rightMenu_Video_Speed_08.Size = new System.Drawing.Size(152, 22);
			this.rightMenu_Video_Speed_08.Tag = "0.8";
			this.rightMenu_Video_Speed_08.Text = "x0.8";
			this.rightMenu_Video_Speed_08.Click += new System.EventHandler(this.rightMenu_Video_Speed_Item_Click);
			// 
			// rightMenu_Video_Speed_10
			// 
			this.rightMenu_Video_Speed_10.Name = "rightMenu_Video_Speed_10";
			this.rightMenu_Video_Speed_10.Size = new System.Drawing.Size(152, 22);
			this.rightMenu_Video_Speed_10.Tag = "1.0";
			this.rightMenu_Video_Speed_10.Text = "x1.0";
			this.rightMenu_Video_Speed_10.Click += new System.EventHandler(this.rightMenu_Video_Speed_Item_Click);
			// 
			// rightMenu_Video_Speed_15
			// 
			this.rightMenu_Video_Speed_15.Name = "rightMenu_Video_Speed_15";
			this.rightMenu_Video_Speed_15.Size = new System.Drawing.Size(152, 22);
			this.rightMenu_Video_Speed_15.Tag = "1.5";
			this.rightMenu_Video_Speed_15.Text = "x1.5";
			this.rightMenu_Video_Speed_15.Click += new System.EventHandler(this.rightMenu_Video_Speed_Item_Click);
			// 
			// rightMenu_Video_Speed_20
			// 
			this.rightMenu_Video_Speed_20.Name = "rightMenu_Video_Speed_20";
			this.rightMenu_Video_Speed_20.Size = new System.Drawing.Size(152, 22);
			this.rightMenu_Video_Speed_20.Tag = "2.0";
			this.rightMenu_Video_Speed_20.Text = "x2.0";
			this.rightMenu_Video_Speed_20.Click += new System.EventHandler(this.rightMenu_Video_Speed_Item_Click);
			// 
			// rightMenu_Video_Speed_50
			// 
			this.rightMenu_Video_Speed_50.Name = "rightMenu_Video_Speed_50";
			this.rightMenu_Video_Speed_50.Size = new System.Drawing.Size(152, 22);
			this.rightMenu_Video_Speed_50.Tag = "5.0";
			this.rightMenu_Video_Speed_50.Text = "x5.0";
			this.rightMenu_Video_Speed_50.Click += new System.EventHandler(this.rightMenu_Video_Speed_Item_Click);
			// 
			// rightMenu_Video_Tracks
			// 
			this.rightMenu_Video_Tracks.Name = "rightMenu_Video_Tracks";
			this.rightMenu_Video_Tracks.Size = new System.Drawing.Size(152, 22);
			this.rightMenu_Video_Tracks.Text = "Track";
			// 
			// rightMenu_DEV
			// 
			this.rightMenu_DEV.Name = "rightMenu_DEV";
			this.rightMenu_DEV.Size = new System.Drawing.Size(149, 22);
			this.rightMenu_DEV.Text = "DEV";
			this.rightMenu_DEV.Click += new System.EventHandler(this.rightMenu_DEV_Click);
			// 
			// pictureBox1
			// 
			this.pictureBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
			this.pictureBox1.Location = new System.Drawing.Point(0, 0);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(834, 2);
			this.pictureBox1.TabIndex = 5;
			this.pictureBox1.TabStop = false;
			this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.videobar_MouseDown);
			this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.videobar_MouseMove);
			this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.videobar_MouseUp);
			// 
			// videobar
			// 
			this.videobar.BackColor = System.Drawing.Color.Black;
			this.videobar.Controls.Add(this.pictureBox1);
			this.videobar.Location = new System.Drawing.Point(0, 510);
			this.videobar.Name = "videobar";
			this.videobar.Size = new System.Drawing.Size(834, 2);
			this.videobar.TabIndex = 6;
			this.videobar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.videobar_MouseDown);
			this.videobar.MouseMove += new System.Windows.Forms.MouseEventHandler(this.videobar_MouseMove);
			this.videobar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.videobar_MouseUp);
			// 
			// MainForm
			// 
			this.AllowDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(834, 511);
			this.Controls.Add(this.videobar);
			this.Controls.Add(this.vlcControl1);
			this.Controls.Add(this.label1);
			this.Name = "MainForm";
			this.ShowIcon = false;
			this.Text = "Bilibili Player";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.SizeChanged += new System.EventHandler(this.MainForm_SizeChanged);
			this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
			this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainForm_DragEnter);
			((System.ComponentModel.ISupportInitialize)(this.vlcControl1)).EndInit();
			this.contextMenuStrip1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.videobar.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.Timer timer1;
		private Vlc.DotNet.Forms.VlcControl vlcControl1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ToolStripMenuItem rightMenu_Pause;
		private System.Windows.Forms.ToolStripMenuItem rightMenu_Stop;
		private System.Windows.Forms.ToolStripMenuItem rightMenu_Fullscreen;
		private System.Windows.Forms.ToolStripMenuItem rightMenu_DEV;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Panel videobar;
		private System.Windows.Forms.ToolStripMenuItem rightMenu_Video;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem rightMenu_Video_Speed;
		private System.Windows.Forms.ToolStripMenuItem rightMenu_Video_Speed_02;
		private System.Windows.Forms.ToolStripMenuItem rightMenu_Video_Speed_05;
		private System.Windows.Forms.ToolStripMenuItem rightMenu_Video_Speed_08;
		private System.Windows.Forms.ToolStripMenuItem rightMenu_Video_Speed_10;
		private System.Windows.Forms.ToolStripMenuItem rightMenu_Video_Speed_15;
		private System.Windows.Forms.ToolStripMenuItem rightMenu_Video_Speed_20;
		private System.Windows.Forms.ToolStripMenuItem rightMenu_Video_Speed_50;
		private System.Windows.Forms.ToolStripMenuItem rightMenu_Video_Tracks;
		private System.Windows.Forms.ToolStripMenuItem rightMenu_OpenFolder;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
	}
}

