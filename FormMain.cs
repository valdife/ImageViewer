using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ImageViever
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void treeView1_Update()
        {
            treeView1.Nodes.Clear();
            if (textBox1.Text != "" && Directory.Exists(textBox1.Text))
                LoadDirectory(textBox1.Text);
            else
            {
                //MessageBox.Show("Select Directory!!");
                listBox1.Items.Clear();
            }
        }

        public void LoadDirectory(string Dir)
        {
            DirectoryInfo di = new DirectoryInfo(Dir);
            TreeNode tds = treeView1.Nodes.Add(di.Name);
            tds.Tag = di.FullName;
            tds.StateImageIndex = 0;
            LoadFiles(Dir);
            LoadSubDirectories(Dir, tds);
            toolStripStatusLabelPictureNumber.Text = $"Number of directories: {treeView1.GetNodeCount(true)}";
        }

        private void LoadSubDirectories(string dir, TreeNode td)
        {
            // Get all subdirectories
            string[] subdirectoryEntries = Directory.GetDirectories(dir.Trim());
            // Loop through them to see if they have any other subdirectories
            foreach (string subdirectory in subdirectoryEntries)
            {
                DirectoryInfo di = new DirectoryInfo(subdirectory);
                TreeNode tds = td.Nodes.Add(di.Name);
                tds.StateImageIndex = 0;
                tds.Tag = di.FullName;
                LoadSubDirectories(subdirectory, tds);
            }
        }

        private void LoadFiles(string dir)
        {
            listBox1.Items.Clear();
            string[] Files = Directory.GetFiles(dir, "*.*");
            // Loop through them to see files
            foreach (string file in Files)
            {
                FileInfo fi = new FileInfo(file);
                listBox1.Items.Add(fi.Name);
            }
        }

        private void changeDirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = textBox1.Text;
            DialogResult drResult = folderBrowserDialog1.ShowDialog();
            if (drResult == DialogResult.OK)
                textBox1.Text = folderBrowserDialog1.SelectedPath;
            treeView1_Update();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<string> ImageExtensions = new List<string> { ".JPG", ".JPEG", ".JPE", ".BMP", ".GIF", ".PNG", ".JFIF", ".TIF", ".TIFF", ".DIB" };
            string imageName = (string)listBox1.Items[listBox1.SelectedIndex];
            string path = textBox1.Text + "\\" + imageName;
            if (ImageExtensions.Contains(Path.GetExtension(path).ToUpperInvariant()))
            {
                Image image = Image.FromFile(path);
                pictureBox1.Image = image;
                toolStripStatusLabelPictureName.Text = imageName + $" ({image.Width},{image.Height})";
            }
            else
                MessageBox.Show("This is not an image");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            treeView1_Update();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var node = treeView1.SelectedNode;
            List<string> dirList = new List<string>();
            if (treeView1.SelectedNode.Parent != null)
            {
                while (node.Parent != null)
                {
                    dirList.Add(node.Text);
                    node = node.Parent;
                }
                dirList.Reverse();
                LoadFiles(textBox1.Text + "\\" + string.Join("\\", dirList.ToArray()));

            }
            else
            {
                LoadFiles(textBox1.Text);
            }
        }
    }
}
