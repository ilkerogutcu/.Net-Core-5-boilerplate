using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Business.Constants
{
    /// <summary>
    /// Useful functions
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        /// Getting dates between 2 dates
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static IEnumerable<DateTime> GetDateRange(DateTime startDate, DateTime endDate)
        {
            if (endDate < startDate)
            {
                throw new ArgumentException(Messages.GetDateRangeError);
            }

            while (startDate <= endDate)
            {
                yield return startDate;
                startDate = startDate.AddDays(1);
            }
        }
        /// <summary>
        /// Get ip address
        /// </summary>
        /// <returns></returns>
        public static string GetIpAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return string.Empty;
        }
    }
}