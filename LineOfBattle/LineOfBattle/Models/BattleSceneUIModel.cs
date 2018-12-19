using LineOfBattle.MessageArgsTypes;
using ShootighLibrary.Messenger;
using ShootighLibrary.MVVM;
namespace LineOfBattle.Models
{
    internal class BattleSceneUIModel : ModelBase
    {
        private int _highScore;
        internal int HighScore { get => _highScore; private set => SetProperty( ref _highScore, value ); }

        private int _score;
        internal int Score { get => _score; private set => SetProperty( ref _score, value ); }

        internal BattleSceneUIModel()
        {
            HighScore = Properties.Settings.Default.HighScore;
            Score = 0;

            Mediator.Singleton.Subscribe<GainScoreMessageArgs>( typeof( BattleSceneUIModel ), args => Score += args.Value );
        }

        public override void Dispose()
        {
            base.Dispose();

            if ( Score > HighScore ) {
                Properties.Settings.Default.HighScore = Score;
                Properties.Settings.Default.Save();
            }
        }
    }
}
