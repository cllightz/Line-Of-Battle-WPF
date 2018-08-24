using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using SharpDX.Direct2D1;
using ShootighLibrary;

namespace LineOfBattle
{
    class AlliesLine : IEnumerable<Unit>
    {
        /// <summary>
        /// AlliesLineの各Unitの移動方法
        /// </summary>
        public enum Maneuver {
            /// <summary>
            /// 逐次回頭
            /// </summary>
            Successively,

            /// <summary>
            /// 一斉回頭
            /// </summary>
            Simultaneously
        }

        private LoB Game;
        public List<Unit> Units { get; private set; }
        private Queue<Unit> UnitAdditionQueue;
        public const float Speed = 2.0f;

        public AlliesLine( LoB game )
        {
            Game = game;
            Units = new List<Unit>();
            UnitAdditionQueue = new Queue<Unit>();
        }

        /// <summary>
        /// 中立Unitの味方Unitへの追加待ちキューへのエンキュー
        /// </summary>
        /// <param name="u"></param>
        public void Add( Unit u )
            => UnitAdditionQueue.Enqueue( u );

        /// <summary>
        /// 移動
        /// </summary>
        public void Move()
        {
            if ( Units.Any() && Key.AnyDirection && CanMove ) {
                if ( Key.Shift ) {
                    foreach ( var u in Units ) {
                        u.MoveV( Speed * Key.Direction, Maneuver.Simultaneously );
                    }
                } else {
                    Units[ 0 ].MoveV( GetCorrectedDirection( Units[ 0 ] ), Maneuver.Successively );

                    for ( var i = 1; i < Units.Count; i++ ) {
                        if ( Units[ i - 1 ].HasFollowPos ) {
                            Units[ i ].Move( Units[ i - 1 ].GetFollowPos() );
                        }
                    }
                }
            }

            if ( (!Units.Any() || Units.Last().HasFollowPos) && UnitAdditionQueue.Any() ) {
                Units.Add( UnitAdditionQueue.Peek() );
                UnitAdditionQueue.Dequeue();
            }
        }

        /// <summary>
        /// 移動しようとする方向に移動可能かどうか
        /// </summary>
        public bool CanMove
        {
            get {
                (var x, var y) = (Units[ 0 ].DrawOptions.Position + Speed * Key.Direction).Tuple();

                if ( Game.Left <= x && x <= Game.Right && Game.Top <= y && y <= Game.Bottom ) {
                    return true;
                }

                if ( !(Game.Left <= x && x <= Game.Right || Game.Top <= y && y <= Game.Bottom) ) {
                    return false;
                }

                if ( Key.A && (Key.W || Key.S) && x < Game.Left ) {
                    return true;
                }

                if ( Key.D && (Key.W || Key.S) && Game.Right < x ) {
                    return true;
                }

                if ( Key.W && (Key.A || Key.D) && y < Game.Top ) {
                    return true;
                }

                if ( Key.S && (Key.A || Key.D) && Game.Left < y ) {
                    return true;
                }

                return false;
            }
        }
        
        /// <summary>
        /// 移動可能エリアを考慮した移動方向の計算
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        public Vector2 GetCorrectedDirection( Unit u )
        {
            float to1( float f ) => f < 0 ? -1 : f > 0 ? 1 : 0;

            var newposition = u.DrawOptions.Position + Speed * Key.Direction;
            var x = newposition.X;
            var y = newposition.Y;

            if ( x < Game.Left || Game.Right < x ) {
                return new Vector2( 0, Speed * to1( Key.Direction.Y ) );
            }

            if ( y < Game.Top || Game.Bottom < y ) {
                return new Vector2( Speed * to1( Key.Direction.X ), 0 );
            }

            return Speed * Key.Direction;
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="target"></param>
        public void Draw( RenderTarget target )
            => Units.ForEach( unit => unit.Draw( target ) );

        #region IEnumerableの実装
        public IEnumerator<Unit> GetEnumerator() => Units.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => Units.GetEnumerator();
        #endregion
    }
}
