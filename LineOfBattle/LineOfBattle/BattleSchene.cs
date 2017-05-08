﻿using System;
using System.Linq;
using System.Numerics;
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

            if ( lob.FrameCount % 100 == 0 ) {
                var theta = 2 * Math.PI * lob.Rand.NextDouble();

                lob.Enemies.Add(
                    new Unit(
                        lob,
                        new DrawOptions(
                            new Vector2( lob.Width / 2, lob.Height / 2 ),
                            5,
                            new RawColor4( 1, 0, 0, 1 )
                            ),
                        1,
                        pos => pos + new Vector2( (float)Math.Cos( theta ), (float)Math.Sin( theta ) )
                        )
                    );
            }

            MoveEnemies( lob );
            MoveAllies( lob );
            MoveAlliesShells( lob );
            MoveEnemiesShells( lob );
            ShootAlliesShells( lob );
            ShootEnemiesShells( lob );
            CalculateAlliesShellsCollision( lob );
            CalculateEnemiesShellsCollision( lob );

            DrawEnemies( lob, target );
            DrawAllies( lob, target );
            DrawAlliesShells( lob, target );
            DrawEnemiesShells( lob, target );

            lob.FrameCount++;
        }

        #region 移動・判定・描画
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

            for ( var i = lob.EnemiesShells.Count - 1; 0 <= i; i++ ) {
                var x = lob.EnemiesShells[ i ].DrawOptions.Position.X;
                var y = lob.EnemiesShells[ i ].DrawOptions.Position.Y;

                if ( x < -100 || lob.Width + 100 < x || y < -100 || lob.Height + 100 < y ) {
                    lob.EnemiesShells.RemoveAt( i );
                }
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

        private void CalculateAlliesShellsCollision( LoB lob ) { }

        private void CalculateEnemiesShellsCollision( LoB lob ) { }

        private void DrawEnemies( LoB lob, RenderTarget target )
        {
            foreach ( var u in lob.Enemies ) {
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
