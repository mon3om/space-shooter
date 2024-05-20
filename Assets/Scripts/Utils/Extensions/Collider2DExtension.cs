using UnityEngine;

public static class Collider2DExtensions
{
    // Extension method for Collider2D to compare multiple tags
    public static bool CompareMultipleTags(this Collider2D collider, params string[] tags)
    {
        // Check if the collider is null
        if (collider == null)
        {
            Debug.LogError("The provided Collider2D is null.");
            return false;
        }

        // Iterate through the provided tags
        foreach (string tag in tags)
        {
            // Compare the tag of the collider with the current tag
            if (collider.CompareTag(tag))
            {
                return true; // If there's a match, return true
            }
        }

        // If no tags match, return false
        return false;
    }
}