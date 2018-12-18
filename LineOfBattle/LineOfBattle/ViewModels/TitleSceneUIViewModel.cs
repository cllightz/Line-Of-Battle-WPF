using LineOfBattle.Models;
using ShootighLibrary.MVVM;

namespace LineOfBattle.ViewModels
{
    public class TitleSceneUIViewModel : ViewModelBase
    {
        public string Title => "Line of Title";
        public string Prompt => "Press Left Mouse Button to Start.";

        internal TitleSceneUIViewModel( TitleSceneUIModel model ) { }
    }
}
