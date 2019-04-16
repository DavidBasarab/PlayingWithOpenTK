using System;

namespace TestingGame.Common
{
	[Flags]
	public enum NewLineLocation
	{
		None = 0,
		Before = 1,
		After = 2,
		Both = Before | After
	}
}