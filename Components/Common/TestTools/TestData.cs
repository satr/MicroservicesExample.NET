using System;

namespace TestTools
{
    public static class TestData
    {
        public static string GetRandomGuidString()
        {
            return Guid.NewGuid().ToString();
        }

        public static string GetRandomString()
        {
            return GetRandomGuidString();
        }
    }
}