using Reactive.Bindings;
using System.Numerics;
using System.Reactive.Linq;

namespace ShootighLibrary.Device
{
    /// <summary>
    /// マウスボタンとカーソルの位置を保持する静的クラス。
    /// </summary>
    public static class Mouse
    {
        /// <summary>左ボタンが押されているか</summary>
        public static ReactiveProperty<bool> Left { get; } = new ReactiveProperty<bool>( false );

        /// <summary>右ボタンが押されているか</summary>
        public static ReactiveProperty<bool> Right { get; } = new ReactiveProperty<bool>( false );

        /// <summary>GameControl 上のカーソルのX座標</summary>
        public static ReactiveProperty<float> X { get; } = new ReactiveProperty<float>( 0 );

        /// <summary>GameControl 上のカーソルのY座標。</summary>
        public static ReactiveProperty<float> Y { get; } = new ReactiveProperty<float>( 0 );

        /// <summary>左ボタンまたは右ボタンが押されているか</summary>
        public static ReadOnlyReactiveProperty<bool> Any { get; } = Left.CombineLatest( Right, ( l, r ) => l || r ).ToReadOnlyReactiveProperty();

        /// <summary>GameControl 上のカーソルの座標の2次元ベクトル</summary>
        public static ReadOnlyReactiveProperty<Vector2> Position { get; } = X.CombineLatest( Y, ( x, y ) => new Vector2( x, y ) ).ToReadOnlyReactiveProperty();
    }
}
