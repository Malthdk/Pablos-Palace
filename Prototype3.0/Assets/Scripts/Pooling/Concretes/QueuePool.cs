using System;

// https://www.youtube.com/watch?v=_mxLLRQ1BQY

namespace Pablo.Pooling {
	public class QueuePool<T> : IPool<T> {

		Func<T> produce;
		int capacity;
		T[] objects;
		int index;

		public QueuePool(Func<T> factoryMethod, int maxSize) {
			capacity = maxSize;
			produce = factoryMethod;
			index = -1;
			objects = new T[maxSize];
		}

		public T GetInstance() {
			index = (index + 1) % capacity;

			if (objects[index] == null) {
				objects[index] = produce();
			}

			return objects[index];
		}

	}
}
