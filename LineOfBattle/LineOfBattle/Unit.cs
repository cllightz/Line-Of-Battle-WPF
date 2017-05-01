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
        private LoB Game;
        public DrawOptions DrawOptions { get; set; }
        private const int HistoryLength = 20;
        private List<Vector2> History;
        public float RoundsPerSecond;
        public float CoolDownTimer;
        public Func<Vector2, Vector2> MotionRule;
        private Faction Faction;

        #region Constructors
        /// <summary>
        /// Constructor of ally unit.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="roundspersecond"></param>
        /// <param name="size"></param>
        /// <param name="color"></param>
        public Unit( LoB game, DrawOptions drawoptions, float roundspersecond )
        {
            Game = game;
            DrawOptions = drawoptions;
            History = new List<Vector2>();
            RoundsPerSecond = roundspersecond;
            CoolDownTimer = 0;
            Faction = Faction.Ally;
        }

        /// <summary>
        /// Constructor of enemy unit.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="roundspersecond"></param>
        /// <param name="size"></param>
        /// <param name="color"></param>
        /// <param name="motionrule"></param>
        public Unit( LoB game, DrawOptions drawoptions, float roundspersecond, Func<Vector2, Vector2> motionrule )
        {
            Game = game;
            DrawOptions = drawoptions;
            History = new List<Vector2>();
            RoundsPerSecond = roundspersecond;
            CoolDownTimer = 0;
            MotionRule = motionrule;
            Faction = Faction.Enemy;
        }
        #endregion

        public bool HasFollowPos
            => History.Count >= HistoryLength;

        #region Move Methods
        public void Move()
            => DrawOptions.Position = MotionRule( DrawOptions.Position );

        public void Move( Vector2 newposition )
        {
            History.Add( DrawOptions.Position );
            DrawOptions.Position = newposition;
        }

        public void MoveV( Vector2 v, Maneuver maneuver )
        {
            switch ( maneuver ) {
                case Maneuver.Successively:
                    History.Add( DrawOptions.Position );
                    DrawOptions.Position += v;
                    break;

                case Maneuver.Simultaneously:
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
            switch ( Faction ) {
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

        public Vector2 GetFollowPos()
        {
            var res = History.First();
            History.RemoveAt( 0 );
            return res;
        }

        public void Draw( RenderTarget target )
        {
            var ellipse = new Ellipse( DrawOptions.Position.ToRawVector2(), DrawOptions.Size, DrawOptions.Size );
            var brush = new SolidColorBrush( target, DrawOptions.Color );
            target.DrawEllipse( ellipse, brush );
        }
    }
}
