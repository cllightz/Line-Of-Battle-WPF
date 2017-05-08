using System.Numerics;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;
using ShootighLibrary;

namespace LineOfBattle
{
    class TitleSchene : ISchene<ScheneState>
    {
        public bool Match( ScheneState schene )
            => schene == ScheneState.Title;

        public void Execute( Game game, RenderTarget target )
        {
            var white = new RawColor4( 1, 1, 1, 1 );

            new Label( new DrawOptions( new Vector2( 0, 100 ),50, white ), "Line of Battle" )
                .Draw( target );

            new Label( new DrawOptions( new Vector2( 0, 200 ), 25, white ), "Press Left Mouse Button to Start" )
                .Draw( target );

            if ( Mouse.Left ) {
                ((LoB)game).Schene = ScheneState.Battle;
            }
        }
    }
}
