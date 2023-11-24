using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fakeGameManager : MonoBehaviour
{
    public enum GameState
    {
        Intro,
        Lobby,
        InGame
    }
    public GameState gState = GameState.Intro;
}
