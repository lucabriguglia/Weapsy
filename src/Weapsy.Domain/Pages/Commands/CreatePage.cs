using System;
using System.Collections.Generic;

namespace Weapsy.Domain.Pages.Commands
{
    public class CreatePage : PageDetails
    {
        public IEnumerable<Guid> MenuIds { get; set; } = new List<Guid>();
    }
}