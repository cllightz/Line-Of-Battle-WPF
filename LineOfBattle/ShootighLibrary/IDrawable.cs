using SharpDX.Direct2D1;

namespace ShootighLibrary
{
    /// <summary>
    /// 描画可能オブジェクトのインターフェイス。
    /// </summary>
    public interface IDrawable
    {
        #region Property
        /// <summary>
        /// 描画可能オブジェクトの描画オプション。
        /// 座標と大きさと Brush の色が保持される。
        /// </summary>
        DrawOptions DrawOptions { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Game.MainLoop( RenderTarget ) から呼ばれる描画可能オブジェクトの移動処理。
        /// </summary>
        void Move();

        /// <summary>
        /// Game.MainLoop( RenderTarget ) から呼ばれる描画可能オブジェクトの描画処理。
        /// </summary>
        /// <param name="target"></param>
        void Draw( RenderTarget target );
        #endregion
    }
}
