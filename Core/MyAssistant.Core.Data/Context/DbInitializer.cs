using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MyAssistant.Core.Data.Context;

internal class DbInitializer(ModelBuilder modelBuilder)
{
    public void Seed()
    {
        SeedEntityName();
    }

    private void SeedEntityName()
    {
        //modelBuilder.Entity<Actor>(e =>
        //{
        //    e.HasData(new Actor
        //    {
        //        Id = 1,
        //        FirstName = "Ітан",
        //        LastName = "Гоук",
        //        DateOfBirth = new DateTime(1970, 11, 06),
        //        Description = "Актор, сценарист, режисер, продюсер"
        //    });
        //    e.HasData(new Actor
        //    {
        //        Id = 2,
        //        FirstName = "Дженніфер",
        //        LastName = "Еністон",
        //        DateOfBirth = new DateTime(1969, 02, 11),
        //        Description = "Актриса, продюсер, режисер"
        //    });
        //    e.HasData(new Actor
        //    {
        //        Id = 3,
        //        FirstName = "Жан",
        //        LastName = "Рено",
        //        DateOfBirth = new DateTime(1948, 07, 30),
        //        Description = "Актор"
        //    });
        //    e.HasData(new Actor
        //    {
        //        Id = 4,
        //        FirstName = "Крістофер",
        //        LastName = "Ллойд",
        //        DateOfBirth = new DateTime(1938, 10, 22),
        //        Description = "Актор"
        //    });
        //    e.HasData(new Actor
        //    {
        //        Id = 5,
        //        FirstName = "Майкл",
        //        LastName = "Дж. Фокс",
        //        DateOfBirth = new DateTime(1961, 06, 09),
        //        Description = "Актор"
        //    });
        //    e.HasData(new Actor
        //    {
        //        Id = 6,
        //        FirstName = "Дженніфер",
        //        LastName = "Моррісон",
        //        DateOfBirth = new DateTime(1979, 04, 12),
        //        Description = "Актриса"
        //    });
        //    e.HasData(new Actor
        //    {
        //        Id = 7,
        //        FirstName = "Дайан",
        //        LastName = "Крюгер",
        //        DateOfBirth = new DateTime(1976, 07, 15),
        //        Description = "Актриса"
        //    });
        //});
    }
}