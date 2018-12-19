using LineOfBattle.Models;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using ShootighLibrary.MVVM;

namespace LineOfBattle.ViewModels
{
    public class BattleSceneUIViewModel : SceneUIViewModelBase
    {
        private BattleSceneUIModel _model;

        public ReadOnlyReactiveProperty<int> HighScore { get; }
        public ReadOnlyReactiveProperty<int> Score { get; }

        public BattleSceneUIViewModel( ModelBase bm ) : base( bm )
        {
            _model = (BattleSceneUIModel)bm;
            HighScore = _model.ObserveProperty( m => m.HighScore ).ToReadOnlyReactiveProperty().AddTo( Disposables );
            Score = _model.ObserveProperty( m => m.Score ).ToReadOnlyReactiveProperty().AddTo( Disposables );
        }
    }
}
