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
            if ( Mouse.Left ) {
                GameInstance.TransitScene<BattleScene>();
            }
        }
    }
}
