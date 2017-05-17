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

        private void Start()
        {
            CameraT = Camera.main.GetComponent<Scenario.CameraControl>();
            //Players= new List<Character.Player>();
            //Debug.Log("Found " + Players.Count + " players, just telling.");
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
            CameraT.SwitchTarget(Players[currentPlayer].transform);
        }

        public void DisableControls()
        {
            Players[currentPlayer].enabled = false;
        }

        public void EnableControls()
        {
            Players[currentPlayer].enabled = true;
        }

        public void DirectCamera()
        {
            //To do

        }

    }
}
