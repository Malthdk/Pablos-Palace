using System;
using System.Collections.Generic;

namespace Pablo.Pooling {
	public class ListPool<T> : IPool<T> where T : class {

		Func<T> produce;
		int capacity;
		List<T> objects;
		Func<T, bool> useTest;
		bool expandable;

		public ListPool(Func<T> factoryMethod, int maxSize, Func<T,bool> inUse, bool expandable = true) {
			capacity = maxSize;
			produce = factoryMethod;
			objects = new List<T>(maxSize);
			useTest = inUse;
			this.expandable = expandable;
		}

		public T GetInstance() {
			var count = objects.Count;

			foreach (var item in objects) {
				if (!useTest(item))
					return item;
			}
			if (count >= capacity && !expandable) {
				return null;
			}
			var obj = produce();
			objects.Add(obj);
			return obj;
		}

	}
}
