using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO.Compression;
using System.Net;
using System.Net.WebSockets;
using System.IO;
using System.Diagnostics;
using System.Media;
using CmlLib.Launcher;
using Newtonsoft.Json.Linq;
namespace Craften_Minecraft
{

    public partial class Main : Form
    {

        public Main()
        {
            InitializeComponent();
            DeactiveMods.MouseDoubleClick += new MouseEventHandler(DeactiveMods_DoubleClick);
            ActiveMods.MouseDoubleClick += new MouseEventHandler(ActiveMods_DoubleClick);
        }
        string[,] AllMods_1_14_4 = { {"Forge", "http://files.minecraftforge.net/" },{"Optifine", "https://optifine.net/downloadx?f=OptiFine_1.14.4_HD_U_F4.jar&x=5f34b113c0098bde4e8436e9bdd52039" },{"NAY", "https://www.curseforge.com/minecraft/mc-mods/jei/download/2825683/file" } };
        private void Form1_Load(object sender, EventArgs e)
        {
            if(Properties.Settings.Default.Keks == null)
            {

            }
            else
            {
                McName.Text = "Eingeloggt als: " + Properties.Settings.Default.Keks;
            }

            Minecraft.Initialize(Minecraft.DefaultPath);
            var infos = MProfileInfo.GetProfiles();
            foreach (var item in infos)
            {
                if (item.Type == "release" || item.Name == "1.7.10" || item.Name == "1.14.4")
                    MC_Versions.Items.Add(item.Name);

            }
            UpdateCheck();
            MC_Versions.SelectedItem = "1.14.4";
            //label1.Text = test[1, 0];
            DeactiveMods.Items.Add(AllMods_1_14_4[0,0]);
            DeactiveMods.Items.Add(AllMods_1_14_4[1, 0]);
            DeactiveMods.Items.Add(AllMods_1_14_4[2, 0]);
        }
        WebClient wc = new WebClient();


        void UpdateCheck()
        {
            string version = "1.2";
            string checkv = wc.DownloadString("http://tulexnow.de/Craften-Minecraft/hallo").Trim();
            if (checkv != version)
            {
                
               DialogResult dialogresult = MessageBox.Show("New Update Has Been Found,\n Do You Want To Update?", "New Update", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
               if(dialogresult == DialogResult.Yes)
               {
                    string text = Path.GetDirectoryName(Application.ExecutablePath) + "\\Updater.exe";
                    if (File.Exists(text))
                    {
                        Process.Start(text);
                        Application.Exit();
                    }
                    else
                    {
                        MessageBox.Show("Der Updater konnte nicht gefunden werden!", "Updater nicht gefunden", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    }
               }
               else
               {

               }
            }

        }

        private void DeactiveMods_DoubleClick(object sender, MouseEventArgs e)
        {
            int index = this.DeactiveMods.IndexFromPoint(e.Location);
            if (index != System.Windows.Forms.ListBox.NoMatches)
            {
                ActiveMods.Items.Add(DeactiveMods.SelectedItem);
                DeactiveMods.Items.Remove(DeactiveMods.SelectedItem);
            }
        }
        private void ActiveMods_DoubleClick(object sender,MouseEventArgs e)
        {
            int index = this.ActiveMods.IndexFromPoint(e.Location);
            if (index != System.Windows.Forms.ListBox.NoMatches)
            {
                DeactiveMods.Items.Add(ActiveMods.SelectedItem);
                ActiveMods.Items.Remove(ActiveMods.SelectedItem);
            }
        }

        WebClient dl = new WebClient();
        Stopwatch sw = new Stopwatch();
        MProfileInfo[] versions;
        private void Button1_Click(object sender, EventArgs e)
        {
            string version = MC_Versions.SelectedItem.ToString();

            //MProfileType.Release;
            Minecraft.Initialize(Minecraft.DefaultPath);

            MLogin login = new MLogin();


            MSession session = MSession.GetOfflineSession(Properties.Settings.Default.Keks);
            MProfileInfo[] infos = MProfileInfo.GetProfilesFromWeb();
            MProfile profile = MProfile.FindProfile(infos, version);


            //var local = MProfileInfo.GetProfilesFromLocal();
            var web = MProfileInfo.GetProfilesFromWeb();

            versions = MProfileInfo.GetProfiles();
             //|| ActiveMods.Items.Contains("Forge")
            if (ActiveMods.Items.Count > 0)
            {
                if(ActiveMods.Items.Contains("Optifine"))
                {
                    string optifine = AllMods_1_14_4[1, 1].ToString();
                    dl.DownloadFile("http://tulexnow.de/Craften-Minecraft/1.14.4-OptiFine_HD_U_F4.zip", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.Minecraft\\versions\\1.14.4-OptiFine_HD_U_F4.zip");
                    System.IO.Compression.ZipFile.ExtractToDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.Minecraft\\versions\\1.14.4-OptiFine_HD_U_F4.zip", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.Minecraft\\versions");
                    //WebRequest.Create(optifine);
                }
            }
            DownloadGame(profile);
            var java = new CmlLib.Utils.MJava(Minecraft.DefaultPath + "\\runtime");
            if (!java.CheckJavaw())
            {

                java.DownloadJavaAsync();

            }




            MLaunchOption option = new MLaunchOption() // set options
            {
                StartProfile = profile,
                JavaPath = Minecraft.DefaultPath + "\\runtime\\bin\\javaw.exe", //SET YOUR JAVA PATH (if you want autoset, goto wiki)
                MaximumRamMb = 5024,
                Session = session,
                LauncherName = ""
            };
            MLaunch launch = new MLaunch(option);
            launch.GetProcess().Start(); // launch


        }
        private void DownloadGame(MProfile profile, bool downloadResource = true) // download game files
        {
            MDownloader downloader = new MDownloader(profile);
            downloader.ChangeProgress += Downloader_ChangeProgress;
            downloader.DownloadAll(downloadResource);
        }
        private void Downloader_ChangeProgress(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            labelSpeed.Text = string.Format("{0} kb/s", (e.ProgressPercentage / 1024d / sw.Elapsed.TotalSeconds).ToString("0.00"));

            progressBar.Value = e.ProgressPercentage;

            labelPerc.Text = e.ProgressPercentage.ToString() + "%";

        }
        public void ExistsAlready()
        {
            var test = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads\\test.jar");
            Process doofi = new Process();
            doofi.StartInfo.FileName = "Javaw";
            doofi.StartInfo.Arguments = @"-jar " + test;
            doofi.Start();
            doofi.WaitForExit();
            MessageBox.Show("Forge Installiert");
        }
        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            // Reset the stopwatch.
            sw.Reset();

            if (e.Cancelled == true)
            {
                MessageBox.Show("Download has been canceled.");
            }
            else
            {
                var test = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads\\test.jar");
                Process doofi = new Process();
                doofi.StartInfo.FileName = "Javaw";
                doofi.StartInfo.Arguments = @"-jar " + test;
                doofi.Start();
                doofi.WaitForExit();
                MessageBox.Show("Forge Installiert");
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            //string zippath = @"";
            //string extract = @"";

            ZipFile.ExtractToDirectory("", "");
        }


        private void button4_Click(object sender, EventArgs e)
        {
            MCLogin Login = new MCLogin();
            Login.Show(this);

        }


        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string desc = MC_Versions.SelectedItem.ToString();
            var index = MC_Versions.SelectedIndex;
            if (desc == "1.14.4")
            {
                richTextBox1.Text =

                "Game Commands\n" +

                "Added report as an option to the / debug command.Please include this while making bug reports to Minecraft about performance\n" +
                "/ reload command and / forceload commands are not available to gamemasters\n" +
                "function - permission - level is a new setting in server.properties that controls which commands functions have access to\n" +

                "Changes\n" +

                "Suspicious Stew made from poppies now gives you Night Vision instead of Speed\n" +
                "Villagers now stock more items\n" +
                "Villagers now remember their gossip after becoming a Zombie Villager\n" +
                "Villagers can now work without also restocking at the same time\n" +
                " Villagers now wait with restocking until they have trades that need restocking\n" +
                " Gossip about players who converted a Zombie Villager will now last longer\n" +
                "Village sieges no longer occur on mushroom islands\n" +
                "Improved performance of Villager pathfinding\n" +
                "Mobs will no longer try to pathfind their way through bamboo\n" +
                "The Player Activity button on the Realm screen has been removed\n" +
                "Removed camera pivot offset in first - person\n" +
                "Improved chunk loading when travelling at high speed\n" +
                "Fixed incorrect Pillager texture\n" +
                "Fixed a memory leak\n" +
                "Performance improvements\n" +
                "Bug fixes\n" +
                "";
            }
            else if (desc == "1.7.10")
            {
                string keks =
                "1.7.10\n" +
                "Changes\n" +
                "Added Realms world uploader\n" +
                "Added Realms player activity monitor\n" +
                "Realms worlds now have more options and settings\n" +
                "Command blocks in Realms have been re-enabled\n" +
                "Improvements in Realms UI\n" +
                "Bug fixes\n";
                richTextBox1.Text = keks;

            }
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reset();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void DeactiveMods_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form launcher = new Launcher();
            launcher.Show();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {            
            Point ptLowerLeft = new Point(MousePosition.X, MousePosition.Y);
            contextMenuStrip1.Show(ptLowerLeft);
            contextMenuStrip1.Enabled = true;
        }
    }
}
