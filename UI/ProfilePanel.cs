using Assets.Scripts.Office;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Scripts.UI.Profile {
    public abstract class ProfilePanel : MonoBehaviour, IInitializablePanel {
        [Inject] private IOfficeControl officeControl;

        [SerializeField] private Button backButton;

        public virtual void Start() {
            backButton.onClick.AddListener(() => {
                Destroy(FindObjectOfType<ProfileNavigationHandler>().gameObject);
                if (officeControl != null) {
                    officeControl.ReturnToOffice();
                }
            });
        }
        
        public abstract void Init();

        public abstract void Reset();


    }
}