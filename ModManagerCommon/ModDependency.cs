using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ModManagerCommon
{
	public class ModDependency
	{
		public string ID { get; set; }
		public string Name { get; set; }
		public string Link { get; set; }

		public ModDependency(string depdency)
		{
			string[] strings = depdency.Split('|');

			ID = strings[0];
			Name = strings[1];
			Link = strings[2];
		}

		public string GetDependencyName()
		{
			if (Name != "")
				return Name;
			else if (ID != "")
				return ID;
			else
				return "[Mod Name Not Provided]";
		}
	}
}
