using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.Mathematics.Interop;

namespace ShootighLibrary
{
    /// <summary>
    /// 文字列を GameControl 上に表示するためのクラス。
    /// </summary>
    public class Label : IDrawable
    {
        #region Fields
        /// <summary>
        /// Position はラベル領域の矩形の左上の座標（変更予定あり）。
        /// Size はフォントサイズ
        /// Color は文字色。
        /// </summary>
        public DrawOptions DrawOptions { get; set; }

        /// <summary>
        /// ラベルの文字列。
        /// </summary>
        public string Text;

        /// <summary>
        /// 文字列を揃える位置。
        /// デフォルトで中央揃え。
        /// </summary>
        public TextAlignment TextAlignment;
        #endregion

        #region Constructor
        /// <summary>
        /// コンストラクタ。
        /// 今後、フォントフェイスの変更等に対応予定。
        /// </summary>
        /// <param name="drawoptions">ラベル領域の矩形の左上の座標（変更予定あり）、フォントサイズ、文字色を渡す。</param>
        /// <param name="text">ラベルの文字列。</param>
        /// <param name="textalignment">文字列を揃える位置。デフォルトで中央揃え。</param>
        public Label( DrawOptions drawoptions, string text, TextAlignment textalignment = TextAlignment.Center )
        {
            DrawOptions = drawoptions;
            Text = text;
            TextAlignment = textalignment;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Canvas へのテキストの描画
        /// </summary>
        /// <param name="target">Game.MainLoop( RenderTarget ) で受け取った RenderTarget のインスタンスを渡す。</param>
        public void Draw( RenderTarget target )
        {
            using ( var format = new TextFormat( new SharpDX.DirectWrite.Factory(), "游ゴシック", DrawOptions.Size ) { TextAlignment = TextAlignment } )
            using ( var brush = new SolidColorBrush( target, DrawOptions.Color ) ) {
                var rect = new RawRectangleF(
                    DrawOptions.Position.X,
                    DrawOptions.Position.Y,
                    DrawOptions.Position.X + target.Size.Width,
                    DrawOptions.Position.Y + DrawOptions.Size
                    );

                target.DrawText( Text, format, rect, brush );
            }
        }

        /// <summary>
        /// IDrawable の抽象メソッドの実装。
        /// </summary>
        public void Move() { }
        #endregion
    }
}
