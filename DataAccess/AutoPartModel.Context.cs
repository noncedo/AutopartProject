﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataAccess
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class AutoPartEntities : DbContext
    {
        public AutoPartEntities()
            : base("name=AutoPartEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Dealer> Dealers { get; set; }
        public virtual DbSet<DealerGroup> DealerGroups { get; set; }
        public virtual DbSet<DmsProvider> DmsProviders { get; set; }
        public virtual DbSet<FileDestination> FileDestinations { get; set; }
        public virtual DbSet<FileLine> FileLines { get; set; }
        public virtual DbSet<FileSource> FileSources { get; set; }
        public virtual DbSet<ILHAudit> ILHAudits { get; set; }
        public virtual DbSet<Log> Logs { get; set; }
        public virtual DbSet<LogType> LogTypes { get; set; }
        public virtual DbSet<Process> Processes { get; set; }
        public virtual DbSet<ProcessRun> ProcessRuns { get; set; }
        public virtual DbSet<Setting> Settings { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<vwProcessRunConsolidation> vwProcessRunConsolidations { get; set; }
        public virtual DbSet<vwILHConsolidation> vwILHConsolidations { get; set; }
    }
}
