using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

using fastJSON;
using Vlc.DotNet.Core;
using Vlc.DotNet.Forms;

namespace FBiliPlayer
{
	public partial class MainForm : Form
	{
		private float playSpeed = 1.0f;
		private int playIndex = -1, toPlayIndex = -1, old_width = 0, old_height = 0;
		private bool lockTrackbar = false, isFullscreen = false;
		private long max_length = 0, current_offset = 0, current_length = 0, doubleClick = -1;

		private IList<string> playList = new List<string>();
		private IList<EntryVideoData> toPlay = new List<EntryVideoData>();
		private IDictionary<int,long[]> segment_table = new Dictionary<int,long[]>();

		public MainForm()
		{
			InitializeComponent();
			OnSizeChanged(null);
			RefreshRightMenu();
		}

		private void PlayNextEntry(int increase = 1)
		{
			if(toPlay.Count > (toPlayIndex = toPlayIndex + increase))
			{
				EntryVideoData next = toPlay[toPlayIndex];
				Text = next.Title + " - " + next.Index + "." + next.IndexTitle + " [" + (toPlayIndex + 1) + "/" + toPlay.Count + "]";
				PlayByIndexFolder(next.IndexPath);
			}
			else
			{
				Stop();
			}
		}

		private bool PlayNextSegment(int increase = 1)
		{
			if(playList.Count > (playIndex = playIndex + increase))
			{
				vlcControl1.Play(new Uri(playList[playIndex]));
				current_offset = playIndex == 0 ? 0 : segment_table[playIndex - 1][0];
				current_length = segment_table[playIndex][1];
				Console.WriteLine("Playing:" + playList[playIndex]);
				return true;
			}
			playIndex = -1;
			vlcControl1.OnStopped();
			return false;
		}

		private void PlayByEntryFolder(string path,bool play = false)
		{
			path = path.Replace("\\","/");
			if(path[path.Length - 1] != '/')
			{
				path += "/";
			}
			if(!Directory.Exists(path) || !File.Exists(path + "entry.json"))
			{
				throw new Exception("Path not exists or invalid.");
			}
			dynamic json = JSON.Parse(File.ReadAllText(path + "entry.json"));
			if(!json["is_completed"])
			{
				if(MessageBox.Show("The video \"" + json["title"] + "\" may not downloaded correctly,continue?","Warning",MessageBoxButtons.OKCancel,MessageBoxIcon.Warning) != DialogResult.OK)
				{
					return;
				}
			}
			int index = 0;
			string index_title = "";
			try
			{
				dynamic ep = json["ep"];
				if(ep["index"] is int)
				{
					index = ep["index"];
				}
				else if(ep["index"] is string)
				{
					index = int.Parse(ep["index"]);
				}
				index_title = ep["index_title"];
			}
			catch { }
			try
			{
				dynamic page_data = json["page_data"];
				if(page_data["page"] is int)
				{
					index = page_data["page"];
				}
				else if(page_data["page"] is string)
				{
					index = int.Parse(page_data["page"]);
				}
				index_title = page_data["part"];
			}
			catch { }
			toPlay.Add(new EntryVideoData()
			{
				Path = path,
				Title = json["title"],
				Index = index,
				IndexPath = path + json["type_tag"],
				IndexTitle = index_title
			});
			Console.WriteLine("Added " + json["title"] + " - " + index + "." + index_title + "(" + path + ")");
			if(play)
			{
				PlayNextEntry();
			}
		}

		private void PlayByIndexFolder(string path)
		{
			path = path.Replace("\\","/");
			if(path[path.Length - 1] != '/')
			{
				path += "/";
			}
			if(!Directory.Exists(path) || !File.Exists(path + "index.json"))
			{
				throw new Exception("Path not exists or invalid.");
			}
			dynamic json = JSON.Parse(File.ReadAllText(path + "index.json"));
			IList<object> segment_list = json["segment_list"];
			Stop();
			for(int i = 0;i < segment_list.Count;i++)
			{
				IDictionary<string,object> segment = (IDictionary<string,object>)segment_list[i];
				segment_table.Add(i,new long[]
				{
					(segment_table.Count == 0 ? 0 : segment_table[segment_table.Count - 1][0]) + (long)segment["duration"],
					(long)segment["duration"]
				});
				if(File.Exists(path + i + ".mp4"))
				{
					playList.Add(path + i + ".mp4");
				}
				else if(File.Exists(path + i + ".flv"))
				{
					playList.Add(path + i + ".flv");
				}
			}
			max_length = segment_table[segment_table.Count - 1][0];
			playIndex = -1;
			PlayNextSegment();
		}

		private void Seek(long time)
		{
			if(playIndex == -1)
			{
				return;
			}
			if(time < 0)
			{
				time = 0;
			}
			for(int i = 0;i < segment_table.Count;i++)
			{
				if(segment_table[i][0] > time)
				{
					if(i != 0)
					{
						time -= segment_table[i - 1][0];
					}
					if(i != playIndex || !vlcControl1.IsPlaying)
					{
						playIndex = i;
						PlayNextSegment(0);
					}
					vlcControl1.Position = time / (float)segment_table[i][1];
					lockTrackbar = false;
					return;
				}
			}
			playIndex = -1;
			vlcControl1.Stop();
		}

		private void Stop()
		{
			playIndex = -1;
			playList.Clear();
			vlcControl1.Stop();
			Text = "Bilibili Player";
		}

		private void Pause()
		{
			if(vlcControl1.IsPlaying)
			{
				vlcControl1.Pause();
			}
			else if(playIndex != -1)
			{
				vlcControl1.Play();
			}
		}

		private void Fullscreen()
		{
			isFullscreen = !isFullscreen;
			OnSizeChanged(null);
			if(isFullscreen)
			{
				TopMost = true;
				old_width = Width;
				old_height = Height;
				FormBorderStyle = FormBorderStyle.None;
				WindowState = FormWindowState.Maximized;
			}
			else if(!isFullscreen)
			{
				TopMost = false;
				WindowState = FormWindowState.Normal;
				FormBorderStyle = FormBorderStyle.Sizable;
				Height = old_height;
				Width = old_width;
			}
			RefreshRightMenu();
		}

		private void RefreshRightMenu()
		{
			rightMenu_Pause.Checked = playIndex != -1 && !vlcControl1.IsPlaying;
			rightMenu_Fullscreen.Checked = isFullscreen;
			foreach(ToolStripMenuItem item in rightMenu_Video_Speed.DropDownItems)
			{
				item.Checked = float.Parse((string)item.Tag) == playSpeed;
			}
			rightMenu_Video_Tracks.DropDownItems.Clear();
			foreach(TrackDescription track in vlcControl1.Video.Tracks.All)
			{
				ToolStripItem item = rightMenu_Video_Tracks.DropDownItems.Add(track.Name);
				item.Tag = track.ID;
				item.Click += rightMenu_Video_Track_Item_Click;
				if(track.ID == vlcControl1.Video.Tracks.Current.ID)
				{
					((ToolStripMenuItem)item).Checked = true;
				}
			}
			rightMenu_Video_Tracks.Enabled = rightMenu_Video_Tracks.DropDownItems.Count != 0;
		}

		private void ReInitVlc(string[] args = null)
		{
			if(args == null)
			{
				List<string> tmpargs = new List<string>();
				tmpargs.Add("--quiet");
				tmpargs.Add("--rate=" + playSpeed);
				args = tmpargs.ToArray();
			}
			bool playing = false;
			float play_pos = 0;
			if(vlcControl1 != null)
			{
				playing = vlcControl1.IsPlaying;
				play_pos = vlcControl1.Position;
				vlcControl1.Dispose();
			}
			vlcControl1 = new VlcControl();
			vlcControl1.BeginInit();
			vlcControl1.VlcMediaplayerOptions = args;
			vlcControl1.BackColor = Color.Black;
			vlcControl1.Location = new Point(0,0);
			vlcControl1.Name = "vlcControl1";
			vlcControl1.Size = new Size(834,510);
			vlcControl1.Spu = -1;
			vlcControl1.TabIndex = 2;
			vlcControl1.Visible = false;
			vlcControl1.VlcLibDirectory = null;
			vlcControl1.VlcLibDirectoryNeeded += new EventHandler<VlcLibDirectoryNeededEventArgs>(this.vlcControl1_VlcLibDirectoryNeeded);
			vlcControl1.Paused += new EventHandler<VlcMediaPlayerPausedEventArgs>(this.vlcControl1_Paused);
			vlcControl1.Playing += new EventHandler<VlcMediaPlayerPlayingEventArgs>(this.vlcControl1_Playing);
			vlcControl1.PositionChanged += new EventHandler<VlcMediaPlayerPositionChangedEventArgs>(this.vlcControl1_PositionChanged);
			vlcControl1.Stopped += new EventHandler<VlcMediaPlayerStoppedEventArgs>(this.vlcControl1_Stopped);
			Controls.Add(vlcControl1);
			vlcControl1.EndInit();
			OnSizeChanged(null);
			if(playIndex != -1)
			{
				vlcControl1.SetMedia(new Uri(playList[playIndex]));
				if(playing)
				{
					vlcControl1.Play();
				}
				vlcControl1.Position = play_pos;
			}
			RefreshRightMenu();
		}

		private void timer1_Tick(object sender,EventArgs e)
		{
			if(vlcControl1.State == Vlc.DotNet.Core.Interops.Signatures.MediaStates.Ended && playIndex != -1)
			{
				PlayNextSegment();
			}
		}

		private void MainForm_Load(object sender,EventArgs e)
		{

		}

		private void MainForm_FormClosing(object sender,FormClosingEventArgs e)
		{
			Stop();
		}

		private void MainForm_SizeChanged(object sender,EventArgs e)
		{
			videobar.Top = vlcControl1.Height = ClientSize.Height - videobar.Height;
			vlcControl1.Width = videobar.Width = ClientSize.Width;
			if(playIndex != -1)
			{
				vlcControl1_PositionChanged(null,null);
			}
		}

		private void videobar_MouseDown(object sender,MouseEventArgs e)
		{
			lockTrackbar = true;
			pictureBox1.Width = e.X;
		}

		private void videobar_MouseMove(object sender,MouseEventArgs e)
		{
			if(lockTrackbar)
			{
				pictureBox1.Width = e.X;
			}
		}

		private void videobar_MouseUp(object sender,MouseEventArgs e)
		{
			if(lockTrackbar)
			{
				Seek((long)(max_length * ((float)(e.X) / videobar.Width)));
			}
		}

		private void vlcControl1_Playing(object sender,VlcMediaPlayerPlayingEventArgs e)
		{
			if(IsDisposed)
			{
				return;
			}
			Invoke(new Action(delegate
			{
				if(IsDisposed)
				{
					return;
				}
				label1.Visible = false;
				videobar.Visible = timer1.Enabled = vlcControl1.Visible = !label1.Visible;
				RefreshRightMenu();
			}));
		}

		private void vlcControl1_Paused(object sender,VlcMediaPlayerPausedEventArgs e)
		{
			if(IsDisposed)
			{
				return;
			}
			Invoke(new Action(delegate
			{
				if(IsDisposed)
				{
					return;
				}
				timer1.Enabled = false;
				RefreshRightMenu();
			}));
		}

		private void vlcControl1_Stopped(object sender,VlcMediaPlayerStoppedEventArgs e)
		{
			if(IsDisposed)
			{
				return;
			}
			Invoke(new Action(delegate
			{
				if(IsDisposed)
				{
					return;
				}
				label1.Visible = true;
				videobar.Visible = timer1.Enabled = vlcControl1.Visible = !label1.Visible;
				RefreshRightMenu();
			}));
		}

		private void vlcControl1_PositionChanged(object sender,VlcMediaPlayerPositionChangedEventArgs e)
		{
			if(IsDisposed)
			{
				return;
			}
			if(!lockTrackbar)
			{
				Invoke(new Action(delegate
				{
					if(IsDisposed)
					{
						return;
					}
					pictureBox1.Width = (int)(((current_offset + vlcControl1.Position * current_length) / max_length) * videobar.Width);
				}));
			}
		}

		private void vlcControl1_VlcLibDirectoryNeeded(object sender,VlcLibDirectoryNeededEventArgs e)
		{
			e.VlcLibDirectory = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(),"./VLC/"));
			if(!e.VlcLibDirectory.Exists)
			{
				FolderBrowserDialog dialog = new FolderBrowserDialog();
				dialog.Description = "Select Vlc libraries folder.\nSuch as your VLC installation path.";
				dialog.RootFolder = Environment.SpecialFolder.Desktop;
				if(dialog.ShowDialog() == DialogResult.OK)
				{
					e.VlcLibDirectory = new DirectoryInfo(dialog.SelectedPath);
				}
				else
				{
					MessageBox.Show("Vlc libraries not found,exiting...","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
					Environment.Exit(-1);
				}
			}
		}

		private void rightMenu_OpenFolder_Click(object sender,EventArgs e)
		{
			FolderBrowserDialog dialog = new FolderBrowserDialog();
			dialog.Description = "Open any folder in tv.danmaku.bilibili/download/";
			dialog.RootFolder = Environment.SpecialFolder.Desktop;
			if(dialog.ShowDialog() == DialogResult.OK)
			{
				if(File.Exists(Path.Combine(dialog.SelectedPath,"entry.json")))
				{
					PlayByEntryFolder(dialog.SelectedPath,true);
				}
				else if(File.Exists(Path.Combine(dialog.SelectedPath,"index.json")))
				{
					PlayByIndexFolder(dialog.SelectedPath);
				}
				else
				{

				}
			}
		}

		private void rightMenu_Pause_Click(object sender,EventArgs e)
		{
			Pause();
		}

		private void MainForm_DragDrop(object sender,DragEventArgs e)
		{
			string[] files = (string[])e.Data.GetData(DataFormats.FileDrop,false);
			foreach(string file in files)
			{
				if(File.Exists(Path.Combine(file,"entry.json")))
				{
					PlayByEntryFolder(file);
				}
				else if(File.Exists(Path.Combine(file,"index.json")))
				{
					PlayByIndexFolder(file);
				}
				else
				{
				}
			}
		}

		private void MainForm_DragEnter(object sender,DragEventArgs e)
		{
			if(e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				e.Effect = DragDropEffects.All;
			}
			else
			{
				e.Effect = DragDropEffects.None;
			}
		}

		private void rightMenu_Stop_Click(object sender,EventArgs e)
		{
			Stop();
		}

		private void rightMenu_Fullscreen_Click(object sender,EventArgs e)
		{
			Fullscreen();
		}

		private void rightMenu_Video_Speed_Item_Click(object sender,EventArgs e)
		{
			playSpeed = float.Parse((string)((ToolStripMenuItem)sender).Tag);
			ReInitVlc();
		}

		private void rightMenu_Video_Track_Item_Click(object sender,EventArgs e)
		{
			foreach(TrackDescription track in vlcControl1.Video.Tracks.All)
			{
				if((int)((ToolStripItem)sender).Tag == track.ID)
				{
					vlcControl1.Video.Tracks.Current = track;
					break;
				}
			}
			RefreshRightMenu();
		}

		private void rightMenu_DEV_Click(object sender,EventArgs e)
		{
			PlayNextEntry();
		}

		protected override void WndProc(ref Message m)
		{
			if(m.Msg == WindowsMessages.WM_PARENTNOTIFY)
			{
				Point point = new Point(m.LParam.ToInt32());
				switch(m.WParam.ToInt32())
				{
				case 513: // Left
					if(contextMenuStrip1.Visible)
					{
						doubleClick = -1;
						contextMenuStrip1.Hide();
						return;
					}
					if(vlcControl1.DisplayRectangle.Contains(point))
					{
						if(playIndex != -1)
						{
							if(vlcControl1.IsPlaying)
							{
								vlcControl1.Pause();
							}
							else
							{
								vlcControl1.Play();
							}
						}
						long seconds = DateTime.Now.Ticks / 1000000;
						if(doubleClick > seconds)
						{
							doubleClick = -1;
							Fullscreen();
						}
						else
						{
							doubleClick = seconds + 6;
						}
					}
					else
					{
						doubleClick = -1;
					}
					break;
				case 516: // Right
					doubleClick = -1;
					contextMenuStrip1.Show(this,new Point(m.LParam.ToInt32()));
					return;
				default:
					doubleClick = -1;
					break;
				}
			}
			base.WndProc(ref m);
		}

		protected override bool ProcessCmdKey(ref Message msg,Keys keyData)
		{
			if(msg.Msg == WindowsMessages.WM_KEYDOWN || msg.Msg == WindowsMessages.WM_SYSKEYDOWN)
			{
				Console.WriteLine(keyData);
				switch(keyData)
				{
				case Keys.Up:
					vlcControl1.Audio.Volume = Math.Min(vlcControl1.Audio.Volume + 5,150);
					Console.WriteLine("Volume:" + vlcControl1.Audio.Volume);
					break;
				case Keys.Down:
					vlcControl1.Audio.Volume = Math.Max(vlcControl1.Audio.Volume - 5,0);
					Console.WriteLine("Volume:" + vlcControl1.Audio.Volume);
					break;
				case Keys.Control | Keys.Up:
					vlcControl1.Audio.Volume = Math.Min(vlcControl1.Audio.Volume + 1,150);
					Console.WriteLine("Volume:" + vlcControl1.Audio.Volume);
					break;
				case Keys.Control | Keys.Down:
					vlcControl1.Audio.Volume = Math.Max(vlcControl1.Audio.Volume - 1,0);
					Console.WriteLine("Volume:" + vlcControl1.Audio.Volume);
					break;
				case Keys.Left:
					Seek(current_offset + (long)(vlcControl1.Position * current_length) - 10000);
					Console.WriteLine("-10s");
					break;
				case Keys.Right:
					Seek(current_offset + (long)(vlcControl1.Position * current_length) + 10000);
					Console.WriteLine("+10s");
					break;
				case Keys.Control | Keys.Left:
					Seek(current_offset + (long)(vlcControl1.Position * current_length) - 1000);
					Console.WriteLine("-1s");
					break;
				case Keys.Control | Keys.Right:
					Seek(current_offset + (long)(vlcControl1.Position * current_length) + 1000);
					Console.WriteLine("+1s");
					break;
				case Keys.Space:
					Pause();
					break;
				}
			}
			return base.ProcessCmdKey(ref msg,keyData);
		}
	}
}
