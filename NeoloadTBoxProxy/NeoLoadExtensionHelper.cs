﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Tricentis.Automation.Contract;
using Tricentis.Automation.Engines.Technicals.Html;
using Tricentis.Automation.Execution.Context;

namespace NeoLoadAddOn
{
    public static class NeoLoadExtensionHelper
    {
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int GetWindowTextLength(IntPtr hwnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        public static string GetForeGroundWindowCaption() {
            var win = GetForegroundWindow();
            if (win == IntPtr.Zero) return string.Empty;

            var length = GetWindowTextLength(win);
            if (length == 0) return string.Empty;

            var buffer = new StringBuilder(length + 1);
            GetWindowText(win, buffer, buffer.Capacity);
            return buffer.ToString();
        }

        /// <summary>
        /// Creates a List of strings of the currently executed AbstractAuomationObject and its parent up to the TestCase level
        /// Including all folder levels
        /// </summary>
        public static List<string> CreatePath()
        {
            List<string> path = new List<string>();
            RunContext context = RunContext.Current;

            while (context != null)
            {
                if (context.ExecutedItem is AbstractAutomationObject ao) // TestStep, Folder
                {
                    path.Add(ao.Name);
                }

                context = context.Parent;
            }

            path.Reverse();

            return path;
        }

        /// <summary>
        /// Gets the browsers performance timing information
        /// </summary>
        public static Dictionary<string, long> GetBrowserPerformanceTimings(IHtmlEntryPointTechnical htmlEntryPoint)
        {
            long redirectStart = 0, fetchStart = 0, responseStart = 0, domContentLoadedEventStart = 0, domLoadEventStart = 0, domLoadEventEnd = 0;
            Dictionary<string, long> performanceDictionary = new Dictionary<string, long>();

            bool jsSuccess =
                long.TryParse(htmlEntryPoint.GetJavaScriptResult("return window.performance.timing.redirectStart"), out redirectStart) &&
                long.TryParse(htmlEntryPoint.GetJavaScriptResult("return window.performance.timing.fetchStart"), out fetchStart) &&
                long.TryParse(htmlEntryPoint.GetJavaScriptResult("return window.performance.timing.responseStart"), out responseStart) &&
                long.TryParse(htmlEntryPoint.GetJavaScriptResult("return window.performance.timing.domContentLoadedEventStart"), out domContentLoadedEventStart) &&
                long.TryParse(htmlEntryPoint.GetJavaScriptResult("return window.performance.timing.loadEventStart"), out domLoadEventStart) &&
                long.TryParse(htmlEntryPoint.GetJavaScriptResult("return window.performance.timing.loadEventEnd"), out domLoadEventEnd);

            if (!jsSuccess) return performanceDictionary;

            long start = redirectStart == 0 ? fetchStart : redirectStart;

            long timeToFirstByte = responseStart - start;
            long domContentLoaded = domContentLoadedEventStart - start;
            long onLoad = domLoadEventStart - start;
            long documentComplete = domLoadEventEnd - start;

            if (timeToFirstByte > 0)
                performanceDictionary.Add("Time To First Byte", timeToFirstByte);
            if (domContentLoaded > 0)
                performanceDictionary.Add("DOM Content Loaded", domContentLoaded);
            if (onLoad > 0)
                performanceDictionary.Add("On Load", onLoad);
            if (documentComplete > 0)
                performanceDictionary.Add("Document Complete", documentComplete);

            if (documentComplete > 0)
                performanceDictionary.Add("timestamp", domLoadEventEnd);

            return performanceDictionary;

        }
    }
}
