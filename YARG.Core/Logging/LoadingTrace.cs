using System.Runtime.CompilerServices;

namespace YARG.Core.Logging
{
    public static class LoadingTrace
    {
        public const long StepThresholdMilliseconds = 10;
        public const long OutlierThresholdMilliseconds = 25;

        public static bool IsSlow(long elapsedMilliseconds, long thresholdMilliseconds = StepThresholdMilliseconds)
        {
            return elapsedMilliseconds >= thresholdMilliseconds;
        }

        public static void LogIfSlow(long elapsedMilliseconds, string format, [CallerFilePath] string source = "",
            [CallerLineNumber] int line = -1, [CallerMemberName] string member = "")
        {
            LogIfSlow(elapsedMilliseconds, StepThresholdMilliseconds, format, source, line, member);
        }

        public static void LogIfSlow(long elapsedMilliseconds, long thresholdMilliseconds, string format,
            [CallerFilePath] string source = "", [CallerLineNumber] int line = -1,
            [CallerMemberName] string member = "")
        {
            if (IsSlow(elapsedMilliseconds, thresholdMilliseconds))
            {
                YargLogger.LogInfo(format, source: source, line: line, member: member);
            }
        }

        public static void LogIfSlow<T1>(long elapsedMilliseconds, string format, T1 item1,
            [CallerFilePath] string source = "", [CallerLineNumber] int line = -1,
            [CallerMemberName] string member = "")
        {
            LogIfSlow(elapsedMilliseconds, StepThresholdMilliseconds, format, item1, source, line, member);
        }

        public static void LogIfSlow<T1>(long elapsedMilliseconds, long thresholdMilliseconds, string format, T1 item1,
            [CallerFilePath] string source = "", [CallerLineNumber] int line = -1,
            [CallerMemberName] string member = "")
        {
            if (IsSlow(elapsedMilliseconds, thresholdMilliseconds))
            {
                YargLogger.LogFormatInfo(format, item1, source: source, line: line, member: member);
            }
        }

        public static void LogIfSlow<T1, T2>(long elapsedMilliseconds, string format, T1 item1, T2 item2,
            [CallerFilePath] string source = "", [CallerLineNumber] int line = -1,
            [CallerMemberName] string member = "")
        {
            LogIfSlow(elapsedMilliseconds, StepThresholdMilliseconds, format, item1, item2, source, line, member);
        }

        public static void LogIfSlow<T1, T2>(long elapsedMilliseconds, long thresholdMilliseconds, string format,
            T1 item1, T2 item2, [CallerFilePath] string source = "", [CallerLineNumber] int line = -1,
            [CallerMemberName] string member = "")
        {
            if (IsSlow(elapsedMilliseconds, thresholdMilliseconds))
            {
                YargLogger.LogFormatInfo(format, item1, item2, source: source, line: line, member: member);
            }
        }

        public static void LogIfSlow<T1, T2, T3>(long elapsedMilliseconds, string format, T1 item1, T2 item2,
            T3 item3, [CallerFilePath] string source = "", [CallerLineNumber] int line = -1,
            [CallerMemberName] string member = "")
        {
            LogIfSlow(elapsedMilliseconds, StepThresholdMilliseconds, format, item1, item2, item3, source, line, member);
        }

        public static void LogIfSlow<T1, T2, T3>(long elapsedMilliseconds, long thresholdMilliseconds, string format,
            T1 item1, T2 item2, T3 item3, [CallerFilePath] string source = "", [CallerLineNumber] int line = -1,
            [CallerMemberName] string member = "")
        {
            if (IsSlow(elapsedMilliseconds, thresholdMilliseconds))
            {
                YargLogger.LogFormatInfo(format, item1, item2, item3, source: source, line: line, member: member);
            }
        }

        public static void LogIfSlow<T1, T2, T3, T4>(long elapsedMilliseconds, string format, T1 item1, T2 item2,
            T3 item3, T4 item4, [CallerFilePath] string source = "", [CallerLineNumber] int line = -1,
            [CallerMemberName] string member = "")
        {
            LogIfSlow(elapsedMilliseconds, StepThresholdMilliseconds, format, item1, item2, item3, item4, source, line, member);
        }

        public static void LogIfSlow<T1, T2, T3, T4>(long elapsedMilliseconds, long thresholdMilliseconds, string format,
            T1 item1, T2 item2, T3 item3, T4 item4, [CallerFilePath] string source = "", [CallerLineNumber] int line = -1,
            [CallerMemberName] string member = "")
        {
            if (IsSlow(elapsedMilliseconds, thresholdMilliseconds))
            {
                YargLogger.LogFormatInfo(format, item1, item2, item3, item4, source: source, line: line, member: member);
            }
        }

        public static void LogIfSlow<T1, T2, T3, T4, T5>(long elapsedMilliseconds, string format, T1 item1, T2 item2,
            T3 item3, T4 item4, T5 item5, [CallerFilePath] string source = "", [CallerLineNumber] int line = -1,
            [CallerMemberName] string member = "")
        {
            LogIfSlow(elapsedMilliseconds, StepThresholdMilliseconds, format, item1, item2, item3, item4, item5, source, line, member);
        }

        public static void LogIfSlow<T1, T2, T3, T4, T5>(long elapsedMilliseconds, long thresholdMilliseconds, string format,
            T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, [CallerFilePath] string source = "", [CallerLineNumber] int line = -1,
            [CallerMemberName] string member = "")
        {
            if (IsSlow(elapsedMilliseconds, thresholdMilliseconds))
            {
                YargLogger.LogFormatInfo(format, item1, item2, item3, item4, item5, source: source, line: line, member: member);
            }
        }

        public static void LogIfSlow<T1, T2, T3, T4, T5, T6>(long elapsedMilliseconds, string format, T1 item1, T2 item2,
            T3 item3, T4 item4, T5 item5, T6 item6, [CallerFilePath] string source = "", [CallerLineNumber] int line = -1,
            [CallerMemberName] string member = "")
        {
            LogIfSlow(elapsedMilliseconds, StepThresholdMilliseconds, format, item1, item2, item3, item4, item5, item6, source, line, member);
        }

        public static void LogIfSlow<T1, T2, T3, T4, T5, T6>(long elapsedMilliseconds, long thresholdMilliseconds, string format,
            T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, [CallerFilePath] string source = "", [CallerLineNumber] int line = -1,
            [CallerMemberName] string member = "")
        {
            if (IsSlow(elapsedMilliseconds, thresholdMilliseconds))
            {
                YargLogger.LogFormatInfo(format, item1, item2, item3, item4, item5, item6, source: source, line: line, member: member);
            }
        }

        public static void LogIfSlow<T1, T2, T3, T4, T5, T6, T7>(long elapsedMilliseconds, string format, T1 item1, T2 item2,
            T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, [CallerFilePath] string source = "", [CallerLineNumber] int line = -1,
            [CallerMemberName] string member = "")
        {
            LogIfSlow(elapsedMilliseconds, StepThresholdMilliseconds, format, item1, item2, item3, item4, item5, item6, item7, source, line, member);
        }

        public static void LogIfSlow<T1, T2, T3, T4, T5, T6, T7>(long elapsedMilliseconds, long thresholdMilliseconds, string format,
            T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, [CallerFilePath] string source = "", [CallerLineNumber] int line = -1,
            [CallerMemberName] string member = "")
        {
            if (IsSlow(elapsedMilliseconds, thresholdMilliseconds))
            {
                YargLogger.LogFormatInfo(format, item1, item2, item3, item4, item5, item6, item7, source: source, line: line, member: member);
            }
        }

        public static void LogIfSlow<T1, T2, T3, T4, T5, T6, T7, T8>(long elapsedMilliseconds, string format, T1 item1, T2 item2,
            T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, [CallerFilePath] string source = "", [CallerLineNumber] int line = -1,
            [CallerMemberName] string member = "")
        {
            LogIfSlow(elapsedMilliseconds, StepThresholdMilliseconds, format, item1, item2, item3, item4, item5, item6, item7, item8, source, line, member);
        }

        public static void LogIfSlow<T1, T2, T3, T4, T5, T6, T7, T8>(long elapsedMilliseconds, long thresholdMilliseconds, string format,
            T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, [CallerFilePath] string source = "", [CallerLineNumber] int line = -1,
            [CallerMemberName] string member = "")
        {
            if (IsSlow(elapsedMilliseconds, thresholdMilliseconds))
            {
                YargLogger.LogFormatInfo(format, item1, item2, item3, item4, item5, item6, item7, item8, source: source, line: line, member: member);
            }
        }

        public static void LogIfSlow<T1, T2, T3, T4, T5, T6, T7, T8, T9>(long elapsedMilliseconds, string format, T1 item1, T2 item2,
            T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, [CallerFilePath] string source = "", [CallerLineNumber] int line = -1,
            [CallerMemberName] string member = "")
        {
            LogIfSlow(elapsedMilliseconds, StepThresholdMilliseconds, format, item1, item2, item3, item4, item5, item6, item7, item8, item9, source, line, member);
        }

        public static void LogIfSlow<T1, T2, T3, T4, T5, T6, T7, T8, T9>(long elapsedMilliseconds, long thresholdMilliseconds, string format,
            T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, [CallerFilePath] string source = "", [CallerLineNumber] int line = -1,
            [CallerMemberName] string member = "")
        {
            if (IsSlow(elapsedMilliseconds, thresholdMilliseconds))
            {
                YargLogger.LogFormatInfo(format, item1, item2, item3, item4, item5, item6, item7, item8, item9, source: source, line: line, member: member);
            }
        }

        public static void LogIfSlow<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(long elapsedMilliseconds, string format, T1 item1, T2 item2,
            T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10, [CallerFilePath] string source = "",
            [CallerLineNumber] int line = -1, [CallerMemberName] string member = "")
        {
            LogIfSlow(elapsedMilliseconds, StepThresholdMilliseconds, format, item1, item2, item3, item4, item5, item6, item7, item8, item9, item10, source, line, member);
        }

        public static void LogIfSlow<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(long elapsedMilliseconds, long thresholdMilliseconds, string format,
            T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10,
            [CallerFilePath] string source = "", [CallerLineNumber] int line = -1, [CallerMemberName] string member = "")
        {
            if (IsSlow(elapsedMilliseconds, thresholdMilliseconds))
            {
                YargLogger.LogFormatInfo(format, item1, item2, item3, item4, item5, item6, item7, item8, item9, item10,
                    source: source, line: line, member: member);
            }
        }
    }
}
