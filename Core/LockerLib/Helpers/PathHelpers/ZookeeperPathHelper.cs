using System.Text;

namespace LockerLib.Helpers.PathHelpers;

/// <summary>
/// Provides functionality to interact with a ZooKeeper instance for locking purposes.
/// </summary>
public class ZookeeperPathHelper : IPathHelper
{
    private const char PathSeparator = '/';

    /// <inheritdoc cref="IPathHelper.GetPathSeparator"/>>
    public char GetPathSeparator => PathSeparator;

    /// <inheritdoc cref="IPathHelper.SplitPath"/>>
    public string[] SplitPath(string path)
    {
        return path.Split(PathSeparator, StringSplitOptions.RemoveEmptyEntries);
    }

    /// <inheritdoc cref="IPathHelper.GetNodeFromPath"/>>
    public string GetNodeFromPath(string path)
    {
        path = ValidatePath(path);
        var lastSeparatorIndex = path.LastIndexOf(PathSeparator);
        if (lastSeparatorIndex < 0)
            return path;
        return lastSeparatorIndex + 1 >= path.Length ? "" : path.Substring(lastSeparatorIndex + 1);
    }

    /// <inheritdoc cref="IPathHelper.ValidatePath"/>>
    public string ValidatePath(string path)
    {
        if (!IsPathValid(path))
            throw new ArgumentException(nameof(path));

        return path;
    }

    /// <inheritdoc cref="IPathHelper.ExtractSuffixForNodesSorting"/>>
    public string ExtractSuffixForNodesSorting(string str, string lockName)
    {
        var index = str.LastIndexOf(lockName, StringComparison.Ordinal);
        if (index < 0) return str;
        index += lockName.Length;

        return index <= str.Length ? str.Substring(index) : "";
    }

    /// <inheritdoc cref="IPathHelper.IsPathValid"/>>
    public bool IsPathValid(string path)
    {
        ArgumentNullException.ThrowIfNull(path);
        if (path.Length == 0)
            return false;
        if (path[0] != PathSeparator)
            return false;
        if (path.Length == 1)
            return true;
        if (path[^1] == PathSeparator)
            return false;

        for (var i = 1; i < path.Length; ++i)
        {
            var symbol = path[i];
            switch (symbol)
            {
                case '\0':
                    return false;
                case PathSeparator when path[i - 1] == PathSeparator:
                    return false;
                case '.' when path[i - (path[i - 1] == '.' ? 2 : 1)] == PathSeparator
                              && (i + 1 == path.Length || path[i + 1] == PathSeparator):
                    return false;
            }

            if (symbol is > '\u0000' and < '\u001f' or > '\u007f' and < '\u009F'
                or > '\ud800' and < '\uf8ff' or > '\ufff0' and < '\uffff')
                return false;
        }

        return true;
    }

    /// <inheritdoc cref="IPathHelper.ConstructPath"/>>
    public string ConstructPath(string parent, string child)
    {
        var pathBuilder = new StringBuilder();

        if (!string.IsNullOrEmpty(parent))
            pathBuilder.Append(ConstructParentPath(parent));

        if (!string.IsNullOrEmpty(child))
            pathBuilder.Append(ConstructChildPath(child));

        if (pathBuilder.Length == 0)
            pathBuilder.Append(PathSeparator);

        return pathBuilder.ToString();
    }

    private static StringBuilder ConstructParentPath(string parent)
    {
        var pathBuilder = new StringBuilder();
        if (parent[0] != PathSeparator)
            pathBuilder.Append(PathSeparator);

        pathBuilder.Append(parent);

        if (parent[^1] == PathSeparator)
            pathBuilder.Length--;

        return pathBuilder;
    }

    private static StringBuilder ConstructChildPath(string child)
    {
        var pathBuilder = new StringBuilder();
        pathBuilder.Append(PathSeparator);
        var startIndex = child[0] == PathSeparator ? 1 : 0;
        var endIndex = child[^1] == PathSeparator ? child.Length - 1 : child.Length;
        pathBuilder.Append(child, startIndex, endIndex - startIndex + 1);

        return pathBuilder;
    }
}