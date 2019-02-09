using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Harmony;
using TUNING;
using UnityEngine;
using Klei.CustomSettings;


namespace newmap {
	class makenewmap
	{
		public void newmap()
		{
			//OfflineWorldGen.isLoadingScene;
			NewBaseScreen.SetStartingMinionStats(MinionStartingStats[] stats);
			CustomGameSettings.Instance.LoadWorlds();
			// add new storage manually.
		}
	}

}