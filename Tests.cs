using NUnit.Framework;
using System;

namespace GameTimeMod
{
	[TestFixture()]
	public class Tests
	{
		[Test()]
		public void TestTimeSpanToString ()
		{
			TimeSpan t = new TimeSpan (1, 3, 5);
			string result = GameTimeMod.timeSpanToString (t);
			Assert.AreEqual ("1:03:05", result);
			t = new TimeSpan (0, 3, 5);
			result = GameTimeMod.timeSpanToString (t);
			Assert.AreEqual ("3:05", result);
		}
	}
}

