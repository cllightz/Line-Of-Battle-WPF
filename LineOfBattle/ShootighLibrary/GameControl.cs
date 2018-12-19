using System;
using System.Windows;
using SWI = System.Windows.Input;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;
using ShootighLibrary.Device;

namespace ShootighLibrary
{
    /// <summary>
    /// ゲームのコントロール
    /// </summary>
    public class GameControl : D2dControl.D2dControl
    {
        #region Fields
        /// <summary>
        /// GameControl のインスタンスが保持するゲームのロジックのインスタンス。
        /// </summary>
        private Game GameInstance;

        /// <summary>
        /// GameInstance.Initialize() を呼び出したかどうかのフラグ。
        /// </summary>
        private bool IsGameInitialized;
        #endregion

        #region Constructor
        /// <summary>
        /// コントロールのコンストラクタ。
        /// </summary>
        public GameControl()
            => IsGameInitialized = false;
        #endregion

        #region Methods
        /// <summary>
        /// MainWindow から抽象クラス Game を実装したクラスのインスタンスを渡す。
        /// </summary>
        /// <param name="game">抽象クラス Game を実装したクラスのインスタンスを渡す。</param>
        public void SetGameInstance( Game game )
            => GameInstance = game;

        /// <summary>
        /// 毎フレーム呼び出されるメソッド。
        /// </summary>
        /// <param name="target"></param>
        public override void Render( RenderTarget target )
        {
            if ( GameInstance == null ) {
                return;
            }

            if ( !IsGameInitialized ) {
                GameInstance.Initialize();
                IsGameInitialized = true;
            }

            target.Clear( new RawColor4( 0, 0, 0, 1 ) );
            GameInstance.MainLoop( target );
            GC.Collect();
        }

        /// <summary>
        /// 引数で渡された MainWindow のインスタンスの各種イベントハンドラを設定する。
        /// </summary>
        /// <param name="mainwindow">MainWindow のインスタンスを渡す。</param>
        public void SetEventHandlers( Window mainwindow )
        {
            mainwindow.KeyDown += KeyDownEventHandler;
            mainwindow.KeyUp += KeyUpEventHandler;

            mainwindow.MouseDown += MouseDownEventHandler;
            mainwindow.MouseUp += MouseUpEventHandler;
            mainwindow.MouseMove += MouseMoveEventHandler;
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// キーを押した時のイベントハンドラ。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KeyDownEventHandler( object sender, SWI.KeyEventArgs e )
        {
            switch ( e.Key ) {
                case SWI.Key.W: Key.W.Value = true; break;
                case SWI.Key.A: Key.A.Value = true; break;
                case SWI.Key.S: Key.S.Value = true; break;
                case SWI.Key.D: Key.D.Value = true; break;
                case SWI.Key.Space: Key.Space.Value = true; break;
            }
        }

        /// <summary>
        /// キーを離した時のイベントハンドラ。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KeyUpEventHandler( object sender, SWI.KeyEventArgs e )
        {
            switch ( e.Key ) {
                case SWI.Key.W: Key.W.Value = false; break;
                case SWI.Key.A: Key.A.Value = false; break;
                case SWI.Key.S: Key.S.Value = false; break;
                case SWI.Key.D: Key.D.Value = false; break;
                case SWI.Key.Space: Key.Space.Value = false; break;
            }
        }

        /// <summary>
        /// マウスボタンを押した時のイベントハンドラ。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseDownEventHandler( object sender, SWI.MouseButtonEventArgs e )
        {
            switch ( e.ChangedButton ) {
                case SWI.MouseButton.Left: Mouse.Left.Value = true; break;
                case SWI.MouseButton.Right: Mouse.Right.Value = true; break;
            }
        }

        /// <summary>
        /// マウスボタンを離した時のイベントハンドラ。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseUpEventHandler( object sender, SWI.MouseButtonEventArgs e )
        {
            switch ( e.ChangedButton ) {
                case SWI.MouseButton.Left: Mouse.Left.Value = false; break;
                case SWI.MouseButton.Right: Mouse.Right.Value = false; break;
            }
        }

        /// <summary>
        /// マウスカーソルを動かした時のイベントハンドラ。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseMoveEventHandler( object sender, SWI.MouseEventArgs e )
        {
            var pos = e.GetPosition( this );
            Mouse.X.Value = (float)pos.X;
            Mouse.Y.Value = (float)pos.Y;
        }
        #endregion
    }
}
