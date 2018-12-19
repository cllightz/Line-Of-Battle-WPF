using Reactive.Bindings;
using ShootighLibrary.Extensions;
using System.Numerics;
using System.Reactive.Linq;
using System.Windows.Input;

namespace ShootighLibrary.Device
{
    /// <summary>
    /// キーボードの入力状態を保持する静的クラス。
    /// 方向入力をベクトルとして計算可能。
    /// </summary>
    public static class Key
    {
        /// <summary>スペースキー</summary>
        public static ReactiveProperty<bool> Space { get; } = new ReactiveProperty<bool>( false );

        /// <summary>Wキー</summary>
        public static ReactiveProperty<bool> W { get; } = new ReactiveProperty<bool>( false );

        /// <summary>Aキー</summary>
        public static ReactiveProperty<bool> A { get; } = new ReactiveProperty<bool>( false );

        /// <summary>Sキー</summary>
        public static ReactiveProperty<bool> S { get; } = new ReactiveProperty<bool>( false );

        /// <summary>Dキー</summary>
        public static ReactiveProperty<bool> D { get; } = new ReactiveProperty<bool>( false );
        
        /// <summary>W, A, S, D のいずれかが押されているならば真を返す。</summary>
        public static ReadOnlyReactiveProperty<bool> AnyDirection { get; } = W.CombineLatest( A, S, D, ( w, a, s, d ) => w || a || s || d ).ToReadOnlyReactiveProperty();

        /// <summary>Shiftキー</summary>
        public static bool Shift => (Keyboard.GetKeyStates( System.Windows.Input.Key.LeftShift ) | (Keyboard.GetKeyStates( System.Windows.Input.Key.RightShift )) & KeyStates.Down) == KeyStates.Down;

        /// <summary>WASD入力による方向入力の正規化された2次元ベクトル</summary>
        public static ReadOnlyReactiveProperty<Vector2> Direction { get; } = W.CombineLatest( A, S, D, ( w, a, s, d ) => new Vector2( a ? -1 : d ? 1 : 0, w ? -1 : s ? 1 : 0 ).Versor() ).ToReadOnlyReactiveProperty();
    }
}
