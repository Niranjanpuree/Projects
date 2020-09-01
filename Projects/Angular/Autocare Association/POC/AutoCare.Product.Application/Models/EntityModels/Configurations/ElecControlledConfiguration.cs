using System;
using System.Collections.Generic;
using AutoCare.Product.Vcdb.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class ElecControlledConfiguration : EntityTypeConfiguration<ElecControlled>
    {
        public ElecControlledConfiguration()
            : this("dbo")
        {
        }

        public ElecControlledConfiguration(string schema)
        {
            ToTable("ElecControlled", schema);
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"ElecControlledID").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.ElectronicallyControlled).HasColumnName(@"ElecControlled").IsRequired().IsFixedLength().IsUnicode(false).HasColumnType("char").HasMaxLength(3);
            Property(x => x.ChangeRequestId).HasColumnName("ChangeRequestId").HasColumnType("bigint");

            Ignore(x => x.TransmissionCount);
        }
    }
}
