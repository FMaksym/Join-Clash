using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private Image fadeImage;
    [SerializeField] private Image handImage;
    [SerializeField] private TMP_Text textInstruction;
    public PlayerManager PlayerManager { get; private set; }

    void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        PlayerManager = FindObjectOfType<PlayerManager>();
    }

    private void Start()
    {
        fadeImage.DOFade(0f, 1f).SetAutoKill();
        handImage.transform.DOLocalMoveX(300f, 1f).SetLoops(10000, LoopType.Yoyo).SetEase(Ease.InOutSine);
        textInstruction.transform.DOScale(1.2f, 0.8f).SetLoops(10000, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }

    public void OnClickPlay()
    {
        menuPanel.SetActive(false);
        PlayerManager.IsGame = true;
    }
}
