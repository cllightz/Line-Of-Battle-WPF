using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;
using ShootighLibrary;

namespace LineOfBattle
{
    class Unit : IDrawable
    {
        /// <summary>
        /// 陣営
        /// </summary>
        public enum Faction {
            /// <summary>
            /// 味方
            /// </summary>
            Ally,

            /// <summary>
            /// 中立
            /// </summary>
            Neutral,

            /// <summary>
            /// 敵
            /// </summary>
            Enemy
        }

        private LoB Game;
        public DrawOptions DrawOptions { get; set; }

        /// <summary>
        /// 移動の軌跡
        /// </summary>
        private List<Vector2> History;

        /// <summary>
        /// Historyに記録する座標の数
        /// </summary>
        private const int HistoryLength = 20;

        /// <summary>
        /// 1秒あたりの射撃回数
        /// </summary>
        public float RoundsPerSecond;
        
        /// <summary>
        /// 次の射撃までの時間[s]
        /// </summary>
        public float CoolDownTimer;

        /// <summary>
        /// 毎フレームの移動ルール
        /// </summary>
        public Func<Vector2, Vector2> MotionRule;

        /// <summary>
        /// ユニットの属する陣営
        /// </summary>
        public Faction UnitFaction;

        /// <summary>
        /// 敵味方共通のコンストラクタ
        /// </summary>
        /// <param name="game"></param>
        /// <param name="drawOptions"></param>
        /// <param name="roundsPerSecond"></param>
        /// <param name="motionRule"></param>
        public Unit( LoB game, DrawOptions drawOptions, float roundsPerSecond, Func<Vector2, Vector2> motionRule = null )
        {
            Game = game;
            DrawOptions = drawOptions;
            History = new List<Vector2>();
            RoundsPerSecond = roundsPerSecond;
            CoolDownTimer = 0;
            MotionRule = motionRule;
            UnitFaction = motionRule == null ? Faction.Ally : Faction.Enemy;
        }

        /// <summary>
        /// AlliesLineの後続のUnitに渡せるだけの移動の軌跡を持っているか
        /// </summary>
        public bool HasFollowPos
            => History.Count >= HistoryLength;

        #region Move Methods
        /// <summary>
        /// 移動ルールに従って移動
        /// </summary>
        public void Move()
            => DrawOptions.Position = MotionRule( DrawOptions.Position );

        /// <summary>
        /// 指定座標に移動
        /// </summary>
        /// <param name="newPosition"></param>
        public void Move( Vector2 newPosition )
        {
            History.Add( DrawOptions.Position );
            DrawOptions.Position = newPosition;
        }

        /// <summary>
        /// 指定
        /// </summary>
        /// <param name="v"></param>
        /// <param name="maneuver"></param>
        public void MoveV( Vector2 v, AlliesLine.Maneuver maneuver )
        {
            switch ( maneuver ) {
                case AlliesLine.Maneuver.Successively:
                    History.Add( DrawOptions.Position );
                    DrawOptions.Position += v;
                    break;

                case AlliesLine.Maneuver.Simultaneously:
                    DrawOptions.Position += v;

                    for ( var i = 0; i < History.Count; i++ ) {
                        History[i] += v;
                    }

                    break;
            }
        }
        #endregion 

        public void Shoot()
        {
            switch ( UnitFaction ) {
                case Faction.Ally:
                    if ( Mouse.Any && CoolDownTimer <= 0 ) {
                        var cursor = Mouse.Position;
                        var posL = DrawOptions.Position;
                        var posR = Game.Allies.Units.First().DrawOptions.Position;
                        var posLR = (posL + posR) / 2;
                        var pos = Mouse.Left ? (Mouse.Right ? posLR : posL) : (Mouse.Right ? posR : throw new InvalidOperationException());
                        var direction = (cursor - pos).Versor();
                        var velocity = 5 * direction; // TODO: 速度の係数をフィールドまたはプロパティにする。
                        var drawoptions = new DrawOptions( DrawOptions.Position, 5, new RawColor4( 0, 1, 1, 1 ) );
                        Game.AlliesShells.Add( new Shell( drawoptions, velocity ) );

                        CoolDownTimer += 1.0f / RoundsPerSecond;
                    } else {
                        CoolDownTimer -= 1.0f / 60.0f;
                        CoolDownTimer = (CoolDownTimer < 0) ? 0 : CoolDownTimer;
                    }

                    break;

                case Faction.Enemy:
                    if ( CoolDownTimer <= 0 ) {
                        Vector2 radtovector2( double rad ) { return new Vector2( (float)Math.Cos( rad ), (float)Math.Sin( rad ) ); };

                        var theta = 2 * Math.PI * Game.Rand.NextDouble();
                        var direction = radtovector2( theta ).Versor();
                        var velocity = 5 * direction; // TODO: 速度の係数をフィールドまたはプロパティにする。
                        var drawoptions = new DrawOptions( DrawOptions.Position, 5, new RawColor4( 1, 0.5f, 0, 1 ) );
                        Game.AlliesShells.Add( new Shell( drawoptions, velocity ) );

                        CoolDownTimer += 1.0f / RoundsPerSecond;                           
                    } else {
                        CoolDownTimer -= 1.0f / 60.0f;
                        CoolDownTimer = (CoolDownTimer < 0) ? 0 : CoolDownTimer;
                    }

                    break;

                case Faction.Neutral:
                    break;
            }
        }

        /// <summary>
        /// AlliesLineでの後続のUnitが移動すべき座標の取得
        /// </summary>
        /// <returns></returns>
        public Vector2 GetFollowPos()
        {
            var res = History.First();
            History.RemoveAt( 0 );
            return res;
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="target"></param>
        public void Draw( RenderTarget target )
        {
            var ellipse = new Ellipse( DrawOptions.Position.ToRawVector2(), DrawOptions.Size, DrawOptions.Size );
            var brush = new SolidColorBrush( target, DrawOptions.Color );
            target.DrawEllipse( ellipse, brush );
        }

        public void Neutralize()
            => DrawOptions.Color = new RawColor4( 1, 1, 0, 1 );

        public void Unite()
            => DrawOptions.Color = new RawColor4( 0, 1, 0, 0 );
    }
}
