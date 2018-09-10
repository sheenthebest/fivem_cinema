using System;
using System.Drawing;
using System.Dynamic;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using NativeUI;

namespace FiveM_Plugin
{
	// Token: 0x02000003 RID: 3
	public class FXClient : BaseScript
	{
		// Token: 0x06000009 RID: 9 RVA: 0x00002464 File Offset: 0x00000664
		public FXClient()
		{
			this.RegisterTickHandler(new Func<Task>(this.OnTick));
			this.RegisterTickHandler(new Func<Task>(this.OnVolumeControlTick));
			this.carVideo = new VideoPlayer(this, FXClient.PLAYER_URL);
			this.cursor = new Sprite("commonmenu", "arrowleft", new PointF(0f, 0f), new SizeF(20f, 20f));
			this.cursor.Color = Color.FromArgb(0, 255, 20);
			this.RegisterEventHandler("Video:SetVolume", new Action<float>(this.OnVolumeReceived));
			this.RegisterEventHandler("Video:Play", new Action<string>(this.OnVideoPlayReceived));
			this.RegisterEventHandler("Video:Pause", new Action(this.OnVideoPauseReceived));
			this.RegisterEventHandler("Video:Stop", new Action(this.OnVideoStopReceived));
			this.RegisterEventHandler("Video:Time", new Action<int, int, int>(this.OnVideoTimeReceived));
			this.RegisterEventHandler("Video:URL", new Action<string>(this.OnSwitchToURLReceived));
			this.RegisterEventHandler("Video:YT", new Action<string>(this.OnSwitchToYTReceived));
			this.RegisterEventHandler("Video:Player", new Action(this.OnSwitchToPlayerReceived));
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002610 File Offset: 0x00000810
		public void OnClientReceiveTimeFromPlayer(ExpandoObject data, CallbackDelegate cb)
		{
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002612 File Offset: 0x00000812
		private void OnVolumeReceived(float vol)
		{
			this.CustomVolume = vol;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x0000261B File Offset: 0x0000081B
		private void OnVideoPlayReceived(string video)
		{
			this.carVideo.SetVideoPlay(video);
			this.OnVolumeControlTick();
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002630 File Offset: 0x00000830
		private void OnVideoPauseReceived()
		{
			this.carVideo.PauseVideo();
		}

		// Token: 0x0600000E RID: 14 RVA: 0x0000263D File Offset: 0x0000083D
		private void OnVideoStopReceived()
		{
			this.carVideo.StopVideo();
		}

		// Token: 0x0600000F RID: 15 RVA: 0x0000264A File Offset: 0x0000084A
		private void OnVideoTimeReceived(int h, int m, int s)
		{
			this.carVideo.SetVideoTime(h, m, s);
		}

		// Token: 0x06000010 RID: 16 RVA: 0x0000265A File Offset: 0x0000085A
		private void OnSwitchToURLReceived(string url)
		{
			this.carVideo.SetIframeUrl(url);
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002668 File Offset: 0x00000868
		private void OnSwitchToYTReceived(string url)
		{
			this.carVideo.SetYT(url);
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002676 File Offset: 0x00000876
		private void OnSwitchToPlayerReceived()
		{
			this.carVideo.Refresh();
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002684 File Offset: 0x00000884
		public void DrawDevText(string text)
		{
			API.SetTextFont(4);
			API.SetTextProportional(true);
			API.SetTextScale(0.5f, 0.5f);
			API.SetTextColour(255, 255, 255, 255);
			API.SetTextDropshadow(0, 0, 0, 0, 255);
			API.SetTextEdge(1, 0, 0, 0, 255);
			API.SetTextDropShadow();
			API.SetTextOutline();
			API.SetTextEntry("STRING");
			API.AddTextComponentString(text);
			API.DrawText(0.005f, 0.005f + (float)this.nextIndex * 0.025f);
			this.nextIndex++;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002728 File Offset: 0x00000928
		public async Task OnVolumeControlTick()
		{
			Vector3 position = Game.PlayerPed.Position;
			float num = 1f - Math.Min(Math.Abs(API.GetDistanceBetweenCoords(position.X, position.Y, position.Z, this.CLoc.X, this.CLoc.Y, this.CLoc.Z, true)), (float)this.MAX_DISTANCE) / (float)this.MAX_DISTANCE;
			this.v = num * this.CustomVolume;
			if (this.carVideo != null)
			{
				this.carVideo.SetVolume(num * this.CustomVolume);
			}
			await BaseScript.Delay(500);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002770 File Offset: 0x00000970
		public async Task OnTick()
		{
			this.carVideo.Play(this.CLoc, this.CRot, this.CScl);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000027B8 File Offset: 0x000009B8
		public void RegisterEventHandler(string name, Delegate action)
		{
			try
			{
				EventHandlerDictionary eventHandlers = base.EventHandlers;
				eventHandlers[name] += action;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002800 File Offset: 0x00000A00
		public void RegisterNuiEventHandler(string name, Delegate action)
		{
			try
			{
				Function.Call((ulong)-855388759, new InputArgument[]
				{
					name
				});
				this.RegisterEventHandler("__cfx_nui:" + name, action);
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002850 File Offset: 0x00000A50
		public void RegisterTickHandler(Func<Task> action)
		{
			try
			{
				base.Tick += action;
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000019 RID: 25 RVA: 0x0000287C File Offset: 0x00000A7C
		public void DeregisterTickHandler(Func<Task> action)
		{
			try
			{
				base.Tick -= action;
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000028A8 File Offset: 0x00000AA8
		public void RegisterExport(string name, Delegate action)
		{
			base.Exports.Add(name, action);
		}

		// Token: 0x04000008 RID: 8
		private int nextIndex;

		// Token: 0x04000009 RID: 9
		private Sprite cursor;

		// Token: 0x0400000A RID: 10
		private static string PLAYER_URL = "http://173.249.59.200/cinema/";

		// Token: 0x0400000B RID: 11
		private VideoPlayer carVideo;

		// Token: 0x0400000C RID: 12
		private bool spawned;

		// Token: 0x0400000D RID: 13
		private Vector3 poss;

		// Token: 0x0400000E RID: 14
		private bool cursorEnabled;

		// Token: 0x0400000F RID: 15
		private Vector3 cursorPos;

		// Token: 0x04000010 RID: 16
		private float CustomVolume = 1f;

		// Token: 0x04000011 RID: 17
		private int wmxi;

		// Token: 0x04000012 RID: 18
		private int wmyi;

		// Token: 0x04000013 RID: 19
		private int MAX_DISTANCE = 100;

		// Token: 0x04000014 RID: 20
		private float v;

		// Token: 0x04000015 RID: 21
		private Vector3 CLoc = new Vector3(-1678.949f, -928.3431f, 20.6290932f);

		// Token: 0x04000016 RID: 22
		private Vector3 CRot = new Vector3(0f, 0f, -140f);

		// Token: 0x04000017 RID: 23
		private Vector3 CScl = new Vector3(0.969999969f, 0.484999985f, -0.1f);
	}
}
