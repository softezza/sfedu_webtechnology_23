#if UNITY_5_3_OR_NEWER
#define NOESIS
using System;
using Zenject;
#else
using SmartTwin.NoesisGUI.Controls;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
#endif

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace SmartTwin.NoesisGUI.Views
{
    /// <summary>
    /// Базовая модель представления, реализующий интерфейс <see cref="INotifyPropertyChanged"/> для MVVM-подхода
    /// </summary>
    public class BaseViewModel : INotifyPropertyChanged, IInitializable, IDisposable
	{
        public event PropertyChangedEventHandler PropertyChanged = delegate { };


        /// <summary>
        /// Проинициализировать модель представления.
        /// ToDo: грамотно внести их на основе Zenject (некоторые VM создаются через Instantiate)
        /// </summary>
		public virtual void Initialize() { }

		/// <summary>
		/// Очистить модель представления.
		/// ToDo: грамотно внести их на основе Zenject (некоторые VM создаются через Instantiate)
		/// </summary>
		public virtual void Dispose() { }


        /// <summary>
        /// Установить значение для свойства
        /// </summary>
        /// <typeparam name="T">Тип свойства</typeparam>
        /// <param name="backingStore">Кешируемое поле, которое должно быть изменено</param>
        /// <param name="value">Новое значение</param>
        /// <param name="propertyName">Имя свойства</param>
        /// <param name="onChanged">Обратный вызов при изменении свойства</param>
        /// <param name="validateValue">Обратный вызов для проверки нового значения. Указываются старое и новое значение соответственно</param>
        /// <returns>True, если значение свойства удалось изменить</returns>
		protected bool SetProperty<T>(
            ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null,
            Func<T, T, bool> validateValue = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            if (validateValue != null && !validateValue(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// Вызывает событие <see cref="PropertyChanged"/> для уведомления об изменении значения и свойства
        /// </summary>
        /// <param name="propertyName">Имя свойства</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
         PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
	}


#if !NOESIS
    //ToDo: заглушка
    public interface IInitializable
    {
        void Initialize();
	}

#endif
}
