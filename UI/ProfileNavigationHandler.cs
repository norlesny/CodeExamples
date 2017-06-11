using System;
using System.Collections;
using Assets.Scripts.Office;
using Assets.Scripts.UI.Profile;
using Assets.Scripts.UI.UserProfile;
using Assets.Scripts.UI.Wallet;
using UnityEngine;
using UnityEngine.UI;

public class ProfileNavigationHandler : MonoBehaviour {
    private static readonly Color CurrentPanelColor = new Color(61 / 255f, 115 / 255f, 128 / 255f);
    private static readonly Color NotCurrentPanelColor = new Color(42 / 255f, 75 / 255f, 86 / 255f);
    private const float PanelChangeTime = 0.5f;

    private enum Panel {
        Wallet,
        UserProfile,
        UserSettings,
    }

    [Header("UserSettings")] [SerializeField] private RectTransform userSettingsPanel;

    [SerializeField] private Button userSettingsButton;

    [Header("UserProfile")] [SerializeField] private RectTransform userProfilePanel;

    [SerializeField] private Button userProfileButton;

    [Header("Wallet")] [SerializeField] private RectTransform walletPanel;

    [SerializeField] private Button walletButton;

    private UserProfilePanelHandler userProfileHandler;
    private UserSettingsPanelHandler userSettingsHandler;
    private WalletPanelHandler walletHandler;

    private Image userSettingsImage, userProfileImage, walletImage;

    private Panel currentPanel;
    private float screenWidth;

    void Start() {
        GetButtonsImages();
        GetPanelsHandlers();
        UpdatePositions();
        AddButtonsListeners();
    }

    private void GetButtonsImages() {
        userProfileImage = userProfileButton.transform.GetChild(0).GetComponent<Image>();
        userSettingsImage = userSettingsButton.transform.GetChild(0).GetComponent<Image>();
        walletImage = walletButton.transform.GetChild(0).GetComponent<Image>();
    }

    private void GetPanelsHandlers() {
        userProfileHandler = userProfilePanel.GetComponent<UserProfilePanelHandler>();
        userSettingsHandler = userSettingsPanel.GetComponent<UserSettingsPanelHandler>();
        walletHandler = walletPanel.GetComponent<WalletPanelHandler>();
    }

    private void UpdatePositions() {
        var canvasScaler = GetComponentInParent<CanvasScaler>();
        if (canvasScaler == null) {
            Debug.LogError("No CanvasScaler found!");
            return;
        }
        screenWidth = canvasScaler.referenceResolution.x;

        userSettingsPanel.anchoredPosition = new Vector2(
            screenWidth, userSettingsPanel.anchoredPosition.y
        );
        userProfilePanel.anchoredPosition = new Vector2(
            screenWidth, userProfilePanel.anchoredPosition.y
        );
    }

    private void AddButtonsListeners() {
        userProfileButton.onClick.AddListener(() => ChangeCurrentPanel(Panel.UserProfile));
        userSettingsButton.onClick.AddListener(() => ChangeCurrentPanel(Panel.UserSettings));
        walletButton.onClick.AddListener(() => ChangeCurrentPanel(Panel.Wallet));
    }

    private void ChangeCurrentPanel(Panel panel) {
        if (panel == currentPanel) return;

        StartCoroutine(MovePanel(currentPanel, false));
        StartCoroutine(MovePanel(panel, true));

        currentPanel = panel;
    }

    private IEnumerator MovePanel(Panel panel, bool moveIn) {
        var rect = GetPanelsRect(panel);
        var panelHandler = GetPanelHandler(panel);
        var panelButtonImage = GetPanelButtonImage(panel);

        if (moveIn) {
            panelHandler.Reset();
        }

        var wait = new WaitForEndOfFrame();
        var progress = 0f;
        var newPosition = rect.anchoredPosition;
        while (progress < 1f) {
            progress += Time.deltaTime / PanelChangeTime;
            var easeProgress = Easing.easeOutExpo2(progress);
            if (moveIn) {
                newPosition.x = (easeProgress - 1) * screenWidth;
                panelButtonImage.color = Color.Lerp(NotCurrentPanelColor, CurrentPanelColor, easeProgress);
            }
            else {
                newPosition.x = easeProgress * screenWidth;
                panelButtonImage.color = Color.Lerp(CurrentPanelColor, NotCurrentPanelColor, easeProgress);
            }
            rect.anchoredPosition = newPosition;
            yield return wait;
        }

        newPosition.x = moveIn ? 0 : screenWidth;
        rect.anchoredPosition = newPosition;

        if (moveIn) {
            panelHandler.Init();
        }
    }

    private RectTransform GetPanelsRect(Panel panel) {
        switch (panel) {
            case Panel.UserProfile:
                return userProfilePanel;
            case Panel.UserSettings:
                return userSettingsPanel;
            case Panel.Wallet:
                return walletPanel;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private IInitializablePanel GetPanelHandler(Panel panel) {
        switch (panel) {
            case Panel.UserProfile:
                return userProfileHandler;
            case Panel.UserSettings:
                return userSettingsHandler;
            case Panel.Wallet:
                return walletHandler;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private Image GetPanelButtonImage(Panel panel) {
        switch (panel) {
            case Panel.UserProfile:
                return userProfileImage;
            case Panel.UserSettings:
                return userSettingsImage;
            case Panel.Wallet:
                return walletImage;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}