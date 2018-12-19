using SharpDX.Direct2D1;
using System;

namespace ShootighLibrary
{
    public abstract class SceneBase : IDisposable
    {
        protected Game GameInstance;
        public int FrameCount = 0;
        public Random Rand = new Random();

        public SceneBase() { }

        public void Initialize( Game game )
        {
            GameInstance = game;
            Initialize();
        }

        protected abstract void Initialize();

        // リソースの参照も渡す
        public void Execute( Game game, RenderTarget target )
        {
            GameInstance = game;
            FrameCount++;
            Execute( target );
        }

        protected abstract void Execute( RenderTarget target );

        public void Dispose() { }
    }
}