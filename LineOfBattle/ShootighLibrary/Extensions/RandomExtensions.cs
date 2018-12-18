using System;

namespace ShootighLibrary.Extensions
{
    public static class RandomExtensions
    {
        public static float NextFloat( this Random rand )
            => (float)rand.NextDouble();
    }
}
