using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FileUploadAspCore5._0.ViewModel
{
   [NotMapped]
    public class VmEmployee
    {
        public int Id { get; set; }
        public string EmpName { get; set; }
        public string Email { get; set; }

        public string FileName { get; set; }
        public string FileType { get; set; }
        public byte[] FileByte { get; set; }
        public string FilePath { get; set; }
        public string FileFullPath { get; set; }


        [NotMapped]
        public IFormFile IDataFile { get; set; } //add this extra property to receive file from view
    }
}
