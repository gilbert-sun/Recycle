using Recycle.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Newtonsoft.Json;

namespace Recycle.ViewModels
{
	public class StatusPageViewModel : PageViewModel
	{
		public override string Name => App.Current.Resources["strNaviStatus"] as string;

		public override string Description => null;

		public override Geometry PathData => App.Current.Resources["geoStatus"] as Geometry;

		public List<RobotParameter> ArmParameters { get; private set; } = new List<RobotParameter>
		{
			new RobotParameter{ Value = "Good", Subtitle= new ResourceObject("strComReducer") },
			new RobotParameter{ Value = "Good", Subtitle= new ResourceObject("strComController") },
			new RobotParameter{ Value = "6000", Subtitle= new ResourceObject("strComMaxSpeed") },
			new RobotParameter{ Value = "6800", Subtitle= new ResourceObject("strComCurrentSpeed") }
		};
		public class PetModelName
		{
			public DateTimeOffset _DataOffset { get; set; }
			public long  _Date { get; set; }
			public string _FS { get; set; }
		}
		public static PetModelName _petModel = new PetModelName
		{
			_DataOffset = (DateTimeOffset)DateTime.Now,
			_Date = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds(),
			_FS = "pet_7cate_2021-6-14.weights"
		};
		
		//ArmModelNames = "pet_7cate_2021-6-14.weights"
		// public string ArmModelNames { get; set; } =  JsonSerializer.Deserialize<PetModelName>(File.ReadAllText("PetModelName1.json"))._FS;
		public string ArmModelNames { get; set; } =  "pet_7cate_2021-6-14.weights";//JsonSerializer.Deserialize<PetModelName>(JsonSerializer.Serialize(_petModel))._FS;
	}
}
