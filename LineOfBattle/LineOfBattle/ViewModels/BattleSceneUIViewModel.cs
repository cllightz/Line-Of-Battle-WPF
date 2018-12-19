using LineOfBattle.Models;
using ShootighLibrary.MVVM;

namespace LineOfBattle.ViewModels
{
    public class BattleSceneUIViewModel : SceneUIViewModelBase
    {
        private BattleSceneUIModel _model;

        public int Score { get; set; }

        public BattleSceneUIViewModel( ModelBase bm ) : base( bm )
            => _model = (BattleSceneUIModel)bm;
    }
}
