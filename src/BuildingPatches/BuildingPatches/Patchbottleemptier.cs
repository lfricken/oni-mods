using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using UnityEngine;

namespace BuildingPatches
{// Decompiled with JetBrains decompiler
 // Type: BottleEmptier
 // Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
 // MVID: C3209CA9-6918-453B-840E-8A1BF32B92D7
 // Assembly location: C:\Users\name\source\lib\Assembly-CSharp.dll
 // Compiler-generated code is shown

	using Klei;
	using KSerialization;
	using STRINGS;
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Runtime.CompilerServices;
	using UnityEngine;

	/*
	[HarmonyPatch(typeof(BottleEmptier), "Emit", null)]
	public static class BottleEmptierConfigM {
		static public Postfix(StatesInstance master)
		  : base(master)
		{
			this.operational = master.GetComponent<Operational>();
			this.converter = master.GetComponent<ElementConverter>();
			this.consumer = master.GetComponent<ConduitConsumer>();
		}
	}*/
	[HarmonyPatch(typeof(BottleEmptier), "Emit", null)]
	public static class BottleEmptierConfigMo
	{
		static public void Postfix(GameObject go)
		{
			PrimaryElement firstPrimaryElement = this.GetFirstPrimaryElement();
			if ((UnityEngine.Object)firstPrimaryElement == (UnityEngine.Object)null)
				return;
			Storage component = this.GetComponent<Storage>();
			float num = Mathf.Min(firstPrimaryElement.Mass, this.master.emptyRate * dt);
			if ((double)num <= 0.0)
				return;
			Tag prefabTag = firstPrimaryElement.GetComponent<KPrefabID>().PrefabTag;
			SimUtil.DiseaseInfo disease_info;
			float aggregate_temperature;
			component.ConsumeAndGetDisease(prefabTag, num, out disease_info, out aggregate_temperature);
			Vector3 position = this.transform.GetPosition();
			position.y += 1.8f;
			bool flag = this.GetComponent<Rotatable>().GetOrientation() == Orientation.FlipH;
			position.x += !flag ? 0.2f : -0.2f;
			int index = Grid.PosToCell(position) + (!flag ? 1 : -1);
			if (Grid.Solid[index])
				index += !flag ? -1 : 1;
			Element element = firstPrimaryElement.Element;
			byte idx = element.idx;

			ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
			if (!conduitDispenser.IsConnected) {
				if (element.IsLiquid)
					FallingWater.instance.AddParticle(index, idx, num, aggregate_temperature, disease_info.idx, disease_info.count, true, false, false, false);
				else
					SimMessages.ModifyCell(index, (int)idx, aggregate_temperature, num, disease_info.idx, disease_info.count, SimMessages.ReplaceType.None, false, -1);
			}
		}
	}

}
