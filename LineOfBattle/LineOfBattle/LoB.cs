using System;
using System.Collections.Generic;
using System.Numerics;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;
using ShootighLibrary;

namespace LineOfBattle
{
    /// <summary>
    /// ゲームのロジック
    /// </summary>
    class LoB : Game
    {
        #region Fields
        private RenderTarget Target;
        public Random Rand = new Random();
        public ulong FrameCount = 0;

        public ScheneState Schene = ScheneState.Title;

        public List<ISchene<ScheneState>> ScheneList = new List<ISchene<ScheneState>>() {
            new TitleSchene(),
            new BattleSchene(),
        };

        public AlliesLine Allies;
        public List<Unit> Neutrals = new List<Unit>();
        public List<Unit> Enemies = new List<Unit>();
        public List<Shell> AlliesShells = new List<Shell>();
        public List<Shell> EnemiesShells = new List<Shell>();
        #endregion

        #region Constructor
        public LoB( GameControl control ) : base( control )
            => Control = control;
        #endregion

        /// <summary>
        /// 1回目のゲームループで呼ばれる初期化処理．
        /// RenderTargetの情報を必要とするものを記述する．
        /// </summary>
        public override void Initialize()
        {
            var drawoptions = new DrawOptions( new Vector2( Width / 2, Height / 2 ), 6, new RawColor4( 0, 1, 0, 1 ) );

            Allies = new AlliesLine( this ) {
                new Unit( this, drawoptions.Clone, 10 ),
                new Unit( this, drawoptions.Clone, 10 ),
                new Unit( this, drawoptions.Clone, 10 ),
                new Unit( this, drawoptions.Clone, 10 ),
                new Unit( this, drawoptions.Clone, 10 ),
            };
        }

        /// <summary>
        /// ゲームループ
        /// </summary>
        public override void MainLoop( RenderTarget target )
        {
            Target = target;

            foreach ( var schene in ScheneList ) {
                if ( schene.Match( Schene ) ) {
                    schene.Execute( this, target );
                }
            }
        }
    }
}
