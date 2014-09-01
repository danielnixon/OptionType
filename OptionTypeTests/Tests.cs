using NUnit.Framework;
using OptionType;
using System;
using System.Linq;

namespace OptionTypeTests
{
	[TestFixture]
	public class Tests
	{
		[Test]
		public void AllNonEmpty()
		{
			var foo = "foo".ToOption();
			Assert.IsTrue(foo.All(x => x.StartsWith("f")));
			Assert.IsFalse(foo.All(x => x.StartsWith("a")));
		}

		[Test]
		public void AllEmpty()
		{
			Option<string> foo = Option.Empty;
			Assert.IsTrue(foo.All(x => x.StartsWith("f")));
		}

		[Test]
		public void AnyNonEmpty()
		{
			var foo = "foo".ToOption();
			Assert.IsTrue(foo.Any(x => x.StartsWith("f")));
			Assert.IsFalse(foo.All(x => x.StartsWith("a")));
		}

		[Test]
		public void AnyEmpty()
		{
			Option<string> foo = Option.Empty;
			Assert.IsFalse(foo.Any(x => x.StartsWith("f")));
		}

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

		[Test]
		public void HasValueNonEmpty()
		{
			var foo = "foo".ToOption();
			Assert.IsTrue(foo.HasValue);
		}

		[Test]
		public void HasValueEmpty()
		{
			Option<string> foo = Option.Empty;
			Assert.IsFalse(foo.HasValue);
		}

		[Test]
		public void SelectNonEmpty()
		{
			var foo = 1.ToOption();
			var expected = 2.ToOption();
			Assert.AreEqual(expected, foo.Select(x => x + 1));
		}

		[Test]
		public void SelectEmpty()
		{
			Option<int> foo = Option.Empty;
			Option<int> expected = Option.Empty;
			Assert.AreEqual(expected, foo.Select(x => x + 1));
		}

		[Test]
		public void SelectManyNonEmpty()
		{
			Option<int> foo = 1.ToOption();
			Option<Option<int>> bar = foo.ToOption();
			Assert.AreEqual(foo, bar.SelectMany(x => x));
		}

		[Test]
		public void SelectManyEmpty()
        {
            Option<int> foo = Option.Empty;
            Option<Option<int>> bar = foo.ToOption();
            Assert.AreEqual(foo, bar.SelectMany(x => x));
        }

		[Test]
		public void ValueNonEmpty()
		{
			var foo = "foo".ToOption();
			Assert.AreEqual("foo", foo.Value);
		}

		[Test]
		public void ValueEmpty()
		{
			Option<string> foo = Option.Empty;
			Assert.Throws<InvalidOperationException> (() => {
				#pragma warning disable 219
				var v = foo.Value;
				#pragma warning restore 219
			});
		}

		[Test]
		public void WhereNonEmpty()
		{
			var foo = "foo".ToOption();
			Option<string> empty = Option.Empty;
			Assert.AreEqual(foo, foo.Where(x => x.StartsWith("f")));
			Assert.AreEqual(empty, foo.Where(x => x.StartsWith("a")));
		}

		[Test]
		public void WhereEmpty()
		{
			Option<string> foo = Option.Empty;
			Option<string> empty = Option.Empty;
			Assert.AreEqual(empty, foo.Where(x => x.StartsWith("a")));
		}
	}
}
