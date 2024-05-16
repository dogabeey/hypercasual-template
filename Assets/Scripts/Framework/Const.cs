using UnityEngine;

namespace Dogabeey
{
    public struct Const
    {

        public enum Screens
        {
            MainMenu,
            LevelList,
            WorldList,
            GameScene,
            PauseMenu
        }
        public struct Values
        {
            public const float PICKUP_DROP_HEIGHT_TRY_STEP = 1f;
            public const float MOVEMENT_OVERLAP_SPHERE_SENSITIVITY = 0.2f;
            public const float MOVEMENT_DURATION = 0.2f;
            internal const float MOVEMENT_STEP = 1;
        }

        public struct TAGS
        {
            public const string PLAYER = "Player";
            public const string ENEMY = "Enemy";
            public const string COLLECTIBLE = "Collectible";
            public const string GROUND = "Ground";
        }

        public struct BindingNames
        {
            public const string KEYBOARD = "Keyboard";
            public const string GAMEPAD = "Gamepad";
        }
        public struct SOUNDS
        {
            public struct MUSICS
            {
                public const string MAIN_MENU = "MainMenu";
                public const string GAMEPLAY = "Gameplay";
            }
            public struct EFFECTS
            {
                public const string TYPEWRITER = "Typewriter";
                public const string JUMP = "Jump";
                public const string DEATH = "Death";
                public const string PICKUP = "Pickup";
                public const string LEVEL_COMPLETE = "LevelComplete";
                public const string LEVEL_FAILED = "LevelFailed";
            }
        }

        public struct GameEvents
        {
            public const string ENTITY_MOVED = "ENTITY_MOVED";
            public const string CREATURE_DEATH = "CREATURE_DEATH";
            public const string CREATURE_JUMP = "CREATURE_JUMP";
            public const string COLLECTIBLE_EARNED = "COLLECTIBLE_EARNED";
            public const string OBJECTIVE_COMPLETED = "OBJECTIVE_COMPLETED";
            public const string OBJECTIVE_FAILED = "OBJECTIVE_FAILED";

            public const string LEVEL_COMPLETED = "LEVEL_COMPLETED";
            public const string LEVEL_FAILED = "LEVEL_FAILED";
            public const string LEVEL_STARTED = "LEVEL_STARTED";
            public const string LEVEL_LOCKED = "LEVEL_LOCKED";
            public const string LEVEL_UNLOCKED = "LEVEL_UNLOCKED";

            public const string PLAYER_CREATED = "PLAYER_CREATED";

            public const string PLAYER_ENTERED_RANGE = "PLAYER_ENTERED_RANGE";
            public const string PLAYER_EXITED_RANGE = "PLAYER_EXITED_RANGE";
            public const string PLAYER_PICKED_OBJECT = "PLAYER_PICKED_OBJECT";
            public const string PLAYER_DROPPED_OBJECT = "PLAYER_DROPPED_OBJECT";

            public const string CURRENT_WORLD_CHANGED = "CURRENT_WORLD_CHANGED";
        }
    }
}