namespace LockerLib.Helpers.PathHelpers;

/// <summary>
/// Provides methods for working with paths.
/// </summary>
public interface IPathHelper
{
    /// <summary>
    /// Gets the path separator character.
    /// </summary>
    public char GetPathSeparator { get; }

    /// <summary>
    /// Splits the specified path into its components.
    /// </summary>
    /// <param name="path">The path to split.</param>
    /// <returns>An array containing the components of the path.</returns>
    string[] SplitPath(string path);

    /// <summary>
    /// Gets the last node from the specified path.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <returns>The last node from the path.</returns>
    string GetNodeFromPath(string path);
    
    /// <summary>
    /// Validates the specified path.
    /// </summary>
    /// <param name="path">The path to validate.</param>
    /// <returns>The validated path.</returns>
    public string ValidatePath(string path);
    
    /// <summary>
    /// Extracts the suffix for sorting nodes based on the lock name.
    /// </summary>
    /// <param name="str">The string to extract the suffix from.</param>
    /// <param name="lockName">The lock name.</param>
    /// <returns>The suffix for sorting nodes.</returns>
    public string ExtractSuffixForNodesSorting(string str, string lockName);

    /// <summary>
    /// Determines whether the specified path is valid.
    /// </summary>
    /// <param name="path">The path to validate.</param>
    /// <returns><see langword="true"/> if the path is valid; otherwise, <see langword="false"/>.</returns>
    public bool IsPathValid(string path);

    /// <summary>
    /// Constructs a path by combining the specified parent and child paths.
    /// </summary>
    /// <param name="parent">The parent path.</param>
    /// <param name="child">The child path.</param>
    /// <returns>The combined path.</returns>
    public string ConstructPath(string parent, string child);
}