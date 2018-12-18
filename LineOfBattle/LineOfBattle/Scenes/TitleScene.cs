using System.Numerics;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;
using ShootighLibrary;
using ShootighLibrary.Device;
using ShootighLibrary.Drawables;

namespace LineOfBattle.Scenes
{
    class TitleScene : SceneBase
    {
        public TitleScene() : base()
        {
        }

        public void Execute( Game game, RenderTarget target )
        {
            var white = new RawColor4( 1, 1, 1, 1 );

            new Label( new DrawOptions( new Vector2( 0, 100 ),50, white ), "Line of Battle" )
                .Draw( target );

            new Label( new DrawOptions( new Vector2( 0, 200 ), 25, white ), "Press Left Mouse Button to Start" )
                .Draw( target );

            if ( Mouse.Left ) {
                game.TransitScene<BattleScene>();
            }
        }

        public void Dispose() { }
    }
}
