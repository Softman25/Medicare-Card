using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ThisisaTest.Database;
using System.Linq;

namespace ThisisaTest.Database
{
    public static class Data
    {
        public static async Task SaveProfile(int Profile, ulong UserId, int Amount)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Invoice.Where(x => x.UserId == UserId).Count() < 1)
                {
                    DbContext.Invoice.Add(new Invoice
                    {
                        Profile = Profile,
                        UserId = UserId,
                        Amount = Amount
                    });
                }
                else
                {
                    Invoice Current = DbContext.Invoice.Where(x => x.UserId == UserId).FirstOrDefault();
                    Current.Profile = Profile;
                    DbContext.Invoice.Update(Current);
                }
                await DbContext.SaveChangesAsync();
            }
        }

        public static int ReviveRecord(ulong UserId)
        {
            using (var DbContext = new SqliteDbContext())
            {
                int result = DbContext.Invoice.Sum(a => a.Amount);
                return result;
            }
        }

        public static int SeeProfile(ulong UserId)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Invoice.Where(x => x.UserId == UserId).Count() < 1)
                    return 0;
                return DbContext.Invoice.Where(x => x.UserId == UserId).Select(x => x.Profile).FirstOrDefault();
            }
        }

        public static async Task SaveRecord(ulong UserId, int Amount, int Profile)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Invoice.Where(x => x.UserId == UserId).Count() < 1)
                {
                    DbContext.Invoice.Add(new Invoice
                    {
                        UserId = UserId,
                        Amount = Amount,
                        Profile = Profile
                    });
                } else
                {
                    Invoice Current = DbContext.Invoice.Where(x => x.UserId == UserId).FirstOrDefault();
                    Current.Amount += 1;
                    DbContext.Invoice.Update(Current);
                }
                await DbContext.SaveChangesAsync();
            }
        }
    }
}
