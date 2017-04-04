using System;
using SwinGameSDK;


namespace battleship
{

    class GameLogic {

        public static void Main() {
			GameController game = new GameController ();

            // Opens a new Graphics Window
            SwinGame.OpenGraphicsWindow("Battle Ships", 800, 600);
            // Load Resources
            GameResources.LoadResources();
            SwinGame.PlayMusic(GameResources.GameMusic("Background"));
            // Game Loop
            for (
            ; (((SwinGame.WindowCloseRequested() == true)
                        || (game.CurrentState == GameState.Quitting))
                        == false);
            ) {
                GameController.HandleUserInput();
                GameController.DrawScreen();
            }

            SwinGame.StopMusic();
            // Free Resources and Close Audio, to end the program.
            GameResources.FreeResources();
        }
    }
}