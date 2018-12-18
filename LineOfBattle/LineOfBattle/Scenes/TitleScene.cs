using SharpDX.Direct2D1;
using ShootighLibrary;
using ShootighLibrary.Device;

namespace LineOfBattle.Scenes
{
    internal class TitleScene : SceneBase
    {
        public TitleScene() : base()
        {
        }

        protected override void Execute( RenderTarget target )
        {
            //var white = new RawColor4( 1, 1, 1, 1 );

            //new Label( new DrawOptions( new Vector2( 0, 100 ), 50, white ), "Line of Battle" )
            //    .Draw( target );

            //new Label( new DrawOptions( new Vector2( 0, 200 ), 25, white ), "Press Left Mouse Button to Start" )
            //    .Draw( target );

            if ( Mouse.Left ) {
                GameInstance.TransitScene<BattleScene>();
            }
        }
    }
}
