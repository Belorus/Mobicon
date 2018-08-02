using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Mobicon.Models;

namespace Mobicon.Pages
{
    public class SegmentModel : PageModel
    {
        private readonly DataContext _dataContext;

        public Config[] Configs { get; private set; }

        public SegmentModel(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void OnGet(int id)
        {
            Configs = _dataContext.Configs.Where(c => c.SegmentId == id).ToArray();
        }
    }
}