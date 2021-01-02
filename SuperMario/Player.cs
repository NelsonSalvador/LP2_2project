using System;
using System.Threading;
using System.Collections.Generic;
using CoreGameEngine;

namespace SuperMario
{
    public class Player : Component
    {
       private KeyObserver keyObserver;
       private Position position;
       private List<Vector2> Occupied;
       private List<GameObject> coins;
       private List<GameObject> boxes;

       private Score actualScore;

       private bool inAir = false;
       private bool ground = true;
       private bool coinScore = false;
       private bool boxHit = false;
       private int jumpFrames = 0;
       private float x, y;
       private ConsoleKey Lastkey = ConsoleKey.LeftArrow;

        public override void Start()
        {
            keyObserver = ParentGameObject.GetComponent<KeyObserver>();
            position = ParentGameObject.GetComponent<Position>();
        }

        public Player (List<Vector2> Occupied, Score score, List<GameObject> boxes, List<GameObject> coins)
        {
            this.Occupied = Occupied; 
            this.coins = coins;
            this.boxes = boxes;
            actualScore = score;
        }

        public override void Update()
        {
            x = position.Pos.X;
            y = position.Pos.Y;
            bool colide = false;

            ground = checkGround();
            
            
            if(!inAir && !ground)
            {
                while(!ground)
                {
                    falling();
                    ground = true;
                }
            }
            else if(!inAir)
            {
                foreach(ConsoleKey key in keyObserver.GetCurrentKeys())
                {
                    switch (key)
                    {
                        case ConsoleKey.RightArrow:
                            Lastkey = ConsoleKey.RightArrow;
                            foreach (Vector2 v in Occupied)
                            {
                                if (position.Pos.X + 7 == v.X && position.Pos.Y == v.Y)
                                    colide = true;
                            }
                            if (!colide)
                                x += 1;
                            position.Pos = new Vector3(x, y, position.Pos.Z);
                            
                            turnSprite(true);
                            //colide = false;
                            break;
                        case ConsoleKey.UpArrow:
                            Lastkey = ConsoleKey.UpArrow;
                            position.Pos = new Vector3(x, y, position.Pos.Z);
                            break;
                        case ConsoleKey.LeftArrow:
                            Lastkey = ConsoleKey.LeftArrow;
                            foreach (Vector2 v in Occupied)
                            {
                                if (position.Pos.X - 1 == v.X && position.Pos.Y == v.Y)
                                    colide = true;
                            }
                            if (!colide)
                                x -= 1;
                            position.Pos = new Vector3(x, y, position.Pos.Z);
                            turnSprite(false);
                            break;
                        case ConsoleKey.Spacebar:
                            inAir = true;
                            position.Pos = new Vector3(x, y, position.Pos.Z);
                            break;
                    }
                }

                x = Math.Clamp(x, 0, ParentScene.xdim - 8);
                y = Math.Clamp(y, 0, ParentScene.ydim - 3);

                //position.Pos = new Vector3(x, y, position.Pos.Z);
            }
            else if(inAir)
            {
                if(jumpFrames == 0)
                {
                    y -= 2;
                    if(Lastkey == ConsoleKey.RightArrow)
                        x += 3;
                    else if (Lastkey == ConsoleKey.LeftArrow)
                        x -= 3;
                    jumpFrames++;
                }
                else if(jumpFrames == 1)
                {
                    y -= 2;
                    if(Lastkey == ConsoleKey.RightArrow)
                        x += 3;
                    else if (Lastkey == ConsoleKey.LeftArrow)
                        x -= 3;
                    jumpFrames++;
                }
                else if (jumpFrames == 2)
                {
                    y -= 2;
                    if(Lastkey == ConsoleKey.RightArrow)
                        x += 3;
                    else if (Lastkey == ConsoleKey.LeftArrow)
                        x -= 3;
                    jumpFrames++;
                }
                else if(jumpFrames == 3)
                {
                    y -= 2;
                    if(Lastkey == ConsoleKey.RightArrow)
                        x += 3;
                    else if (Lastkey == ConsoleKey.LeftArrow)
                        x -= 3;
                    jumpFrames = 0;
                    inAir = false;
                }
                x = Math.Clamp(x, 0, ParentScene.xdim - 8);
                y = Math.Clamp(y, 0, ParentScene.ydim - 3);
                position.Pos = new Vector3(x, y, position.Pos.Z);
                Thread.Sleep(80);
            }

            if (coins != null)
                coinScore = checkCoin();
            if(coinScore)
            {
                actualScore.score += 1000;
                coinScore = false;
            }
            
        }
        private bool checkBox()
        {
            char[,] emptyBoxSprite =
                {
                    { '█', '█', '█' , '█'},
                    { '█', ' ', ' ' , '█'},
                    { '█', ' ', ' ' , '█'},
                    { '█', ' ', ' ' , '█'},
                    { '█', ' ', ' ' , '█'},
                    { '█', '█', '█' , '█'} 
                };
            boxHit = false;
            foreach(GameObject Box in boxes)
            {
                if ((position.Pos.X  >= Box.GetComponent<Position>().Pos.X && position.Pos.X <= Box.GetComponent<Position>().Pos.X + 4 &&
                    position.Pos.Y == Box.GetComponent<Position>().Pos.Y + 7) && Box.GetComponent<BoxConfirmation>().boxUsed == 0)
                {
                    boxHit = true;
                    Box.GetComponent<BoxConfirmation>().boxUsed = 1;
                    actualScore.score += 1000;
                    Box.GetComponent<ConsoleSprite>().SwitchSprite(emptyBoxSprite, ConsoleColor.Yellow, ConsoleColor.DarkGray);
                } 
                
            }
            if(!boxHit)
                return false;
            
            return true;
        }

        private bool checkGround()
        {
            bool ground = false;
            // check if there is ground beneth the player 
            foreach (Vector2 v in Occupied)
            {
                if ((position.Pos.X + 1 == v.X && position.Pos.Y + 4 == v.Y) || (position.Pos.X + 5 == v.X && position.Pos.Y + 4 == v.Y))
                {
                    ground = true;
                } 
            }
            if(!ground)
                return false;
            return true;
        }

        private bool checkCoin()
        {
            coinScore = false;
            char[,] noMoreCoinSprite =
            {
                {' '}
            };
            // check if there is ground beneth the player
            
            foreach (GameObject coin in coins)
            {
                if ((position.Pos.X  == coin.GetComponent<Position>().Pos.X && position.Pos.Y == coin.GetComponent<Position>().Pos.Y) && 
                    coin.GetComponent<CoinConfirmation>().coinUsed == 0)
                {
                    coinScore = true;
                    coin.GetComponent<CoinConfirmation>().coinUsed = 1;
                    coin.GetComponent<ConsoleSprite>().SwitchSprite(noMoreCoinSprite, ConsoleColor.Gray, ConsoleColor.Gray);
                } 
            }
            if(!coinScore)
                return false;
                
            return true;
        }

        private void falling()
        {
            if (boxes != null)
                boxHit = checkBox();
            if(boxHit)
            {
                actualScore.score += 1000;
                boxHit = false;
            }
            x = position.Pos.X;
            y = position.Pos.Y;
            y++;
            x = Math.Clamp(x, 0, ParentScene.xdim - 8);
            y = Math.Clamp(y, 0, ParentScene.ydim - 3);
            
            position.Pos = new Vector3(x, y, position.Pos.Z);
            Thread.Sleep(50);
            if (ParentScene.ydim - 4 == position.Pos.Y)
            {
                ParentScene.Terminate();
                Level1 level1 = new Level1();
                level1.Run();
            }
        }

        private void turnSprite(bool right)
        {
            char[,] playerSprite =
            {
                { '─', '▄', '█' , '└'},
                { '▄', '▀', '▄' , '▄'},
                { '█', '█', '▐' , '▄'},
                { '█', '▀', '▐' , '▄'},
                { '█', '▐', '▄' , '▄'},
                { '█', '└', '█' , '▄'},
                { '▄', '─', '▄' , '┘'},
                { '▄', '┐', '┘' , ' '}
            };
            char[,] playerSpriteT =
            {
                { '▄', '┌', '└' , ' '},
                { '▄', '─', '▄' , '└'},
                { '█', '┘', '█' , '▄'},
                { '█', '▌', '▄' , '▄'},
                { '█', '▀', '▌' , '▄'},
                { '█', '█', '▌' , '▄'},
                { '▄', '▀', '▄' , '▄'},
                { '─', '▄', '█' , '┘'}
            };

            if(right == false)
                ParentGameObject.GetComponent<ConsoleSprite>().SwitchSprite(playerSpriteT, ConsoleColor.Red, ConsoleColor.Gray);
            else
                ParentGameObject.GetComponent<ConsoleSprite>().SwitchSprite(playerSprite, ConsoleColor.Red, ConsoleColor.Gray);

        }
    }
}