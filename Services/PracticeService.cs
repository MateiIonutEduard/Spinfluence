﻿using System.Linq;
using Spinfluence.Data;
using Spinfluence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.StaticFiles;
#pragma warning disable

namespace Spinfluence.Services
{
    public class PracticeService : IPracticeService
    {
        readonly SpinContext spinContext;

        public PracticeService(SpinContext spinContext)
        { this.spinContext = spinContext; }

        public string GetContentType(string filePath)
        {
            const string DefaultContentType = "application/octet-stream";
            var provider = new FileExtensionContentTypeProvider();

            // get MIME content type of specified file
            if (!provider.TryGetContentType(filePath, out string contentType))
                contentType = DefaultContentType;

            return contentType;
        }

        public async Task<string> GetPracticeAsync(int id, string? type)
        {
            Practice practice = await spinContext.Practice
                .FirstOrDefaultAsync(e => e.Id == id);

            if(practice != null)
            {
                // load document by type
                if(type != null)
                {
                    if (type.CompareTo("resume") == 0)
                        return practice.Resume;
                    else
                        return practice.CoverLetter;
                }

                return string.Empty;
            }

            return null;
        }

        public async Task<int> ApprovePracticeAsync(string token, int id, bool IsApproved)
        {
            Account? account = await spinContext.Account
                .FirstOrDefaultAsync(e => e.token.CompareTo(token) == 0);

            // check if account has rights
            if(account != null && account.GrantType == 2)
            {
                /* get specific practice */
                Practice? practice = await spinContext.Practice
                    .FirstOrDefaultAsync(e => e.Id == id);

                /* verify if applicant practice is on pending */
                if(practice != null && !practice.IsCanceled && practice.IsApproved == null)
                {
                    // if yes update practice
                    practice.IsApproved = IsApproved;
                    await spinContext.SaveChangesAsync();
                    return 1;
                }

                // practice is not in pending
                return 0;
            }

            return -1;
        }

        public async Task<PracticeEventModel[]> GetNotificationsAsync(PracticeEventSearchFilter? filter)
        {
            var list = new List<PracticeEventModel>();
            List<Practice>? practices = await spinContext.Practice.ToListAsync();

            PracticeEventModel[] events = (
                from p in practices
                join e in await spinContext.CompanyEvent.ToListAsync() on p.CompanyEventId equals e.Id
                join c in await spinContext.Company.ToListAsync()
                on e.CompanyId equals c.Id
                let counter = (from pr in spinContext.Practice.ToList() where pr.CompanyEventId == e.Id select pr).Count()
                let account = (from a in spinContext.Account.ToList() where a.Id == p.AccountId select a).FirstOrDefault()
                select new PracticeEventModel
                {
                    AccountId = account.Id,
                    ApplicantName = account.username,
                    Name = e.Name,
                    PracticeId = p.Id,
                    BeginDate = e.BeginDate,
                    Seats = e.Seats - counter,
                    Resume = p.Resume,
                    CoverLetter = p.CoverLetter,
                    IsApproved = p.IsApproved,
                    IsCanceled = p.IsCanceled,
                    CompanyName = c.Name,
                    EndDate = e.EndDate,
                    Body = p.Body,
                }).ToArray();

            if (filter != null && events.Length > 0)
            {
                // filter by specific practice event name, practice acceptation status and cancellation
                string filterPracticeName = !string.IsNullOrEmpty(filter.EventName) ? filter.EventName.ToLower() : null;

                for (int k = 0; k < events.Length; k++)
                {
                    bool isCanceled = events[k].IsCanceled == filter.IsCanceled;
                    int approveStatus = events[k].IsApproved != null ? (events[k].IsApproved.Value ? 2 : 3) : 1;
                    string practiceName = events[k].Name.ToLower();

                    // if have practice name not null
                    if (!string.IsNullOrEmpty(filterPracticeName))
                    {
                        if (practiceName.StartsWith(filterPracticeName) && approveStatus == filter.PracticeStatus && isCanceled)
                            list.Add(events[k]);
                    }
                    else
                    {
                        /* that's not working properly in the past */
                        if (approveStatus == filter.PracticeStatus && isCanceled)
                            list.Add(events[k]);
                    }
                }
            }
            else
            {
                // filter not applied
                if (practices != null)
                    list.AddRange(events);
            }

            return list.ToArray();
        }

        public async Task<PracticeEventModel[]> GetPracticesAsync(PracticeEventSearchFilter? filter, string token)
        {
            var list = new List<PracticeEventModel>();

            Account? account = await spinContext.Account.
                FirstOrDefaultAsync(a => a.token.CompareTo(token) == 0);

            if(account != null)
            {
                List<Practice>? practices = await spinContext.Practice.Where(p => p.AccountId == account.Id)
                    .ToListAsync();

                PracticeEventModel[] events = (
                    from p in practices join e in await spinContext.CompanyEvent.ToListAsync() on p.CompanyEventId equals e.Id join c in await spinContext.Company.ToListAsync()
                    on e.CompanyId equals c.Id
                    let counter = (from pr in spinContext.Practice.ToList() where pr.CompanyEventId == e.Id && !pr.IsCanceled select pr).Count()
                    select new PracticeEventModel
                    {
                        Name = e.Name,
                        PracticeId = p.Id,
                        BeginDate = e.BeginDate,
                        Seats = e.Seats - counter,
                        Resume = p.Resume,
                        CoverLetter = p.CoverLetter,
                        IsApproved = p.IsApproved,
                        IsCanceled = p.IsCanceled,
                        CompanyName = c.Name,
                        EndDate = e.EndDate,
                        Body = p.Body,
                    }).ToArray();

                if (filter != null && events.Length > 0)
                {
                    // filter by specific practice event name, practice acceptation status and cancellation
                    string filterPracticeName = !string.IsNullOrEmpty(filter.EventName) ? filter.EventName.ToLower() : null;

                    for (int k = 0; k < events.Length; k++)
                    {
                        bool isCanceled = events[k].IsCanceled == filter.IsCanceled;
                        int approveStatus = events[k].IsApproved != null ? (events[k].IsApproved.Value ? 2 : 3) : 1;
                        string practiceName = events[k].Name.ToLower();

                        /* fix practice event list filter */
                        if (!string.IsNullOrEmpty(filterPracticeName))
                        {
                            if(practiceName.StartsWith(filterPracticeName) && approveStatus == filter.PracticeStatus && isCanceled)
                                list.Add(events[k]);
                        }
                        else
                        {
                            if(approveStatus == filter.PracticeStatus && isCanceled)
                                list.Add(events[k]);
                        }
                    }
                }
                else
                {
                    // filter not applied
                    if (practices != null)
                        list.AddRange(events);
                }
            }

            return list.ToArray();
        }

        public async Task<int> AddPracticeAsync(PracticeModel practiceModel, string token)
        {
            Account? account = await spinContext.Account.
                FirstOrDefaultAsync(a => a.token.CompareTo(token) == 0);

            if(account != null)
            {
                Practice? obj = await spinContext.Practice
                    .FirstOrDefaultAsync(p => p.AccountId == account.Id && p.CompanyEventId == practiceModel.CompanyEventId);

                if (obj != null && !obj.IsCanceled) return 0;
                string resumeFolder = "./Storage/attachments/resumes";
                if (!Directory.Exists(resumeFolder)) Directory.CreateDirectory(resumeFolder);

                string coverLetterFolder = "./Storage/attachments/coverLetters";
                if (!Directory.Exists(coverLetterFolder)) Directory.CreateDirectory(coverLetterFolder);
                MemoryStream ms = new MemoryStream();

                string resumePath = string.Empty;
                string coverPath = string.Empty;

                if (practiceModel.Resume != null)
                {
                    // create resume file at disk
                    await practiceModel.Resume.CopyToAsync(ms);
                    resumePath = $"{resumeFolder}/{practiceModel.Resume.FileName}";
                    File.WriteAllBytes(resumePath, ms.ToArray());
                }

                if(practiceModel.CoverLetter != null)
                {
                    // write cover letter file to disk
                    await practiceModel.CoverLetter.CopyToAsync(ms);
                    coverPath = $"{coverLetterFolder}/{practiceModel.CoverLetter.FileName}";
                    File.WriteAllBytes(coverPath, ms.ToArray());
                }

                if(obj != null && obj.IsCanceled)
                {
                    obj.IsCanceled = false;
                    obj.Body = practiceModel.Body;

                    // only if the attachments has been uploaded
                    if (!string.IsNullOrEmpty(resumePath))
                    {
                        // remove old resume document
                        if (!string.IsNullOrEmpty(obj.Resume) && !obj.Resume.Contains(practiceModel.Resume.FileName) && File.Exists(obj.Resume))
                            File.Delete(obj.Resume);

                        obj.Resume = resumePath;
                    }

                    if (!string.IsNullOrEmpty(coverPath))
                    {
                        // remove old cover letter document
                        if (!string.IsNullOrEmpty(obj.CoverLetter) && !obj.CoverLetter.Contains(practiceModel.CoverLetter.FileName) && File.Exists(obj.CoverLetter))
                            File.Delete(obj.CoverLetter);

                        obj.CoverLetter = coverPath;
                    }

                    await spinContext.SaveChangesAsync();
                    return 2;
                }

                Practice practice = new Practice
                {
                    Body = practiceModel.Body,
                    IsCanceled = false,
                    CompanyEventId = practiceModel.CompanyEventId,
                    AccountId = account.Id
                };

                if (!string.IsNullOrEmpty(resumePath))
                    practice.Resume = resumePath;

                if (!string.IsNullOrEmpty(coverPath))
                    practice.CoverLetter = coverPath;

                // save new practice entity
                spinContext.Practice.Add(practice);
                await spinContext.SaveChangesAsync();
                return 1;
            }

            return -1;
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
