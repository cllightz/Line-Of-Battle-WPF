using System;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Runtime.CompilerServices;

namespace ShootighLibrary.MVVM
{
    public abstract class ModelBase : INotifyPropertyChanged, IDisposable
    {
        #region INotifyPropertyChanged
        public virtual event PropertyChangedEventHandler PropertyChanged;

        protected virtual bool SetProperty<T>( ref T field, T value, [CallerMemberName]string propertyName = null )
        {
            if ( Equals( field, value ) ) { return false; }
            field = value;
            PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propertyName ) );
            return true;
        }
        #endregion

        #region IDisposable
        protected CompositeDisposable Disposables { get; } = new CompositeDisposable();

        public virtual void Dispose() => Disposables.Dispose();
        #endregion
    }
}
