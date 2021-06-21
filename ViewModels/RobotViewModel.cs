using Recycle.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using Recycle.Services;
namespace Recycle.ViewModels
{
	public class RobotViewModel : BaseViewModel
	{
		#region [ Parameters ]
		public const string PARA_ARM_AXIS_1 = "arm_axis_1";
		public const string PARA_ARM_AXIS_2 = "arm_axis_2";
		public const string PARA_ARM_AXIS_3 = "arm_axis_3";
		public const string PARA_ARM_X = "arm_x";
		public const string PARA_ARM_Y = "arm_y";
		public const string PARA_ARM_Z = "arm_z";
		public const string PARA_CONTROLLER = "controller";
		public const string PARA_CONVEYOR_AXIS_1 = "conveyor_axis_1";
		public const string PARA_CONVEYOR_STATUS = "conveyor_status";
		public const string PARA_CURRENT_SPEED = "current_speed";
		public const string PARA_MAX_SPEED = "max_speed";
		public const string PARA_REDUCER_STATUS = "reducer_status";
		public const string PARA_VISION_RECENTLY = "vision_recently";
		public const string PARA_VISION_STATUS = "vision_status";
		public const string PARA_VISION_TOTAL_TIME = "vision_total_time";
		#endregion

		private readonly RobotPickMongoServices _robotmongodbServices;
		private readonly RobotLogMongoServices robotLogMongoServices;
		public RobotViewModel()
		{
			ArmParameters = new RobotParameter[]
			{
				Parameters[PARA_REDUCER_STATUS],
				Parameters[PARA_CONTROLLER],
				Parameters[PARA_MAX_SPEED],
				Parameters[PARA_CURRENT_SPEED]
			};

			// for demo
			_robotmongodbServices = new RobotPickMongoServices();
			robotLogMongoServices = new RobotLogMongoServices();
			
			Timer = new DispatcherTimer
			{
				Interval = TimeSpan.FromSeconds(2)
			};
			Timer.Tick += Timer_Tick;
			Timer.Start();
		}

		private void Timer_Tick(object sender, EventArgs e)
		{
			Random rand = new Random(Environment.TickCount);
			
			var timetag = ((DateTimeOffset) DateTime.Now).ToUnixTimeMilliseconds();
			var roDBmode1 = _robotmongodbServices.Dumpdata(nameof(RobotPickMongoServices.Btype.P),rand.Next(10, 40));
			var pickTime = _robotmongodbServices.GetTime(timetag); 
			
			var pickDayTime = _robotmongodbServices.GetDayTime(timetag); 
			ComponentStatus randStatus()
			{
				return rand.Next(0, 4) == 3 ? ComponentStatus.ERROR : ComponentStatus.GOOD;
			}
			RobotApi.SetControllerStatus(ID, randStatus());
			RobotApi.SetReducerStatus(ID, randStatus());
			Parameters[PARA_CURRENT_SPEED].SetParameter(rand.Next(1, 10000), randStatus());
			Parameters[PARA_MAX_SPEED].SetParameter(rand.Next(1, 10000), randStatus());
			SinkA.SetParameter(
				acc:  (int)pickDayTime,//rand.Next(1, 100),
				percent: rand.NextDouble(),
				status: randStatus());
			SinkB.SetParameter(
				acc: SinkB.Accumulation + rand.Next(1, 100),
				percent: rand.NextDouble(),
				status: randStatus());
			SinkC.SetParameter(
				acc: SinkC.Accumulation + rand.Next(1, 100),
				percent: rand.NextDouble(),
				status: randStatus());
			SinkD.SetParameter(
				acc: SinkD.Accumulation + rand.Next(1, 100),
				percent: rand.NextDouble(),
				status: randStatus());


			// var logDBmodel = robotLogMongoServices.Dumplog( "Hi->:: "+rand.Next(10, 40).ToString() );
			
			
			if (pickTime != null)
				TimePicks.Add((double)pickTime);//rand.Next(0, 45));
			else
				TimePicks.Add(0);

		}

		// Fields
		private readonly DispatcherTimer Timer;

		public readonly Dictionary<string, RobotParameter> Parameters = new Dictionary<string, RobotParameter>
		{
			[PARA_ARM_AXIS_1] = new RobotParameter { Value = "46 cm/s", Subtitle = "Axis 1st" },
			[PARA_ARM_AXIS_2] = new RobotParameter { Value = "0.0", Subtitle = "Axis 2nd" },
			[PARA_ARM_AXIS_3] = new RobotParameter { Value = "0.0", Subtitle = "Axis 3rd" },
			[PARA_ARM_X] = new RobotParameter { Value = "0.0", Subtitle = "x" },
			[PARA_ARM_Y] = new RobotParameter { Value = "0.0", Subtitle = "y" },
			[PARA_ARM_Z] = new RobotParameter { Value = "0.0", Subtitle = "z" },
			[PARA_CONTROLLER] = new RobotParameter { Value = "Good", Subtitle = new ResourceObject("strComController") },
			[PARA_CONVEYOR_AXIS_1] = new RobotParameter { Value = "46 cm/s", Subtitle = "Axis 1st" },
			[PARA_CONVEYOR_STATUS] = new RobotParameter { Value = "Good" },
			[PARA_CURRENT_SPEED] = new RobotParameter { Value = "6800", Subtitle = new ResourceObject("strComCurrentSpeed") },
			[PARA_MAX_SPEED] = new RobotParameter { Value = "6000", Subtitle = new ResourceObject("strComMaxSpeed") },
			[PARA_REDUCER_STATUS] = new RobotParameter { Value = "Good", Subtitle = new ResourceObject("strComReducer") },
			[PARA_VISION_RECENTLY] = new RobotParameter { Value = "2021/01/01", Subtitle = "Recently" },
			[PARA_VISION_STATUS] = new RobotParameter { Value = "Good", Subtitle = new ResourceObject("strNaviStatus") },
			[PARA_VISION_TOTAL_TIME] = new RobotParameter { Value = "7 days 17 hours", Subtitle = "Total Time" },
		};

		// Properties
		public string ID { get; set; }

		public string Name { get; set; }

		public double ImageWidth { get; set; }

		public double ImageHeight { get; set; }

		public string ImageFilename { get; set; }

		public Point PointC { get; set; }

		public Point PointM { get; set; }

		public Point PointS { get; set; }

		public Point PointV { get; set; }

		public DateTime StartTime { get; private set; }

		public TimeSpan Duration { get; private set; }

		public ComponentStatus Status => Parameters.Values.GetComponentStatus();

		public ComponentStatus ArmStatus => ArmParameters.GetComponentStatus();

		public List<HistogramItemViewModel> Picks { get; private set; } = new List<HistogramItemViewModel>
		{
			new HistogramItemViewModel
			{
				Header = ClassData.P,
				Value = 36000,
				Percent = 0.8
			},
			new HistogramItemViewModel
			{
				Header = ClassData.Color,
				Value = 80000,
				Percent = 1
			},
			new HistogramItemViewModel
			{
				Header = ClassData.Oil,
				Value = 36000,
				Percent = 0.7
			},
			new HistogramItemViewModel
			{
				Header = ClassData.Soy,
				Value = 88888,
				Percent = 0.27
			},
			new HistogramItemViewModel
			{
				Header = ClassData.Tray,
				Value = 55555,
				Percent = 0.50
			},
			new HistogramItemViewModel
			{
				Header = ClassData.Ch,
				Value = 66666,
				Percent = 0.72
			},
			new HistogramItemViewModel
			{
				Header = ClassData.Other,
				Value = 66666,
				Percent = 0.01
			}
		};

		public TimelineChartViewModel TimePicks { get; private set; } = new TimelineChartViewModel();

		public RobotParameter[] ArmParameters { get; private set; }

		public SinkViewModel SinkA { get; private set; } = new SinkViewModel
		{
			Label = "A Sink"
		};

		public SinkViewModel SinkB { get; private set; } = new SinkViewModel
		{
			Label = "B Sink"
		};

		public SinkViewModel SinkC { get; private set; } = new SinkViewModel
		{
			Label = "C Sink"
		};

		public SinkViewModel SinkD { get; private set; } = new SinkViewModel
		{
			Label = "D Sink"
		};

		// Methods
		public void SetStartTime(DateTime dateTime)
		{
			StartTime = dateTime;
			RaisePropertyChanged(nameof(StartTime));
		}

		public void SetDuration(TimeSpan duration)
		{
			Duration = duration;
			RaisePropertyChanged(nameof(Duration));
		}

		// Methods
		public override string ToString() => Name;
	}

	public enum ComponentStatus
	{
		GOOD,
		WARNING,
		ERROR
	}
}
