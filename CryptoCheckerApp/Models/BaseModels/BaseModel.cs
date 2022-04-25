namespace CryptoChecker.Models.BaseModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// The base model class to be inherited by the XAML properties.
    /// </summary>
    public class BaseModel : INotifyPropertyChanged
    {
        /// <summary>
        /// The property event changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Setting method to store and invoke the changes on XAML.
        /// </summary>
        /// <typeparam name="T">
        /// Type of the field to be updated.
        /// </typeparam>
        /// <param name="referenceField">
        /// THe field being referenced.
        /// </param>
        /// <param name="value">
        /// The new value to be set.
        /// </param>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        /// <param name="onChanged">
        /// The action to be called on property change.
        /// </param>
        /// <returns>
        /// A boolean which represents weather the property was updated.
        /// </returns>
        protected bool SetProperty<T>(ref T referenceField, T value, [CallerMemberName] string propertyName = "", Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(referenceField, value))
            {
                return false;
            }

            referenceField = value;
            onChanged?.Invoke();
            this.OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// Method attached to the event for changing a property.
        /// </summary>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = this.PropertyChanged;

            changed?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}