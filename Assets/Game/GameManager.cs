using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    /*
     * lives: describe number of chances player has left
     * score: player obtain points when brick is broken
     * coin: some bricks give coins; get extra lives once enough coin
     * is saved up
     */
    public int lives;
    public int score;
    public int coin;
    public int coin_to_live; // # of coins to become 1 life
    public TMP_Text scoretext;
    public TMP_Text cointext;
    public TMP_Text final;
    public TMP_Text result;
    public bool gameover;
    public GameObject screen;
    int bricks;
    public GameObject brick; // reference to a brick
    public GameObject spawnbrick; // reference to a spawn brick
    public GameObject coinbrick;
    public GameObject scorebrick;
    int current_level = 1;

    public GameObject ball;
    public GameObject Endless;
    public GameObject L1;
    public GameObject L2;
    public GameObject L3;
    public GameObject L4;
    public GameObject L5;

    public Image[] hearts;

    // Start is called before the first frame update
    void Start() {
        scoretext.text = "Score: " + score;
        cointext.text = "Coins: " + coin + "/" + coin_to_live;
        switch (Info.Gamemode) {
            case 1:
                L1.SetActive(true);
                break;
            case 2:
                Endless.SetActive(true);
                break;
            case 3:
                // random mode
                lives = 3;
                GameObject temp;
                for (int i = 0; i < 30; i++) {
                    Vector2 temp_vector = new Vector2(Random.Range(-7, 7), Random.Range(-3, 4) * 0.5f);
                    if (Physics2D.OverlapCircle(temp_vector, 0.4f) != null) continue;
                    // randomly decides type of brick to spawn
                    if (Random.Range(0, 10) <= 7) temp = Instantiate(brick,
                        temp_vector, Quaternion.identity);
                    else if (Random.Range(0, 3) == 1) temp = Instantiate(spawnbrick,
                        temp_vector, Quaternion.identity);
                    else if (Random.Range(0, 2) == 1) temp = Instantiate(coinbrick,
                        temp_vector, Quaternion.identity);
                    else temp = Instantiate(scorebrick, temp_vector, Quaternion.identity);
                    temp.SetActive(true);
                }
                break;
        }
        for (int i = 0; i < lives; i++) {
            hearts[i].enabled = true;
        }
        for (int i = lives; i < 10; i++) {
            hearts[i].enabled = false;
        }
        bricks = GameObject.FindGameObjectsWithTag("Brick").Length;
    }

    // Update is called once per frame
    void Update() {

    }


    // these three functions are called to change live, score and coin
    public void updateLive(int change) {
        lives += change;
        for (int i = 0; i < lives; i++) {
            hearts[i].enabled = true;
        }
        for (int i = lives; i < 10; i++) {
            hearts[i].enabled = false;
        }
        if (lives <= 0) {
            lives = 0;
            GameOver();
        }
    }
    public void updateScore(int change) {
        score += change;
        scoretext.text = "Score: " + score;
    }

    public void updateCoin(int change) {
        coin += change;
        if (coin >= coin_to_live) {
            coin -= coin_to_live;
            updateLive(1);

        }
        cointext.text = "Coins: " + coin + "/" + coin_to_live;
    }


    public void spawn() {
        // spawn up to 10 extra normal bricks
        if (Info.Gamemode == 1 || Info.Gamemode == 3) {
            GameObject temp;
            Vector2 temp_vector;
            for (int i = 0; i < 10; i++) {
                temp_vector = new Vector2(Random.Range(-7, 7), Random.Range(-3, 4) * 0.5f);
                if (Physics2D.OverlapCircle(temp_vector, 0.4f) != null) continue;
                temp = Instantiate(brick, temp_vector, Quaternion.identity);
                temp.SetActive(true);
            }
        } else if (Info.Gamemode == 2) {
            GameObject temp;
            Vector2 temp_vector = new Vector2(Random.Range(-7, 7), Random.Range(-3, 4) * 0.5f);
            // in endless mode, always spawn a red brick
            int tries = 0;
            while (Physics2D.OverlapCircle(temp_vector, 0.4f) != null) {
                temp_vector = new Vector2(Random.Range(-7, 7), Random.Range(-3, 4) * 0.5f);
                tries++;
                if (tries >= 100) break; // safety measure
            }
            temp = Instantiate(spawnbrick, temp_vector, Quaternion.identity);
            temp.SetActive(true);
            for (int i = 0; i < 10; i++) {
                temp_vector = new Vector2(Random.Range(-7, 7), Random.Range(-3, 4) * 0.5f);
                if (Physics2D.OverlapCircle(temp_vector, 0.4f) != null) continue;
                // randomly decides type of brick to spawn
                if (Random.Range(0, 10) <= 8) temp = Instantiate(brick, 
                    temp_vector, Quaternion.identity);
                else if (Random.Range(0, 2) == 1) temp = Instantiate(coinbrick,
                    temp_vector, Quaternion.identity);
                else temp = Instantiate(scorebrick, temp_vector, Quaternion.identity);
                temp.SetActive(true);
            }
            

        }
        bricks = GameObject.FindGameObjectsWithTag("Brick").Length;
    }

    // change levels
    void GameOver() {
        current_level++;
        if (Info.Gamemode == 1 && current_level > 5) {
            // passed all levels
            gameover = true;
            screen.SetActive(true);
            // extra life gives extra point
            score += 200 * lives;
            if (score >= Info.classic[Info.diff]) {
                Info.classic[Info.diff] = score;
            }
            result.text = "Game Cleared!";
            final.text = "Final Score: " + score + '\n' +
                "High Score: " + Info.classic[Info.diff];
            return;
        }
        else if (Info.Gamemode == 2 && lives <= 0) {
            // have no life left in endless mode
            gameover = true;
            screen.SetActive(true);
            if (score >= Info.endless[Info.diff]) {
                Info.endless[Info.diff] = score;
            }
            result.text = "Game Over!";
            final.text = "Final Score: " + score + '\n' +
                "High Score: " + Info.endless[Info.diff];
            return;
        }
        else if (Info.Gamemode == 1 && lives <= 0) {
            // have no life left and in classic mode
            gameover = true;
            screen.SetActive(true);
            if (score >= Info.classic[Info.diff]) {
                Info.classic[Info.diff] = score;
            }
            result.text = "Game Over!";
            final.text = "Final Score: " + score + '\n' +
                "High Score: " + Info.classic[Info.diff];
            return;
        }
        else if (Info.Gamemode == 3 && lives > 0) {
            // victory in random mode
            gameover = true;
            screen.SetActive(true);
            // extra life gives extra point
            score += 200 * lives;
            result.text = "Game Cleared!";
            final.text = "Final Score: " + score;
            return;
        }
        else if (Info.Gamemode == 3) {
            // no life left and in random mode
            gameover = true;
            screen.SetActive(true);
            result.text = "Game Over!";
            // no high score for random mode
            final.text = "Final Score: " + score;
            return;

        }
        // move on to next level
        switch(current_level) {
            case 2:
                L2.SetActive(true);
                break;
            case 3:
                L3.SetActive(true);
                break;
            case 4:
                L4.SetActive(true);
                break;
            case 5:
                L5.SetActive(true);
                break;
        }
        ball.GetComponent<Ball>().not_in_play();
        bricks = GameObject.FindGameObjectsWithTag("Brick").Length;
    }

    public void reset() {
        SceneManager.LoadScene("Play");
    }

    public void quit() {
        Application.Quit();
    }

    public void brickHit() {
        bricks -= 1;
        if (bricks <= 0) GameOver();
    }

    public void menu() {
        SceneManager.LoadScene("Menu");
    }
}
