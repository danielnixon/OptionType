using NUnit.Framework;
using OptionType;
using System;

namespace OptionTypeTests
{
	[TestFixture]
	public class Tests
	{
		[Test]
		public void FoldNonEmpty()
		{
			var foo = "foo".ToOption();
			var expected = "foo";
			var actual = foo.Fold(() => "bar", r => r);
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void FoldEmpty()
		{
			Option<string> foo = Option.Empty;
			var expected = "bar";
			var actual = foo.Fold(() => "bar", r => r);
			Assert.AreEqual(expected, actual);
		}
	}
}
