using LineOfBattle.Scenes;
using ShootighLibrary;

namespace LineOfBattle
{
    /// <summary>
    /// ゲームのロジック
    /// </summary>
    internal class LoB : Game
    {
        public static LoB Instance = null;

        public LoB( GameControl control ) : base( control ) { }

        /// <summary>
        /// 1回目のゲームループで呼ばれる初期化処理．
        /// RenderTargetの情報を必要とするものを記述する．
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            TransitScene<TitleScene>();
        }
    }
}
