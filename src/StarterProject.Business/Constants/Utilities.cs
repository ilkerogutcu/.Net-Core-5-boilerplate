using System;
using System.Collections.Generic;

namespace StarterProject.Business.Constants
{
    public static class Utilities
    {
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
    }
}