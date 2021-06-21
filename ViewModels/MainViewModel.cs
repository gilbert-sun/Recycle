using Recycle.Models;
using Recycle.Libraries;
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Windows;

namespace Recycle.ViewModels
{
	public class MainViewModel : BaseViewModel
	{
		public static MainViewModel Instance => App.Current.Resources["mainViewModel"] as MainViewModel;

		public MainViewModel()
		{
			Initialize();
			SelectedRobot = Robots.FirstOrDefault();
		}

		private string picksUnit = TimeUnit.SECOND;

		public string CurrentLanguage { get; private set; }

		public void ChangeLanguage(string language)
		{
			if (CurrentLanguage == language ||
				!App.ChangeCulture(language))
			{
				return;
			}
			CurrentLanguage = language;
			RaisePropertyChanged(nameof(CurrentLanguage));
		}

		// Privilege
		public Role Role { get; private set; } = Role.OPERATOR;

		public bool IsAdmin => Role == Role.ADMINISTRATOR;

		public bool CanAdjust => IsAdmin;// && !IsRunning;

		public void SetRole(Role role)
		{
			Role = role;
			RaisePropertyChanged("IsAdmin");
			RaisePropertyChanged("CanAdjust");
		}

		// Robots
		private readonly Dictionary<string, RobotViewModel> robotsSrc = new Dictionary<string, RobotViewModel>();

		private RobotViewModel selectedRobot;

		public bool IsRunning { get; private set; }

		public ObservableCollection<RobotViewModel> Robots { get; private set; } = new ObservableCollection<RobotViewModel>();

		public RobotViewModel SelectedRobot
		{
			get => selectedRobot;
			set
			{
				selectedRobot = value;
				RaisePropertyChanged(nameof(SelectedRobot));
			}
		}

		public IEnumerable<RobotViewModel> RobotsStatus => Robots.Where(i => i.Status != ComponentStatus.GOOD);

		public string PicksUnit
		{
			get => picksUnit;
			set
			{
				picksUnit = value;
				RaisePropertyChanged(nameof(PicksUnit));
			}
		}

		public void Start()
		{
			IsRunning = true;
			RaisePropertyChanged(nameof(CanAdjust));
		}

		public RobotViewModel GetRobot(string robotID)
		{
			robotsSrc.TryGetValue(robotID, out RobotViewModel value);
			return value;
		}

		private void Initialize()
		{
			var dir = Properties.Settings.Default.RobotDir;
			var root = new DirectoryInfo(dir);
			if (!root.Exists)
			{
				return;
			}
			foreach (var dirInfo in root.GetDirectories())
			{
				if (dirInfo.GetFiles("index.ini").FirstOrDefault() is FileInfo fileInfo)
				{
					CreateRobotViewModel(fileInfo);
					if (Robots.Count >= 4)
					{
						return;
					}
				}
			}
		}

		private void CreateRobotViewModel(FileInfo fileInfo)
		{
			Dictionary<string, string> data = new Dictionary<string, string>
			{
				{ "id", null },
				{ "name", null },
				{ "image", null },
				{ "width", null },
				{ "height", null },
				{ "point_c", null },
				{ "point_m", null },
				{ "point_s", null },
				{ "point_v", null }
			};
			data.ReadFromINI("Main", fileInfo.FullName);

			if (string.IsNullOrEmpty(data["id"]) ||
				string.IsNullOrEmpty(data["name"]) ||
				Robots.Any(i => i.ID == data["id"]))
			{
				return;
			}
			var robot = new RobotViewModel
			{
				ID = data["id"],
				Name = data["name"],
				ImageFilename = Path.Combine(fileInfo.DirectoryName, data["image"])
			};
			if (double.TryParse(data["width"], out double width))
			{
				robot.ImageWidth = width;
			}
			if (double.TryParse(data["height"], out double height))
			{
				robot.ImageHeight = height;
			}
			#region points
			try
			{
				robot.PointC = Point.Parse(data["point_c"]);
				robot.PointM = Point.Parse(data["point_m"]);
				robot.PointS = Point.Parse(data["point_s"]);
				robot.PointV = Point.Parse(data["point_v"]);
			}
			catch { }
			#endregion

			robotsSrc.Add(robot.ID, robot);
			Robots.Add(robot);
		}

		// Classes
		private ClassData selectedClassA = ClassData.P;

		private ClassData selectedClassB = ClassData.P;

		private ClassData selectedClassC = ClassData.P;

		private ClassData selectedClassD = ClassData.P;

		public ClassData SelectedClassA
		{
			get => selectedClassA;
			set
			{
				selectedClassA = value;
				RaisePropertyChanged(nameof(SelectedClassA));
			}
		}

		public ClassData SelectedClassB
		{
			get => selectedClassB;
			set
			{
				selectedClassB = value;
				RaisePropertyChanged(nameof(SelectedClassB));
			}
		}

		public ClassData SelectedClassC
		{
			get => selectedClassC;
			set
			{
				selectedClassC = value;
				RaisePropertyChanged(nameof(SelectedClassC));
			}
		}

		public ClassData SelectedClassD
		{
			get => selectedClassD;
			set
			{
				selectedClassD = value;
				RaisePropertyChanged(nameof(SelectedClassD));
			}
		}

		public ClassData[] ClassDatas { get; private set; } = new[]
		{
			ClassData.P,
			ClassData.Color,
			ClassData.Oil,
			ClassData.Soy,
			ClassData.Ch,
			ClassData.Tray,
			ClassData.Other,
			ClassData.None
		};

		// Page
		public PageViewModel CurrentContent { get; private set; }

		public void SelectPage(PageIndex value)
		{
			switch (value)
			{
				case PageIndex.HOME:
					CurrentContent = new HomePageViewModel(Robots);
					break;
				case PageIndex.TASK:
					CurrentContent = new TaskPageViewModel();
					break;
				case PageIndex.STATUS:
					CurrentContent = new StatusPageViewModel();
					break;
				case PageIndex.ADJUST:
					CurrentContent = new AdjustPageViewModel();
					break;
				case PageIndex.LOG:
					CurrentContent = new LogPageViewModel();
					break;
				default:
					CurrentContent = null;
					break;
			}
			RaisePropertyChanged("CurrentContent");
		}

	}
}
