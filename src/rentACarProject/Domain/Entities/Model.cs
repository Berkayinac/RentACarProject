using NArchitecture.Core.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;
//Araba modeli
// BMW 4 serisi 3 serisi gibi araba modeli

public class Model : Entity<Guid>
{
    public Guid BrandId { get; set; }
    public Guid FuelId { get; set; }

    public string Name { get; set; }
    public decimal DailyPrice { get; set; }
    public string ImageUrl { get; set; }

    // Bir modelin markası olur 1-1 ilişki
    public virtual Brand? Brand { get; set; }
    public virtual Fuel? Fuel { get; set; }
}
