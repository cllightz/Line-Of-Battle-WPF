using System.Windows;
using System.Windows.Controls;

namespace LineOfBattle
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
            var MainGrid = new Grid();
            AddChild( MainGrid );
            var MainControl = new ShootighLibrary.GameControl();
            MainGrid.Children.Add( MainControl );
            MainControl.SetGameInstance( new LoB( MainControl ) );
            MainControl.SetEventHandlers( this );
        }
    }
}
