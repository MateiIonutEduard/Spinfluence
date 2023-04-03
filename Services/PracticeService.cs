using System.Linq;
using Spinfluence.Data;
using Spinfluence.Models;
using Microsoft.EntityFrameworkCore;

namespace Spinfluence.Services
{
    public class PracticeService : IPracticeService
    {
        readonly SpinContext spinContext;

        public PracticeService(SpinContext spinContext)
        { this.spinContext = spinContext; }

        public async Task<PracticeEventModel[]> GetPracticesAsync(string token)
        {
            var list = new List<PracticeEventModel>();

            Account? account = await spinContext.Account.
                FirstOrDefaultAsync(a => a.token.CompareTo(token) == 0);

            if(account != null)
            {
                List<Practice>? practices = await spinContext.Practice.Where(p => p.AccountId == account.Id && !p.IsCanceled)
                    .ToListAsync();

                PracticeEventModel[] events = (
                    from p in practices join e in await spinContext.CompanyEvent.ToListAsync() on p.CompanyEventId equals e.Id join c in await spinContext.Company.ToListAsync()
                    on e.CompanyId equals c.Id
                    select new PracticeEventModel
                    {
                        Name = e.Name,
                        PracticeId = p.Id,
                        BeginDate = e.BeginDate,
                        CompanyName = c.Name,
                        EndDate = e.EndDate,
                        Body = p.Body
                    }).ToArray();

                if(practices != null) 
                    list.AddRange(events);
            }

            return list.ToArray();
        }

        public async Task<bool> AddPracticeAsync(PracticeModel practiceModel, string token)
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
                await spinContext.SaveChangesAsync();
                return true;
            }

            return false;
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
