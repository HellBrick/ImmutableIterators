namespace HellBrick.ImmutableIterators
{
	public interface IIteratorState<T, TState>
		where TState : struct, IIteratorState<T, TState>
	{
		bool HasValue { get; }
		T CurrentValue { get; }

		TState GetNext();
	}
}
