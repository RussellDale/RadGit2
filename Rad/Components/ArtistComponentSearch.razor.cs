using Microsoft.AspNetCore.Components;
using Rad.Models.Domian;

namespace Rad.Components
{
    public partial class ArtistComponentSearch
    {
        [Parameter] 
        public int InvoiceId { get; set; }
        [Parameter]
        public int PlaylistId { get; set; }
    }
}
