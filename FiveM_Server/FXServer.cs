using System;
using System.Collections.Generic;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace FiveM_Server
{
	// Token: 0x02000002 RID: 2
	internal class FXServer : BaseScript
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public FXServer()
		{
			API.RegisterCommand("volume", new Action<int, List<object>, string>(this.OnVolumeCmd), false);
			API.RegisterCommand("play", new Action<int, List<object>, string>(this.OnVideoPlayCmd), false);
			API.RegisterCommand("pause", new Action<int, List<object>, string>(this.OnVideoPauseCmd), false);
			API.RegisterCommand("stop", new Action<int, List<object>, string>(this.OnVideoStopCmd), false);
			API.RegisterCommand("vtime", new Action<int, List<object>, string>(this.OnVideoTimeCmd), false);
			API.RegisterCommand("url", new Action<int, List<object>, string>(this.OnUrlCmd), false);
			API.RegisterCommand("yt", new Action<int, List<object>, string>(this.OnYTCmd), false);
			API.RegisterCommand("player", new Action<int, List<object>, string>(this.OnPlayerCmd), false);
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002144 File Offset: 0x00000344
		private void OnVolumeCmd(int playerId, List<object> args, string cmd)
		{
			if (playerId >= 0 && args.Count == 1)
			{
				base.Players[playerId].TriggerEvent("Video:SetVolume", new object[]
				{
					float.Parse((string)args[0])
				});
			}
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002194 File Offset: 0x00000394
		private void OnVideoPlayCmd(int playerId, List<object> args, string cm)
		{
			if (!this.IsAllowedToChange(base.Players[playerId]))
			{
				return;
			}
			if (args.Count == 1)
			{
				foreach (Player player in base.Players)
				{
					player.TriggerEvent("Video:Play", new object[]
					{
						(string)args[0]
					});
				}
			}
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002218 File Offset: 0x00000418
		private void OnVideoPauseCmd(int playerId, List<object> args, string cm)
		{
			if (!this.IsAllowedToChange(base.Players[playerId]))
			{
				return;
			}
			foreach (Player player in base.Players)
			{
				player.TriggerEvent("Video:Pause", new object[0]);
			}
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002284 File Offset: 0x00000484
		private void OnVideoStopCmd(int playerId, List<object> args, string cm)
		{
			if (!this.IsAllowedToChange(base.Players[playerId]))
			{
				return;
			}
			foreach (Player player in base.Players)
			{
				player.TriggerEvent("Video:Stop", new object[0]);
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000022F0 File Offset: 0x000004F0
		private void OnVideoTimeCmd(int playerId, List<object> args, string cm)
		{
			if (!this.IsAllowedToChange(base.Players[playerId]))
			{
				return;
			}
			foreach (Player player in base.Players)
			{
				if (args.Count == 3)
				{
					int num = int.Parse((string)args[0]);
					int num2 = int.Parse((string)args[1]);
					int num3 = int.Parse((string)args[2]);
					player.TriggerEvent("Video:Time", new object[]
					{
						num,
						num2,
						num3
					});
				}
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000023B8 File Offset: 0x000005B8
		private void OnUrlCmd(int playerId, List<object> args, string cm)
		{
			if (!this.IsAllowedToChange(base.Players[playerId]))
			{
				return;
			}
			if (args.Count == 1)
			{
				foreach (Player player in base.Players)
				{
					player.TriggerEvent("Video:URL", new object[]
					{
						(string)args[0]
					});
				}
			}
		}

		// Token: 0x06000008 RID: 8 RVA: 0x0000243C File Offset: 0x0000063C
		private void OnYTCmd(int playerId, List<object> args, string cm)
		{
			if (!this.IsAllowedToChange(base.Players[playerId]))
			{
				return;
			}
			if (args.Count == 1)
			{
				foreach (Player player in base.Players)
				{
					player.TriggerEvent("Video:YT", new object[]
					{
						(string)args[0]
					});
				}
			}
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000024C0 File Offset: 0x000006C0
		private void OnPlayerCmd(int playerId, List<object> args, string cm)
		{
			if (!this.IsAllowedToChange(base.Players[playerId]))
			{
				return;
			}
			foreach (Player player in base.Players)
			{
				player.TriggerEvent("Video:Player", new object[0]);
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x0000252C File Offset: 0x0000072C
		private bool IsAllowedToChange(Player player)
		{
			foreach (string a in player.Identifiers)
			{
				foreach (string b in FXServer.AllowedPlayers)
				{
					if (a == b)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000025A0 File Offset: 0x000007A0
		private int GetRandom(int min, int max)
		{
			return new Random().Next(min, max);
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000025B0 File Offset: 0x000007B0
		public void RegisterEventHandler(string name, Delegate action)
		{
			EventHandlerDictionary eventHandlers = base.EventHandlers;
			eventHandlers[name] += action;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000025DA File Offset: 0x000007DA
		public void RegisterExport(string name, Delegate action)
		{
			base.Exports.Add(name, action);
		}

		// Token: 0x04000001 RID: 1
		public static string[] AllowedPlayers = new string[]
		{
			"steam:11000010610ca2d",
			"steam:11000010f3457b0"
		};
	}
}
