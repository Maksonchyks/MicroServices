using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Persistence
{
    public class CatalogDbContextFactory : IDesignTimeDbContextFactory<CatalogDbContext>
    {
        public CatalogDbContext CreateDbContext(string[] args)
        {
            // Увага: цей рядок підключення використовується ТІЛЬКИ для створення міграцій.
            // При запуску програми буде використовуватися рядок з appsettings.json
            var designTimeConnectionString =
                "server=localhost;database=dbprod;user=root;password=1234";
            var serverVersion = ServerVersion.AutoDetect(designTimeConnectionString);

            var optionsBuilder = new DbContextOptionsBuilder<CatalogDbContext>();

            optionsBuilder.UseMySql(designTimeConnectionString, serverVersion, mySqlOptions =>
            {
                // Обов'язково вказуємо, що міграції будуть зберігатися в цій збірці (Infrastructure)
                mySqlOptions.MigrationsAssembly(typeof(CatalogDbContext).Assembly.FullName);
            });

            return new CatalogDbContext(optionsBuilder.Options);
        }
    }
}
