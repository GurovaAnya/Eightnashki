using System;

namespace Eightnashki
{
    public enum Path
    {
        Up,
        Down,
        Right,
        Left
    }

    public static class PathExtensions
    {
        public static Path GetOppositePath(this Path path)
        {
            return path switch
            {
                Path.Up => Path.Down,
                Path.Down => Path.Up,
                Path.Right => Path.Left,
                Path.Left => Path.Right,
                _ => throw new ArgumentOutOfRangeException(nameof(path), path, null)
            };
        }
    } 
}