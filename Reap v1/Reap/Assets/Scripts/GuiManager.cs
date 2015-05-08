using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GuiManager : MonoBehaviour {
    public const string format = "Health: {0}\r\nSamples: {1}";
    public const string GAME_OVER = "Game Over!";
    public const string VICTORY = "Victory!";
    public const string DROP_SHIP = "The drop ship will arive in {0}.";
    public const string DALE_AVAILABLE = "Dale will once again be available in {0}.";
    public const string EMPTY = "";
    public Canvas canvas;
    public Text heroStatus;
    public Text largeNotification;
    public Text smallNotification;
    public AudioClip gameOver;
    public AudioClip gameVictory;
    private bool played = false;
	
	// Update is called once per frame
	void Update () {
        if (Hero_Management.self != null && heroStatus != null) {
            heroStatus.text = string.Format(format, Hero_Management.self.getHealth(), Hero_Management.self.getSamplesCollected());
        }

        if (Hero_Management.self == null && !DropZoneManager.self.picked_up) {
            largeNotification.text = GAME_OVER;
            if (!played) {
                AudioSource.PlayClipAtPoint(gameOver, Camera.main.transform.position);
                played = true;
            }
        } else if (Hero_Management.self == null && DropZoneManager.self.picked_up) {
            largeNotification.text = VICTORY;
            largeNotification.color = Color.green;
            if (!played) {
                AudioSource.PlayClipAtPoint(gameVictory, Camera.main.transform.position);
                played = true;
            }
        }

        if (Hero_Management.self != null && DropZoneManager.self.delayTime > 0) {
            smallNotification.text = string.Format(DROP_SHIP, Constants.formatSecondsToMinute(DropZoneManager.self.delayTime));
        } else if (Hero_Management.self != null && DaleManagement.self.delayTime > 0) {
            smallNotification.text = string.Format(DALE_AVAILABLE, Constants.formatSecondsToMinute(DaleManagement.self.delayTime));
        } else {
            smallNotification.text = EMPTY;
        }
	}
}
