using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(FollowCameraRotation))]
public class magicBarForPlayer : MonoBehaviour
{
    [SerializeField] bool isBillboarded = true;
    [SerializeField] bool shouldShowHealthNumbers = true;

    float finalValue;
    float animationSpeed = 0.1f;
    float leftoverAmount = 0f;

    // Caches
    public HealthSystemForDummies magicSystem;
    public Image image;
    Text text;
    FollowCameraRotation followCameraRotation;

    private void Start()
    {
        /*magicSystem = GetComponentInParent<HealthSystemForDummies>();*/
        /*image = GetComponentInChildren<Image>();*/
        text = GetComponentInChildren<Text>();
        followCameraRotation = GetComponent<FollowCameraRotation>();
        magicSystem.OnCurrentHealthChanged.AddListener(ChangeHealthFill);
    }

    void Update()
    {
        animationSpeed = magicSystem.AnimationDuration;

        if (!magicSystem.HasAnimationWhenHealthChanges)
        {
            image.fillAmount = magicSystem.CurrentHealthPercentage / 100;
        }

        text.text = $"{magicSystem.CurrentHealth}/{magicSystem.MaximumHealth}";

        text.enabled = shouldShowHealthNumbers;

        followCameraRotation.enabled = isBillboarded;
    }

    private void ChangeHealthFill(CurrentHealth currentHealth)
    {
        if (!magicSystem.HasAnimationWhenHealthChanges) return;

        StopAllCoroutines();
        StartCoroutine(ChangeFillAmount(currentHealth));
    }

    private IEnumerator ChangeFillAmount(CurrentHealth currentHealth)
    {
        finalValue = currentHealth.percentage / 100;

        float cacheLeftoverAmount = this.leftoverAmount;

        float timeElapsed = 0;

        while (timeElapsed < animationSpeed)
        {
            float leftoverAmount = Mathf.Lerp((currentHealth.previous / magicSystem.MaximumHealth) + cacheLeftoverAmount, finalValue, timeElapsed / animationSpeed);
            this.leftoverAmount = leftoverAmount - finalValue;
            image.fillAmount = leftoverAmount;
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        this.leftoverAmount = 0;
        image.fillAmount = finalValue;
    }
}