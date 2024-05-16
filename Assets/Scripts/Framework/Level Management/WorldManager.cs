using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dogabeey
{
    public class WorldManager : SingletonComponent<WorldManager>
    {
        public List<World> worlds;
        public Transform levelContainer;
        public Transform mainMenu;

        private World currentWorld;

        public World CurrentWorld
        {
            get
            {
                return currentWorld;
            }
            set
            {

                currentWorld = value;
                
                EventManager.TriggerEvent(Const.GameEvents.CURRENT_WORLD_CHANGED, new EventParam());
            }
        }
        public World MainWorld
        {
            get
            {
                return worlds.Find(world => world.mainWorld);
            }
        }

        public void LoadLevel(LevelScene levelScene)
        {
            EndCurrentLevel();
            World.Instance.CurrentLevel = Instantiate(levelScene, levelContainer);
            ScreenManager.Instance.Show(Const.Screens.GameScene);
        }
        public void LoadCurrentLevel()
        {
            LoadLevel(FindCurrentLevel());
        }
        public void EndCurrentLevel()
        {
            if (World.Instance.CurrentLevel != null)
            {
                Destroy(World.Instance.CurrentLevel.gameObject);
                World.Instance.CurrentLevel = null;

            }
        }
        public void LoadNextLevel()
        {
            if (World.Instance.CurrentLevel != null)
            {
                LoadLevel(FindNextLevel());
            }
        }
        public void ResetCurrentLevel()
        {
            if (World.Instance.CurrentLevel != null)
            {
                LoadLevel(FindCurrentLevel());
            }
        }
        private LevelScene FindCurrentLevel()
        {
            return World.Instance.levelScenes[World.Instance.lastPlayedLevelIndex];
        }
        private LevelScene FindNextLevel()
        {
            World.Instance.lastPlayedLevelIndex++;
            return World.Instance.levelScenes[World.Instance.lastPlayedLevelIndex];
        }

        private void Update()
        {
        }

    }
}

