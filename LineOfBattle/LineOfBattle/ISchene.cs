using SharpDX.Direct2D1;
using ShootighLibrary;

namespace LineOfBattle
{
    interface ISchene<T>
    {
        bool Match( T t );
        void Execute( Game game, RenderTarget target );
    }
}
