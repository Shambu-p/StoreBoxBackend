using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StoreBackend.Data;
using StoreBackend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace StoreBackend.Services.Cartoons {
    
    public class BoxItemService {

        private readonly StoreBackendContext context;

        public BoxItemService(StoreBackendContext db_context){
            this.context = db_context;
        }

        public async Task<IEnumerable<BoxItem>> getAll() {
            return await context.BoxItems.ToListAsync();
        }
        
    }

}