using System;
using System.Collections.Generic;

namespace HellBrick.ImmutableIterators
{
	public static class WhereImmutableIterator
	{
		[NoCapture]
		public static ImmutableIterator<T, State<T, TState, Func<T, bool>>> Where<T, TState>
		(
			this ImmutableIterator<T, TState> iterator,
			Func<T, bool> predicate
		)
			where TState : struct, IIteratorState<T, TState>
			=> iterator
			.Where( predicate, ( capturedPredicate, item ) => capturedPredicate( item ) );

		[NoCapture]
		public static ImmutableIterator<T, State<T, TState, TClosure>> Where<T, TState, TClosure>
		(
			this ImmutableIterator<T, TState> iterator,
			TClosure closure,
			Func<TClosure, T, bool> predicate
		)
			where TState : struct, IIteratorState<T, TState>
			=> new ImmutableIterator<T, State<T, TState, TClosure>>
			(
				new State<T, TState, TClosure>( iterator.State, closure, predicate )
			);

		public readonly struct State<T, TState, TClosure>
			: IIteratorState<T, State<T, TState, TClosure>>, IEquatable<State<T, TState, TClosure>>
			where TState : struct, IIteratorState<T, TState>
		{
			private readonly TState _state;
			private readonly TClosure _closure;
			private readonly Func<TClosure, T, bool> _predicate;

			public State( TState state, TClosure closure, Func<TClosure, T, bool> predicate )
			{
				_state = new ImmutableIterator<T, TState>( state ).SkipUntil( closure, predicate ).State;
				_closure = closure;
				_predicate = predicate;
			}

			public bool HasValue => _state.HasValue;

			public T CurrentValue => _state.CurrentValue;

			public State<T, TState, TClosure> GetNext() => new State<T, TState, TClosure>( _state.GetNext(), _closure, _predicate );

			public override int GetHashCode() => (_state, _closure, _predicate).GetHashCode();

			public bool Equals( State<T, TState, TClosure> other )
				=> EqualityComparer<TState>.Default.Equals( _state, other._state )
				&& EqualityComparer<TClosure>.Default.Equals( _closure, other._closure )
				&& EqualityComparer<Func<TClosure, T, bool>>.Default.Equals( _predicate, other._predicate );

			public override bool Equals( object obj ) => obj is State<T, TState, TClosure> other && Equals( other );

			public static bool operator ==( State<T, TState, TClosure> x, State<T, TState, TClosure> y ) => x.Equals( y );
			public static bool operator !=( State<T, TState, TClosure> x, State<T, TState, TClosure> y ) => !x.Equals( y );
		}
	}
}
