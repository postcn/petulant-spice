using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GuiManager : MonoBehaviour {
	public const string format = "Health: {0}\r\nAmmo: {2}\r\nSamples: {1}";
    public const string GAME_OVER = "Game Over!";
    public const string VICTORY = "Victory!";
    public const string DROP_SHIP = "The drop ship will arive in {0}.";
    public const string DALE_AVAILABLE = "Dale will once again be available in {0}.";
    public const string EMPTY = "";
    public Canvas canvas;
    public Text heroStatus;
    public Text largeNotification;
    public Text smallNotification;
    public Text topHeroStatus;
    public Text topSmallNotification;
    public AudioClip gameOver;
    public AudioClip gameVictory;
    private bool played = false;
    public Button ReturnButton;
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey("escape")) {
            Application.Quit();
        }
        //If Both are alive, then set the top text to controller and bottom text to the mouse player
        if (Hero_Management.mousePlayer != null) {
            setTopText(Hero_Management.controllerPlayer);
            setBottomText(Hero_Management.mousePlayer);
        }
        else {
            setTopText(Hero_Management.mousePlayer);
            setBottomText(Hero_Management.controllerPlayer);
        }

        //If only one is alive, then set the bottom text to that player.
            
        ReturnButton.gameObject.SetActive(false);

		if (Hero_Management.mousePlayer == null && Hero_Management.controllerPlayer == null && !DropZoneManager.self.picked_up) {
            largeNotification.text = GAME_OVER;
            ReturnButton.gameObject.SetActive(true);
            if (!played) {
                AudioSource.PlayClipAtPoint(gameOver, Camera.main.transform.position);
                played = true;
            }
		} else if (Hero_Management.mousePlayer == null && Hero_Management.controllerPlayer == null && DropZoneManager.self.picked_up) {
            largeNotification.text = VICTORY;
            largeNotification.color = Color.green;
            ReturnButton.gameObject.SetActive(true);
            if (!played) {
                AudioSource.PlayClipAtPoint(gameVictory, Camera.main.transform.position);
                played = true;
            }
        }

        setNotificationText();
	}

    private void setTopText(Hero_Management hero) {
        if (hero == null) {
            topHeroStatus.text = EMPTY;
            return;
        }
        topHeroStatus.text = string.Format(format, hero.getHealth(), Hero_Management.getSamplesCollected(), hero.getCurrentAmmunition());
    }

    private void setBottomText(Hero_Management hero) {
        if (hero == null) {
            return;
        }
        heroStatus.text = string.Format(format, hero.getHealth(), Hero_Management.getSamplesCollected(), hero.getCurrentAmmunition());
    }

    private void setNotificationText() {
        string text = EMPTY;
        if (DropZoneManager.self.delayTime > 0) {
            text += string.Format(DROP_SHIP, Constants.formatSecondsToMinute(DropZoneManager.self.delayTime));
        }
        if (DaleManagement.self.delayTime > 0) {
            if (text.Length > 0) {
                text += "\r\n";
            }
            text += string.Format(DALE_AVAILABLE, Constants.formatSecondsToMinute(DaleManagement.self.delayTime));
        }

        if (Hero_Management.controllerPlayer != null && Hero_Management.mousePlayer != null) {
            smallNotification.text = text;
            topSmallNotification.text = text;
        }
        else if (Hero_Management.controllerPlayer != null || Hero_Management.mousePlayer != null) {
            smallNotification.text = text;
            topSmallNotification.text = EMPTY;
        }
        else {
            smallNotification.text = EMPTY;
            topSmallNotification.text = EMPTY;
        }
    }
}
