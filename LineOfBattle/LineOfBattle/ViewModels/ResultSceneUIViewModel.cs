using ShootighLibrary.MVVM;

namespace LineOfBattle.ViewModels
{
    public class ResultSceneUIViewModel : SceneUIViewModelBase
    {
        public string GameOver => "GAME OVER";
        public string Prompt => "Press Left Mouse Button to Return to Title!";

        internal ResultSceneUIViewModel( ModelBase m ) : base( m )
        {
        }
    }
}
