using Microsoft.EntityFrameworkCore;
using Spinfluence.Data;
using Spinfluence.Models;

namespace Spinfluence.Services
{
    public class PracticeService : IPracticeService
    {
        readonly SpinContext spinContext;

        public PracticeService(SpinContext spinContext)
        { this.spinContext = spinContext; }

        public async Task<Practice[]> GetPracticesAsync(string token)
        {
            var list = new List<Practice>();

            Account? account = await spinContext.Account.
                FirstOrDefaultAsync(a => a.token.CompareTo(token) == 0);

            if(account != null)
            {
                List<Practice>? practices = await spinContext.Practice.Where(p => p.AccountId == account.Id && !p.IsCanceled)
                    .ToListAsync();

                if(practices != null) 
                    list.AddRange(practices);
            }

            return list.ToArray();
        }

        public async Task<bool?> AddPracticeAsync(PracticeModel practiceModel, string token)
        {
            Account? account = await spinContext.Account.
                FirstOrDefaultAsync(a => a.token.CompareTo(token) == 0);

            if(account != null)
            {
                Practice practice = new Practice
                {
                    Body = practiceModel.Body,
                    IsCanceled = false,
                    CompanyEventId = practiceModel.CompanyEventId,
                    AccountId = account.Id
                };

                spinContext.Practice.Add(practice);
            }

            return null;
        }

        public async Task<int> CancelPracticeEventAsync(int practiceId, string token)
        {
            Account? account = await spinContext.Account.
                FirstOrDefaultAsync(a => a.token.CompareTo(token) == 0);

            if(account != null)
            {
                Practice? practice = await spinContext.Practice
                    .FirstOrDefaultAsync(p => p.Id == practiceId && p.AccountId == account.Id);

                if(practice != null)
                {
                    var companyEvent = await spinContext.CompanyEvent
                        .FirstOrDefaultAsync(e => e.Id == practice.CompanyEventId);

                    var now = DateTime.UtcNow;

                    if (companyEvent != null)
                    {
                        if (now > companyEvent.EndDate)
                            return 0;

                        practice.IsCanceled = true;
                        await spinContext.SaveChangesAsync();
                        return 1;
                    }

                    return 2;
                }
            }

            return -1;
        }
    }
}
