using System;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace HellBrick.ImmutableIterators.Test
{
	public class WhereIteratorTests
	{
		[Fact]
		public void Empty() => VerifyWhere( Array.Empty<int>(), x => true );

		[Fact]
		public void NoMatches() => VerifyWhere( new int[] { 42, 64, 128 }, x => x > 1000 );

		[Fact]
		public void ConsecutiveMatches() => VerifyWhere( new int[] { 127, 42, 64, 128 }, x => x < 100 );

		[Fact]
		public void InconsecutiveMatches() => VerifyWhere( new int[] { 127, 42, 128, 64, 57, 32 }, x => x < 100 );

		[Fact]
		public void MatchesOnly() => VerifyWhere( Enumerable.Range( 0, 10 ).ToArray(), x => true );

		private void VerifyWhere( int[] source, Func<int, bool> predicate )
		{
			int[] expected = source.Where( predicate ).ToArray();
			int[] actual = source.AsImmutableIterator().Where( predicate ).ToArray();
			actual.Should().BeEquivalentTo( expected );
		}
	}
}
