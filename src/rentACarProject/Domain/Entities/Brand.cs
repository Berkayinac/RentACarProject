using NArchitecture.Core.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;
// Guid -> Unique Identifier
public class Brand : Entity<Guid>
{
    public string Name { get; set; }

    // bir markanın birden fazla modelleri olur -> 1-N ilişki
    public virtual ICollection<Model> Models { get; set; }
}


//CQRS
// Command -> CUD
// Query -> R

//CQRS avantajları
// 1- Gereksiz bağımlılıkları ortadan kaldırır
// 2- Kod generator desteği ile kod standartlaşır ve hızlı yapılır
// 3- test edilebilitesi yüksek bir şekildedir.