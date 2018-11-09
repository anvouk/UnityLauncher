using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using Microsoft.Win32;
using UnityLauncher.Models;

namespace UnityLauncher.ViewModels
{
	public class EditViewModel : Screen
	{
		public UnityVersion SelectedVersion { get; set; }

		public EditViewModel()
		{
			SelectedVersion = new UnityVersion();
		}

		public string Name
		{
			get => SelectedVersion.name;
			set
			{
				if (!Set(ref SelectedVersion.name, value))
					return;
				NotifyOfPropertyChange(() => CanConfirm);
			}
		}

		public string UnityApp
		{
			get => SelectedVersion.unityPath;
			set
			{
				if (!Set(ref SelectedVersion.unityPath, value))
					return;
				NotifyOfPropertyChange(() => CanConfirm);
			}
		}

		public string LicensePath
		{
			get => SelectedVersion.licensePath;
			set
			{
				if (!Set(ref SelectedVersion.licensePath, value))
					return;
				NotifyOfPropertyChange(() => CanConfirm);
			}
		}

		public void SearchUnityApp()
		{
			OpenFileDialog openFileDialog = new OpenFileDialog()
			{
				Title = "Unity Application Path",
				CheckFileExists = true,
				Filter = "Executable Files (.exe)|*.exe|All Files (*.*)|*.*",
			};

			if (openFileDialog.ShowDialog() == true)
			{
				if (File.Exists(openFileDialog.FileName))
				{
					UnityApp = openFileDialog.FileName;
				}
			}
		}

		public void SearchLicense()
		{
			OpenFileDialog openFileDialog = new OpenFileDialog()
			{
				Title = "Unity License",
				CheckFileExists = true,
				Filter = "Unity License Files (.ulf)|*.ulf|All Files (*.*)|*.*",
				InitialDirectory = AppDomain.CurrentDomain.BaseDirectory
			};

			if (openFileDialog.ShowDialog() == true)
			{
				if (File.Exists(openFileDialog.FileName))
				{
					LicensePath = openFileDialog.FileName;
				}
			}
		}

		public bool CanConfirm
		{
			get => !string.IsNullOrWhiteSpace(Name) &&
				!string.IsNullOrWhiteSpace(UnityApp) &&
				!string.IsNullOrWhiteSpace(LicensePath);
		}

		public void Confirm()
		{
			if (!File.Exists(UnityApp))
			{
				MessageBox.Show("Unity application path is invalid!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			if (!File.Exists(LicensePath))
			{
				MessageBox.Show("Unity license path is invalid!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			TryClose(true);
		}
	}
}
