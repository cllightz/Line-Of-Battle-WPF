using System.Numerics;

namespace ShootighLibrary
{
    /// <summary>
    /// マウスボタンとカーソルの位置を保持する静的クラス。
    /// </summary>
    public static class Mouse
    {
        #region Fields
        /// <summary>
        /// 左ボタン。
        /// </summary>
        public static bool Left;

        /// <summary>
        /// 右ボタン。
        /// </summary>
        public static bool Right;

        /// <summary>
        /// GameControl 上のカーソルのX座標。
        /// </summary>
        public static float X;

        /// <summary>
        /// GameControl 上のカーソルのY座標。
        /// </summary>
        public static float Y;
        #endregion

        #region Properties
        /// <summary>
        /// 左ボタンまたは右ボタンが押されているならば真を返す。
        /// </summary>
        public static bool Any
            => Left || Right;

        /// <summary>
        /// GameControl 上のカーソルの2次元座標を2次元ベクトルで返す。
        /// </summary>
        public static Vector2 Position
            => new Vector2( X, Y );
        #endregion
    }
}
