using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

using fastJSON;

namespace FBiliPlayer
{
	public partial class MainForm : Form
	{
		private int playIndex = -1;
		private bool lockTrackbar = false;
		private long max_length = 0, current_offset = 0, current_length = 0;

		private IList<string> playList = new List<string>();
		private IDictionary<int,long[]> segment_table = new Dictionary<int,long[]>();

		public MainForm()
		{
			InitializeComponent();
			vlcControl1.Dock = DockStyle.Top;
			trackBar1.Dock = DockStyle.Bottom;
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
			trackBar1.Value = 0;
			trackBar1.Maximum = (int)(max_length / 1);
			PlayNext();
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

		private void MainForm_SizeChanged(object sender,EventArgs e)
		{
			vlcControl1.Height = ClientSize.Height - trackBar1.Height;
		}
		
		private void trackBar1_MouseDown(object sender,MouseEventArgs e)
		{
			lockTrackbar = true;
		}

		private void trackBar1_MouseUp(object sender,MouseEventArgs e)
		{
			long equal = trackBar1.Value * 1;
			for(int i = 0;i < segment_table.Count;i++)
			{
				if(segment_table[i][0] > equal)
				{
					if(i != 0)
					{
						equal -= segment_table[i - 1][0];
					}
					if(i != playIndex || !vlcControl1.IsPlaying)
					{
						playIndex = i;
						PlayNext(0);
					}
					vlcControl1.Position = equal / (float)segment_table[i][1];
					lockTrackbar = false;
					return;
				}
			}
			playIndex = -1;
			vlcControl1.Stop();
		}

		private void vlcControl1_Playing(object sender,Vlc.DotNet.Core.VlcMediaPlayerPlayingEventArgs e)
		{
			Invoke(new Action(delegate
			{
				label1.Visible = false;
				timer1.Enabled = trackBar1.Visible = vlcControl1.Visible = !label1.Visible;
			}));
		}

		private void vlcControl1_Stopped(object sender,Vlc.DotNet.Core.VlcMediaPlayerStoppedEventArgs e)
		{
			Invoke(new Action(delegate
			{
				label1.Visible = true;
				timer1.Enabled = trackBar1.Visible = vlcControl1.Visible = !label1.Visible;
			}));
		}

		private void vlcControl1_PositionChanged(object sender,Vlc.DotNet.Core.VlcMediaPlayerPositionChangedEventArgs e)
		{
			if(!lockTrackbar)
			{
				Invoke(new Action(delegate
				{
					trackBar1.Value = Math.Min(trackBar1.Maximum,(int)(current_offset + vlcControl1.Position * current_length));
				}));
			}
		}

		private void vlcControl1_VlcLibDirectoryNeeded(object sender,Vlc.DotNet.Forms.VlcLibDirectoryNeededEventArgs e)
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

		private void button1_Click(object sender,EventArgs e)
		{
			vlcControl1.Position = 0.99f;
		}
	}
}
