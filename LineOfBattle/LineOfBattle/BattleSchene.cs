using System;
using System.Collections.Generic;
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

        /// <summary>
        /// 毎フレームの処理
        /// </summary>
        /// <param name="game"></param>
        /// <param name="target"></param>
        public void Execute( Game game, RenderTarget target )
        {
            var lob = (LoB)game;

            // 100フレームに1回敵がスポーン
            if ( lob.FrameCount % 100 == 0 ) {
                var theta = 2 * Math.PI * lob.Rand.NextDouble();
                var spawnPos = new Vector2( lob.Width * (float)lob.Rand.NextDouble(), lob.Height * (float)lob.Rand.NextDouble() );
                var drawOptions = new DrawOptions( spawnPos, 5, new RawColor4( 1, 0, 0, 1 ) );
                Vector2 motionRule( Vector2 pos ) => pos + new Vector2( (float)Math.Cos( theta ), (float)Math.Sin( theta ) );
                var newEnemy = new Unit( lob, drawOptions, 1, motionRule );
                lob.Enemies.Add( newEnemy );
            }

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
        /// <summary>
        /// 敵Unitの移動
        /// </summary>
        /// <param name="lob"></param>
        private void MoveEnemies( LoB lob )
            => lob.Enemies.ForEach( unit => unit.Move() );

        /// <summary>
        /// 味方Unitの移動
        /// </summary>
        /// <param name="lob"></param>
        private void MoveAllies( LoB lob )
            => lob.Allies.Move();

        /// <summary>
        /// 味方Shellの移動
        /// </summary>
        /// <param name="lob"></param>
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

        /// <summary>
        /// 敵Shellの移動
        /// </summary>
        /// <param name="lob"></param>
        private void MoveEnemiesShells( LoB lob )
        {
            // 移動
            lob.EnemiesShells.ForEach( shell => shell.Move() );

            // 削除（Removeするため、リストの後方から実行）
            for ( var i = lob.EnemiesShells.Count - 1; 0 <= i; i++ ) {
                var x = lob.EnemiesShells[ i ].DrawOptions.Position.X;
                var y = lob.EnemiesShells[ i ].DrawOptions.Position.Y;

                if ( x < -100 || lob.Width + 100 < x || y < -100 || lob.Height + 100 < y ) {
                    lob.EnemiesShells.RemoveAt( i );
                }
            }
        }

        /// <summary>
        /// 味方Shellの発射
        /// </summary>
        /// <param name="lob"></param>
        private void ShootAlliesShells( LoB lob )
            => lob.Allies.Units.ForEach( unit => unit.Shoot() );

        /// <summary>
        /// 敵Shellの発射
        /// </summary>
        /// <param name="lob"></param>
        private void ShootEnemiesShells( LoB lob )
            => lob.Enemies.ForEach( unit => unit.Shoot() );

        /// <summary>
        /// 味方のShellと敵Unitの衝突判定
        /// </summary>
        /// <param name="lob"></param>
        private void CalculateAlliesShellsCollision( LoB lob )
        {
            // 敵Unitに衝突した味方のShell
            var collidedShells = new List<Shell>();

            // 味方のShellに衝突された敵Unit
            var collidedUnits = new List<Unit>();

            // 衝突判定
            lob.AlliesShells.ForEach( shell => lob.Enemies.ForEach( unit => {
                if ( Vector2.Distance( shell.DrawOptions.Position, unit.DrawOptions.Position ) < shell.DrawOptions.Size + unit.DrawOptions.Size ) {
                    collidedShells.Add( shell );
                    collidedUnits.Add( unit );
                }
            } ) );

            // 衝突したShellの削除
            collidedShells.ForEach( shell => lob.AlliesShells.Remove( shell ) );

            // 衝突されたUnitの中立化
            collidedUnits.ForEach( unit => {
                unit.Neutralize();
                lob.Neutrals.Add( unit );
                lob.Enemies.Remove( unit );
            } );
        }

        /// <summary>
        /// 敵のShellと味方Unitの衝突判定
        /// </summary>
        /// <param name="lob"></param>
        private void CalculateEnemiesShellsCollision( LoB lob ) { }

        /// <summary>
        /// 中立Unitの描画
        /// </summary>
        /// <param name="lob"></param>
        /// <param name="target"></param>
        private void DrawNeutrals( LoB lob, RenderTarget target )
            => lob.Neutrals.ForEach( unit => unit.Draw( target ) );

        /// <summary>
        /// 敵Unitの描画
        /// </summary>
        /// <param name="lob"></param>
        /// <param name="target"></param>
        private void DrawEnemies( LoB lob, RenderTarget target )
            => lob.Enemies.ForEach( unit => unit.Draw( target ) );

        /// <summary>
        /// 味方Unitの描画
        /// </summary>
        /// <param name="lob"></param>
        /// <param name="target"></param>
        private void DrawAllies( LoB lob, RenderTarget target )
            => lob.Allies.Draw( target );

        /// <summary>
        /// 味方Shellの描画
        /// </summary>
        /// <param name="lob"></param>
        /// <param name="target"></param>
        private void DrawAlliesShells( LoB lob, RenderTarget target )
            => lob.AlliesShells.ForEach( shell => shell.Draw( target ) );

        /// <summary>
        /// 敵Shellの描画
        /// </summary>
        /// <param name="lob"></param>
        /// <param name="target"></param>
        private void DrawEnemiesShells( LoB lob, RenderTarget target )
            => lob.EnemiesShells.ForEach( shell => shell.Draw( target ) );
        #endregion
    }
}
