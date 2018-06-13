﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceHub.User.Context.Utilities
{
    public static class UserModelMapper
    {
        /// <summary>
        /// Convert a Context-User model to a Library-User model 
        /// </summary>
        /// <remarks>Use this mapper to convert an object retrieved from the 
        /// Context data source into a Library model to return to the client</remarks>
        /// <param name="contextUser">The user model to convert</param>
        /// <returns>A Library User model, or null if the input Context User was null</returns>
        public static Library.Models.User ContextToLibrary(Context.Models.User contextUser)
        {
            if(contextUser is null)
            {
                return null;
            }

            Library.Models.User libraryUser = new Library.Models.User();

            libraryUser.UserId = contextUser.UserId;
            libraryUser.Email = contextUser.Email;
            libraryUser.Gender = contextUser.Gender;
            libraryUser.Location = contextUser.Location;
            libraryUser.Type = contextUser.Type;

            libraryUser.Address.AddressId = contextUser.Address.AddressId;
            libraryUser.Address.Address1 = contextUser.Address.Address1;
            libraryUser.Address.Address2 = contextUser.Address.Address2;
            libraryUser.Address.City = contextUser.Address.City;
            libraryUser.Address.State = contextUser.Address.State;
            libraryUser.Address.PostalCode = contextUser.Address.PostalCode;
            libraryUser.Address.Country = contextUser.Address.Country;

            libraryUser.Name.NameId = contextUser.Name.NameId;
            libraryUser.Name.First = contextUser.Name.First;
            libraryUser.Name.Middle = contextUser.Name.Middle;
            libraryUser.Name.Last = contextUser.Name.Last;

            return libraryUser;
        }

        /// <summary>
        /// Convert the Library-User model to a Context-User model
        /// </summary>
        /// <remarks>
        /// Convert models with this method before trying to use them to
        /// interact with the Context data source.
        /// </remarks>
        /// <param name="libraryUser">The library user to convert to a context user</param>
        /// <returns>A Context User if the Library User was valid, null otherwise</returns>
        public static Context.Models.User LibraryToContext(Library.Models.User libraryUser)
        {
            if (libraryUser is null || libraryUser.Validate() == false)
            {
                return null;
            }

            Context.Models.User contextUser = new Context.Models.User();

            contextUser.UserId = libraryUser.UserId;
            contextUser.Email = libraryUser.Email;
            contextUser.Gender =libraryUser.Gender;
            contextUser.Location = libraryUser.Location;
            contextUser.Type = libraryUser.Type;

            contextUser.Address.AddressId = libraryUser.Address.AddressId;
            contextUser.Address.Address1 = libraryUser.Address.Address1;
            contextUser.Address.Address2 = libraryUser.Address.Address2;
            contextUser.Address.City = libraryUser.Address.City;
            contextUser.Address.State = libraryUser.Address.State;
            contextUser.Address.PostalCode = libraryUser.Address.PostalCode;
            contextUser.Address.Country = libraryUser.Address.Country;

            contextUser.Name.NameId = libraryUser.Name.NameId;
            contextUser.Name.First = libraryUser.Name.First;
            contextUser.Name.Middle = libraryUser.Name.Middle;
            contextUser.Name.Last = libraryUser.Name.Last;

            return contextUser;
        }

    }
}
