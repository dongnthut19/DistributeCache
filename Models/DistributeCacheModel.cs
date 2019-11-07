using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DistributeCache.Models
{
    [Table("DistributeCache")]
    public class DistributeCacheModel
    {
        [Key]
        public string Key {get; set;}
        public byte[] Value {get;set;}
        public int SlidingExpirationInSecond {get; set;}
        public DateTime CreatedDate {get;set;}
    }
}