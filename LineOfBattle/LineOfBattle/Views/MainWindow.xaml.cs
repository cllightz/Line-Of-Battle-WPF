using LineOfBattle.ViewModels;
using ShootighLibrary;
using ShootighLibrary.Messenger;
using System.Windows;

namespace LineOfBattle.Views
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 初期化処理
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            
            Mediator.Singleton.RegisterPublisher<GameControl>( typeof( MainWindow ) )
                              .Publish( typeof( MainWindow ), Graphics );
            
            Graphics.SetEventHandlers( this );
            DataContext = new UIViewModel();
        }
    }
}
