using System.Numerics;
using SharpDX.Mathematics.Interop;

namespace ShootighLibrary
{
    /// <summary>
    /// IDrawable を実装するクラスに渡す描画設定をまとめたクラス。
    /// 構造体ではないため、同じインスタンスを複数回渡すと不具合の原因となる。
    /// 同じ設定を使い回したいときは、1つのインスタンスの .Clone を利用してコピーのインスタンスを渡す。
    /// </summary>
    public class DrawOptions
    {
        #region Fields
        /// <summary>
        /// 描画されるオブジェクトの座標。
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// 描画されるオブジェクトの大きさ。
        /// </summary>
        public float Size;

        /// <summary>
        /// 描画されるオブジェクトの描画時の Brush の色。
        /// </summary>
        public RawColor4 Color;
        #endregion

        #region Constructor
        /// <summary>
        /// </summary>
        /// <param name="position">描画されるオブジェクトの位置。</param>
        /// <param name="size">描画されるオブジェクトの大きさ。</param>
        /// <param name="color">描画されるオブジェクトの描画時の Brush の色。</param>
        public DrawOptions( Vector2 position, float size, RawColor4 color )
        {
            Position = position;
            Size = size;
            Color = color;
        }
        #endregion

        #region Property
        /// <summary>
        /// インスタンスの内容をコピーしたインスタンスを返す。
        /// コピーコンストラクタの代替。
        /// </summary>
        public DrawOptions Clone
            => new DrawOptions( Position, Size, Color );
        #endregion
    }
}
