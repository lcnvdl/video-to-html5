using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using VTH.Logic;

namespace VTH.UI
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {

        }

        private void FormMain_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) 
                e.Effect = DragDropEffects.Copy;
        }

        private void FormMain_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in files)
            {
                if(File.Exists(file))
                    listBox1.Items.Add(file);
            }
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            var args = (ProcessArguments)e.Argument;
            var files = args.Files;
            
            int total = files.Count;
            int current = 0;

            foreach (string file in files)
            {
                var ffmpeg = new Ffmpeg(file, args.Audio);
                foreach (int percent in ffmpeg.Process())
                {
                    worker.ReportProgress(percent, "partial");
                }

                current++;
                worker.ReportProgress((int)(current * 100f / total), "total");
            }
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if ((string)e.UserState == "total")
                progressBar.Value = e.ProgressPercentage;
            else
                progressBar2.Value = e.ProgressPercentage;
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            SetLoading(false);
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message, "VTH Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("Conversion finished successfully!", "VTH");
                listBox1.Items.Clear();
            }
        }

        private void buttonProcess_Click(object sender, EventArgs e)
        {
            VTH.UI.Properties.Settings.Default.Save();

            List<string> files = new List<string>();
            foreach (string item in listBox1.Items)
            {
                files.Add(item);
            }

            SetLoading(true);

            ProcessArguments args = new ProcessArguments(files, !checkBoxRemoveAudio.Checked);

            worker.RunWorkerAsync(args);
        }

        private void SetLoading(bool loading)
        {
            buttonConvert.Enabled = !loading;
            buttonRemove.Enabled = !loading;
            pictureBox1.Visible = loading;
            checkBoxRemoveAudio.Enabled = !loading;
            listBox1.Enabled = !loading;
            buttonAdd.Enabled = !loading;
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                listBox1.Items.Remove(listBox1.SelectedItem);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (openVideo.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.listBox1.Items.AddRange(openVideo.FileNames);
            }
        }
    }
}
