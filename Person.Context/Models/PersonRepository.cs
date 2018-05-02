using System;
using System.Collections.Generic;
using AutoMapper;
using Person.Library.Abstracts;
using Person.Library.Interfaces;
using Person.Library.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace Person.Context.Models
{
    public class PersonRepository : ARepository<Person>
    {

        /// <summary>
        /// These fields are the keys inside of the JSON object
        /// that relate to the fields of the object model.
        /// { ObjectModelProperty, DataSourceJsonKey }
        /// 
        /// Example: Salesforce provides a FirstName and LastName fields in their Contact object 
        /// </summary>
        // TODO: Determine which phone field should be entered in the salesforce sandbox.
        private Dictionary<string, string> jsonKeys = new Dictionary<string, string>() {
            {"Id", "Id" }, {"Name", "Name"}, {"Email", "Email"}, {"BatchName", "Training_Skill_Type_Test__c"},
            { "Phone", "MobilePhone"}, {"HasCar", "HR_Has_Car__c"}, {"IsMale", "rnm__Gender__c"},
            {"Street", "rnm__Street_Address__c"}, {"City", "rnm__City__c"}, {"State", "rnm__State__c"},
            {"PostalCode", "rnm__Postal_Code__c"}, {"Country", "rnm__Country__c"}
        };

        public PersonRepository(IOptions<Settings> settings) : base(settings) { }

        public string ConvertToHexString(string Id)
        {
            string hexString = "";
            foreach (char ch in Id.ToCharArray())
            {
                var strToAdd = BitConverter.ToString(new byte[] { Convert.ToByte(ch) });
                hexString += int.TryParse(strToAdd, out _) ? strToAdd : strToAdd.ToLower();
            }
            return hexString.Length >= 24 ? hexString.Substring((hexString.Length - 24), 24) : hexString;
        }

        protected override Person MapJsonToModel(JObject jsonObject)
        {
            
            var config = new MapperConfiguration(
                  cfg => cfg.CreateMap<JObject, Person>()
                      .ForMember(person => person.ModelId, options => options.MapFrom(json => ConvertToHexString(json[jsonKeys["Id"]].ToString())))
                      .ForMember(person => person.Name, options => options.MapFrom(json => json[jsonKeys["Name"]].ToString()))
                      .ForMember(person => person.Email, options => options.MapFrom(json => json[jsonKeys["Email"]].ToString()))
                      .ForMember(person => person.BatchName, options => options.MapFrom(json => json[jsonKeys["BatchName"]]))
                      .ForMember(person => person.IsMale, options => options.MapFrom(json => json[jsonKeys["IsMale"]].ToString() == "Male"))
                      .ForMember(person => person.Phone, options => options.MapFrom(json => json[jsonKeys["Phone"]]))
                      .ForMember(person => person.HasCar, options => options.MapFrom(json => json[jsonKeys["HasCar"]].ToString() == "Yes"))
                      .ForMember(person => person.Address, options => options.MapFrom(json => json[jsonKeys["Street"]] + ", " + json[jsonKeys["City"]] + ", " +
                        json[jsonKeys["State"]] + ", " + json[jsonKeys["PostalCode"]] + ", " + json[jsonKeys["Country"]]))
            );
            IMapper imap = config.CreateMapper();
            return imap.Map<JObject, Person>(jsonObject);
        }
    }
}
