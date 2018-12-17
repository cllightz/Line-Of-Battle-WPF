using System.Numerics;
using SharpDX.Mathematics.Interop;

namespace ShootighLibrary
{
    /// <summary>
    /// 各種のベクトルを表すクラス間の変換を行う拡張メソッド類。
    /// </summary>
    public static class VectorExtensions
    {
        /// <summary>
        /// Vector2 から RawVector2 への変換。
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static RawVector2 ToRawVector2( this Vector2 v )
            => new RawVector2() { X = v.X, Y = v.Y };

        /// <summary>
        /// RawVector2 から Vector2 への変換。
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector2 ToVector2( this RawVector2 v )
            => new Vector2( v.X, v.Y );

        /// <summary>
        /// 正規化したベクトルを返すメソッド。
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector2 Versor( this Vector2 v )
            => v / v.Length();

        /// <summary>
        /// Vector2 から (float, float) のタプルへの変換。
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static (float, float) Tuple( this Vector2 v )
            => (v.X, v.Y);
    }
}
