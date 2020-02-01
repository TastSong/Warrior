
using System.Collections.Generic;

class Factory
{
}
/*	private Queue<T> Unused = new Queue<T>();
	private List<T> inAction = new List<T>();

	public T Create()
	{
		T t;
		if (Unused.Count > 0)
			t = Unused.Dequeue();
		else
			t = new T();

		inAction.Add(t);

		return t;
	}

	public void Return(T t)
	{
		inAction.Remove(t);
		Unused.Enqueue(t);
	}
}*/