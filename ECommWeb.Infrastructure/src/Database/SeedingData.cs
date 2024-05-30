using ECommWeb.Core.src.Entity;
using ECommWeb.Core.src.ValueObject;
using ECommWeb.Business.src.Shared;
using ECommWeb.Business.src.ServiceAbstract.AuthServiceAbstract;
using ECommWeb.Infrastructure.src;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using Microsoft.Extensions.Logging;

namespace ECommWeb.Infrastructure.src.Database;

public class SeedingData
{
    private static readonly IPasswordService _passwordService = new PasswordService();
    private static Random random = new Random();
    private static Category category1 = new()
    {
        Name = "Electronics",
        Image = $"https://picsum.photos/200/?random={random.Next(10)}"
    };
    private static Category category2 = new()
    {
        Name = "Clothing",
        Image = $"https://picsum.photos/200/?random={random.Next(10)}"
    };

    private static Category category3 = new()
    {
        Name = "Home and Furnitures",
        Image = $"https://picsum.photos/200/?random={random.Next(10)}"
    };
    private static Category category4 = new()
    {
        Name = "Books",
        Image = $"https://picsum.photos/200/?random={random.Next(10)}"
    };
    private static Category category5 = new()
    {
        Name = "Toys and Games",
        Image = $"https://picsum.photos/200/?random={random.Next(10)}"
    };
    private static Category category6 = new()
    {
        Name = "Sports",
        Image = $"https://picsum.photos/200/?random={random.Next(10)}"
    };

    private static User _admin = new User
    {
        Id = Guid.NewGuid(),
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow,
        UserName = "john",
        Email = "john.doe@mail.com",
        Avatar = "https://res.cloudinary.com/dpdhvztg3/image/upload/v1713770308/samples/smile.jpg",
        Password = _passwordService.HashPassword("Admin123", out var salt),
        Salt = salt,
        Role = Role.Admin,
    };
    private static User _admin1 = new User
    {
        Id = Guid.NewGuid(),
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow,
        UserName = "carlos",
        Email = "caroles.castro@mail.com",
        Avatar = "https://res.cloudinary.com/dpdhvztg3/image/upload/v1713770286/samples/people/smiling-man.jpg",
        Password = _passwordService.HashPassword("Admin123", out var salt),
        Salt = salt,
        Role = Role.Admin,
    };

    private static User _customer1 = new User
    {
        Id = Guid.NewGuid(),
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow,
        UserName = "Maria",
        Email = "maria@testmail.com",
        Avatar = "https://res.cloudinary.com/dpdhvztg3/image/upload/v1716223447/12e7a993-c505-4a43-b0a6-c3a3c2be4dac.avif",
        Password = _passwordService.HashPassword("customer1", out var salt),
        Salt = salt,
        Role = Role.Customer,
    };
    private static User _customer2 = new User
    {
        Id = Guid.NewGuid(),
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow,
        UserName = "Harry",
        Email = "harry@testmail.com",
        Avatar = "https://res.cloudinary.com/dpdhvztg3/image/upload/v1716415263/53b8eb58-6408-469a-bd77-523c29db2892.avif",
        Password = _passwordService.HashPassword("harry@Test1", out var salt),
        Salt = salt,
        Role = Role.Customer,
    };

    public static List<User> GetUsers()
    {
        return new List<User>{
            _admin,_admin1, _customer1,_customer2
        };
    }
    public static List<User> Users = GetUsers();

    private static Address user1Address = new Address
    {
        Id = Guid.NewGuid(),
        AddressLine = "41C",
        Street = "Kauppakatu",
        City = "Pori",
        Country = "Finland",
        Postcode = "61200",
        PhoneNumber = "4198767000",
        Landmark = "K-market",
        UserId = Users[0].Id
    };
    private static Address user2Address = new Address
    {
        Id = Guid.NewGuid(),
        AddressLine = "2C",
        Street = "Mannerheimintie",
        City = "Helsinki",
        Country = "Finland",
        Postcode = "00260",
        PhoneNumber = "4198767000",
        Landmark = "Taka-Töölö",
        UserId = Users[1].Id
    };

    private static Address user3Address = new Address
    {
        Id = Guid.NewGuid(),
        AddressLine = "11A",
        Street = "Mykkla",
        City = "Espoo",
        Country = "Finland",
        Postcode = "00200",
        PhoneNumber = "06786324",
        Landmark = "near Tula paivakoti",
        UserId = Users[2].Id
    };

    private static Address user4Address = new Address
    {
        Id = Guid.NewGuid(),
        AddressLine = "59B",
        Street = "Itamerenkatu 1",
        City = "Helsinki",
        Country = "Finland",
        Postcode = "00200",
        PhoneNumber = "02610",
        Landmark = "beside K-Market",
        UserId = Users[3].Id
    };

    public static List<Address> GetAddresses()
    {
        return new List<Address>{
            user1Address, user2Address, user3Address, user4Address
        };
    }
    public static List<Address> Addresses = GetAddresses();


    public static List<Category> GetCategories()
    {
        return new List<Category>
        {
            category1, category2, category3, category4, category5, category6
        };
    }

    private static List<Product> GenerateProductsForCategory(Category category, int count)
    {
        var products = new List<Product>();
        for (int i = 1; i <= count; i++)
        {
            var product = new Product()
            {
                Id = Guid.NewGuid(),
                Name = $"{category.Name} product {i}",
                Price = random.Next(1000),      // price
                Description = $"Description of {category.Name} product {i}",
                Inventory = random.Next(10),        // inventory
                Weight = random.Next(1, 10) / 10.0,               // weight
                CategoryId = category.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            products.Add(product);
        }
        return products;
    }

    public static List<Product> GetProducts()
    {
        var products = new List<Product>();
        products.AddRange(GenerateProductsForCategory(category1, 5));
        products.AddRange(GenerateProductsForCategory(category2, 5));
        products.AddRange(GenerateProductsForCategory(category3, 5));
        products.AddRange(GenerateProductsForCategory(category4, 5));
        products.AddRange(GenerateProductsForCategory(category5, 5));
        products.AddRange(GenerateProductsForCategory(category6, 5));

        return products;
    }

    public static List<Product> Products = GetProducts();

    public static List<ProductImage> GetProductImages()
    {
        var productImages = new List<ProductImage>();
        foreach (var product in Products)
        {
            for (int i = 0; i < 3; i++)
            {
                var productImage = new ProductImage
                {
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    ProductImageUrl = $"https://picsum.photos/200/?random={random.Next(100, 1000)}",
                    ProductId = product.Id
                };
                productImages.Add(productImage);
            }
        }
        return productImages;
    }

    public static List<Wishlist> GetWishlists()
    {
        return new List<Wishlist>{
            new Wishlist{Name="My wishlist 1", UserId=Users[1].Id}
        };
    }
    public static List<Wishlist> Wishlists = GetWishlists();

    public static List<WishlistItem> GetWishlistItems()
    {
        return new List<WishlistItem>{
            new WishlistItem
            {
                ProductId = Products[0].Id,
                WishlistId= Wishlists[0].Id
            }
        };
    }

    public static List<WishlistItem> WishlistItems = GetWishlistItems();

    public static List<Order> GetOrders()
    {
        return new List<Order>{
            new Order
            {
                OrderDate = DateTime.Now,
                Status = Status.processing,
                UserId = Users[0].Id,
                AddressId = Addresses[0].Id
            },
            new Order
            {
                OrderDate = DateTime.Now,
                Status = Status.pending,
                UserId = Users[0].Id,
                AddressId = Addresses[1].Id
            }
        };
    }
    public static List<Order> Orders = GetOrders();
    public static List<OrderProduct> GetOrderedProducts()
    {
        return new List<OrderProduct>{
            new OrderProduct
            {
                OrderId = Orders[0].Id,
                ProductId = Products[0].Id,
                Quantity = 3,
                PriceAtPurchase = 17.99,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            },

            new OrderProduct
            {
                OrderId = Orders[0].Id,
                ProductId = Products[1].Id,
                Quantity = 1,
                PriceAtPurchase = 25.99,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            },

        };
    }
    public static List<OrderProduct> OrderProducts = GetOrderedProducts();
}