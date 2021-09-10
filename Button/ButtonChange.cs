using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonChange : MonoBehaviour
{
    public string SceneName;


    public void ChangeGame()
    {
        SceneManager.LoadScene("GamePlay");
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void PlayerEndTurn()
    {
        TurnBaseBattleManager tm = TurnBaseBattleManager.GetInst();
        tm.PlayerEndTurn();
    }

    public void SetPlayer()
    {
        PlayerPositionSet pps = PlayerPositionSet.GetInst();
        pps.StartBattle();
    }
}
