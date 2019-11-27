using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CmlLib.Launcher;

namespace Craften_Minecraft
{
    public partial class Launcher : Form
    {
        public Launcher()
        {
            InitializeComponent();
        }
        MLogin login = new MLogin();
        MSession session;
        String[] names = { "Gronkh", "SparkofPhoenix", "mrfunky21", "M1XT3NZ", "Notch" };
        private void Launcher_Load(object sender, EventArgs e)
        {
            Random rd = new Random();

            if (Properties.Settings.Default.Keks.Length < 1)
            {
                String MCP = names[rd.Next(0,4)];
                MCPicture.Load("https://minotar.net/avatar/" + MCP + "/100.png");
            }
            else
            {
                string MCP =Properties.Settings.Default.Keks;
                MCPicture.Load("https://minotar.net/avatar/"+MCP+"/100.png");
            }

            Minecraft.Initialize(Minecraft.DefaultPath);

            var infos = MProfileInfo.GetProfilesFromLocal();
            foreach (var item in infos)
            {
                    Versions.Items.Add(item.Name);

            }

        }

        private void Btn_Launch_Click(object sender, EventArgs e)
        {
            var infos = MProfileInfo.GetProfilesFromLocal();
            string selected = Versions.SelectedItem.ToString();
            MProfile profile = MProfile.FindProfile(infos, selected);
            var login = new MLogin();
            MSession result = login.TryAutoLogin();

            if (result.Result != MLoginResult.Success)
                return;
            session = result;

            var java = new CmlLib.Utils.MJava(Minecraft.DefaultPath + "\\runtime");
            if (!java.CheckJavaw())
            {

                java.DownloadJavaAsync();

            }

            DownloadGame(profile);


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
            downloader.DownloadAll(downloadResource);
        }
    }
}
