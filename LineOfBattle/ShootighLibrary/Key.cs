using System.Numerics;
using System.Windows.Input;

namespace ShootighLibrary
{
    /// <summary>
    /// キーボードの入力状態を保持する静的クラス。
    /// 方向入力をベクトルとして計算可能。
    /// </summary>
    public static class Key
    {
        #region Fields
        /// <summary>
        /// Wキー
        /// </summary>
        public static bool W;

        /// <summary>
        /// Aキー
        /// </summary>
        public static bool A;

        /// <summary>
        /// Sキー
        /// </summary>
        public static bool S;

        /// <summary>
        /// Dキー
        /// </summary>
        public static bool D;
        #endregion

        #region Properties
        /// <summary>
        /// W, A, S, D のいずれかが押されているならば真を返す。
        /// </summary>
        public static bool AnyDirection
            => W || A || S || D;

        /// <summary>
        /// Shiftキー
        /// </summary>
        public static bool Shift
            => (Keyboard.GetKeyStates( System.Windows.Input.Key.LeftShift ) & KeyStates.Down) == KeyStates.Down
            || (Keyboard.GetKeyStates( System.Windows.Input.Key.RightShift ) & KeyStates.Down) == KeyStates.Down;

        /// <summary>
        /// WASDキーによる入力方向の2次元ベクトルを正規化したものを返す。
        /// </summary>
        public static Vector2 Direction
            => new Vector2( A ? -1 : D ? 1 : 0, W ? -1 : S ? 1 : 0 ).Versor();
        #endregion
    }
}
