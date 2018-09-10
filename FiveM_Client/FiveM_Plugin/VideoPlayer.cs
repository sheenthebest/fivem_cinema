using System;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace FiveM_Plugin
{
	// Token: 0x02000005 RID: 5
	public class VideoPlayer : IDisposable
	{
		// Token: 0x0600001C RID: 28 RVA: 0x000028C4 File Offset: 0x00000AC4
		public VideoPlayer(FXClient client, string url, string resourceName)
		{
			this.resourceName = resourceName;
			this.URL = url;
			this.RTI = VideoPlayer.INDEX++;
			client.RegisterEventHandler("onClientResourceStart", new Action<string>(this.OnClientResourceStart));
			client.RegisterEventHandler("onResourceStop", new Action<string>(this.OnResourceStop));
		}

		// Token: 0x0600001D RID: 29 RVA: 0x0000294E File Offset: 0x00000B4E
		public VideoPlayer(FXClient client, string url) : this(client, url, VideoPlayer.DEFAULT_RESOURCE)
		{
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002960 File Offset: 0x00000B60
		private async void OnClientResourceStart(string resName)
		{
			if (resName == API.GetCurrentResourceName())
			{
				this.scaleform = new Scaleform(this.resourceName);
				while (!this.scaleform.IsLoaded)
				{
					await BaseScript.Delay(0);
				}
				if (VideoPlayer.TXD == -1L)
				{
					VideoPlayer.TXD = API.CreateRuntimeTxd("meows");
				}
				this.duiObj = API.CreateDui(this.URL, 1280, 720);
				this.dui = API.GetDuiHandle(this.duiObj);
				this.tx = Function.Call<long>((ulong)-1321908437, new InputArgument[]
				{
					VideoPlayer.TXD,
					"woof",
					this.dui
				});
			}
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000029A1 File Offset: 0x00000BA1
		private void OnResourceStop(string resName)
		{
			if (resName == API.GetCurrentResourceName())
			{
				this.Dispose();
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000029B6 File Offset: 0x00000BB6
		public void SetURL(string url)
		{
			this.NEW_URL = url;
			this.LastVolume = -1f;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000029CA File Offset: 0x00000BCA
		public void SendMessage(string json)
		{
			if (this.messaging)
			{
				API.SendDuiMessage(this.duiObj, json);
			}
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000029E0 File Offset: 0x00000BE0
		public void SetMessagingEnabled(bool state)
		{
			this.messaging = state;
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000029E9 File Offset: 0x00000BE9
		public void SetVolume(float volume)
		{
			this.Volume = volume;
		}

		// Token: 0x06000024 RID: 36 RVA: 0x000029F2 File Offset: 0x00000BF2
		public void SetVideoPlay(string webmVideoUrl)
		{
			this.SendMessage("{ \"type\": \"video\", \"video\":\"" + webmVideoUrl + "\" }");
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002A0A File Offset: 0x00000C0A
		public void PauseVideo()
		{
			this.SendMessage("{ \"type\": \"pause\" }");
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002A17 File Offset: 0x00000C17
		public void StopVideo()
		{
			this.SendMessage("{ \"type\": \"stop\" }");
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002A24 File Offset: 0x00000C24
		public void SetVideoTime(int hours, int minutes, int seconds)
		{
			int num = seconds + minutes * 60 + hours * 60 * 60;
			this.SendMessage("{ \"type\": \"time\", \"time\":\"" + num + "\" }");
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002A5B File Offset: 0x00000C5B
		public void SetIframeUrl(string url)
		{
			this.SendMessage("{ \"type\": \"urlframe\", \"urlframe\":\"" + url + "\" }");
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002A73 File Offset: 0x00000C73
		public void SetYT(string videoId)
		{
			this.SendMessage("{ \"type\": \"yt\", \"yt\":\"" + videoId + "\" }");
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002A8B File Offset: 0x00000C8B
		public void Refresh()
		{
			this.SetURL(this.URL);
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002A99 File Offset: 0x00000C99
		public void SendMousMove(int x, int y)
		{
			API.SendDuiMouseMove(this.duiObj, x, y);
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002AA8 File Offset: 0x00000CA8
		public void SendMouseClick(string buttonName, bool state)
		{
			if (state)
			{
				API.SendDuiMouseDown(this.duiObj, buttonName);
				return;
			}
			API.SendDuiMouseUp(this.duiObj, buttonName);
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002AC8 File Offset: 0x00000CC8
		public async void SendMouseClick(string buttonName)
		{
			API.SendDuiMouseDown(this.duiObj, buttonName);
			await BaseScript.Delay(10);
			API.SendDuiMouseUp(this.duiObj, buttonName);
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002B0C File Offset: 0x00000D0C
		public void Play(Vector3 location, Vector3 rotation, Vector3 scale)
		{
			if (this.scaleform != null && this.scaleform.IsLoaded && !this.txdHasBeenSet)
			{
				this.scaleform.CallFunction("SET_TEXTURE", new object[]
				{
					"meows",
					"woof",
					0,
					0,
					1280,
					720
				});
				this.txdHasBeenSet = true;
			}
			if (this.scaleform != null && this.scaleform.IsLoaded)
			{
				this.SendMessage("{ \"type\": \"volume\", \"volume\": " + this.Volume + " }");
				if (this.NEW_URL.Length > 0)
				{
					this.URL = this.NEW_URL;
					API.SetDuiUrl(this.duiObj, this.NEW_URL);
					this.NEW_URL = "";
				}
				this.scaleform.Render3D(location, rotation, scale);
			}
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002C08 File Offset: 0x00000E08
		public void Dispose()
		{
			API.DestroyDui(this.duiObj);
			this.scaleform.Dispose();
		}

		// Token: 0x04002CBD RID: 11453
		public static string DEFAULT_RESOURCE = "generic_texture_renderer";

		// Token: 0x04002CBE RID: 11454
		private static int INDEX = 0;

		// Token: 0x04002CBF RID: 11455
		private static long TXD = -1L;

		// Token: 0x04002CC0 RID: 11456
		private string resourceName;

		// Token: 0x04002CC1 RID: 11457
		private string URL;

		// Token: 0x04002CC2 RID: 11458
		private string NEW_URL = "";

		// Token: 0x04002CC3 RID: 11459
		private int RTI;

		// Token: 0x04002CC4 RID: 11460
		private Scaleform scaleform;

		// Token: 0x04002CC5 RID: 11461
		private long duiObj;

		// Token: 0x04002CC6 RID: 11462
		private string dui;

		// Token: 0x04002CC7 RID: 11463
		private long tx;

		// Token: 0x04002CC8 RID: 11464
		private bool txdHasBeenSet;

		// Token: 0x04002CC9 RID: 11465
		private float Volume = 1f;

		// Token: 0x04002CCA RID: 11466
		private float LastVolume = 1f;

		// Token: 0x04002CCB RID: 11467
		private bool messaging = true;
	}
}
