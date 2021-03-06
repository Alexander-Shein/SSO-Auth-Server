using System;
using AuthGuard.BLL.Domain.Entities.Identity;
using DddCore.DAL.DomainStack.EntityFramework.Mapping;

namespace AuthGuard.DAL.Mappings.Identity
{
    public class IdentityMappingModuleInstaller : IMappingModuleInstaller
    {
        public void Install(IModelBuilder config)
        {
            config.Entity<IdentityClaim>(c =>
            {
                c.Property(x => x.Ts).IsRowVersion();
            });

            config
                .Entity<IdentityResourceClaim>(c =>
                {
                    c.Property<Guid>("IdentityResourceId");
                    c.HasOne(x => x.IdentityClaim).WithMany().HasForeignKey(x => x.IdentityClaimId);
                });

            config
                .Entity<IdentityResource>(c =>
                {
                    c.Property(x => x.Ts).IsRowVersion();
                    c.HasMany(x => x.Claims).WithOne().HasForeignKey("IdentityResourceId");
                    c.Ignore(x => x.IsNameChanged);
                });
        }
    }
}