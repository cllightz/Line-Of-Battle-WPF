using System;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Runtime.CompilerServices;

namespace ShootighLibrary.MVVM
{
    public abstract class ViewModelBase : INotifyPropertyChanged, IDisposable
    {
        public virtual event PropertyChangedEventHandler PropertyChanged;

        protected virtual bool SetProperty<T>( ref T field, T value, [CallerMemberName]string propertyName = null )
        {
            if ( Equals( field, value ) ) { return false; }
            field = value;
            PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propertyName ) );
            return true;
        }

        protected CompositeDisposable Disposables { get; } = new CompositeDisposable();

        public void Dispose() => Disposables.Dispose();
    }
}
