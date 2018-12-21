using System;
using System.Collections.Generic;

namespace HellBrick.ImmutableIterators
{
	public static class SelectImmutableIterator
	{
		[NoCapture]
		public static ImmutableIterator<TOut, State<T, TOut, TState, Func<T, TOut>>> Select<T, TState, TOut>
		(
			this ImmutableIterator<T, TState> iterator,
			Func<T, TOut> selector
		)
			where TState : struct, IIteratorState<T, TState>
			=> iterator.Select( selector, ( capturedSelector, item ) => capturedSelector( item ) );

		[NoCapture]
		public static ImmutableIterator<TOut, State<T, TOut, TState, TClosure>> Select<T, TState, TClosure, TOut>
		(
			this ImmutableIterator<T, TState> iterator,
			TClosure closure,
			Func<TClosure, T, TOut> selector
		)
			where TState : struct, IIteratorState<T, TState>
			=> new ImmutableIterator<TOut, State<T, TOut, TState, TClosure>>
			(
				new State<T, TOut, TState, TClosure>( iterator.State, closure, selector )
			);

		public readonly struct State<T, TOut, TInnerState, TClosure>
			: IIteratorState<TOut, State<T, TOut, TInnerState, TClosure>>
			, IEquatable<State<T, TOut, TInnerState, TClosure>>
			where TInnerState : struct, IIteratorState<T, TInnerState>
		{
			private readonly TInnerState _innerState;
			private readonly TClosure _closure;
			private readonly Func<TClosure, T, TOut> _selector;

			public State( TInnerState innerState, TClosure closure, [NoCapture] Func<TClosure, T, TOut> selector )
			{
				_innerState = innerState;
				_closure = closure;
				_selector = selector;
			}

			public bool HasValue => _innerState.HasValue;

			public TOut CurrentValue => _selector( _closure, _innerState.CurrentValue );

			public State<T, TOut, TInnerState, TClosure> GetNext()
				=> new State<T, TOut, TInnerState, TClosure>
				(
					_innerState.GetNext(),
					_closure,
					_selector
				);

			public override int GetHashCode() => (_innerState, _closure, _selector).GetHashCode();

			public bool Equals( State<T, TOut, TInnerState, TClosure> other )
				=> EqualityComparer<TInnerState>.Default.Equals( _innerState, other._innerState )
				&& EqualityComparer<TClosure>.Default.Equals( _closure, other._closure )
				&& EqualityComparer<Func<TClosure, T, TOut>>.Default.Equals( _selector, other._selector );

			public override bool Equals( object obj ) => obj is State<T, TOut, TInnerState, TClosure> other && Equals( other );

			public static bool operator ==( State<T, TOut, TInnerState, TClosure> x, State<T, TOut, TInnerState, TClosure> y ) => x.Equals( y );
			public static bool operator !=( State<T, TOut, TInnerState, TClosure> x, State<T, TOut, TInnerState, TClosure> y ) => !x.Equals( y );
		}
	}
}
