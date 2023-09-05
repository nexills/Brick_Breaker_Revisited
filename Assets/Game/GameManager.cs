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
    // integers storing current gameplay values
    public int lives;
    public int score;
    public int coin;
    public int coin_to_live; // # of coins to exchange for 1 life
    int bricks;
    int current_level = 1;
    // text displayed on screen
    public TMP_Text scoretext;
    public TMP_Text cointext;
    public TMP_Text final;
    public TMP_Text result;
    public bool gameover;
    public GameObject screen;
    // references to types of brick (for spawning purposes)
    public GameObject brick; // reference to a brick
    public GameObject spawnbrick; // reference to a spawn brick
    public GameObject coinbrick;
    public GameObject scorebrick;

    public GameObject top_edge;
    public GameObject top_trigger;
    public GameObject top_paddle;

    // storing the pre-set bricks for different levels
    public GameObject ball;
    public GameObject Endless;
    public GameObject L1;
    public GameObject L2;
    public GameObject L3;
    public GameObject L4;
    public GameObject L5;

    // the number of lives displayed graphically
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
                // spawn bricks in this mode
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
        if (Info.double_paddle) {
            // play with double_paddle
            top_edge.SetActive(false);
            top_trigger.SetActive(true);
            top_paddle.SetActive(true);
        } else {
            // play with single_paddle
            top_edge.SetActive(true);
            top_trigger.SetActive(false);
            top_paddle.SetActive(false);

        }
        // set the number of lives graphically
        for (int i = 0; i < lives; i++) {
            hearts[i].enabled = true;
        }
        for (int i = lives; i < 10; i++) {
            hearts[i].enabled = false;
        }
        bricks = GameObject.FindGameObjectsWithTag("Brick").Length;
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
        if (Info.Gamemode != 2) {
            GameObject temp;
            Vector2 temp_vector;
            for (int i = 0; i < 10; i++) {
                temp_vector = new Vector2(Random.Range(-7, 7), Random.Range(-3, 4) * 0.5f);
                if (Physics2D.OverlapCircle(temp_vector, 0.4f) != null) continue;
                temp = Instantiate(brick, temp_vector, Quaternion.identity);
                temp.SetActive(true);
            }
        } else {
            // in endless mode, spawn all types of bricks
            GameObject temp;
            GameObject brick_ref;
            Vector2 temp_vector;
            // in endless mode, always spawn a red brick
            int tries = 0;
            do {
                temp_vector = new Vector2(Random.Range(-7, 7), Random.Range(-3, 4) * 0.5f);
                tries++;
                if (tries >= 100) break; // safety measure if screen is almost full
            } while (Physics2D.OverlapCircle(temp_vector, 0.4f) != null);
            temp = Instantiate(spawnbrick, temp_vector, Quaternion.identity);
            temp.SetActive(true);
            // spawn different types of bricks
            for (int i = 0; i < 10; i++) {
                temp_vector = new Vector2(Random.Range(-7, 7), Random.Range(-3, 4) * 0.5f);
                if (Physics2D.OverlapCircle(temp_vector, 0.4f) != null) continue;
                int random_int = Random.Range(0, 8);
                switch (random_int) {
                    case 0:
                        brick_ref = coinbrick;
                        break;
                    case 1:
                        brick_ref = scorebrick;
                        break;
                    default:
                        brick_ref = brick;
                        break;
                }
                temp = Instantiate(brick_ref, temp_vector, Quaternion.identity);
                temp.SetActive(true);
            }
        }
        bricks = GameObject.FindGameObjectsWithTag("Brick").Length;
    }

    void GameOver() {
        current_level++;
        // the player loses the game
        gameover = true;
        screen.SetActive(true);
        result.text = "Game Over!";
        switch (Info.Gamemode) {
            case 1:
                // have no life left and in classic mode
                if (Info.double_paddle) {
                    if (score >= Info.classic[Info.diff]) {
                        Info.classic[Info.diff] = score;
                    }
                    final.text = "Final Score: " + score + '\n' +
                        "High Score: " + Info.classic[Info.diff];
                    return;
                } else {
                    if (score >= Info.classic_single[Info.diff]) {
                        Info.classic_single[Info.diff] = score;
                    }
                    final.text = "Final Score: " + score + '\n' +
                        "High Score: " + Info.classic_single[Info.diff];
                    return;

                }
            case 2:
                if (Info.double_paddle) {
                    // have no life left in endless mode
                    if (score >= Info.endless[Info.diff]) {
                        Info.endless[Info.diff] = score;
                    }
                    final.text = "Final Score: " + score + '\n' +
                        "High Score: " + Info.endless[Info.diff];
                    return;
                } else {
                    // have no life left in endless mode
                    if (score >= Info.endless_single[Info.diff]) {
                        Info.endless_single[Info.diff] = score;
                    }
                    final.text = "Final Score: " + score + '\n' +
                        "High Score: " + Info.endless_single[Info.diff];
                    return;

                }
            case 3:
                // no life left and in random mode
                // no high score for random mode
                final.text = "Final Score: " + score;
                return;
        }
    }

    void GameClear() {
        switch(Info.Gamemode) {
            case 1:
                // ie. clearing a level
                switch (current_level) {
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
                    case 6:
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
                ball.GetComponent<Ball>().not_in_play();
                bricks = GameObject.FindGameObjectsWithTag("Brick").Length;
                return;
            case 2:
                // shouldn't happen normally
                if (score >= Info.endless[Info.diff]) {
                    Info.endless[Info.diff] = score;
                }
                final.text = "Final Score: " + score + '\n' +
                    "High Score: " + Info.endless[Info.diff];
                gameover = true;
                screen.SetActive(true);
                result.text = "Game Cleared... but how?";
                return;
            case 3:
                // victory in random mode
                gameover = true;
                screen.SetActive(true);
                // extra life gives extra point
                score += 200 * lives;
                result.text = "Game Cleared!";
                final.text = "Final Score: " + score;
                return;
        }
    }

    // public functions for calling
    public void reset() {
        SceneManager.LoadScene("Play");
    }

    public void quit() {
        Application.Quit();
    }

    public void brickHit() {
        bricks -= 1;
        if (bricks <= 0) GameClear();
    }

    public void menu() {
        SceneManager.LoadScene("Menu");
    }
}
