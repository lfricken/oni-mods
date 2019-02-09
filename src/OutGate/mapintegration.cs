using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Harmony;
using TUNING;
using UnityEngine;
using Klei.CustomSettings;

namespace mapintegration
{

	public List<List<SettingLevel>> getsettings()
	{
		return GodTool.allsettings;
	}

	public void setsettings(string setting)
	{

	}
	public void sethardness(int hardness) {

	}

	public void startnewmap() { }
}