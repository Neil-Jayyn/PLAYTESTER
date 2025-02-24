﻿using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

// AppScript is on all apps that need to open to a new screen. They contain the place to go to once opened.
public class AppScript : MonoBehaviour
{   
    //Declare Variables
    public Vector3 myScreenLocation = new Vector3(0, 0, -10); //position for this app to go when clicked
    GameObject UIController;
    GameObject GameManager;
    GameObject CupcakeGameManager;
    GameObject CoinGameManager;
    GameObject DuckGameManager;
    GameObject NewsAppCaptchaButton;
    GameObject StartButton;
    public bool canBeClicked;
    public bool isMinigameButton = false; //Special case for the play minigame button
    public bool isClockOutButton = false; //Special case for the clock out button
    public bool isCaptchaButton = false;
    public bool isStartButton = false;
    private Vector3 cupcakeGameLocation = new Vector3(50, 50, -10);
    private Vector3 coinGameLocation = new Vector3(50, 35, -10);
    private Vector3 duckGameLocation = new Vector3(50, 20, -10);

    // Audio
    public AudioSource captchaSFX; // sfx for captcha confirmation
    public AudioSource clickSFX; // sfx for start button click
    

    // Start is called before the first frame update
    void Start()
    {
        //Set Variables        
        UIController = GameObject.Find("UI Controller");
        GameManager = GameObject.Find("Game Manager");
        CupcakeGameManager = GameObject.Find("CupcakeGameManager");
        CoinGameManager = GameObject.Find("MinigameManager");
        DuckGameManager = GameObject.Find("DuckGameManager");
        NewsAppCaptchaButton = GameObject.Find("News App Captcha Button");
        StartButton = GameObject.Find("Start Button");
        canBeClicked = true; //true by default
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // This function is triggered when the app is clicked
    void OnMouseUp()
    {
        UpdateCanBeClicked(); // Check if the button is valid to click

        if(canBeClicked)
        {
            if (isMinigameButton)
            {
                //play the next relevant minigame

                //Tell the game manager we are in a game
                GameManager.GetComponent<GameManagerScript>().StartedMinigame();

                int gamesPlayed = GameManager.GetComponent<GameManagerScript>().GetMinigamesPlayed();

                if (gamesPlayed == 0)
                {
                    //PLAY THE CUPCAKE GAME

                    if (GameManager.GetComponent<GameManagerScript>().GetDay() == 2 && GameManager.GetComponent<GameManagerScript>().EMPHappened == false)
                    {
                        //We should do the EMP scene instead since it's day 2 and we haven't done it yet
                        GameManager.GetComponent<GameManagerScript>().StartEMP();

                    }
                    else
                    {
                        //Go into the cupcake game as normal

                        //Tell the game manager we are in a game
                        GameManager.GetComponent<GameManagerScript>().StartedMinigame();

                        UIController.GetComponent<ComputerUIScript>().GoToPosition(cupcakeGameLocation);
                        Debug.Log("In Cupcake Minigame");
                        //trigger the start of the cupcake minigame
                        CupcakeGameManager.GetComponent<CupcakeGameManager>().StartCupcakeMinigame();
                    }

                }
                else if (gamesPlayed == 1)
                {
                    //PLAY THE COIN RUNNER GAME

                    //Tell the game manager we are in a game
                    GameManager.GetComponent<GameManagerScript>().StartedMinigame();

                    UIController.GetComponent<ComputerUIScript>().GoToPosition(coinGameLocation);
                    //trigger the start of the coin minigame
                    CoinGameManager.GetComponent<MinigameManager>().StartCoinMinigame();
                }
                else
                {
                    //PLAY THE DUCK GAME

                    //Tell the game manager we are in a game
                    GameManager.GetComponent<GameManagerScript>().StartedMinigame();

                    UIController.GetComponent<ComputerUIScript>().GoToPosition(duckGameLocation);
                    //trigger the start of the duck minigame
                    DuckGameManager.GetComponent<DuckGameManager>().StartDuckMinigame();

                }

            }
            else if (isClockOutButton)
            {
                //go to the next day
                if (GameManager.GetComponent<GameManagerScript>().GetDay() == 3)
                {
                    //player has finished day 3
                    //TODO: trigger an ending here
                    UIController.GetComponent<ComputerUIScript>().TriggerPopup(new Vector3(0, 0, -2), "You beat the game! Text here depends on your score.");
                }
                else
                {
                    //TODO: add anything fancy we want to happen transition wise, maybe make popup appear or fade to black
                    GameManager.GetComponent<GameManagerScript>().CompletedDay();
                    UIController.GetComponent<ComputerUIScript>().TriggerPopup(new Vector3(0, 0, -2), "Welcome to another day of work!");
                }

            }
            else
            {
                if (isCaptchaButton)
                {
                    captchaSFX.Play();
                }
                else if (isStartButton)
                {
                    clickSFX.Play();
                }
                // All normal apps will execute this script:
                UIController.GetComponent<ComputerUIScript>().GoToPosition(myScreenLocation);
                
            }
            
        }
        else
        {
            //Object is not clickable for some reason.
            //TODO: We can add an popup here specific to the clock out button that tells the 
            //player they haven't completed all the minigames yet, or if its the minigame button
            //we can tell the player they've already completed the minigames for the day

            if(isClockOutButton)
            {
                UIController.GetComponent<ComputerUIScript>().TriggerPopup(new Vector3(0, 0, -2), "Complete all three minigames before clocking out!");

            }
            else if(isMinigameButton)
            {
                UIController.GetComponent<ComputerUIScript>().TriggerPopup(new Vector3(0, 0, -2), "Great work, employee! You can clock out now, you've finished all the minigames today!");
            }
        }
    }

    //Check if the object is allowed to be clicked
    private void UpdateCanBeClicked()
    {
        int gamesPlayed = GameManager.GetComponent<GameManagerScript>().GetMinigamesPlayed();

        if (isMinigameButton)
        {
            if (gamesPlayed == 3)
            {
                //Player has already played all three minigames so they cannot play another today
                canBeClicked = false;
            }
            else
            {
                canBeClicked= true;
            }
        }
        else if (isClockOutButton)
        {
            if (gamesPlayed == 3)
            {
                //Player has played all three minigames so they can clock out
                canBeClicked = true;
            }
            else
            {
                canBeClicked = false;
            }
        }
    }
}


