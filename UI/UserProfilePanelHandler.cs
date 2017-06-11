using Assets.Scripts.Network;
using Assets.Scripts.Network.DataAccess;
using Assets.Scripts.Office;
using Assets.Scripts.UI.Profile;
using ModestTree;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Scripts.UI.UserProfile
{
    public class UserProfilePanelHandler : ProfilePanel
    {
        [Inject] private IOfficeControl officeControl;
        [Inject] private IUserDataAccess userDataAccess;
        [Inject] private Networking networking;

        [Header("Stats")] [SerializeField] private Image avatarImage;
        [SerializeField] private Text levelText, prestigeText, fullNameText, nicknameText;

        [Header("Votings List")] [SerializeField] private GameObject pastVotingPrefab;
        [SerializeField] private Transform listHolder;

        public override void Start()
        {
            base.Start();
            Init();
        }

        public override void Init()
        {
            userDataAccess.GetUserData(data =>
            {
                levelText.text = data.UserLevel.ToString();
                prestigeText.text = data.UserPrestige.ToString();
                fullNameText.text = string.Format("{0} {1}", data.first_name, data.last_name);
                nicknameText.text = data.nickname;

                if (data.user_photo == null || data.user_photo.IsEmpty()) return;
                networking.GetSprite(data.user_photo, sprite => avatarImage.sprite = sprite);
            });
            
            TestData();
        }

        public override void Reset()
        {
        }

        private void TestData()
        {
            AddVoting("Team1", "Team2", NetworkConstants.VotingTypes.STAGE_ONE, 314);
            AddVoting("Team1", "Team2", NetworkConstants.VotingTypes.STAGE_TWO, -314);
            AddVoting("Team1", "Team2", NetworkConstants.VotingTypes.STAGE_TWO, 1231);
            AddVoting("Team1", "Team2", NetworkConstants.VotingTypes.STAGE_ONE, -553);
            AddVoting("Team2", "Team1", NetworkConstants.VotingTypes.STAGE_ONE, 5466);
            AddVoting("Team2", "Team1", NetworkConstants.VotingTypes.STAGE_TWO, -4654);
        }

        private void AddVoting(string team1Name, string team2Name, string votingType, int moneyValue)
        {
            var handler = Instantiate(pastVotingPrefab, listHolder).GetComponent<PastVotingItemHandler>();
            handler.Init(team1Name, team2Name, votingType, moneyValue);
        }
    }
}