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
		private static string[] FILTER_NONE = new string[0],
			FILTER_TRANSFORM_90 = new string[]
			{
				"--video-filter=transform",
				"--transform-type=90"
			}, FILTER_TRANSFORM_180 = new string[]
			{
				"--video-filter=transform",
				"--transform-type=180"
			}, FILTER_TRANSFORM_270 = new string[]
			{
				"--video-filter=transform",
				"--transform-type=270"
			}, FILTER_WAVE = new string[]
			{
				"--video-filter=wave"
			};

		private int playIndex = -1, old_width = 0, old_height = 0;
		private bool lockTrackbar = false, isFullscreen = false;
		private long max_length = 0, current_offset = 0, current_length = 0, doubleClick = -1;

		private string[] filter = new string[0];

		private IList<string> playList = new List<string>();
		private IDictionary<int,long[]> segment_table = new Dictionary<int,long[]>();

		public MainForm()
		{
			InitializeComponent();
			OnSizeChanged(null);
		}

		private bool PlayNext(int increase = 1)
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
			playList.Clear();
			segment_table.Clear();
			vlcControl1.Stop();
			for(int i = 0;i < segment_list.Count;i++)
			{
				IDictionary<string,object> segment = (IDictionary<string,object>)segment_list[i];
				segment_table.Add(i,new long[]
				{
					(segment_table.Count == 0 ? 0 : segment_table[segment_table.Count - 1][0]) + (long)segment["duration"],
					(long)segment["duration"]
				});
				playList.Add(path + i + ".mp4");
			}
			max_length = segment_table[segment_table.Count - 1][0];
			playIndex = -1;
			PlayNext();
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
						PlayNext(0);
					}
					vlcControl1.Position = time / (float)segment_table[i][1];
					lockTrackbar = false;
					return;
				}
			}
			playIndex = -1;
			vlcControl1.Stop();
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
			rightMenu_Video_Filters_None.Checked = rightMenu_Video_Filters_Wave.Checked = rightMenu_Video_Filters_Transform_90.Checked =
				rightMenu_Video_Filters_Transform_180.Checked = rightMenu_Video_Filters_Transform_270.Checked = false;
			rightMenu_Video_Filters_None.Checked = filter == FILTER_NONE;
			rightMenu_Video_Filters_Transform_90.Checked = filter == FILTER_TRANSFORM_90;
			rightMenu_Video_Filters_Transform_180.Checked = filter == FILTER_TRANSFORM_180;
			rightMenu_Video_Filters_Transform_270.Checked = filter == FILTER_TRANSFORM_270;
			rightMenu_Video_Filters_Wave.Checked = filter == FILTER_WAVE;
		}

		private void ReInitVlc(string[] args = null)
		{
			if(args == null)
			{
				List<string> tmpargs = new List<string>();
				tmpargs.Add("--quiet");
				tmpargs.AddRange(filter);
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
		}

		private void timer1_Tick(object sender,EventArgs e)
		{
			if(vlcControl1.State == Vlc.DotNet.Core.Interops.Signatures.MediaStates.Ended && playIndex != -1)
			{
				PlayNext();
			}
		}

		private void MainForm_Load(object sender,EventArgs e)
		{
			PlayByIndexFolder(@"R:\30216\lua.mp4.bapi.9");
		}

		private void MainForm_FormClosing(object sender,FormClosingEventArgs e)
		{
			playIndex = -1;
			playList.Clear();
			vlcControl1.Stop();
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

		private void rightMenu_Pause_Click(object sender,EventArgs e)
		{
			Pause();
		}

		private void rightMenu_Stop_Click(object sender,EventArgs e)
		{
			playIndex = -1;
			playList.Clear();
			vlcControl1.Stop();
		}

		private void rightMenu_Fullscreen_Click(object sender,EventArgs e)
		{
			Fullscreen();
		}

		private void rightMenu_Video_Filters_None_Click(object sender,EventArgs e)
		{
			filter = FILTER_NONE;
			ReInitVlc();
		}

		private void rightMenu_Video_Filters_Transform_90_Click(object sender,EventArgs e)
		{
			filter = FILTER_TRANSFORM_90;
			ReInitVlc();
		}

		private void rightMenu_Video_Filters_Transform_180_Click(object sender,EventArgs e)
		{
			filter = FILTER_TRANSFORM_180;
			ReInitVlc();
		}

		private void rightMenu_Video_Filters_Transform_270_Click(object sender,EventArgs e)
		{
			filter = FILTER_TRANSFORM_270;
			ReInitVlc();
		}

		private void rightMenu_Video_Filters_Wave_Click(object sender,EventArgs e)
		{
			filter = FILTER_WAVE;
			ReInitVlc();
		}

		private void rightMenu_DEV_Click(object sender,EventArgs e)
		{
			vlcControl1.Position = 0.99f;
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
			if((msg.Msg == WindowsMessages.WM_KEYDOWN) || (msg.Msg == WindowsMessages.WM_SYSKEYDOWN))
			{
				Console.WriteLine(keyData);
				switch(keyData)
				{
				case Keys.Down:
					break;
				case Keys.Up:
					break;
				case Keys.Left:
					Seek(current_offset + (long)(vlcControl1.Position * current_length) - 10000);
					break;
				case Keys.Right:
					Seek(current_offset + (long)(vlcControl1.Position * current_length) + 10000);
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
