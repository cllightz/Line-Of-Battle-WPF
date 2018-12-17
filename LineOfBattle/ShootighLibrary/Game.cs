using SharpDX.Direct2D1;

namespace ShootighLibrary
{
    /// <summary>
    /// ゲームのロジックを記述するクラスの抽象クラス。
    /// 実際のコードではこのクラスを実装する。
    /// </summary>
    public abstract class Game
    {
        #region Fields;
        /// <summary>
        /// ゲームのロジックを保持しているコントロール。
        /// 抽象クラス Game を実装するクラスのインスタンスの親インスタンスにあたる。
        /// </summary>
        public GameControl Control;
        #endregion

        #region Constuctor
        /// <summary>
        /// ゲームのロジックを保持している GameControl のインスタンスを渡す。
        /// </summary>
        /// <param name="control"></param>
        public Game( GameControl control )
            => Control = control;
        #endregion

        #region Properties
        /// <summary>
        /// 描画領域の横幅。
        /// </summary>
        public float Width
            => (float)Control.ActualWidth;

        /// <summary>
        /// 描画領域の縦幅。
        /// </summary>
        public float Height
            => (float)Control.ActualHeight;

        /// <summary>
        /// 描画領域の端の進入不可領域の幅。
        /// </summary>
        public float Padding
            => 10;

        /// <summary>
        /// 可動領域の左端X座標。
        /// </summary>
        public float Left
            => Padding;

        /// <summary>
        /// 可動領域の右端X座標。
        /// </summary>
        public float Right
            => Width - Padding;

        /// <summary>
        /// 可動領域の上端Y座標。
        /// </summary>
        public float Top
            => Padding;

        /// <summary>
        /// 可動領域の下端Y座標。
        /// </summary>
        public float Bottom
            => Height - Padding;
        #endregion

        #region Abstract Methods
        /// <summary>
        /// 初期化処理の抽象メソッド。
        /// </summary>
        public abstract void Initialize();

        /// <summary>
        /// 毎フレームの処理の抽象メソッド。
        /// </summary>
        /// <param name="target">GameControl.Render( RenderTarget ) で受け取った RenderTarget のインスタンスを渡す。</param>
        public abstract void MainLoop( RenderTarget target );
        #endregion
    }
}
