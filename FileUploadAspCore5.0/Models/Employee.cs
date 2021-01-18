using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FileUploadAspCore5._0.Models
{
    public class Employee
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        public string EmpName { get; set; }
        [StringLength(50)]
        public string Email { get; set; }
        [StringLength(100)]
        public string FileName { get; set; }
        [StringLength(100)]
        [Required]
        public string FileType { get; set; }
        [MaxLength]
        [Required]
        public byte[] FileByte { get; set; }
        [MaxLength]
        [Required]
        public string FilePath { get; set; }

        [MaxLength]
        [Required]
        public string FileFullPath { get; set; }
    }
}
