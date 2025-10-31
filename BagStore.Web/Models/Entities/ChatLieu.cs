using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BagStore.Domain.Entities
{
    public class ChatLieu
    {
        public int MaChatLieu { get; set; }
        public string TenChatLieu { get; set; }
        public string MoTa { get; set; }

        public ICollection<SanPham> SanPhams { get; set; }
    }
}