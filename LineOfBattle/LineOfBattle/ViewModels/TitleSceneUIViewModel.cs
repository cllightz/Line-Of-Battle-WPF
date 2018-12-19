using ShootighLibrary.MVVM;

namespace LineOfBattle.ViewModels
{
    public class TitleSceneUIViewModel : SceneUIViewModelBase
    {
        public string Title => "Line of Battle";
        public string Prompt => "Press Left Mouse Button to Start!";

        internal TitleSceneUIViewModel( ModelBase m ) : base( m ) { }
    }
}
