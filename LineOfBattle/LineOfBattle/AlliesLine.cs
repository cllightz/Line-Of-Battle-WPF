using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using LineOfBattle.Scenes;
using SharpDX.Direct2D1;
using ShootighLibrary;

namespace LineOfBattle
{
    class AlliesLine : IEnumerable<Unit>
    {
        private BattleScene _scene;
        public List<Unit> Units { get; private set; }
        private Queue<Unit> UnitAdditionQueue;
        public const float Speed = 2.0f;

        public AlliesLine( BattleScene scene )
        {
            _scene = scene;
            Units = new List<Unit>();
            UnitAdditionQueue = new Queue<Unit>();
        }

        public void Add( Unit u )
            => UnitAdditionQueue.Enqueue( u );

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

        public bool CanMove
        {
            get {
                (var x, var y) = (Units[ 0 ].DrawOptions.Position + Speed * Key.Direction).Tuple();

                if ( _scene.Left <= x && x <= _scene.Right && _scene.Top <= y && y <= _scene.Bottom ) {
                    return true;
                }

                if ( !(_scene.Left <= x && x <= _scene.Right || _scene.Top <= y && y <= _scene.Bottom) ) {
                    return false;
                }

                if ( Key.A && (Key.W || Key.S) && x < _scene.Left ) {
                    return true;
                }

                if ( Key.D && (Key.W || Key.S) && _scene.Right < x ) {
                    return true;
                }

                if ( Key.W && (Key.A || Key.D) && y < _scene.Top ) {
                    return true;
                }

                if ( Key.S && (Key.A || Key.D) && _scene.Left < y ) {
                    return true;
                }

                return false;
            }
        }

        public Vector2 GetCorrectedDirection( Unit u )
        {
            float to1( float f ) => f < 0 ? -1 : f > 0 ? 1 : 0;

            var newposition = u.DrawOptions.Position + Speed * Key.Direction;
            var x = newposition.X;
            var y = newposition.Y;

            if ( x < _scene.Left || _scene.Right < x ) {
                return new Vector2( 0, Speed * to1( Key.Direction.Y ) );
            }

            if ( y < _scene.Top || _scene.Bottom < y ) {
                return new Vector2( Speed * to1( Key.Direction.X ), 0 );
            }

            return Speed * Key.Direction;
        }

        public void Draw( RenderTarget target )
        {
            foreach ( var u in Units ) {
                u.Draw( target );
            }
        }

        #region IEnumerableの実装
        public IEnumerator<Unit> GetEnumerator() => Units.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => Units.GetEnumerator();
        #endregion
    }
}
