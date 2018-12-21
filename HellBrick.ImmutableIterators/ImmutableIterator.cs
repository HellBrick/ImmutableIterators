using System;
using System.Collections.Generic;

namespace HellBrick.ImmutableIterators
{
	public readonly struct ImmutableIterator<T, TState>
		: IEquatable<ImmutableIterator<T, TState>>
		where TState : struct, IIteratorState<T, TState>
	{
		public ImmutableIterator( TState state ) => State = state;

		public TState State { get; }

		public override string ToString() => State.ToString();

		public override int GetHashCode() => State.GetHashCode();
		public bool Equals( ImmutableIterator<T, TState> other ) => EqualityComparer<TState>.Default.Equals( State, other.State );
		public override bool Equals( object obj ) => obj is ImmutableIterator<T, TState> other && Equals( other );

		public static bool operator ==( ImmutableIterator<T, TState> x, ImmutableIterator<T, TState> y ) => x.Equals( y );
		public static bool operator !=( ImmutableIterator<T, TState> x, ImmutableIterator<T, TState> y ) => !x.Equals( y );
	}
}
