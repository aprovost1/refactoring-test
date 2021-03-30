using System;

namespace LegacyApp
{
    public class UserService
    {
        public bool AddUser(string firname, string surname, string email, DateTime dateOfBirth, int clientId)
        {
            var clientRepository = new ClientRepository();
            var client = clientRepository.GetById(clientId);
            var user = new User
            {
                Client = client,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                Firstname = firname,
                Surname = surname
            };
            if (!user.IsValid)
            {
                return false;
            }
            UserDataAccess.AddUser(user);
            return true;
        }
    }
}