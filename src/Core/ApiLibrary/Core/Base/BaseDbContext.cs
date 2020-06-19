using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiLibrary.Base
{
    public class BaseDbContext : DbContext
    {
        public BaseDbContext() : base()
        {

        }

        public BaseDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}
