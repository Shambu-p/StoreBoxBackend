using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreBackend.Infrastructure.Persistance
{
    public class PersistanceBuilder {
        
        public static ModelBuilder Build(ModelBuilder builder) {

            builder = BoxConfiguration.onBuild(builder);
            builder = BoxItemConfiguration.onBuild(builder);
            builder = StoreConfiguration.onBuild(builder);
            builder = StoreItemConfiguration.onBuild(builder);
            builder = ItemConfiguration.onBuild(builder);
            builder = UserConfiguration.onBuild(builder);

            return builder;

        }

    }
}