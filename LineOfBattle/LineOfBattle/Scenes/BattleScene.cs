using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using LineOfBattle.MessageArgsTypes;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;
using ShootighLibrary;
using ShootighLibrary.Drawables;
using ShootighLibrary.Extensions;
using ShootighLibrary.Messenger;

namespace LineOfBattle.Scenes
{
    class BattleScene : SceneBase
    {
        //private LoB _lob;

        #region ゲームオブジェクト関連
        internal AlliesLine Allies;
        internal List<Unit> Neutrals = new List<Unit>();
        internal List<Unit> Enemies = new List<Unit>();
        internal List<Shell> AlliesShells = new List<Shell>();
        internal List<Shell> EnemiesShells = new List<Shell>();
        #endregion

        #region ゲームルール関連
        /// <summary>描画領域の幅</summary>
        internal float Width => GameInstance.Width;

        /// <summary>描画領域の高さ</summary>
        internal float Height => GameInstance.Height;

        /// <summary>描画領域の端の進入不可領域の幅</summary>
        internal float Padding => 10;

        /// <summary>可動領域の左端X座標</summary>
        internal float Left => Padding;

        /// <summary>可動領域の右端X座標</summary>
        internal float Right => Width - Padding;

        /// <summary>可動領域の上端Y座標</summary>
        internal float Top => Padding;

        /// <summary>可動領域の下端Y座標</summary>
        internal float Bottom => Height - Padding;

        private int _firingInterval;
        #endregion

        protected override void Initialize()
        {
            var drawoptions = new DrawOptions( new Vector2( Width / 2, Height / 2 ), 6, new RawColor4( 0, 1, 0, 1 ) );

            Allies = new AlliesLine( this ) {
                new Unit( this, drawoptions.Clone, 10 ),
                new Unit( this, drawoptions.Clone, 10 ),
                new Unit( this, drawoptions.Clone, 10 ),
                new Unit( this, drawoptions.Clone, 10 ),
                new Unit( this, drawoptions.Clone, 10 ),
            };

            Mediator.Singleton.RegisterPublisher<GainScoreMessageArgs>( typeof( BattleScene ) );

            _firingInterval = 100;
        }

        protected override void Execute( RenderTarget target )
        {
            var lob = (LoB)GameInstance;

            SpawnEnemy();
            MoveEnemies();
            MoveAllies();
            MoveAlliesShells();
            MoveEnemiesShells();
            ShootAlliesShells();
            ShootEnemiesShells();
            CalculateNeutralsCollision();
            CalculateAlliesShellsCollision();
            CalculateEnemiesShellsCollision();

            if ( !Allies.Units.Any() ) {
                GameInstance.TransitScene<ResultScene>();
            }

            DrawNeutrals( target );
            DrawEnemies( target );
            DrawAllies( target );
            DrawAlliesShells( target );
            DrawEnemiesShells( target );
        }

        #region 移動・判定・描画
        private void SpawnEnemy()
        {
            if ( FrameCount % _firingInterval == 0 ) {
                var r = (float)Math.Sqrt( Width * Width + Height * Height ) / 2 + 50;
                var theta = 2 * (float)Math.PI * Rand.NextFloat();
                var randomPosition = new Vector2( Width / 2 + r * (float)Math.Cos( theta ), Height / 2 + r * (float)Math.Sin( theta ) );
                var drawOptions = new DrawOptions( randomPosition, 5, new RawColor4( 1, 0, 0, 1 ) );
                var phi = theta + (float)Math.PI * Rand.NextFloat() - 3 * (float)Math.PI / 2;
                Vector2 motionRule( Vector2 pos ) => pos + new Vector2( (float)Math.Cos( phi ), (float)Math.Sin( phi ) );
                Enemies.Add( new Unit( this, drawOptions, 1, motionRule ) );

                _firingInterval -= _firingInterval <= 10 ? 0 : 1;
            }
        }

        private void MoveEnemies()
        {
            foreach ( var u in Enemies ) {
                u.Move();
            }
        }

        private void MoveAllies( )
            => Allies.Move();

        private void MoveAlliesShells()
        {
            foreach ( var s in AlliesShells ) {
                s.Move();
            }

            if ( AlliesShells.Any() ) {
                for ( var i = AlliesShells.Count - 1; 0 <= i; i-- ) {
                    var x = AlliesShells[ i ].DrawOptions.Position.X;
                    var y = AlliesShells[ i ].DrawOptions.Position.Y;

                    if ( x < -100 || Width + 100 < x || y < -100 || Height + 100 < y ) {
                        AlliesShells.RemoveAt( i );
                    }
                }
            }
        }

        private void MoveEnemiesShells()
        {
            foreach ( var s in EnemiesShells ) {
                s.Move();
            }

            var removeList = new List<Shell>();

            foreach ( var s in EnemiesShells ) {
                var x = s.DrawOptions.Position.X;
                var y = s.DrawOptions.Position.Y;

                if ( x < -100 || Width + 100 < x || y < -100 || Height + 100 < y ) {
                    removeList.Add( s );
                }
            }

            foreach ( var s in removeList ) {
                EnemiesShells.Remove( s );
            }
        }

        private void ShootAlliesShells()
        {
            foreach ( var u in Allies.Units ) {
                u.Shoot();
            }
        }

        private void ShootEnemiesShells()
        {
            foreach ( var u in Enemies ) {
                u.Shoot();
            }
        }

        private void CalculateNeutralsCollision()
        {
            var collidedUnits = new List<Unit>();

            foreach ( var n in Neutrals ) {
                foreach ( var a in Allies ) {
                    if ( Vector2.Distance( a.DrawOptions.Position, n.DrawOptions.Position ) < a.DrawOptions.Size + n.DrawOptions.Size ) {
                        collidedUnits.Add( n );
                        break;
                    }
                }
            }

            foreach ( var u in collidedUnits ) {
                u.Unite();
                Allies.Add( u );
                Neutrals.Remove( u );
                Mediator.Singleton.Publish( typeof( BattleScene ), new GainScoreMessageArgs( 45 ) );
            }
        }

        private void CalculateAlliesShellsCollision()
        {
            var collidedShells = new List<Shell>();
            var collidedUnits = new List<Unit>();

            foreach ( var s in AlliesShells ) {
                foreach ( var u in Enemies ) {
                    if ( Vector2.Distance( s.DrawOptions.Position, u.DrawOptions.Position ) < s.DrawOptions.Size + u.DrawOptions.Size ) {
                        collidedShells.Add( s );
                        collidedUnits.Add( u );
                    }
                }
            }

            foreach ( var s in collidedShells ) {
                AlliesShells.Remove( s );
            }

            foreach ( var u in collidedUnits ) {
                u.Neutralize();
                Neutrals.Add( u );
                Enemies.Remove( u );
                Mediator.Singleton.Publish( typeof( BattleScene ), new GainScoreMessageArgs( 123 ) );
            }
        }

        private void CalculateEnemiesShellsCollision()
        {
            var collidedShells = new List<Shell>();
            var collidedNeutrals = new List<Unit>();

            foreach ( var n in Neutrals ) {
                foreach ( var s in EnemiesShells ) {
                    if ( Vector2.Distance( s.DrawOptions.Position, n.DrawOptions.Position ) < s.DrawOptions.Size + n.DrawOptions.Size ) {
                        collidedShells.Add( s );
                        collidedNeutrals.Add( n );
                        break;
                    }
                }
            }

            foreach ( var s in collidedShells ) {
                EnemiesShells.Remove( s );
            }

            foreach ( var n in collidedNeutrals ) {
                Neutrals.Remove( n );
            }

            /////

            var collidedCount = 0;

            foreach ( var u in Allies ) {
                foreach ( var s in EnemiesShells ) {
                    if ( Vector2.Distance( s.DrawOptions.Position, u.DrawOptions.Position ) < s.DrawOptions.Size + u.DrawOptions.Size ) {
                        collidedShells.Add( s );
                        collidedCount++;
                        break;
                    }
                }
            }

            foreach ( var s in collidedShells ) {
                EnemiesShells.Remove( s );
            }

            for ( var i = 0; i < collidedCount; i++ ) {
                var last = Allies.Units.Last();
                last.Neutralize();
                Neutrals.Add( last );
                Allies.Units.Remove( last );
            }
        }

        private void DrawEnemies( RenderTarget target )
        {
            foreach ( var u in Enemies ) {
                u.Draw( target );
            }
        }

        private void DrawNeutrals( RenderTarget target )
        {
            foreach ( var u in Neutrals ) {
                u.Draw( target );
            }
        }

        private void DrawAllies( RenderTarget target )
            => Allies.Draw( target );

        private void DrawAlliesShells( RenderTarget target )
        {
            foreach ( var s in AlliesShells ) {
                s.Draw( target );
            }
        }

        private void DrawEnemiesShells( RenderTarget target )
        {
            foreach ( var s in EnemiesShells ) {
                s.Draw( target );
            }
        }
        #endregion
    }
}
