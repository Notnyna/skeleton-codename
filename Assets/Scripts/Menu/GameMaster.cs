using UnityEngine;
using System.Collections.Generic;

namespace Menu
{
    /// <summary>
    /// Should be in the same gameobject as MenuManager
    /// Controls scenario related stuff. Like make player talk to sm, camera pan, drama.
    /// 
    /// </summary>
    public class GameMaster : MonoBehaviour //Wanted to make it static, wont though
    {
        public List<Character.Player> Players = new List<Character.Player>();
        int currentPlayer=0;
        private Scenario.CameraControl CameraT;
        public Texture2D cursor;

        private void Awake()
        {
            CameraT = Camera.main.GetComponent<Scenario.CameraControl>();
            //Players= new List<Character.Player>();
            //Debug.Log("Found " + Players.Count + " players, just telling.");
            if (cursor != null) { Cursor.SetCursor(cursor,Vector2.zero,CursorMode.Auto); }
        }

        public Character.Player GetPlayer()
        {
            return Players[currentPlayer];
        }

        public void SwitchPlayer(int player)
        {
            Players[currentPlayer].enabled = false;
            Players[player].enabled = true;
            currentPlayer = player;
            drama = true;
        }

        public void SwitchPlayer()
        {
            foreach (Character.Player p in Players)
            {
                if (p.available) { SwitchPlayer(Players.IndexOf(p)); count1 = 2f; drama = true; break; }
            }
        }

        public void DisableControls()
        {
            Players[currentPlayer].enabled = false;
        }

        public void EnableControls()
        {
            if (Players[currentPlayer] == null) {  return; } //Debug.Log("No current player, logically shouldn't happen");
            Players[currentPlayer].enabled = true;
        }

        public void DirectCamera(float time, Transform T)
        {
            count0 = time;
            CameraT.SwitchTarget(T);
            drama = true;
            GetComponent<MenuManager>().NoMenu();
            
        }
        bool drama;
        float count0 = 0;
        float count1 = 2;
        private void Update()
        {
            if (drama)
            {
                if (count0 > 0) { count0 -= Time.deltaTime; }
                else { SwitchPlayer(currentPlayer);
                    if (count1 > 0) { count1 -= Time.deltaTime; }
                    else
                    {
                        CameraT.SwitchTarget(Players[currentPlayer].transform);
                        drama = false;
                    }
                }

            }

        }

    }
}
