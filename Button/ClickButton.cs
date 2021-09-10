using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ClickButton : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public Text text;
    public Sprite Default;
    public Sprite Highlight;
    public Sprite Press;
    public string function;
    public bool Animation;
    public bool PlayerPosSelectTurn;
    public bool EnemyTurn;
    public bool PlayerTurn;
    private bool isFirst = true;
    private bool fadeFlag = false;
    private float alpha = 0f;
    private bool PlayOnce = true;
    private bool HoldPress;
    public Sound s;

    private void Start()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "GamePlay":
                s = GameObject.Find("GameSystem").GetComponent<Sound>();
                break;
            default:
                s = GameObject.Find("Canvas").GetComponent<Sound>();
                break;
        }
        
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (PlayerPosSelectTurn || EnemyTurn || PlayerTurn)
        {
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0f);
            text.color = new Color(text.color.r, text.color.g, text.color.b, 0f);
        }
    }

    void Update()
    {
        if (Animation)
        {
            if (alpha <= 0f || alpha >= 1f)
            {
                isFirst = true;
            }

            switch (TurnBaseBattleManager.GetInst().currentState)
            {
                case TurnBaseBattleManager.BattleStates.PLAYERPOSITIONSET:
                    if (PlayerPosSelectTurn)
                    {
                        FadeIn();
                    }
                    else
                    {
                        FadeOut();
                    }
                    break;

                case TurnBaseBattleManager.BattleStates.ENEMYTURN:
                    if (EnemyTurn)
                    {
                        FadeIn();
                    }
                    else
                    {
                        FadeOut();
                    }
                    break;

                case TurnBaseBattleManager.BattleStates.PLAYERTURN:
                    if (PlayerTurn)
                    {
                        FadeIn();
                    }
                    else
                    {
                        FadeOut();
                    }
                    break;
            }
        }
    }

    private void OnMouseEnter()
    {
        s.SoundPlay("EffectSound/ui_general_highlight_button");
        PlayOnce = true;

        if (Highlight && !HoldPress)
        {
            spriteRenderer.sprite = Highlight;
        }
    }

    private void OnMouseExit()
    {
        if(!HoldPress)
        {
            spriteRenderer.sprite = Default;
        }
    }

    private void OnMouseDown()
    {
        s.SoundPlay("EffectSound/ui_map_window_open");

        if (Press)
        {
            spriteRenderer.sprite = Press;
        }
    }

    private void OnMouseUp()
    {
        if (PlayOnce)
        {
            switch (function)
            {
                case "StartBattle":
                    PlayerPositionSet.GetInst().StartBattle();
                    PlayerPositionSet.GetInst().startBattlePressed = true;
                    PlayOnce = false;
                    break;
                case "PlayerEndTurn":
                    TurnBaseBattleManager.GetInst().PlayerEndTurn();
                    PlayOnce = false;
                    break;
                case "SetProfileMiddleToggleOff":
                    GameObject.Find("startingGameUI").transform.GetChild(5).gameObject.SetActive(true);
                    GameObject.Find("startingGameUI").transform.GetChild(2).gameObject.SetActive(false);
                    break;
                case "SetProfileMiddleToggleOn":
                    GameObject.Find("startingGameUI").transform.GetChild(2).gameObject.SetActive(true);
                    GameObject.Find("startingGameUI").transform.GetChild(5).gameObject.SetActive(false);
                    break;
                case "UseWeapon":
                    if (ClickManager.GetInst().player != null && ClickManager.GetInst().player.Mode != Unit.MODE.Done) 
                    {
                        if(!ClickManager.GetInst().player.onWater)
                        {
                            ClickManager.GetInst().player.Mode = Unit.MODE.Attack;
                            HoldPress = true;
                        }
                    }
                    break;
                case "Repair":
                    if (!ClickManager.GetInst().player.onWater)
                    {
                        ClickManager.GetInst().HealPlayer();
                    }
                    break;
                case "MainMenuOn":
                    GameObject.Find("startingGameUI").transform.GetChild(6).gameObject.SetActive(true);
                    break;
                case "MainMenuOff":
                    GameObject.Find("startingGameUI").transform.GetChild(6).gameObject.SetActive(false);
                    break;
                case "SceneChangeMain":
                    TurnBaseBattleManager.GetInst().GameReset();
                    SceneManager.LoadScene("MainTitle");
                    break;
                case "SceneChange_GamePlay":
                    s.SoundPlay("EffectSound/ui_main_menu_pre_start_game");
                    SceneManager.LoadScene("GamePlay");
                    break;
                case "SceneChange_PlayerSelect":
                    SceneManager.LoadScene("PlayerSelect");
                    break;
                case "SceneChange_MapSelect":
                    SceneManager.LoadScene("MapSelect");
                    break;
                case "GameExit":
                    Application.Quit();
                    break;
            }
        }

        if (HoldPress)
        {
            spriteRenderer.sprite = Press;
        }
        else if (Highlight)
        {
            spriteRenderer.sprite = Highlight;
        }
        else
        {
            spriteRenderer.sprite = Default;
        }
    }

    private void OnDisable()
    {
        HoldPress = false;
        spriteRenderer.sprite = Default;
    }

    void FadeIn()
    {
        if (isFirst)
        {
            isFirst = false;
            fadeFlag = true;
        }

        if (fadeFlag)
        {
            if (alpha < 1f)
            {
                alpha += 0.1f;
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
                text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
            }
            else
            {
                fadeFlag = false;
            }
        }
    }

    void FadeOut()
    {
        if (isFirst)
        {
            isFirst = false;
            fadeFlag = true;
        }

        if (fadeFlag)
        {
            if (alpha > 0f)
            {
                alpha -= 0.1f;
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
                text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
            }
            else
            {
                fadeFlag = false;
                this.gameObject.SetActive(false);
            }
        }
    }
}
