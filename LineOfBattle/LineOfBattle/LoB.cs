using System;
using System.Collections.Generic;
using System.Numerics;
using LineOfBattle.Scenes;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;
using ShootighLibrary;
using ShootighLibrary.Messenger;

namespace LineOfBattle
{
    /// <summary>
    /// ゲームのロジック
    /// </summary>
    class LoB : Game
    {
        #region Fields
        public static LoB Instance = null;

        private static Mediator _subscription = Mediator.Singleton.Subscribe<GameControl>( typeof( LoB ), control => {
            Instance = new LoB( control );
            control.SetGameInstance( Instance );
        } );

        private RenderTarget Target;
        #endregion

        #region Constructor
        public LoB( GameControl control ) : base( control )
        {
            Control = control;
            CurrentScene = new TitleScene();
        }
        #endregion

        /// <summary>
        /// 1回目のゲームループで呼ばれる初期化処理．
        /// RenderTargetの情報を必要とするものを記述する．
        /// </summary>
        public override void Initialize()
        {
        }
    }
}
