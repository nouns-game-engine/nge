namespace Nouns.Core.StateMachine
{
	/// <summary> Wrapper around a list for a read-only list that doesn't allocate. </summary>
	/// <typeparam name="T"></typeparam>
	public readonly struct ReadOnlyList<T>
	{
		private readonly List<T> list;

		public ReadOnlyList(List<T> list)
		{
			this.list = list;
		}

	    public int Count => list.Count;

        public T this[int index] => list[index];

        // List already has a perfectly serviceable non-allocating enumerator:
		public List<T>.Enumerator GetEnumerator()
		{
			return list.GetEnumerator();
		}
	}
}