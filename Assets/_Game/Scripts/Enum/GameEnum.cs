    public class GameEnum
    {
        public enum TileState
        {
            Gun, Solar_Generator, Repair_Station, Summons_Engine, ATM_I, ATM_II, ATM_III, ATM_IV, Cross, Siren, Coin_Machine, Speed_Reducer,
            Scan_Machine, Death_Machine, Trap, Computer, Compass, Chemical_Bottle, Power_Booster, Door
        }
        public enum GameState
        {
            Menu, InGame, Pause, Win, Lose
        }
        public enum GameMode
        {
            Survivor, PKMode, Challenge
#if Conf_ios
                ,Defense
#endif
        }
        public enum HomeState
        {
            Open, Closed
        }

        public enum CharacterState
        {
            Alive, Dead
        }
        public enum SoldierState
        {
            Intro, Idle, Run, Attack, Die
        }
        public enum ItemState
        {
            Active, Deactive
        }
        public enum TypePanel
        {
            ONEGIFT, MANYGIFT, STARTER, SKIN, NONE, SUPPORTVIP, SUPPORTNORMAL
        }
    }
