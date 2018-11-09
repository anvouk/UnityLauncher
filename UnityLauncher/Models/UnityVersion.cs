using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;

namespace UnityLauncher.Models
{
	[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
	public class UnityVersion
	{
		[JsonProperty("Name")]
		public string name;
		[JsonProperty("UnityPath")]
		public string unityPath;
		[JsonProperty("LicensePath")]
		public string licensePath;

		// Required for DisplayMemberPath in view
		public string Name { get => name; }

		public void Launch(bool exit = true)
		{
			try
			{
				string programData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
				string unityData = programData + @"\Unity\";

				//MessageBox.Show($"{programData}\n{unityData}\n{LicensePath}");

				if (Directory.Exists(unityData))
					File.Copy(licensePath, unityData + Path.GetFileName(licensePath), true);


				Process.Start(unityPath);
			}
			catch (Exception exc)
			{
				MessageBox.Show(exc.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
			}

			if (exit)
			{
				Application.Current.Shutdown();
			}
		}

		public static UnityVersion Copy(UnityVersion newVer)
		{
			UnityVersion ver = new UnityVersion();
			ver.name = newVer.name;
			ver.unityPath = newVer.unityPath;
			ver.licensePath = newVer.licensePath;
			return ver;
		}
	}
}
