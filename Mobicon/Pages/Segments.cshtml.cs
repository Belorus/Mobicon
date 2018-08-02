using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Mobicon.Models;

namespace Mobicon.Pages
{
    public class SegmentsModel : PageModel
    {
        private readonly DataContext _dataContext;

        public Segment[] Segments { get; private set; }

        public SegmentsModel(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void OnGet()
        {
            Segments = _dataContext.Segments.ToArray();
        }

        public void OnPost(string segmentName)
        {
            _dataContext.Segments.Add(new Segment()
            {
                Name = segmentName
            });

            _dataContext.SaveChanges();
        }
    }
}