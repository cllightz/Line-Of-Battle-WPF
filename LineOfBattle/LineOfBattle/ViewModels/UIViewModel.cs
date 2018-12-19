using LineOfBattle.Models;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using ShootighLibrary.Device;
using ShootighLibrary.MVVM;
using System;

namespace LineOfBattle.ViewModels
{
    public class UIViewModel : ViewModelBase
    {
        private UIModel _model;
        public ReactiveProperty<ViewModelBase> SceneUIViewModel { get; } = new ReactiveProperty<ViewModelBase>();

        public UIViewModel()
        {
            _model = UIModel.Singleton;

            // SceneUIViewModel = _model.ObserveProperty( m => m.SceneUIModel ).ToReadOnlyReactiveProperty();
            _model.ObserveProperty( m => m.SceneUIModel )
                  .Subscribe( m => {
                      if ( m == null ) { return; }

                      switch ( m ) {
                          case TitleSceneUIModel uim:
                              SceneUIViewModel.Value = new TitleSceneUIViewModel( uim );
                              break;

                          case BattleSceneUIModel uim:
                              SceneUIViewModel.Value = new BattleSceneUIViewModel( uim );
                              break;

                          default:
                              throw new InvalidOperationException( $"No related Scene UI View Model found at the constructor of UIViewModel." );
                      }
                  } )
                  .AddTo( Disposables );

            Key.Space.Subscribe( isPressed => {
                if ( isPressed ) {
                    _model.DebugSaveMessengerInfo();
                }
            } );
        }
    }
}
