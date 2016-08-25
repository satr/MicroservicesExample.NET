using System;
using log4net;

namespace ServiceCommon
{
    public static class LogExtensions
    {
        public static string InfoForTicket(this ILog logger, object tiket, string format, params object[] args)
        {
            return GetMessageForTicket(logger.Info, tiket, format, args);
        }

        public static string DebugForTicket(this ILog logger, object tiket, string format, params object[] args)
        {
            return GetMessageForTicket(logger.Debug, tiket, format, args);
        }

        public static string WarnForTicket(this ILog logger, object tiket, string format, params object[] args)
        {
            return GetMessageForTicket(logger.Warn, tiket, format, args);
        }

        public static string ErrorForTicket(this ILog logger, object tiket, string format, params object[] args)
        {
            return GetMessageForTicket(logger.Error, tiket, format, args);
        }

        private static string GetMessageForTicket(Action<object> logAction,  object tiket, string format, object[] args)
        {
            var message = $"[Tiket:{tiket}]{string.Format(format, args)}";
            logAction(message);
            return message;
        }
    }
}