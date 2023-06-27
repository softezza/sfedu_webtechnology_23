#if !UNITY_5_3_OR_NEWER
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace Smartwin.NoesisGUI
{
	//ToDo: Должен находиться в другой сборке, чтобы ею могли пользоваться и модели
	[Obsolete]
	public class ObservableList<T> : ObservableCollection<T>
	{
		private const string CountString = "Count";

		private const string IndexerName = "Item[]";


		public ObservableList() : base() { /**/  }

		public ObservableList(IEnumerable<T> collection) : base(collection) { /**/  }

		public ObservableList(List<T> list) : base(list) { /**/  }


		public void AddRange(IEnumerable<T> collection, bool sendResetAction = false)
		{
			if (collection == null)
				throw new ArgumentNullException(nameof(collection) + " is null!");

			if (!collection.Any())
				return;

			CheckReentrancy();

			var tempList = new List<T>();

			foreach (var item in collection)
			{
				tempList.Add(item);
				Items.Add(item);
			}

			if (sendResetAction)
				InvokeCollectionModificationEvent(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			else
				InvokeCollectionModificationEvent(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, tempList));
		}

		public void ReplaceRange(IEnumerable<T> collection)
		{
			if (collection == null)
				throw new ArgumentNullException(nameof(collection) + " is null!");

			if (!collection.Any())
				return;

			CheckReentrancy();

			Items.Clear();

			foreach (var item in collection)
				Items.Add(item);

			InvokeCollectionModificationEvent(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}

		public void RemoveRange(IEnumerable<T> collection, bool sendResetAction = false)
		{
			if (collection == null)
				throw new ArgumentNullException(nameof(collection) + " is null!");

			if (!collection.Any())
				return;

			CheckReentrancy();

			var tempList = new List<T>();

			foreach (var item in collection)
			{
				tempList.Add(item);
				Items.Remove(item);
			}

			if (sendResetAction)
				InvokeCollectionModificationEvent(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			else
				InvokeCollectionModificationEvent(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, tempList));
		}

		public void InsertRange(int index, IEnumerable<T> collection, bool sendResetAction = false)
		{
			if (collection == null)
				throw new ArgumentNullException(nameof(collection) + " is null!");


			if (!collection.Any())
				return;

			CheckReentrancy();

			var start = index;

			var tempList = new List<T>();

			foreach (var item in collection)
			{
				tempList.Add(item);
				Items.Insert(index++, item);
			}

			if (sendResetAction)
				InvokeCollectionModificationEvent(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			else
				InvokeCollectionModificationEvent(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, tempList, start));
		}

		private void InvokeCollectionModificationEvent(NotifyCollectionChangedEventArgs args)
		{
			OnPropertyChanged(new PropertyChangedEventArgs(CountString));
			OnPropertyChanged(new PropertyChangedEventArgs(IndexerName));
			OnCollectionChanged(args);
		}
	}
}
#endif