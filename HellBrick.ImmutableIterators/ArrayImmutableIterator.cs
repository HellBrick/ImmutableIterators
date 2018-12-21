using System;
using System.Collections.Generic;

namespace HellBrick.ImmutableIterators
{
	public static class ArrayImmutableIterator
	{
		public static ImmutableIterator<T, State<T>> AsImmutableIterator<T>( this T[] array )
			=> new ImmutableIterator<T, State<T>>( new State<T>( array, 0 ) );

		public readonly struct State<T> : IIteratorState<T, State<T>>, IEquatable<State<T>>
		{
			private readonly T[] _array;
			private readonly int _index;

			public State(T[] array, int index)
			{
				_array = array;
				_index = index;
			}

			public bool HasValue => _index < _array.Length;

			public T CurrentValue => HasValue ? _array[ _index ] : default;

			public State<T> GetNext() => HasValue ? new State<T>( _array, _index + 1 ) : this;

			public override int GetHashCode() => (_array, _index).GetHashCode();

			public bool Equals( State<T> other ) => EqualityComparer<T[]>.Default.Equals( _array, other._array ) && _index == other._index;
			public override bool Equals( object obj ) => obj is State<T> other && Equals( other );

			public static bool operator ==( State<T>x, State<T>y ) => x.Equals( y );
			public static bool operator !=( State<T>x, State<T>y ) => !x.Equals( y );
		}
	}
}
