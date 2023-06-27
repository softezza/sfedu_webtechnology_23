#if UNITY_5_3_OR_NEWER
#define NOESIS
using Noesis;
using UnityEngine;
#else
#endif

using System;
using System.Windows;

namespace SmartTwin.NoesisGUI.Storage
{
	/// <summary>
	/// Дизайнерский словарь ресурсов контекста WPF.
	/// Реализован, чтобы в контексте WPF intellisense мог видеть ключи к ресурсам, но при это не использовался в контексте Noesis. 
	/// Это необходимо, т.к. в рабочем контексте словари добавляются динамически.
	/// </summary>
	public class DesignResourcesDictionary : ResourceDictionary
	{
		/// <summary>
		/// Ссылка на словарь ресурсов
		/// </summary>
		public new Uri Source
		{
			get => base.Source;
			set
			{
#if !NOESIS
				base.Source = value;
#endif
			}
		}
	}
}
