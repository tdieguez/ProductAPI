using System;
using System.Collections.Generic;

namespace Product.OpenApi.Entities
{
    public class Product
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string Category { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }

        //TODO: remove static collection once the EF Core is implemented
        public static ICollection<Product> GetCollection()
        {
            return new List<Product>
            {
                new Product
                {
                    Id = 1,
                    Name = "Moqueca Capixaba",
                    Description =
                        "Brazilian seafood stew. It is slowly cooked in a terracotta cassole. Moqueca can be made with shrimp or fish as a base with tomatoes, onions, garlic, lime and coriander.",
                    Category = "Food",
                    Price = 99.99,
                    CreatedAt = DateTimeOffset.Now.AddDays(-100),
                    ModifiedAt = DateTimeOffset.Now.AddDays(-100)
                },
                new Product
                {
                    Id = 2,
                    Name = "Torta Capixaba",
                    Description =
                        "Traditional and complex Brazilian dish originated from Espirito Santo. This seafood pie is made with a massive list of ingredients: fish such as sea bass, hake, and grouper, mussels, siri crabmeat, oysters, salt cod, shrimp, olive oil, garlic, onions, tomatoes, green onions, cilantro, red bell peppers, annatto oil, coconut milk, cloves, cinnamon, white vinegar, palm hearts, olives, and eggs.",
                    Category = "Food",
                    Price = 99.99,
                    CreatedAt = DateTimeOffset.Now.AddDays(-100),
                    ModifiedAt = DateTimeOffset.Now.AddDays(-100)
                }
            };
        }
    }
}