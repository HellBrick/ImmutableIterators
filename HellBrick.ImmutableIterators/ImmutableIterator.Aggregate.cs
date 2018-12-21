using System;

namespace HellBrick.ImmutableIterators
{
	public static partial class ImmutableIterator
	{
		[NoCapture]
		public static TAccumulator Aggregate<T, TState, TAccumulator>
		(
			this ImmutableIterator<T, TState> iterator,
			TAccumulator accumulator,
			Func<TAccumulator, T, TAccumulator> reducer
		)
			where TState : struct, IIteratorState<T, TState>
			=> iterator.Aggregate( accumulator, reducer, acc => acc );

		[NoCapture]
		public static TOut Aggregate<T, TState, TAccumulator, TOut>
		(
			this ImmutableIterator<T, TState> iterator,
			TAccumulator accumulator,
			Func<TAccumulator, T, TAccumulator> reducer,
			Func<TAccumulator, TOut> outputConverter
		)
			where TState : struct, IIteratorState<T, TState>
		{
			for ( TState state = iterator.State; state.HasValue; state = state.GetNext() )
			{
				accumulator = reducer( accumulator, state.CurrentValue );
			}

			return outputConverter( accumulator );
		}
	}
}
