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

        // リソースの参照も渡す
        public virtual void Execute( Game game, RenderTarget target )
        {
            GameInstance = game;
            FrameCount++;
        }

        public void Dispose() { }
    }
}