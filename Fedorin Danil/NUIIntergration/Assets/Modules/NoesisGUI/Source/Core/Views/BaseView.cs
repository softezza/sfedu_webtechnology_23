#if UNITY_5_3_OR_NEWER
#define NOESIS
using Noesis;
#else
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
#endif

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace SmartTwin.NoesisGUI.Views
{
	/// <summary>
	/// Базовое представление.
	/// Спроектирован по MVVM подходу, разделяя UI на различные представления бизнес-логики
	/// </summary>
	public abstract class BaseView : UserControl
	{
		/// <summary>
		/// Общий словарь, где Type - тип представления, а string - путь к xaml-документу этого представления
		/// </summary>
		private static Dictionary<Type, string>  XamlPaths = new Dictionary<Type, string>();

        public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register(
         nameof(IsActive), typeof(object), typeof(BaseView), new PropertyMetadata(true));

		/// <summary>
		/// Свойство не должно находится здесь. Отвечает за то, активно ли представление в регионе или нет.
		/// Но BaseView не знает об "активностях". Свойство должно быть заменено механизмами в регионах
		/// </summary>
		[Obsolete]		
        public bool IsActive
        {
            get => (bool)GetValue(IsActiveProperty);
            set => SetValue(IsActiveProperty, value);
        }
	

		/// <summary>
		/// Вызывается перед добавлением в регион
		/// ToDo: возможно в этих методах нет необходимости
		/// </summary>
        public virtual void OnRegionAdding() { }

		/// <summary>
		/// Вызывается перед удалением из региона
		/// </summary>
		public virtual void OnRegionRemoving() { }

		/// <summary>
		/// Вызывается после добавления в регион
		/// </summary>
		public virtual void OnRegionAdded() { }

		/// <summary>
		/// Вызывается после удаления из региона
		/// </summary>
		public virtual void OnRegionRemoved() { }


		/// <summary>
		/// Получить путь к xaml-документу представления
		/// </summary>
		/// <typeparam name="T">Тип представления, к которому нужно получить путь</typeparam>
		/// <returns></returns>
		public static string GetXamlPath<T>() 
			where T : BaseView
		{
			var type = typeof(T);

			return GetXamlPath(type);
		}

		/// <summary>
		/// Получить путь к xaml-документу представления
		/// </summary>
		/// <param name="type">Тип представления, к которому нужно получить путь</param>
		/// <returns></returns>
		public static string GetXamlPath(Type type)
		{
			if (XamlPaths.TryGetValue(type, out var path))
				return path;

			RuntimeHelpers.RunClassConstructor(type.TypeHandle);

			if (XamlPaths.TryGetValue(type, out path))
				return path;

			return null; //ToDo нужно ли здесь исключение?
		}


		/// <summary>
		/// Зарегестрировать путь к xaml-документу представления
		/// ToDo: логика взята из Noesis, обдумать, как находить xaml проще.
		/// </summary>
		/// <typeparam name="T">Тип представления, для которого нужно зарегестрировать путь</typeparam>
		/// <param name="filename">Путь к файлу (вызывается автоматически)</param>
		protected static void RegisterXamlPath<T>([CallerFilePath] string filename = "")
			where T : BaseView
		{
			var path = filename.Replace('\\', '/');

			int pos = path.IndexOf("/Resources/");

			if (pos != -1)
				path = path.Substring(pos + 11); 

			if (path.EndsWith(".xaml.cs"))
				path = path.Substring(0, path.Length - 3 - 5);

			XamlPaths.Add(typeof(T), path);
		}
	}
}
