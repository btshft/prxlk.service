using System;

namespace Prxlk.Gateway.Features.Throttling.Models
{
    public class ThrottlePolicyEvaluationResult
    {
        public static ThrottlePolicyEvaluationResult Bypass { get; }
            = new ThrottlePolicyEvaluationResult();

        private ThrottlePolicyEvaluationResult()
        {
            ShouldBypass = true;
        }
            
        public ThrottlePolicyEvaluationResult(DateTime reset, int remaining, TimeSpan limit)
        {
            Reset = reset;
            Remaining = remaining;
            Limit = limit;
            ShouldBypass = false;
        }

        public bool ShouldBypass { get; }
        public DateTime Reset { get; }
        public int Remaining { get; }
        public TimeSpan Limit { get; }
    }
}