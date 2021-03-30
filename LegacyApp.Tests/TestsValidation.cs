using LegacyApp.Validation;
using NUnit.Framework;
using System;

namespace LegacyApp.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestValidUser()
        {
            Assert.True(new ModelValidation<User>(new User
            {
                Firstname = "Ted",
                Surname = "Danson",
                Client = new Client
                {
                    Id = 123,
                    Name = "Umbrella"
                },
                DateOfBirth = DateTime.Now.AddYears(-60),
                EmailAddress = "ted.danson@helloa.com"
            }));
        }

        [Test]
        public void TestInvalidFirstName()
        {
            Assert.False(new ModelValidation<User>(new User
            {
                Surname = "Danson",
                Client = new Client
                {
                    Id = 123,
                    Name = "Umbrella"
                },
                DateOfBirth = DateTime.Now.AddYears(-60),
                EmailAddress = "ted.danson@helloa.com"
            }));
        }

        [Test]
        public void TestInvalidSurName()
        {
            Assert.False(new ModelValidation<User>(new User
            {
                Firstname = "Ted",
                Client = new Client
                {
                    Id = 123,
                    Name = "Umbrella"
                },
                DateOfBirth = DateTime.Now.AddYears(-60),
                EmailAddress = "ted.danson@helloa.com"
            }));
        }

        [Test]
        public void TestInvalidEmail()
        {
            Assert.False(new ModelValidation<User>(new User
            {
                Firstname = "Ted",
                Surname = "Danson",
                Client = new Client
                {
                    Id = 123,
                    Name = "Umbrella"
                },
                DateOfBirth = DateTime.Now.AddYears(-60),
                EmailAddress = "teddanson@helloacom"
            }));
        }

        [Test]
        public void TestInvalidAge()
        {
            Assert.False(new ModelValidation<User>(new User
            {
                Firstname = "Ted",
                Surname = "Danson",
                Client = new Client
                {
                    Id = 123,
                    Name = "Umbrella"
                },
                DateOfBirth = DateTime.Now.AddYears(-10),
                EmailAddress = "ted.danson@helloa.com"
            }));
        }

        [Test]
        public void TestInvalidCredit()
        {
            User user = new User
            {
                Firstname = "Ted",
                Surname = "Danson",
                Client = new Client
                {
                    Id = 123,
                    Name = "Umbrella"
                },
                DateOfBirth = DateTime.Now.AddYears(-60),
                EmailAddress = "ted.danson@helloa.com"
            };
            user.SetMockCredit(200);
            Assert.False(new ModelValidation<User>(user));
        }
    }
}