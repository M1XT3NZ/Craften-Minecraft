using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;
using Ionic.Zip;
//Updater from Team-ELAN Arma 3 Server //https://team-elan.de/
namespace ConsoleApplication1
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			Console.WriteLine("Updater wurde gestartet...");
			string str = "https://tulexnow.de/Craften-Minecraft/";
            string text = "Craften-Minecraft.zip";
            //string text = "test.txt";
			string text2 = null;
			string directoryName2 = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string directoryName = Path.GetFullPath(Path.Combine(directoryName2, @"..\"));
			text2 = str + text;
			Console.WriteLine("Warte bis Craften-Minecraft beendet wurde...");
			bool flag = true;
			while (flag)
			{
				Process[] processesByName = Process.GetProcessesByName("Craften Minecraft");
				if (processesByName.Length < 1)
				{
					flag = false;
				}
				Thread.Sleep(1000);
			}
            
			Console.WriteLine("Verbindung zum Download-Server wird hergestellt...");
			Uri requestUri = new Uri(text2);
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUri);
			httpWebRequest.Timeout = 15000;
			try
			{
				HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
			}
			catch (Exception)
			{
				Console.WriteLine("Verbindung zum Download-Server fehlgeschlagen...");
				Console.WriteLine("\r\nBeliebige Taste zum Beenden drücken...");
				Console.ReadLine();
				return;
			}
			WebClient webClient = new WebClient();
			Console.WriteLine("Craften-Minecraft wird aktualisiert...");
			webClient.DownloadFile(text2, directoryName + "\\" + text);
            Console.WriteLine("Aktualisierung abgeschlossen...");

            using (ZipFile zip = ZipFile.Read(directoryName+"\\"+text))
            {
                foreach(ZipEntry e in zip)
                {
                    e.Extract(directoryName, ExtractExistingFileAction.OverwriteSilently);
                }
            }
            File.Delete(directoryName + "\\" +text);
            Process.Start(directoryName+"\\"+ "Craften Minecraft.exe");
		}
	}
}
