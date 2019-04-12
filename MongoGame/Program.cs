using System;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

#pragma warning disable 4014

public class Game1 : Game
{
	private GraphicsDeviceManager graphics;
	private SpriteBatch spriteBatch;

	public Game1()
	{
		graphics = new GraphicsDeviceManager(this);
		Content.RootDirectory = "Content";
	}

	protected override void Draw(GameTime gameTime)
	{
		GraphicsDevice.Clear(Color.CornflowerBlue);
		base.Draw(gameTime);
	}

	protected override void Initialize() { base.Initialize(); }

	protected override void LoadContent() { spriteBatch = new SpriteBatch(GraphicsDevice); }

	protected override void UnloadContent() { }

	protected override void Update(GameTime gameTime)
	{
		if (GamePad.GetState(PlayerIndex.One).Buttons.Back ==
			ButtonState.Pressed || Keyboard.GetState().IsKeyDown(
																Keys.Escape)) Exit();

		base.Update(gameTime);
	}
}

namespace MongoGame
{
	//https://www.gamefromscratch.com/post/2015/06/15/MonoGame-Tutorial-Creating-an-Application.aspx
	public class ProgramSender
	{
		private static bool running = true;

		[STAThread]
		public static void Main(string[] args)
		{
			Console.WriteLine("This is going to play around with MongoGame");

			using (var game = new Game1()) game.Run();

			//Console.CancelKeyPress += OnCancel;

			//await WaitForExit();
		}

		private static void OnCancel(object sender, ConsoleCancelEventArgs e)
		{
			if (e != null) e.Cancel = true;

			running = false;
		}

		private static async Task WaitForExit()
		{
			Console.WriteLine("Press Control-C to exit . . .");

			while (running) await Task.Delay(TimeSpan.FromMilliseconds(10));

			Console.WriteLine("Exiting . . . .");
		}
	}
}