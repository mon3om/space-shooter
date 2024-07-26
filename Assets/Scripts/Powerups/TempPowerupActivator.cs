using UnityEngine;

public class TempPowerupActivator : MonoBehaviour
{
    private bool isActive = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Tags.PLAYER_SHIP))
        {
            isActive = !isActive;

            if (isActive)
            {
                GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                GetComponent<PowerupBase>().Activate();
            }
            else
            {
                GetComponent<PowerupBase>().Deactivate();
                GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 8f / 255f);
            }
        }
    }
}
