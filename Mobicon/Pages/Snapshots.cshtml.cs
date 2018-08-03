using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Mobicon.Models;

namespace Mobicon.Pages
{
    public class SnapshotsModel : PageModel
    {
        private readonly DataContext _dataContext;

        public Snapshot[] Snapshots { get; set; }

        public SnapshotsModel(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void OnGet()
        {
            Snapshots = _dataContext.Snapshots.Include(s => s.Entries).ToArray();
        }

        public void OnPost(string snapshotName)
        {
            _dataContext.Snapshots.Add(new Snapshot()
            {
                Name = snapshotName
            });

            _dataContext.SaveChanges();
        }
    }
}