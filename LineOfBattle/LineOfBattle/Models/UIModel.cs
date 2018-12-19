using ShootighLibrary.Messenger;
using ShootighLibrary.MVVM;
using System;
using System.IO;

namespace LineOfBattle.Models
{
    internal class UIModel : ModelBase
    {
        private static UIModel _singleton;
        internal static UIModel Singleton => _singleton ?? (_singleton = new UIModel());

        private static ModelBase _sceneUIModel;
        internal ModelBase SceneUIModel {
            get => _sceneUIModel;
            private set => SetProperty( ref _sceneUIModel, value );
        }

        private UIModel()
            => Mediator.Singleton.Subscribe( typeof( UIModel ), new Action<string>( TransitUIModel ) );

        private void TransitUIModel( string sceneName )
        {
            SceneUIModel?.Dispose();

            switch ( sceneName ) {
                case "TitleScene":
                    SceneUIModel = new TitleSceneUIModel();
                    break;

                case "BattleScene":
                    SceneUIModel = new BattleSceneUIModel();
                    break;

                case "ResultScene":
                    SceneUIModel = new ResultSceneUIModel();
                    break;

                default:
                    throw new InvalidOperationException( $"Unknown scene name \"{sceneName}\" used at {nameof( UIModel )}.{nameof( TransitUIModel )}()." );
            }
        }

        internal void DebugSaveMessengerInfo()
        {
            void mkdir( string path ) { if ( !Directory.Exists( path ) ) { Directory.CreateDirectory( path ); } };

            var lobDir = Path.Combine( Path.GetTempPath(), "line_of_battle" );
            mkdir( lobDir );
            var msgDir = Path.Combine( lobDir, "messenger_debug_outputs" );
            mkdir( msgDir );
            var timeDir = Path.Combine( msgDir, DateTime.Now.ToString( "yyyyMMdd_HHmmss_fff" ) );
            mkdir( timeDir );

            Mediator.Singleton.DebugOutputPublishers( s => File.WriteAllText( Path.Combine( timeDir, "publishers.csv" ), s ) );
            Mediator.Singleton.DebugOutputSubscribers( s => File.WriteAllText( Path.Combine( timeDir, "subscribers.csv" ), s ) );
        }
    }
}
