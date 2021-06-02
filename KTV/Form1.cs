using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using WMPLib;

namespace KTV
{
    public partial class Form1 : Form
    {
        List<string> pathlist = new List<string>();
        double max, min;
        bool paused = true;
        public Form1()
        {
            InitializeComponent();
        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog song = new OpenFileDialog();
            song.Multiselect = true;
            song.Title = "请选择音乐文件";
            song.Filter = "(*.m4a)|*.m4a";
            //song.ShowDialog();
            if(song.ShowDialog() == DialogResult.OK)
            {
                string[] namelist = song.FileNames;
                foreach(string name in namelist)
                {
                    string p = Path.GetFileNameWithoutExtension(name);
                    listBox1.Items.Add(p);
                    pathlist.Add(name);
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.URL = pathlist[listBox1.SelectedIndex];
            SongNameLabel.Text = listBox1.SelectedItem.ToString();
            paused = false;
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            max = axWindowsMediaPlayer1.currentMedia.duration;
            min = axWindowsMediaPlayer1.Ctlcontrols.currentPosition;
            trackBar1.Maximum = (int)(max);
            trackBar1.Value = (int)(min);
            if(axWindowsMediaPlayer1.playState == WMPPlayState.wmppsStopped)
            {
                int newIndex = listBox1.SelectedIndex + 1;
                newIndex = newIndex == listBox1.Items.Count ? 0 : newIndex;
                axWindowsMediaPlayer1.URL = pathlist[newIndex];
                listBox1.SelectedIndex = newIndex;
                SongNameLabel.Text = listBox1.SelectedItem.ToString();
                trackBar1.Value = 0;
                timer1.Enabled = true;
            }
        }

        private void trackBar1_MouseDown(object sender, MouseEventArgs e)
        {
            timer1.Enabled = false;
            axWindowsMediaPlayer1.Ctlcontrols.pause();
        }

        private void trackBar1_MouseUp(object sender, MouseEventArgs e)
        {
            double targetValue = trackBar1.Value;
            axWindowsMediaPlayer1.Ctlcontrols.currentPosition = targetValue;
            if (!paused) axWindowsMediaPlayer1.Ctlcontrols.play();

            timer1.Enabled = true;
        }

        private void PlayPauseBtn_Click(object sender, EventArgs e)
        {
            if (paused)//暂停时
            {
                int selectedIndex = listBox1.SelectedIndex;
                if (selectedIndex < 0)
                {
                    selectedIndex = 0;
                    listBox1.SelectedIndex = selectedIndex;
                    axWindowsMediaPlayer1.URL = pathlist[selectedIndex];
                    SongNameLabel.Text = listBox1.SelectedItem.ToString();
                }
                else
                    axWindowsMediaPlayer1.Ctlcontrols.play();
                paused = false;
            }
            else//正在播放时
            {
                axWindowsMediaPlayer1.Ctlcontrols.pause();
                paused = true;
            }
        }

        private void ReplayBtn_Click(object sender, EventArgs e)
        {
            int selectedIndex = listBox1.SelectedIndex;
            selectedIndex = selectedIndex < 0 ? 0 : selectedIndex;
            listBox1.SelectedIndex = selectedIndex;
            axWindowsMediaPlayer1.URL = pathlist[selectedIndex];
            SongNameLabel.Text = listBox1.SelectedItem.ToString();
            paused = false;
        }

        private void NextBtn_Click(object sender, EventArgs e)
        {
            int selectedIndex = listBox1.SelectedIndex + 1;
            selectedIndex = selectedIndex == listBox1.Items.Count ? 0 : selectedIndex;
            listBox1.SelectedIndex = selectedIndex;
            axWindowsMediaPlayer1.URL = pathlist[selectedIndex];
            SongNameLabel.Text = listBox1.SelectedItem.ToString();
        }
    }
}
