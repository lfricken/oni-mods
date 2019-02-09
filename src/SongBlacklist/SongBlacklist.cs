using System.Collections.Generic;
using Harmony;
using System.IO;

namespace SongBlacklist
{
	class SongBlacklist
	{

		static string defaultpath = System.Reflection.Assembly.GetExecutingAssembly().Location.Replace(".dll", "") + "songblacklist";
		//static string defaultpath = Path.Combine(configpath, "songblacklist");
		static public List<string> songlist = new List<string>();
		static public string[] blacklist;
		static public void initialize()
		{
			Directory.CreateDirectory(defaultpath);
			if (!File.Exists(Path.Combine(defaultpath, "blacklist.txt")))
				File.AppendAllText((Path.Combine(defaultpath, "blacklist.txt")), "");
			if (!File.Exists(Path.Combine(defaultpath, "allsongs.txt")))
				File.AppendAllText((Path.Combine(defaultpath, "allsongs.txt")), "");
			if (!File.Exists(Path.Combine(defaultpath, "playedsongs.txt")))
				File.AppendAllText((Path.Combine(defaultpath, "playedsongs.txt")), "");
		}



		static public void blacklistsong()
		{
			foreach (string song in MusicManager.instance.activeSongs.Keys) { blacklist.Add(song); File.AppendAllText(Path.Combine(defaultpath, "blacklist.txt"), song); }
		}

		[HarmonyPatch(typeof(MusicManager), "PlaySong")]
		public static void PostFix(ref string song_name)
		{
			File.AppendAllText((Path.Combine(defaultpath, "playedsongs.txt")), song_name);
		}
		[HarmonyPatch(typeof(MusicManager), "ResetUnplayedSongs")]
		public static void PostFix()
		{
			foreach (var song in this.songMap) {
				songlist.Add(song.Key);
			}
			File.WriteAllLines((Path.Combine(defaultpath, "playedsongs.txt")), songlist.ToArray());
			blacklist = File.ReadAllLines(Path.Combine(defaultpath, "blacklist.txt"));
			foreach (string song in blacklist) {
				this.unplayedSongs.Remove(song);
			}
		}
	}
}
