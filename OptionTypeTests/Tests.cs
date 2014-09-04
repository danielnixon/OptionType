using NUnit.Framework;
using OptionType;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OptionTypeTests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void AllNonEmpty()
        {
            var foo = Option.Create("foo");
            Assert.IsTrue(foo.All(x => x.StartsWith("f")));
            Assert.IsFalse(foo.All(x => x.StartsWith("a")));
        }

        [Test]
        public void AllEmpty()
        {
            var foo = Option<string>.Empty;
            Assert.IsTrue(foo.All(x => x.StartsWith("f")));
        }

        [Test]
        public void AnyNonEmpty()
        {
            var foo = Option.Create("foo");
            Assert.IsTrue(foo.Any());
            Assert.IsTrue(foo.Any(x => x.StartsWith("f")));
            Assert.IsFalse(foo.All(x => x.StartsWith("a")));
        }

        [Test]
        public void AnyEmpty()
        {
            var foo = Option<string>.Empty;
            Assert.IsFalse(foo.Any());
            Assert.IsFalse(foo.Any(x => x.StartsWith("f")));
        }

        [Test]
        public void DefaultIfEmptyNonEmpty()
        {
            var foo = Option.Create("foo");
            var bar = "bar";
            var expected = Option.Create("foo");
            Assert.AreEqual(expected, foo.DefaultIfEmpty(bar));
        }

        [Test]
        public void DefaultIfEmptyEmpty()
        {
            var foo = Option<string>.Empty;
            var bar = "bar";
            var expected = Option.Create("bar");
            Assert.AreEqual(expected, foo.DefaultIfEmpty(bar));
        }

        [Test]
        public void FirstOptionNonEmpty()
        {
            var foo = new List<string> { "foo" };
            var expected = Option.Create("foo");
            Assert.AreEqual(expected, foo.FirstOption());
        }

        [Test]
        public void FirstOptionEmpty()
        {
            var foo = new List<string>();
            var expected = Option<string>.Empty;
            Assert.AreEqual(expected, foo.FirstOption());
        }

        [Test]
        public void FirstOptionWithPredicateNonEmpty()
        {
            var foo = new List<string> { "foo" };

            var expected = Option.Create("foo");
            Assert.AreEqual(expected, foo.FirstOption(x => x == "foo"));

            var empty = Option<string>.Empty;
            Assert.AreEqual(empty, foo.FirstOption(x => x == "bar"));
        }

        [Test]
        public void FirstOptionWithPredicateEmpty()
        {
            var foo = new List<string>();
            var expected = Option<string>.Empty;
            Assert.AreEqual(expected, foo.FirstOption(x => x == "foo"));
        }

        [Test]
        public void FoldNonEmpty()
        {
            var foo = Option.Create("foo");
            var expected = "foo";
            var actual = foo.Fold(() => "bar", r => r);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FoldEmpty()
        {
            var foo = Option<string>.Empty;
            var expected = "bar";
            var actual = foo.Fold(() => "bar", r => r);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ForEachNonEmpty()
        {
            var foo = Option.Create("foo");
            var i = 0;

            foo.ForEach(x =>
            {
                i++;
                Assert.AreEqual("foo", x);
            });

            Assert.AreEqual(1, i);
        }

        [Test]
        public void ForEachEmpty()
        {
            var foo = Option<string>.Empty;
            foo.ForEach(x => Assert.Fail());
        }

        [Test]
        public void ForEachLoopNonEmpty()
        {
            var foo = Option.Create("foo");
            var i = 0;

            foreach (var x in foo)
            {
                i++;
                Assert.AreEqual("foo", x);
            }

            Assert.AreEqual(1, i);
        }

        [Test]
        public void ForEachLoopEmpty()
        {
            var foo = Option<string>.Empty;
            foreach (var x in foo)
            {
                Assert.Fail();
            }
        }

        [Test]
        public void HasValueNonEmpty()
        {
            var foo = Option.Create("foo");
            Assert.IsTrue(foo.HasValue);
        }

        [Test]
        public void HasValueEmpty()
        {
            var foo = Option<string>.Empty;
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
            var foo = Option<int>.Empty;
            var expected = Option<int>.Empty;
            Assert.AreEqual(expected, foo.Select(x => x + 1));
        }

        [Test]
        public void SelectManyNonEmpty()
        {
            var foo = 1.ToOption();
            var bar = foo.ToOption();
            Assert.AreEqual(foo, bar.SelectMany(x => x));
        }

        [Test]
        public void SelectManyEmpty()
        {
            var foo = Option<int>.Empty;
            var bar = foo.ToOption();
            Assert.AreEqual(foo, bar.SelectMany(x => x));
        }

        [Test]
        public void ToOptionNonEmpty()
        {
            Assert.AreEqual(Option.Create("foo"), "foo".ToOption());
        }

        [Test]
        public void ToOptionEmpty()
        {
            Assert.AreEqual(Option<string>.Empty, ((string)null).ToOption());
        }

        [Test]
        public void ToOptionNullableStructNonEmpty()
        {
            Assert.AreEqual(Option.Create(1), Option.Create((int?)1));
        }

        [Test]
        public void ToOptionNullableStructEmpty()
        {
            Assert.AreEqual(Option<int>.Empty, Option.Create((int?)null));
        }

        [Test]
        public void ValueNonEmpty()
        {
            var foo = Option.Create("foo");
            Assert.AreEqual("foo", foo.Value);
        }

        [Test]
        public void ValueEmpty()
        {
            var foo = Option<string>.Empty;
            Assert.Throws<InvalidOperationException>(() =>
            {
#pragma warning disable 219
                var v = foo.Value;
#pragma warning restore 219
            });
        }

        [Test]
        public void WhereNonEmpty()
        {
            var foo = Option.Create("foo");
            var empty = Option<string>.Empty;
            Assert.AreEqual(foo, foo.Where(x => x.StartsWith("f")));
            Assert.AreEqual(empty, foo.Where(x => x.StartsWith("a")));
        }

        [Test]
        public void WhereEmpty()
        {
            var foo = Option<string>.Empty;
            var empty = Option<string>.Empty;
            Assert.AreEqual(empty, foo.Where(x => x.StartsWith("a")));
        }

        [Test]
        public void ZipNonEmpty()
        {
            var left = Option.Create("left");
            var right = Option.Create("right");
            var expected = Option.Create(Tuple.Create("left", "right"));
            Assert.AreEqual(expected, left.Zip(right));
        }

        [Test]
        public void ZipLeftEmpty()
        {
            var left = Option<string>.Empty;
            var right = Option.Create("right");
            var expected = Option<Tuple<string, string>>.Empty;
            Assert.AreEqual(expected, left.Zip(right));
        }

        [Test]
        public void ZipRightEmpty()
        {
            var left = Option.Create("left");
            var right = Option<string>.Empty;
            var expected = Option<Tuple<string, string>>.Empty;
            Assert.AreEqual(expected, left.Zip(right));
        }

        [Test]
        public void ZipBothEmpty()
        {
            var left = Option<string>.Empty;
            var right = Option<string>.Empty;
            var expected = Option<Tuple<string, string>>.Empty;
            Assert.AreEqual(expected, left.Zip(right));
        }

        [Test]
        public void ZipWithSelectorNonEmpty()
        {
            var left = Option.Create("left");
            var right = Option.Create("right");
            var expected = Option.Create("leftright");
            Assert.AreEqual(expected, left.Zip(right, (l, r) => l + r));
        }

        [Test]
        public void ZipWithSelectorLeftEmpty()
        {
            var left = Option<string>.Empty;
            var right = Option.Create("right");
            var expected = Option<string>.Empty;
            Assert.AreEqual(expected, left.Zip(right, (l, r) => l + r));
        }

        [Test]
        public void ZipWithSelectorRightEmpty()
        {
            var left = Option.Create("left");
            var right = Option<string>.Empty;
            var expected = Option<string>.Empty;
            Assert.AreEqual(expected, left.Zip(right, (l, r) => l + r));
        }

        [Test]
        public void ZipWithSelectorBothEmpty()
        {
            var left = Option<string>.Empty;
            var right = Option<string>.Empty;
            var expected = Option<string>.Empty;
            Assert.AreEqual(expected, left.Zip(right, (l, r) => l + r));
        }
    }
}
