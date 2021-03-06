﻿using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class GameControls : MonoBehaviour {

    public List<AudioClip> audioSources;
    public EnemySpawner spawner;
    public EnemySpawner collectables;
    public PlayerController player;
    public GameObject InGame;
    public Text counter;
    public Text TitleText;
    public Text GameOver;
    public Text results;
    public Text tapRestart;
    public Text Score;
    public Text TopScore;
    public float restartDelay = 10f;

    private bool gameStarted;
    private int clipIndex = 0;
    private AudioSource source;
    private float restartTimer = 0;
    private bool hasIncreased = true;

    // Use this for initialization
    void Start () {
        this.source = this.GetComponent<AudioSource>();
        spawner.Disable();
        collectables.Disable();
        setTopScore();
        
        ToggleTitle(true);
        ToggleInGame(false);
        ToggleGameOver(false);
    }

    private void ToggleTitle(bool status) {
        TitleText.gameObject.SetActive(status);
    }

    private void ToggleGameOver(bool status) {
        GameOver.gameObject.SetActive(status);
    }
    private void ToggleInGame(bool status) {
        InGame.SetActive(status);
    }
	private void SetResults() {
        float currentScore = (player.GetTimeElapsed() * 100);
        results.text = "Score: " + currentScore.ToString("000000");

        float highscore = 0;
        if(PlayerPrefs.HasKey("Highscore")){
            highscore = PlayerPrefs.GetFloat("Highscore");
        }

        PlayerPrefs.SetFloat("Highscore", (currentScore > highscore ? currentScore : highscore));
    }

    private void setTopScore() {
        float highscore = 0;
        if(PlayerPrefs.HasKey("Highscore")){
            highscore = PlayerPrefs.GetFloat("Highscore");
        }

        TopScore.text = "Current Highscore: " + highscore.ToString("000000");
    }

	private void SetScore() {
        Score.text = "Current Score: " + (player.GetTimeElapsed() * 100).ToString("000000");
    }
	// Update is called once per frame
	void Update () {
        SetScore();
        if(!source.isPlaying)
        {
            source.clip = audioSources[clipIndex];
            clipIndex = (clipIndex + 1) % audioSources.Count;
            source.Play();
        }

        if((Input.GetButtonDown("Jump") ||  Input.GetMouseButtonDown(0)) && !gameStarted) {
            ToggleTitle(false);
            ToggleInGame(true);
            ToggleGameOver(false);
            spawner.Enable();
            collectables.Enable();
            player.SetTimer();
            gameStarted = true;
        }
        
        if(player.IsPlaying()) {
            float looper = player.GetTimeElapsed() % spawner.decrementInterval;

            if(looper < 1 && !hasIncreased) {
                spawner.IncreaseDifficulty();
                hasIncreased = true;
            }

            if(looper > 1) {
                hasIncreased = false;
            }
        }
    }

    void OnGUI() {
        if(gameStarted) {
            counter.text = player.GetTimeLeft().ToString("0") + " til Death";
        }

        if(restartTimer > 0 && Time.time >= restartTimer) {
            
            tapRestart.gameObject.SetActive(true);

            if(Input.GetButtonDown("Jump") ||  Input.GetMouseButtonDown(0)) {
                gameStarted = true;
                SceneManager.LoadScene(0);
            }
        }


        if(gameStarted && player.GetTimeLeft() == 0) {
            if(restartTimer == 0) {
                restartTimer = Time.time + restartDelay;
                SetResults();
            }

            ToggleGameOver(true);
            ToggleInGame(false);

            spawner.Disable();
            collectables.Disable();
        }
    }
}
