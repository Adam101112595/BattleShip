using System;
using SwinGameSDK;

namespace battleship
{

    class GameLogic {

        public static void Main() {
            // Opens a new Graphics Window
            SwinGame.OpenGraphicsWindow("Battle Ships", 800, 600);
            // Load Resources
            GameResources.LoadResources();
            SwinGame.PlayMusic(GameResources.GameMusic("Background"));
            // Game Loop
            for (
            ; (((SwinGame.WindowCloseRequested() == true)
                        || (GameController.CurrentState == GameState.Quitting))
                        == false);
            ) {
                HandleUserInput();
                DrawScreen();
            }

            SwinGame.StopMusic();
            // Free Resources and Close Audio, to end the program.
            GameResources.FreeResources();
        }
    }
}