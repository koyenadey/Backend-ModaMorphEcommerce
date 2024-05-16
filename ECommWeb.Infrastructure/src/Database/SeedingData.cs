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
        Avatar = "example.jpeg",
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
        Avatar = "example1.jpeg",
        Password = _passwordService.HashPassword("customer1", out var salt),
        Salt = salt,
        Role = Role.Customer,
    };

    public static List<User> GetUsers()
    {
        return new List<User>{
            _admin, _customer1
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

    public static List<Address> GetAddresses()
    {
        return new List<Address>{
            user1Address, user2Address
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
        products.AddRange(GenerateProductsForCategory(category1, 3));
        products.AddRange(GenerateProductsForCategory(category2, 3));
        products.AddRange(GenerateProductsForCategory(category3, 3));
        products.AddRange(GenerateProductsForCategory(category4, 3));
        products.AddRange(GenerateProductsForCategory(category5, 3));
        products.AddRange(GenerateProductsForCategory(category6, 3));

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