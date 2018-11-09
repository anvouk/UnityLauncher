using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using Newtonsoft.Json;
using UnityLauncher.Models;

namespace UnityLauncher.ViewModels
{
	public class ShellViewModel : Screen
	{
		private readonly IWindowManager windowManager;
		private static readonly string filename = "UnityVersions.json";

		public BindableCollection<UnityVersion> UnityVersions { get; private set; }

		private UnityVersion _selectedVersion = null;
		public UnityVersion SelectedVersion
		{
			get => _selectedVersion;
			set
			{
				if (!Set(ref _selectedVersion, value))
					return;
				NotifyOfPropertyChange(() => CanEdit);
				NotifyOfPropertyChange(() => CanRemove);
			}
		}
		
		public ShellViewModel(IWindowManager windowManager)
		{
			this.windowManager = windowManager;

			UnityVersions = new BindableCollection<UnityVersion>();
			UnityVersions.CollectionChanged += UnityVersions_CollectionChanged;
			if (File.Exists(filename))
			{
				Load();
			}
			CanSave = false;
		}

		private void UnityVersions_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			CanSave = true;
		}

		public void Add()
		{
			EditViewModel edit = IoC.Get<EditViewModel>();
			if (windowManager.ShowDialog(edit) == true)
			{
				UnityVersions.Add(UnityVersion.Copy(edit.SelectedVersion));
			}
		}

		public bool CanRemove => SelectedVersion != null;
		public void Remove()
		{
			UnityVersions.Remove(SelectedVersion);
		}

		// cal:Message.Attach="[Event MouseDoubleClick] = [Edit(UnityVersions.SelectedItem)]"
		public bool CanEdit => SelectedVersion != null;
		public void Edit()
		{
			if (SelectedVersion == null)
			{
				return;
			}

			EditViewModel edit = IoC.Get<EditViewModel>();
			edit.SelectedVersion = UnityVersion.Copy(SelectedVersion);
			if (windowManager.ShowDialog(edit) == true)
			{
				UnityVersions[UnityVersions.IndexOf(SelectedVersion)] = UnityVersion.Copy(edit.SelectedVersion);
			}
		}

		private bool _collectionChanged;
		public bool CanSave
		{
			get => _collectionChanged;
			private set => Set(ref _collectionChanged, value);
		}

		public void Save()
		{
			try
			{
				JsonSerializer serializer = new JsonSerializer();
				serializer.Formatting = Formatting.Indented;

				using (StreamWriter sw = new StreamWriter(filename))
				using (JsonWriter writer = new JsonTextWriter(sw))
				{
					serializer.Serialize(writer, UnityVersions.ToArray());
				}
				CanSave = false;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Failed To Save Database", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
			MessageBox.Show("Saved!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
		}

		public void Launch() => SelectedVersion?.Launch();

		public void Load()
		{
			try
			{
				JsonSerializer serializer = new JsonSerializer();
				serializer.NullValueHandling = NullValueHandling.Ignore;

				using (StreamReader sw = new StreamReader(filename))
				using (JsonTextReader reader = new JsonTextReader(sw))
				{
					IEnumerable<UnityVersion> vers = serializer.Deserialize<IEnumerable<UnityVersion>>(reader);
					if (UnityVersions.Count != 0)
					{
						UnityVersions.Clear();
					}
					UnityVersions.AddRange(vers);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Failed To Load Database", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
		}
	}
}
