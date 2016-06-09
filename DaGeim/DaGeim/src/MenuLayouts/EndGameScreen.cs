﻿using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DaGeim
{
    public class EndGameScreen : MainGame
    {
        private List<Score> highscores = new List<Score>();
        private char[] delimiters = { ' ', '\t', '\n' };
        private string path = @"TextFile1.txt";
        private bool wasUpdated;
        private int positionAtScorelist;

        private Button newGameButton = new Button("New Game", new Rectangle(950, 400, 300, 80));
        private Button mainMenuButton = new Button("Main Menu", new Rectangle(950, 500, 300, 80));
        private Button quitButton = new Button("Quit", new Rectangle(950, 600, 300, 80));
        private Selector selector;

        private Texture2D background;
        private Texture2D star;
        private Texture2D robotImage;
        private SpriteFont mainFont;
        private SpriteFont font;

        public void Load(ContentManager content)
        {
            //load the content for the scoreboard
            background = content.Load<Texture2D>("background2");
            font = content.Load<SpriteFont>("Font");
            mainFont = content.Load<SpriteFont>("MainFont");
            star = content.Load<Texture2D>("bluestar");
            robotImage = content.Load<Texture2D>("Idle");
            newGameButton.Load(content);
            mainMenuButton.Load(content);
            quitButton.Load(content);
            //give the selector object cords inside the top-most button
            selector = new Selector(1000, 420);

            //load the current highscores from file and fill a list with them
            using (StreamReader reader = new StreamReader(path))
            {
                string line = string.Empty;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] tokens = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                    string name = tokens[0];
                    long points = long.Parse(tokens[1]);
                    highscores.Add(new Score(name, points));
                }
            }
        }
        //this method saves the scores in the file only if the player managed to get in top 10
        //we are using it in the UpdateScore method below
        private void SaveScores()
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                for (int i = 0; i < 10; i++)
                {
                    string line = string.Format("{0}    {1}",
                        highscores[i].playerName, highscores[i].points);
                    writer.WriteLine(line);
                }
            }
        }
        //update the scoreboard each time the player dies or clears the level
        public void UpdateScoreboard(Score playerScore)
        {
            wasUpdated = false;
            positionAtScorelist = -1;
            //set to false every time we call the method
            for (int i = 0; i < highscores.Count; i++)
            {
                if (playerScore.points > highscores[i].points)
                {
                    highscores.Insert(i, playerScore);
                    positionAtScorelist = i;
                    wasUpdated = true;
                    break;
                }
            }
            if (wasUpdated) //remove the last entry in the list and save the changes
            {
                highscores.RemoveAt(10);
                SaveScores();
            }
        }
        //draw the scoreboard on the game over screen (called always after the UpdateScore method)
        public void Draw(SpriteBatch spritebatch)
        {
            int numberX = 300;
            int playerNameX = 350;
            int scoreX = 800;
            int rowY = 90;

            // spritebatch.Begin();
            spritebatch.Draw(background, new Rectangle(0, 0, 1280, 720), Color.White);
            spritebatch.Draw(robotImage, new Rectangle(50, 470, 250, 250), Color.White);
            spritebatch.DrawString(mainFont, "HIGHSCORES", new Vector2(500, 25),
                Color.Ivory);

            newGameButton.DrawButton(spritebatch, font);
            quitButton.DrawButton(spritebatch, font);
            //draw the data from the list
            for (int i = 0; i < highscores.Count; i++)
            {
                if (wasUpdated && positionAtScorelist == i)
                {
                    spritebatch.Draw(star, new Rectangle(310, rowY, 40, 40), Color.White);
                }
                string pos = string.Format("{0}.", i + 1).PadLeft(3);
                string name = highscores[i].playerName;
                string score = highscores[i].points.ToString().PadLeft(10, ' ');
                spritebatch.DrawString(font, pos, new Vector2(numberX, rowY), Color.Ivory);
                spritebatch.DrawString(font, name, new Vector2(playerNameX, rowY),
                    Color.Ivory);
                spritebatch.DrawString(font, score, new Vector2(scoreX, rowY),
                    Color.Ivory);
                rowY += 55;
            }
            //spritebatch.End();
        }
        //update the position of the selector and change the state of the buttons
        //once a button is active, pressing "space" will do something according the button
        public void Update(GameTime gameTime, MainGame game)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                selector.y -= 20; // we move the selector 20px up the screen
                if (selector.y < 420)
                    selector.y = 420; // the selector cant go above this position
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                selector.y += 20; // we move the selector 20px down the screen
                if (selector.y > 620)
                    selector.y = 620; // the selector cant go below this position
            }

            //make a small rectangle to check the if the selector is inside one of the buttons
            Rectangle rect = new Rectangle(selector.x, selector.y, 20, 20);
            if (rect.Intersects(mainMenuButton.location))
            {
                mainMenuButton.isSelected = true;
            }
            else
            {
                mainMenuButton.isSelected = false;
            }
            if (rect.Intersects(newGameButton.location))
            {
                newGameButton.isSelected = true;
            }
            else
            {
                newGameButton.isSelected = false;
            }
            if (rect.Intersects(quitButton.location))
            {
                quitButton.isSelected = true;
            }
            else
            {
                quitButton.isSelected = false;
            }
            //check which button is selected and do the action
            if (mainMenuButton.isSelected)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    //TODO call the main menu screen
                }
            }
            if (newGameButton.isSelected)
            {
                //starts new game
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    //TODO START NEW GAME LOGIC
                }
            }
            if (quitButton.isSelected)
            {
                //the quit button exits the game (you dont say!?!)
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    game.Exit();
                }
            }
        }
    }
}