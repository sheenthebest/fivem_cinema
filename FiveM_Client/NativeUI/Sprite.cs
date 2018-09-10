using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;

namespace NativeUI
{
	// Token: 0x02000002 RID: 2
	public class Sprite
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		// (set) Token: 0x06000002 RID: 2 RVA: 0x00002058 File Offset: 0x00000258
		public string TextureDict
		{
			get
			{
				return this._textureDict;
			}
			set
			{
				this._textureDict = value;
			}
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002061 File Offset: 0x00000261
		public Sprite(string textureDict, string textureName, PointF position, SizeF size, float heading, Color color)
		{
			this.TextureDict = textureDict;
			this.TextureName = textureName;
			this.Position = position;
			this.Size = size;
			this.Heading = heading;
			this.Color = color;
			this.Visible = true;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020A0 File Offset: 0x000002A0
		public Sprite(string textureDict, string textureName, PointF position, SizeF size) : this(textureDict, textureName, position, size, 0f, Color.FromArgb(255, 255, 255, 255))
		{
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000020D8 File Offset: 0x000002D8
		public void Draw()
		{
			if (!this.Visible)
			{
				return;
			}
			if (!Function.Call<bool>(91750494399812324L, new InputArgument[]
			{
				this.TextureDict
			}))
			{
				Function.Call(-2332038263791780395L, new InputArgument[]
				{
					this.TextureDict,
					true
				});
			}
			float width = (float)Screen.Resolution.Width;
			int height = Screen.Resolution.Height;
			float num = width / (float)height;
			float num2 = 1080f * num;
			float num3 = this.Size.Width / num2;
			float num4 = this.Size.Height / 1080f;
			float num5 = this.Position.X / num2 + num3 * 0.5f;
			float num6 = this.Position.Y / 1080f + num4 * 0.5f;
			Function.Call(-1729472009930024816L, new InputArgument[]
			{
				this.TextureDict,
				this.TextureName,
				num5,
				num6,
				num3,
				num4,
				this.Heading,
				this.Color.R,
				this.Color.G,
				this.Color.B,
				this.Color.A
			});
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002270 File Offset: 0x00000470
		public static void Draw(string dict, string name, int xpos, int ypos, int boxWidth, int boxHeight, float rotation, Color color)
		{
			if (!Function.Call<bool>(91750494399812324L, new InputArgument[]
			{
				dict
			}))
			{
				Function.Call(-2332038263791780395L, new InputArgument[]
				{
					dict,
					true
				});
			}
			float width = (float)Screen.Resolution.Width;
			int height = Screen.Resolution.Height;
			float num = width / (float)height;
			float num2 = 1080f * num;
			float num3 = (float)boxWidth / num2;
			float num4 = (float)boxHeight / 1080f;
			float num5 = (float)xpos / num2 + num3 * 0.5f;
			float num6 = (float)ypos / 1080f + num4 * 0.5f;
			Function.Call(-1729472009930024816L, new InputArgument[]
			{
				dict,
				name,
				num5,
				num6,
				num3,
				num4,
				rotation,
				color.R,
				color.G,
				color.B,
				color.A
			});
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000023B4 File Offset: 0x000005B4
		public static string WriteFileFromResources(Assembly yourAssembly, string fullResourceName)
		{
			string tempFileName = Path.GetTempFileName();
			return Sprite.WriteFileFromResources(yourAssembly, fullResourceName, tempFileName);
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000023D0 File Offset: 0x000005D0
		public static string WriteFileFromResources(Assembly yourAssembly, string fullResourceName, string savePath)
		{
			using (Stream manifestResourceStream = yourAssembly.GetManifestResourceStream(fullResourceName))
			{
				if (manifestResourceStream != null)
				{
					byte[] buffer = new byte[manifestResourceStream.Length];
					manifestResourceStream.Read(buffer, 0, Convert.ToInt32(manifestResourceStream.Length));
					using (FileStream fileStream = File.Create(savePath))
					{
						fileStream.Write(buffer, 0, Convert.ToInt32(manifestResourceStream.Length));
						fileStream.Close();
					}
				}
			}
			return Path.GetFullPath(savePath);
		}

		// Token: 0x04000001 RID: 1
		public PointF Position;

		// Token: 0x04000002 RID: 2
		public SizeF Size;

		// Token: 0x04000003 RID: 3
		public Color Color;

		// Token: 0x04000004 RID: 4
		public bool Visible;

		// Token: 0x04000005 RID: 5
		public float Heading;

		// Token: 0x04000006 RID: 6
		public string TextureName;

		// Token: 0x04000007 RID: 7
		private string _textureDict;
	}
}
