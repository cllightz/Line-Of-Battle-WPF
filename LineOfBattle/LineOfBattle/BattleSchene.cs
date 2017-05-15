using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;
using ShootighLibrary;

namespace LineOfBattle
{
    class BattleSchene : ISchene<ScheneState>
    {
        public bool Match( ScheneState schene )
            => schene == ScheneState.Battle;

        public void Execute( Game game, RenderTarget target )
        {
            var lob = (LoB)game;

            SpawnEnemy( lob );
            MoveEnemies( lob );
            MoveAllies( lob );
            MoveAlliesShells( lob );
            MoveEnemiesShells( lob );
            ShootAlliesShells( lob );
            ShootEnemiesShells( lob );
            CalculateAlliesShellsCollision( lob );
            CalculateEnemiesShellsCollision( lob );

            DrawNeutrals( lob, target );
            DrawEnemies( lob, target );
            DrawAllies( lob, target );
            DrawAlliesShells( lob, target );
            DrawEnemiesShells( lob, target );

            lob.FrameCount++;
        }

        #region 移動・判定・描画
        private void SpawnEnemy( LoB lob )
        {
            if ( lob.FrameCount % 100 == 0 ) {
                var randomPosition = new Vector2( lob.Width * (float)lob.Rand.NextDouble(), lob.Height * (float)lob.Rand.NextDouble() );
                var drawOptions = new DrawOptions( randomPosition, 5, new RawColor4( 1, 0, 0, 1 ) );
                var theta = 2 * Math.PI * lob.Rand.NextDouble();
                Func< Vector2, Vector2 > motionRule = pos => pos + new Vector2( (float)Math.Cos( theta ), (float)Math.Sin( theta ) );
                lob.Enemies.Add( new Unit( lob, drawOptions, 1, motionRule ) );
            }
        }

        private void MoveEnemies( LoB lob )
        {
            foreach ( var u in lob.Enemies ) {
                u.Move();
            }
        }

        private void MoveAllies( LoB lob )
            => lob.Allies.Move();

        private void MoveAlliesShells( LoB lob )
        {
            foreach ( var s in lob.AlliesShells ) {
                s.Move();
            }

            if ( lob.AlliesShells.Any() ) {
                for ( var i = lob.AlliesShells.Count - 1; 0 <= i; i-- ) {
                    var x = lob.AlliesShells[ i ].DrawOptions.Position.X;
                    var y = lob.AlliesShells[ i ].DrawOptions.Position.Y;

                    if ( x < -100 || lob.Width + 100 < x || y < -100 || lob.Height + 100 < y ) {
                        lob.AlliesShells.RemoveAt( i );
                    }
                }
            }
        }

        private void MoveEnemiesShells( LoB lob )
        {
            foreach ( var s in lob.EnemiesShells ) {
                s.Move();
            }

            var removeList = new List<Shell>();

            foreach ( var s in lob.EnemiesShells ) {
                var x = s.DrawOptions.Position.X;
                var y = s.DrawOptions.Position.Y;

                if ( x < -100 || lob.Width + 100 < x || y < -100 || lob.Height + 100 < y ) {
                    removeList.Add( s );
                }
            }

            foreach ( var s in removeList ) {
                lob.EnemiesShells.Remove( s );
            }
        }

        private void ShootAlliesShells( LoB lob )
        {
            foreach ( var u in lob.Allies.Units ) {
                u.Shoot();
            }
        }

        private void ShootEnemiesShells( LoB lob )
        {
            foreach ( var u in lob.Enemies ) {
                u.Shoot();
            }
        }

        private void CalculateAlliesShellsCollision( LoB lob )
        {
            var collidedShells = new List<Shell>();
            var collidedUnits = new List<Unit>();

            foreach ( var s in lob.AlliesShells ) {
                foreach ( var u in lob.Enemies ) {
                    if ( Vector2.Distance( s.DrawOptions.Position, u.DrawOptions.Position ) < s.DrawOptions.Size + u.DrawOptions.Size ) {
                        collidedShells.Add( s );
                        collidedUnits.Add( u );
                    }
                }
            }

            foreach ( var s in collidedShells ) {
                lob.AlliesShells.Remove( s );
            }

            foreach ( var u in collidedUnits ) {
                u.Neutralize();
                lob.Neutrals.Add( u );
                lob.Enemies.Remove( u );
            }
        }

        private void CalculateEnemiesShellsCollision( LoB lob ) { }

        private void DrawEnemies( LoB lob, RenderTarget target )
        {
            foreach ( var u in lob.Enemies ) {
                u.Draw( target );
            }
        }

        private void DrawNeutrals( LoB lob, RenderTarget target )
        {
            foreach ( var u in lob.Neutrals ) {
                u.Draw( target );
            }
        }

        private void DrawAllies( LoB lob, RenderTarget target )
            => lob.Allies.Draw( target );

        private void DrawAlliesShells( LoB lob, RenderTarget target )
        {
            foreach ( var s in lob.AlliesShells ) {
                s.Draw( target );
            }
        }

        private void DrawEnemiesShells( LoB lob, RenderTarget target )
        {
            foreach ( var s in lob.EnemiesShells ) {
                s.Draw( target );
            }
        }
        #endregion
    }
}
