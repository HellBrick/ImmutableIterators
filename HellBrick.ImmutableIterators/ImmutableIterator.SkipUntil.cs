using System;

namespace HellBrick.ImmutableIterators
{
	public static partial class ImmutableIterator
	{
		[NoCapture]
		public static ImmutableIterator<T, TState> SkipUntil<T, TState>
		(
			this ImmutableIterator<T, TState> iterator,
			Func<T, bool> stopPredicate
		)
			where TState : struct, IIteratorState<T, TState>
			=> iterator.SkipUntil( stopPredicate, ( capturedStopPredicate, item ) => capturedStopPredicate( item ) );

		[NoCapture]
		public static ImmutableIterator<T, TState> SkipUntil<T, TState, TClosure>
		(
			this ImmutableIterator<T, TState> iterator,
			TClosure closure,
			Func<TClosure, T, bool> stopPredicate
		)
			where TState : struct, IIteratorState<T, TState>
		{
			TState state = iterator.State;
			while ( state.HasValue && !stopPredicate( closure, state.CurrentValue ) )
			{
				state = state.GetNext();
			}

			return new ImmutableIterator<T, TState>( state );
		}
	}
}
