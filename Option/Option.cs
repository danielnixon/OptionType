using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace org.danielnixon.Option
{
	public class OptionEnumerator<T> : IEnumerator<T>
	{
		private readonly Option<T> option;
		private readonly int length;
		private int index;

		public OptionEnumerator(Option<T> option)
		{
			this.option = option;
			length = option.HasValue ? 1 : 0;
			index = -1;
		}

		public bool MoveNext()
		{
			index += 1;
			return index < length;
		}

		public void Reset()
		{
			index = -1;
		}

		T IEnumerator<T>.Current
		{
			get
			{
				return option.Value;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return option.Value;
			}
		}

		public void Dispose()
		{
		}
	}

	[Serializable]
	[SuppressMessage("Microsoft.Naming", "CA1716")]
	[DebuggerDisplay("HasValue: {m_hasValue}, Value: {m_value}")]
	public struct Option<T> : IEquatable<Option<T>>, IEnumerable, IEnumerable<T>
	{
		private readonly T m_value;
		private readonly bool m_hasValue;

		public static Option<T> Empty
		{
			get { return new Option<T>(); }
		}

		public bool HasValue
		{
			get { return m_hasValue; }
		}

		[DebuggerDisplay("m_value")]
		public T Value
		{
			get
			{
				if (!HasValue)
				{
					throw new InvalidOperationException("Option does not have a value");
				}

				return m_value;
			}
		}

		public T ValueOrDefault
		{
			get { return m_hasValue ? m_value : default(T); }
		}

		public Option(T value)
		{
			m_hasValue = true;
			m_value = value;
		}

		#region Operators

		[SuppressMessage("Microsoft.Usage", "CA1801")]
		[SuppressMessage("Microsoft.Usage", "CA2225")]
		public static implicit operator Option<T>(Option option)
		{
			return Option<T>.Empty;
		}

		[SuppressMessage("Microsoft.Usage", "CA1801")]
		public static implicit operator Option<T>(T value)
		{
			return new Option<T>(value);
		}

		public static bool operator ==(Option<T> left, Option<T> right)
		{
			return left.Equals(right);
		}
		public static bool operator !=(Option<T> left, Option<T> right)
		{
			return !left.Equals(right);
		}

		#endregion

		#region IEquatable<T> Members

		public bool Equals(Option<T> other)
		{
			if (other.HasValue != this.HasValue)
			{
				return false;
			}

			// Both don't have a value
			if (!other.HasValue)
			{
				return true;
			}

			return EqualityComparer<T>.Default.Equals(m_value, other.Value);
		}

		#endregion

		#region Overrides

		public override bool Equals(object obj)
		{
			if (obj is Option<T>)
			{
				return Equals((Option<T>)obj);
			}

			return false;
		}

		public override int GetHashCode()
		{
			if (!HasValue)
			{
				return 0;
			}

			return EqualityComparer<T>.Default.GetHashCode(m_value);
		}

		#endregion

		public T ValueOrElse(T defaultValue)
		{
			return HasValue ? Value : defaultValue;
		}

		public Option<TResult> Select<TResult>(Func<T, TResult> selector)
		{
			return HasValue ? selector(Value).ToOption() : Option.Empty;
		}

		public Option<TResult> SelectMany<TResult>(Func<T, Option<TResult>> selector)
		{
			return HasValue ? selector(Value) : Option.Empty;
		}

		public Option<T> Where(Func<T, bool> predicate)
		{
			return HasValue && predicate(Value) ? this : Option.Empty;
		}

		public void ForEach(Action<T> action)
		{
			if (HasValue)
			{
				action(Value);
			}
		}

		public Option<TResult> Zip<TSecond, TResult>(Option<TSecond> second, Func<T, TSecond, TResult> resultSelector)
		{
			return HasValue && second.HasValue ? resultSelector (Value, second.Value).ToOption() : Option.Empty;
		}

		public Option<Tuple<T, TSecond>> Zip<TSecond>(Option<TSecond> second)
		{
			return Zip(second, (item1, item2) => Tuple.Create(item1, item2));
		}

		public Option<T> DefaultIfEmpty(T defaultValue)
		{
			return HasValue ? this : defaultValue.ToOption();
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return new OptionEnumerator<T>(this);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<T>)this).GetEnumerator();
		}
	}

	[Serializable]
	[SuppressMessage("Microsoft.Naming", "CA1716")]
	public sealed class Option
	{
		private static Option s_empty = new Option();

		private Option()
		{
		}

		public static Option<T> Create<T>(T value)
		{
			return new Option<T>(value);
		}

		public static Option Empty
		{
			get { return s_empty; }
		}
	}

	public static class OptionExtensionMethods
	{
		public static Option<T> ToOption<T>(this T value)
		{
			return value != null ? Option.Create(value) : Option.Empty;
		}

		public static Option<T> FirstOption<T>(this IEnumerable<T> source)
		{
			return source.FirstOrDefault().ToOption();
		}

		public static Option<T> FirstOption<T>(this IEnumerable<T> source, Func<T, bool> predicate)
		{
			return source.FirstOrDefault(predicate).ToOption();
		}
	}
}
