// '' <summary>
// '' Player has its own _PlayerGrid, and can see an _EnemyGrid, it can also check if
// '' all ships are deployed and if all ships are detroyed. A Player can also attach.
// '' </summary>
using System;
using System.Collections.Generic;

namespace battleship
{

    public class Player
    {

        protected static Random _Random = new Random();

        private static Dictionary<ShipName, Ship> _Ships = new Dictionary<ShipName, Ship>();

        private static SeaGrid _playerGrid = new SeaGrid(_Ships);

        private static ISeaGrid _enemyGrid;

        protected static BattleShipsGame _game;

        private static int _shots;

        private static int _hits;

        private static int _misses;

        // '' <summary>
        // '' Returns the game that the player is part of.
        // '' </summary>
        // '' <value>The game</value>
        // '' <returns>The game that the player is playing</returns>
        public BattleShipsGame Game
        {
            get
            {
                return _game;
            }
            set
            {
                _game = value;
            }
        }
	//'' <summary>
	//'' Displays enemies hits on your grid
	//'' </summary>
        public ISeaGrid Enemy
        {
            set
            {
                _enemyGrid = value;
            }
        }

        public Player(BattleShipsGame controller)
        {
            _game = controller;
            // for each ship add the ships name so the seagrid knows about them
            foreach (ShipName name in Enum.GetValues(typeof(ShipName)))
            {
                if ((name != ShipName.None))
                {
                    _Ships.Add(name, new Ship(name));
                }

            }

            this.RandomizeDeployment();
        }

        // '' <summary>
        // '' The EnemyGrid is a ISeaGrid because you shouldn't be allowed to see the enemies ships
        // '' </summary>
        public ISeaGrid EnemyGrid
        {
            get
            {
                return _enemyGrid;
            }
            set
            {
                _enemyGrid = value;
            }
        }

	//'' <summary>
	//'' Shows player grid 
	//'' </summary>
        public SeaGrid PlayerGrid
        {
            get
            {
                return _playerGrid;
            }
        }

	
        public bool ReadyToDeploy
        {
            get
            {
		    // checks if all the ships are deployed on the player grid
                return _playerGrid.AllDeployed;
            }
        }

        public bool IsDestroyed
        {
            get
            {
                // Check if all ships are destroyed... -1 for the none ship
				return _playerGrid.ShipsKilled == Enum.GetValues (typeof (ShipName)).Length - 1;
            }
        }

		public Ship Ship(ShipName name)
        {
            if ((name == ShipName.None))
                {
                    return null;
                }

                return _Ships[name];
        }

        public int Shots
        {
            get
            {
		    //displays the amount of shots taken by player
                return _shots;
            }
        }

        public int Hits
        {
            get
            {
		    //displays number of hits player has made
                return _hits;
            }
        }

        public int Missed
        {
            get
            {
		    //displays number of misses made by player
                return _misses;
            }
        }

        public int Score
        {
            get
            {
		    //Displays score
                if (IsDestroyed)
                {
                    return 0;
                }
                else
                {
                    return ((Hits * 12)
                                - (Shots
                                - (PlayerGrid.ShipsKilled * 20)));
                }

            }
        }


		// FixMe: Unused code? or Duplicate?
        public IEnumerator<Ship> GetShipEnumerator()
        {
            Ship[] result = new Ship [_Ships.Values.Count + 1];
            _Ships.Values.CopyTo(result, 0);
            List<Ship> lst = new List<Ship>();
            lst.AddRange(result);
            return lst.GetEnumerator();
        }

        // '' <summary>
        // '' Makes it possible to enumerate over the ships the player
        // '' has.
        // '' </summary>
        // '' <returns>A Ship enumerator</returns>
        public IEnumerator<Ship> GetEnumerator()
        {
            Ship[] result = new Ship[_Ships.Values.Count + 1];
            _Ships.Values.CopyTo(result, 0);
            List<Ship> lst = new List<Ship>();
            lst.AddRange(result);
            return lst.GetEnumerator();
        }

        // '' <summary>
        // '' Vitual Attack allows the player to shoot
        // '' </summary>
        public virtual AttackResult Attack()
        {
            // human does nothing here...
            return null;
        }

        // '' <summary>
        // '' Shoot at a given row/column
        // '' </summary>
        // '' <param name="row">the row to attack</param>
        // '' <param name="col">the column to attack</param>
        // '' <returns>the result of the attack</returns>
        internal AttackResult Shoot(int row, int col)
        {
            _shots++;
            AttackResult result;
            result = EnemyGrid.HitTile(row, col);
            switch (result.Value)
            {
                case ResultOfAttack.Destroyed:
                case ResultOfAttack.Hit:
                    _hits++;
                    break;
                case ResultOfAttack.Miss:
                    _misses++;
                    break;
            }
            return result;
        }

        public virtual void RandomizeDeployment()
        {
            bool placementSuccessful = true;
            Direction heading;
            // for each ship to deploy in shipist
            foreach (ShipName shipToPlace in Enum.GetValues(typeof(ShipName)))
            {
                if ((shipToPlace == ShipName.None))
                {
                    placementSuccessful = false;
                }

               
                for (; !placementSuccessful;)
                {
                    int dir = _Random.Next(2);
                    int x = _Random.Next(0, 11);
                    int y = _Random.Next(0, 11);
                    if ((dir == 0))
                    {
                        heading = Direction.UpDown;
                    }
                    else
                    {
                        heading = Direction.LeftRight;
                    }

                    // try to place ship, if position unplaceable, generate new coordinates
                    try
                    {
                        PlayerGrid.MoveShip(x, y, shipToPlace, heading);
                        placementSuccessful = true;
                    }
                    catch
                    {
                        placementSuccessful = false;
                    }

                }

            }

        }
    }
}
