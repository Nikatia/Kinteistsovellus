﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Kiinteistosovellus.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class KiinteistoDBEntities : DbContext
    {
        public KiinteistoDBEntities()
            : base("name=KiinteistoDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Contacts> Contacts { get; set; }
        public virtual DbSet<Contractors> Contractors { get; set; }
        public virtual DbSet<Logins> Logins { get; set; }
        public virtual DbSet<MonthlySpendings> MonthlySpendings { get; set; }
        public virtual DbSet<MonthlySpendingTypes> MonthlySpendingTypes { get; set; }
        public virtual DbSet<OtherSpendings> OtherSpendings { get; set; }
        public virtual DbSet<OtherSpendingTypes> OtherSpendingTypes { get; set; }
        public virtual DbSet<Persons> Persons { get; set; }
        public virtual DbSet<Plans> Plans { get; set; }
        public virtual DbSet<Post> Post { get; set; }
        public virtual DbSet<database_firewall_rules> database_firewall_rules { get; set; }
        public virtual DbSet<KuukausittainenVaiMuu> KuukausittainenVaiMuu { get; set; }
        public virtual DbSet<ForCategorySortChart> ForCategorySortChart { get; set; }
        public virtual DbSet<ForOtherSpendingTypeCharts> ForOtherSpendingTypeCharts { get; set; }
        public virtual DbSet<ForMonthlySpendingsCharts> ForMonthlySpendingsCharts { get; set; }
        public virtual DbSet<ForCategorySortChartWithYears> ForCategorySortChartWithYears { get; set; }
    }
}
