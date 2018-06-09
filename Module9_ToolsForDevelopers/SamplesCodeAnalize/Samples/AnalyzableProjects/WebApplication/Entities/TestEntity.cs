using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Entities
{
    public class TestEntity
    {
        [DataContract]
        public int Id { get; set; }
        [DataContract]
        public int Name { get; set; }
    }
}