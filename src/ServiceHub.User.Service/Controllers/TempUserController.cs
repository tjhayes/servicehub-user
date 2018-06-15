using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using ServiceHub.User.Context.Repositories;
using ServiceHub.User.Context.Utilities;

namespace ServiceHub.User.Service.Controllers
{
    [Route("api/[controller]")]
    public class TempUserController : Controller
    {
        private readonly UserStorage _userStorage;

        public TempUserController(IUserRepository userRepository)
        {
            _userStorage = new UserStorage(userRepository);
        }

        private string GetUsers()
        {
            return "[    {        \"UserId\": \"a043c475-a8ea-4421-bb6e-ee94289c67e5\",        \"Name\": {            \"NameId\": \"1f38efe1-cd4a-4573-8524-d638f33e4cb0\",            \"First\": \"Oren\",            \"Middle\": null,            \"Last\": \"Chartman\"        },        \"Email\": \"cchartman0@vk.com\",        \"Gender\": \"F\",        \"Type\": \"Associate\",        \"Location\": \"Reston\",        \"Address\": null    },    {        \"UserId\": \"94060e03-7278-4d6a-a5ec-c22ae85cbb8b\",        \"Name\": {            \"NameId\": \"0eea8667-ca89-4865-898c-c0a309fe511f\",            \"First\": \"Peria\",            \"Middle\": \"Penn\",            \"Last\": \"Liptrot\"        },        \"Email\": \"pliptrot1@gov.uk\",        \"Gender\": \"M\",        \"Type\": \"Associate\",        \"Location\": \"Tampa\",        \"Address\": null    },    {        \"UserId\": \"35e333b3-8f04-4093-907c-37e13078200f\",        \"Name\": {            \"NameId\": \"cc31fe4d-ff84-40af-8853-13dc95eb5883\",            \"First\": \"Wendie\",            \"Middle\": \"Katalin\",            \"Last\": \"Stiegar\"        },        \"Email\": \"kstiegar2@usda.gov\",        \"Gender\": \"M\",        \"Type\": \"Associate\",        \"Location\": \"Tampa\",        \"Address\": null    },    {        \"UserId\": \"0c807213-8004-4323-a097-a9f023dce0ab\",        \"Name\": {            \"NameId\": \"49237796-7568-4c4d-8b6e-29a4cf18b6bf\",            \"First\": \"Nappy\",            \"Middle\": null,            \"Last\": \"Ravillas\"        },        \"Email\": \"aravillas3@zimbio.com\",        \"Gender\": \"M\",        \"Type\": \"Associate\",        \"Location\": \"Reston\",        \"Address\": null    },    {        \"UserId\": \"d84bc0d6-8496-4cf0-bc5e-5f19c6ce01ca\",        \"Name\": {            \"NameId\": \"6a52f2ae-d1dc-42af-b7c1-04907179e274\",            \"First\": \"Adiana\",            \"Middle\": null,            \"Last\": \"Smye\"        },        \"Email\": \"msmye4@feedburner.com\",        \"Gender\": \"F\",        \"Type\": \"Associate\",        \"Location\": \"Reston\",        \"Address\": null    },    {        \"UserId\": \"6d886dcc-3712-43bd-b372-f0d30143bde7\",        \"Name\": {            \"NameId\": \"a58442fc-148a-4c14-ba35-5849716bc136\",            \"First\": \"Leonelle\",            \"Middle\": \"Mose\",            \"Last\": \"Kubin\"        },        \"Email\": \"mkubin5@mail.ru\",        \"Gender\": \"F\",        \"Type\": \"Associate\",        \"Location\": \"Tampa\",        \"Address\": null    },    {        \"UserId\": \"53b54c1b-2030-489d-840b-38f11ecc6085\",        \"Name\": {            \"NameId\": \"d037af89-f0b7-43cf-9dba-0622da0fa66f\",            \"First\": \"Mitzi\",            \"Middle\": null,            \"Last\": \"Swanston\"        },        \"Email\": \"sswanston6@google.cn\",        \"Gender\": \"F\",        \"Type\": \"Associate\",        \"Location\": \"Tampa\",        \"Address\": null    },    {        \"UserId\": \"a2c3b01c-fe5d-4b31-9822-39714f831412\",        \"Name\": {            \"NameId\": \"432dec8a-9a0a-46ca-a6ea-405ef2b3b15b\",            \"First\": \"Sammy\",            \"Middle\": \"Kev\",            \"Last\": \"Gleder\"        },        \"Email\": \"kgleder7@un.org\",        \"Gender\": \"M\",        \"Type\": \"Associate\",        \"Location\": \"Reston\",        \"Address\": null    },    {        \"UserId\": \"d94d0d9c-40d3-4b65-a852-a21742f5776f\",        \"Name\": {            \"NameId\": \"a10bc6ec-91d2-4f09-9316-9751f718f533\",            \"First\": \"Mateo\",            \"Middle\": null,            \"Last\": \"Maseyk\"        },        \"Email\": \"emaseyk8@bbc.co.uk\",        \"Gender\": \"M\",        \"Type\": \"Associate\",        \"Location\": \"Reston\",        \"Address\": null    },    {        \"UserId\": \"ff77467f-4d6b-441f-9662-8c2636293908\",        \"Name\": {            \"NameId\": \"4aa21ae9-bc5f-49e2-8467-3a3ddbf1dadd\",            \"First\": \"Lisette\",            \"Middle\": \"Alexandros\",            \"Last\": \"Wabey\"        },        \"Email\": \"awabey9@rakuten.co.jp\",        \"Gender\": \"M\",        \"Type\": \"Associate\",        \"Location\": \"Tampa\",        \"Address\": null    },    {        \"UserId\": \"e6c47b83-d608-46c1-bb19-9623f331f47d\",        \"Name\": {            \"NameId\": \"d9903eb6-3ec2-493c-93ab-78d9fb4f4d3d\",            \"First\": \"Ashlen\",            \"Middle\": \"Gerianna\",            \"Last\": \"Vaud\"        },        \"Email\": \"gvauda@shutterfly.com\",        \"Gender\": \"M\",        \"Type\": \"Associate\",        \"Location\": \"Tampa\",        \"Address\": null    },    {        \"UserId\": \"c71685b7-dca1-40f2-8f93-022c63a4622c\",        \"Name\": {            \"NameId\": \"8b1ffb4a-b61c-42f3-b28e-3ad55de5aba5\",            \"First\": \"Mohandas\",            \"Middle\": null,            \"Last\": \"Cudmore\"        },        \"Email\": \"ncudmoreb@ovh.net\",        \"Gender\": \"F\",        \"Type\": \"Associate\",        \"Location\": \"Reston\",        \"Address\": null    },    {        \"UserId\": \"37ab32cc-26df-478a-8cfa-cc9d3563a0f2\",        \"Name\": {            \"NameId\": \"f9a0d187-3d66-4bc1-b433-beae99379713\",            \"First\": \"Joanne\",            \"Middle\": \"Mindy\",            \"Last\": \"Pyvis\"        },        \"Email\": \"mpyvisc@ow.ly\",        \"Gender\": \"M\",        \"Type\": \"Associate\",        \"Location\": \"Tampa\",        \"Address\": null    },    {        \"UserId\": \"8d76e671-814a-4d23-bd3b-8f55dc532a47\",        \"Name\": {            \"NameId\": \"f710df23-8238-472d-9740-0f7df17b5dd7\",            \"First\": \"Duke\",            \"Middle\": \"Bevvy\",            \"Last\": \"Blumer\"        },        \"Email\": \"bblumerd@w3.org\",        \"Gender\": \"F\",        \"Type\": \"Associate\",        \"Location\": \"Tampa\",        \"Address\": null    },    {        \"UserId\": \"b29dfabd-20c9-4a21-84cb-25dc9463a2b1\",        \"Name\": {            \"NameId\": \"a6cd61e7-66e9-4f74-b11b-821ad978ddfd\",            \"First\": \"Julienne\",            \"Middle\": \"Jimmy\",            \"Last\": \"Lock\"        },        \"Email\": \"jlocke@soup.io\",        \"Gender\": \"M\",        \"Type\": \"Associate\",        \"Location\": \"Reston\",        \"Address\": null    },    {        \"UserId\": \"be7e8bce-da50-4a07-961e-0a180e4b9303\",        \"Name\": {            \"NameId\": \"6e35effe-d603-4e30-b102-957f455563ac\",            \"First\": \"Itch\",            \"Middle\": null,            \"Last\": \"Magor\"        },        \"Email\": \"lmagorf@chicagotribune.com\",        \"Gender\": \"F\",        \"Type\": \"Associate\",        \"Location\": \"Reston\",        \"Address\": null    },    {        \"UserId\": \"f2ddb323-1ac4-4049-baee-d1b1274e0070\",        \"Name\": {            \"NameId\": \"c1e5a2d3-c7e7-410e-ad82-bae5e260a9b1\",            \"First\": \"Web\",            \"Middle\": null,            \"Last\": \"Scutts\"        },        \"Email\": \"jscuttsg@scribd.com\",        \"Gender\": \"M\",        \"Type\": \"Associate\",        \"Location\": \"Reston\",        \"Address\": null    },    {        \"UserId\": \"a6020109-cceb-43c0-abd5-56de213cb8b3\",        \"Name\": {            \"NameId\": \"ee7e65f9-3ee2-405e-9b18-48332e656ba9\",            \"First\": \"Tiffany\",            \"Middle\": null,            \"Last\": \"Miles\"        },        \"Email\": \"mmilesh@123-reg.co.uk\",        \"Gender\": \"M\",        \"Type\": \"Associate\",        \"Location\": \"Reston\",        \"Address\": null    },    {        \"UserId\": \"e49e1fa0-9cde-4a80-a1b5-8b10918cad71\",        \"Name\": {            \"NameId\": \"b0d7ae9b-0e90-492b-a158-ab767d53ae8c\",            \"First\": \"Wenonah\",            \"Middle\": \"Marabel\",            \"Last\": \"Burg\"        },        \"Email\": \"mburgi@cloudflare.com\",        \"Gender\": \"F\",        \"Type\": \"Associate\",        \"Location\": \"Reston\",        \"Address\": null    },    {        \"UserId\": \"822c472b-4ded-4a8f-ad8a-ffd7cede1b76\",        \"Name\": {            \"NameId\": \"54f28a9e-28ef-48eb-a515-25528b44f9fb\",            \"First\": \"Abby\",            \"Middle\": \"Davita\",            \"Last\": \"Hasell\"        },        \"Email\": \"dhasellj@flavors.me\",        \"Gender\": \"M\",        \"Type\": \"Associate\",        \"Location\": \"Tampa\",        \"Address\": null    },    {        \"UserId\": \"2c3399ab-29a2-4c9b-98c0-7fb60d1400d9\",        \"Name\": {            \"NameId\": \"647eea95-2555-48b2-b106-a494c6cf527c\",            \"First\": \"Sallyanne\",            \"Middle\": null,            \"Last\": \"Hauxley\"        },        \"Email\": \"ihauxleyk@cargocollective.com\",        \"Gender\": \"M\",        \"Type\": \"Associate\",        \"Location\": \"Reston\",        \"Address\": null    },    {        \"UserId\": \"b165e54e-30e3-4f5c-8f94-83e55d8acb3b\",        \"Name\": {            \"NameId\": \"5e50f7a0-50f9-4738-86a3-345047ba5ce9\",            \"First\": \"Mildred\",            \"Middle\": \"Ulrick\",            \"Last\": \"Tointon\"        },        \"Email\": \"utointonl@army.mil\",        \"Gender\": \"M\",        \"Type\": \"Associate\",        \"Location\": \"Reston\",        \"Address\": null    },    {        \"UserId\": \"d4b50c4c-06bd-46f7-824c-00329af3366d\",        \"Name\": {            \"NameId\": \"fa5e931d-3f31-4b26-a174-41fced42dc46\",            \"First\": \"Raimund\",            \"Middle\": \"Drake\",            \"Last\": \"Burchmore\"        },        \"Email\": \"dburchmorem@slashdot.org\",        \"Gender\": \"M\",        \"Type\": \"Associate\",        \"Location\": \"Tampa\",        \"Address\": null    },    {        \"UserId\": \"cbf7223e-8996-428e-a221-7b2030cbf2b4\",        \"Name\": {            \"NameId\": \"97880499-6c92-4a9c-9dc0-a558e5e9eb95\",            \"First\": \"Lora\",            \"Middle\": \"Mellicent\",            \"Last\": \"Birkmyre\"        },        \"Email\": \"mbirkmyren@uiuc.edu\",        \"Gender\": \"M\",        \"Type\": \"Associate\",        \"Location\": \"Reston\",        \"Address\": null    },    {        \"UserId\": \"1fac76fd-7fa3-4417-90fa-23267000ae9a\",        \"Name\": {            \"NameId\": \"d7c6cdfc-99bb-488f-af68-fb91b52a0a8a\",            \"First\": \"Cornela\",            \"Middle\": null,            \"Last\": \"Leyden\"        },        \"Email\": \"dleydeno@bluehost.com\",        \"Gender\": \"M\",        \"Type\": \"Associate\",        \"Location\": \"Reston\",        \"Address\": null    },    {        \"UserId\": \"c329f3af-c302-4fe6-a73f-7f1f93815c17\",        \"Name\": {            \"NameId\": \"16385980-d478-49f1-900c-b4fb8bda1ce9\",            \"First\": \"Carling\",            \"Middle\": \"Susan\",            \"Last\": \"Ecclestone\"        },        \"Email\": \"secclestonep@about.com\",        \"Gender\": \"F\",        \"Type\": \"Associate\",        \"Location\": \"Tampa\",        \"Address\": null    },    {        \"UserId\": \"dcc68954-3dde-4b0b-afdc-65c32a27cc77\",        \"Name\": {            \"NameId\": \"1e59bc83-d428-4c69-85f3-457c9ac3a864\",            \"First\": \"Odette\",            \"Middle\": \"Chaim\",            \"Last\": \"Collicott\"        },        \"Email\": \"ccollicottq@people.com.cn\",        \"Gender\": \"F\",        \"Type\": \"Associate\",        \"Location\": \"Tampa\",        \"Address\": null    },    {        \"UserId\": \"724fb5ac-9edc-45a0-b16a-d180f44403b3\",        \"Name\": {            \"NameId\": \"b6d5edda-eefb-48e5-830d-c432222c96eb\",            \"First\": \"Jillian\",            \"Middle\": \"Aluino\",            \"Last\": \"Loidl\"        },        \"Email\": \"aloidlr@auda.org.au\",        \"Gender\": \"F\",        \"Type\": \"Associate\",        \"Location\": \"Tampa\",        \"Address\": null    },    {        \"UserId\": \"22a3dcf1-a05f-46c7-a1f7-d813c0f8f729\",        \"Name\": {            \"NameId\": \"09ad1373-6808-4d8a-9526-4d1924dc4513\",            \"First\": \"Christi\",            \"Middle\": null,            \"Last\": \"Peacher\"        },        \"Email\": \"npeachers@nifty.com\",        \"Gender\": \"F\",        \"Type\": \"Associate\",        \"Location\": \"Reston\",        \"Address\": null    },    {        \"UserId\": \"2ae4b78e-7405-4fe8-b476-3b11fbf9f9c2\",        \"Name\": {            \"NameId\": \"a6e7a787-bb9a-4237-813c-947b6f6d1e51\",            \"First\": \"Kenon\",            \"Middle\": \"Allissa\",            \"Last\": \"Budnk\"        },        \"Email\": \"abudnkt@toplist.cz\",        \"Gender\": \"F\",        \"Type\": \"Associate\",        \"Location\": \"Reston\",        \"Address\": null    },    {        \"UserId\": \"d99e449e-0d7a-4901-908f-9a1244a822e6\",        \"Name\": {            \"NameId\": \"cf1b1d32-a86a-46fd-ab4f-d9341d6c7b3e\",            \"First\": \"Jacynth\",            \"Middle\": \"Rozamond\",            \"Last\": \"Cuffley\"        },        \"Email\": \"rcuffleyu@amazon.co.jp\",        \"Gender\": \"F\",        \"Type\": \"Associate\",        \"Location\": \"Reston\",        \"Address\": null    },    {        \"UserId\": \"fc55df63-467e-40fc-9165-c6b9b29b2c63\",        \"Name\": {            \"NameId\": \"c60216bc-edb6-4af0-be9c-cdcbe031393d\",            \"First\": \"Othilie\",            \"Middle\": \"Shayne\",            \"Last\": \"Dearsley\"        },        \"Email\": \"sdearsleyv@vistaprint.com\",        \"Gender\": \"F\",        \"Type\": \"Associate\",        \"Location\": \"Tampa\",        \"Address\": null    },    {        \"UserId\": \"3095a9bb-63e7-410f-adaa-83ee59b7d4b9\",        \"Name\": {            \"NameId\": \"7a711454-dfbd-4f16-a214-2701b541e9ef\",            \"First\": \"Veriee\",            \"Middle\": \"Burtie\",            \"Last\": \"Yashunin\"        },        \"Email\": \"byashuninw@eventbrite.com\",        \"Gender\": \"F\",        \"Type\": \"Associate\",        \"Location\": \"Tampa\",        \"Address\": null    },    {        \"UserId\": \"70925a0c-89fd-4233-a23d-7b5fb80bef42\",        \"Name\": {            \"NameId\": \"a7dc053c-9059-4f43-9e6e-e64174780041\",            \"First\": \"Claudine\",            \"Middle\": null,            \"Last\": \"Wilce\"        },        \"Email\": \"jwilcex@toplist.cz\",        \"Gender\": \"M\",        \"Type\": \"Associate\",        \"Location\": \"Tampa\",        \"Address\": null    },    {        \"UserId\": \"d4d20c37-7ea9-41ad-84ba-2a0097200069\",        \"Name\": {            \"NameId\": \"90d438e5-7d19-4622-a057-10750c676518\",            \"First\": \"Rubia\",            \"Middle\": \"Guinevere\",            \"Last\": \"Grimstead\"        },        \"Email\": \"ggrimsteady@macromedia.com\",        \"Gender\": \"M\",        \"Type\": \"Associate\",        \"Location\": \"Tampa\",        \"Address\": null    },    {        \"UserId\": \"33270a95-7591-444f-b3d0-c37c72473a8e\",        \"Name\": {            \"NameId\": \"54216ac5-014f-4f40-a6e2-77ca173b31fa\",            \"First\": \"Chauncey\",            \"Middle\": \"Rudd\",            \"Last\": \"McHan\"        },        \"Email\": \"rmchanz@abc.net.au\",        \"Gender\": \"M\",        \"Type\": \"Associate\",        \"Location\": \"Reston\",        \"Address\": null    },    {        \"UserId\": \"2f7ae9f2-f528-4ffd-8738-7b5bd633bdc0\",        \"Name\": {            \"NameId\": \"24d8d5e1-e430-4f90-b074-505798fc6317\",            \"First\": \"Kerrill\",            \"Middle\": \"Kippy\",            \"Last\": \"Newing\"        },        \"Email\": \"knewing10@umich.edu\",        \"Gender\": \"M\",        \"Type\": \"Associate\",        \"Location\": \"Tampa\",        \"Address\": null    },    {        \"UserId\": \"d1a22a63-0c83-490d-84eb-3ce2107a1965\",        \"Name\": {            \"NameId\": \"56f4b7c7-7956-4d0f-9f86-99a7ec1aeeee\",            \"First\": \"Ardra\",            \"Middle\": \"Mead\",            \"Last\": \"Skeene\"        },        \"Email\": \"mskeene11@china.com.cn\",        \"Gender\": \"M\",        \"Type\": \"Associate\",        \"Location\": \"Reston\",        \"Address\": null    },    {        \"UserId\": \"10756f25-481e-43ba-b560-581159ee14ce\",        \"Name\": {            \"NameId\": \"e457a115-3f28-4cb4-a377-fee41da6bde4\",            \"First\": \"Manolo\",            \"Middle\": null,            \"Last\": \"Quartermaine\"        },        \"Email\": \"tquartermaine12@zdnet.com\",        \"Gender\": \"M\",        \"Type\": \"Associate\",        \"Location\": \"Reston\",        \"Address\": null    },    {        \"UserId\": \"5bc164a4-27fd-43ac-9e49-052a5b621b8a\",        \"Name\": {            \"NameId\": \"734e0542-db96-4de4-a643-910199855983\",            \"First\": \"Wendel\",            \"Middle\": null,            \"Last\": \"Kerkham\"        },        \"Email\": \"ekerkham13@dropbox.com\",        \"Gender\": \"M\",        \"Type\": \"Associate\",        \"Location\": \"Reston\",        \"Address\": null    },    {        \"UserId\": \"605a1e5d-49b3-400a-946c-d811a0d29454\",        \"Name\": {            \"NameId\": \"5b483e25-8175-4406-b3ee-a71294af92c5\",            \"First\": \"Livy\",            \"Middle\": \"Leicester\",            \"Last\": \"Burgher\"        },        \"Email\": \"lburgher14@dagondesign.com\",        \"Gender\": \"M\",        \"Type\": \"Associate\",        \"Location\": \"Reston\",        \"Address\": null    },    {        \"UserId\": \"16eed2f4-627e-46a9-ba8d-684886decbfd\",        \"Name\": {            \"NameId\": \"e480ec9f-4461-4aa4-88e9-41c2f1787233\",            \"First\": \"Jilli\",            \"Middle\": null,            \"Last\": \"Ruddell\"        },        \"Email\": \"cruddell15@1und1.de\",        \"Gender\": \"F\",        \"Type\": \"Associate\",        \"Location\": \"Tampa\",        \"Address\": null    },    {        \"UserId\": \"522d473b-2294-4151-9b81-885cbcef8d82\",        \"Name\": {            \"NameId\": \"581dee5d-8deb-4d1a-be72-29bbcd1091af\",            \"First\": \"Olly\",            \"Middle\": null,            \"Last\": \"Wrathall\"        },        \"Email\": \"dwrathall16@zimbio.com\",        \"Gender\": \"F\",        \"Type\": \"Associate\",        \"Location\": \"Tampa\",        \"Address\": null    },    {        \"UserId\": \"e33ab8fd-b784-412d-8408-0d7fa5d910be\",        \"Name\": {            \"NameId\": \"fe6f8fc2-d89e-44ee-ba50-fb39b3785482\",            \"First\": \"Gwynne\",            \"Middle\": \"Clo\",            \"Last\": \"Swalwel\"        },        \"Email\": \"cswalwel17@fema.gov\",        \"Gender\": \"M\",        \"Type\": \"Associate\",        \"Location\": \"Tampa\",        \"Address\": null    },    {        \"UserId\": \"fa228d7a-3754-45cd-83b7-79c1e93996a7\",        \"Name\": {            \"NameId\": \"b60c8e59-212a-4224-9ed5-124d3d3d54f4\",            \"First\": \"Nappy\",            \"Middle\": null,            \"Last\": \"Milmoe\"        },        \"Email\": \"amilmoe18@pcworld.com\",        \"Gender\": \"M\",        \"Type\": \"Associate\",        \"Location\": \"Tampa\",        \"Address\": null    },    {        \"UserId\": \"ead96b6c-de0d-4cf8-a112-516915abee8f\",        \"Name\": {            \"NameId\": \"7135926e-0059-4509-bebe-42bb55c26a3b\",            \"First\": \"Emmanuel\",            \"Middle\": \"Nana\",            \"Last\": \"Lerego\"        },        \"Email\": \"nlerego19@csmonitor.com\",        \"Gender\": \"M\",        \"Type\": \"Associate\",        \"Location\": \"Reston\",        \"Address\": null    },    {        \"UserId\": \"6e35d28f-fd41-4230-a42e-82b3dbcccf04\",        \"Name\": {            \"NameId\": \"0407d6ca-8d5a-4c87-818b-16f6a9a63c22\",            \"First\": \"Jasmine\",            \"Middle\": null,            \"Last\": \"Juorio\"        },        \"Email\": \"ejuorio1a@jimdo.com\",        \"Gender\": \"M\",        \"Type\": \"Associate\",        \"Location\": \"Tampa\",        \"Address\": null    },    {        \"UserId\": \"af9ea656-ab1b-4d01-94f1-8ad07960136d\",        \"Name\": {            \"NameId\": \"3e634aeb-dbfb-49bd-9e40-dd939b5bd125\",            \"First\": \"Jemmy\",            \"Middle\": \"Emyle\",            \"Last\": \"Filtness\"        },        \"Email\": \"efiltness1b@narod.ru\",        \"Gender\": \"F\",        \"Type\": \"Associate\",        \"Location\": \"Tampa\",        \"Address\": null    },    {        \"UserId\": \"20a69a47-921b-42ff-a88f-83e71b5bdd06\",        \"Name\": {            \"NameId\": \"56cd9e3c-ce15-4022-ab09-44a2a48068dc\",            \"First\": \"Karoly\",            \"Middle\": null,            \"Last\": \"Landall\"        },        \"Email\": \"blandall1c@furl.net\",        \"Gender\": \"M\",        \"Type\": \"Associate\",        \"Location\": \"Reston\",        \"Address\": null    },    {        \"UserId\": \"ce6af605-98bd-4bcc-97c4-9cfcd6781ecf\",        \"Name\": {            \"NameId\": \"6888df4a-1868-4f59-a122-601b49bac5f1\",            \"First\": \"Winonah\",            \"Middle\": \"Isahella\",            \"Last\": \"Tippell\"        },        \"Email\": \"itippell1d@moonfruit.com\",        \"Gender\": \"M\",        \"Type\": \"Associate\",        \"Location\": \"Tampa\",        \"Address\": null    },    {        \"UserId\": \"2a1a98ea-12a6-4570-9202-e95a3eac6486\",        \"Name\": {            \"NameId\": \"7d3851b0-6690-478d-9403-d886b7349d69\",            \"First\": \"Amelina\",            \"Middle\": null,            \"Last\": \"Cruces\"        },        \"Email\": \"ccruces1e@xrea.com\",        \"Gender\": \"F\",        \"Type\": \"Associate\",        \"Location\": \"Tampa\",        \"Address\": null    },    {        \"UserId\": \"5e371598-043c-4235-8c3b-aaf805cd5430\",        \"Name\": {            \"NameId\": \"ef45b706-2ec2-48a8-9164-dacafa03d961\",            \"First\": \"Marcelo\",            \"Middle\": \"Leif\",            \"Last\": \"Benedite\"        },        \"Email\": \"lbenedite1f@cbslocal.com\",        \"Gender\": \"M\",        \"Type\": \"Associate\",        \"Location\": \"Tampa\",        \"Address\": null    },    {        \"UserId\": \"788f76ef-09ca-4dfe-a527-408d1483c834\",        \"Name\": {            \"NameId\": \"b2f9a305-9531-4970-b79a-e8b905980933\",            \"First\": \"Clarance\",            \"Middle\": \"Shaine\",            \"Last\": \"Cheng\"        },        \"Email\": \"scheng1g@topsy.com\",        \"Gender\": \"M\",        \"Type\": \"Associate\",        \"Location\": \"Tampa\",        \"Address\": null    },    {        \"UserId\": \"0550e1a0-9ee8-487d-88b5-80f55744aeb2\",        \"Name\": {            \"NameId\": \"9d581e53-0245-4462-b5f0-2e0f3769fac7\",            \"First\": \"Errol\",            \"Middle\": null,            \"Last\": \"Draxford\"        },        \"Email\": \"fdraxford1h@people.com.cn\",        \"Gender\": \"F\",        \"Type\": \"Associate\",        \"Location\": \"Tampa\",        \"Address\": null    },    {        \"UserId\": \"eafc925a-890f-4c94-8423-13178f9382dc\",        \"Name\": {            \"NameId\": \"de877e67-3411-434a-a6fc-9c237afc9f77\",            \"First\": \"Geri\",            \"Middle\": null,            \"Last\": \"Stallworth\"        },        \"Email\": \"lstallworth1i@wisc.edu\",        \"Gender\": \"M\",        \"Type\": \"Associate\",        \"Location\": \"Tampa\",        \"Address\": null    },    {        \"UserId\": \"9bf77209-a1cc-4bc5-be38-43c27ac8168a\",        \"Name\": {            \"NameId\": \"243e3bb6-c70b-4484-a2c7-695d79a91372\",            \"First\": \"Edwina\",            \"Middle\": null,            \"Last\": \"Beech\"        },        \"Email\": \"mbeech1j@springer.com\",        \"Gender\": \"M\",        \"Type\": \"Associate\",        \"Location\": \"Reston\",        \"Address\": null    },    {        \"UserId\": \"9681a308-e80d-41c4-af6a-b78b5b9c8fb2\",        \"Name\": {            \"NameId\": \"decb4329-6dae-46b4-abd0-3e13a436ad21\",            \"First\": \"Aksel\",            \"Middle\": \"Catherina\",            \"Last\": \"Gransden\"        },        \"Email\": \"cgransden1k@free.fr\",        \"Gender\": \"M\",        \"Type\": \"Associate\",        \"Location\": \"Tampa\",        \"Address\": null    },    {        \"UserId\": \"3991c055-25fb-4bfe-a00d-c5d18b381f34\",        \"Name\": {            \"NameId\": \"96816472-8799-4757-b822-bc4710aea429\",            \"First\": \"Broddy\",            \"Middle\": null,            \"Last\": \"Mayward\"        },        \"Email\": \"pmayward1l@angelfire.com\",        \"Gender\": \"F\",        \"Type\": \"Associate\",        \"Location\": \"Reston\",        \"Address\": null    },    {        \"UserId\": \"38954181-fe8c-4c72-be41-2324fd59530d\",        \"Name\": {            \"NameId\": \"6e04ef78-dd6d-4519-a966-f67066a20439\",            \"First\": \"Mord\",            \"Middle\": null,            \"Last\": \"Harriott\"        },        \"Email\": \"mharriott1m@howstuffworks.com\",        \"Gender\": \"F\",        \"Type\": \"Associate\",        \"Location\": \"Tampa\",        \"Address\": null    },    {        \"UserId\": \"67bb921c-b537-4b2c-833c-f219be6e5882\",        \"Name\": {            \"NameId\": \"5d8ef565-5ed5-4a3b-a09e-11467ae0226b\",            \"First\": \"Scarlett\",            \"Middle\": null,            \"Last\": \"McFadzean\"        },        \"Email\": \"hmcfadzean1n@lycos.com\",        \"Gender\": \"F\",        \"Type\": \"Associate\",        \"Location\": \"Tampa\",        \"Address\": null    }]";
        }

        [HttpPost]
        [Route("seed")]
        public async Task<IActionResult> Post()
        {
            try
            {
                const string connectionString = @"mongodb://db";
                IMongoCollection<User.Context.Models.User> mc =
                    new MongoClient(connectionString)
                        .GetDatabase("userdb")
                        .GetCollection<User.Context.Models.User>("users");

                UserStorage context = new UserStorage(new UserRepository(mc));
                //string jsonStr = System.IO.File.ReadAllText("../MockUsers.json");
                string jsonStr = GetUsers();
                List<User.Context.Models.User> users = 
                    Deserialize<List<User.Context.Models.User>>(jsonStr);

                foreach (var user in users)
                {
                    await Task.Run(() => context.Insert(user));
                }
            }
            catch(Exception e)
            {
                return BadRequest(e);
            }

            return Ok();
        }

        // Deserialize JSON string and return object.
        private T Deserialize<T>(string jsonStr)
        {
            T obj = default(T);
            MemoryStream ms = new MemoryStream();
            try
            {
                DataContractJsonSerializer ser =
                    new DataContractJsonSerializer(typeof(T));
                StreamWriter writer = new StreamWriter(ms);
                writer.Write(jsonStr);
                writer.Flush();
                ms.Position = 0;
                obj = (T)ser.ReadObject(ms);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                ms.Close();
            }
            return obj;
        }

        /// <summary>
        /// Get all users.
        /// </summary>
        /// <returns>OkObjectResult with an IEnumerable of all users,
        /// or a 500 StatusCodeResult if an error occurs.</returns>
        [HttpGet]
        [ProducesResponseType(500)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Library.Models.User>))]
        public async Task<IActionResult> Get()
        {
            try
            {
                var contextUsers = _userStorage.Get();
                var libraryUsers = UserModelMapper.List_ContextToLibrary(contextUsers);
                if(libraryUsers == null) { return new StatusCodeResult(500); }
                return await Task.Run(() => Ok(libraryUsers));
            }
            catch
            {
                return new StatusCodeResult(500);
            }
        }

        /// <summary>
        /// Finds the user based on the id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>the user with matching Id, or a 404 error</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(Library.Models.User))]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var libraryUser = UserModelMapper.ContextToLibrary(_userStorage.GetById(id));
                if (libraryUser == null)
                {
                    return NotFound();
                }
                else
                {
                    return await Task.Run(() => Ok(libraryUser));
                }
            }
            catch (Exception e)
            {
                return NotFound(e);
            }
        }

        /// <summary>
        /// Finds the users by Gender.
        /// </summary>
        /// <param name="gender"></param>
        /// <returns>all users with the specified gender, or a 400 error
        /// if the gender isn't valid, or a 500 error if a database error
        /// occured.</returns>
        [HttpGet("gender/{gender}")]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Library.Models.User>))]
        public async Task<IActionResult> GetByGender(string gender)
        {
            string[] genders = ServiceHub.User.Library.Models.User.ValidUppercaseGenders;
            string upperGender = gender.ToUpper();
            bool validGender = false;

            foreach (var x in genders)
            {
                if (upperGender == x)
                {
                    validGender = true;
                }
            }

            if (!validGender)
            {
                return BadRequest($"Invalid gender: {gender}.");
            }
            else
            {
                var users = _userStorage.Get();
                var GUsers = new List<ServiceHub.User.Library.Models.User>();

                foreach (var x in users)
                {
                    if(x.Gender.ToUpper() == upperGender)
                    {
                        var libraryUser = UserModelMapper.ContextToLibrary(x);
                        if(libraryUser == null) { return new StatusCodeResult(500); }
                        GUsers.Add(libraryUser);
                    }
                }
                return await Task.Run(() => Ok(GUsers));
            }
        }


        /// <summary>
        /// Gets all users of a certain type.
        /// </summary>
        /// <param name="type"> A string representing the type of user to filter by. </param>
        /// <returns>If the type is not an accepted type, returns a 400 StatusCodeResult. If
        /// the server fails to return a complete list of valid users, returns a 500
        /// StatusCodeResut. Otherwise returns a list of validated Users. </returns>
        [HttpPost]
        [Route("type")]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Library.Models.User>))]
        public async Task<IActionResult> GetByType([FromBody] string type)
        {
            if(type == null) { return BadRequest("Invalid type."); }
            bool isValidType = false;
            foreach (var validType in Library.Models.User.ValidUppercaseTypes)
            {
                if (type.ToUpper() == validType) { isValidType = true; }
            }
            if (!isValidType) { return BadRequest("Invalid type."); }

            try
            {
                var users = await Task.Run(() => _userStorage.Get());
                var contextUsers = new List<Context.Models.User>();
                foreach (var contextUser in users)
                {
                    if(contextUser.Type.ToUpper() == type.ToUpper())
                    {
                        contextUsers.Add(contextUser);
                    }
                }
                var libraryUsers = UserModelMapper.List_ContextToLibrary(contextUsers);
                return Ok(libraryUsers);
            }
            catch
            {
                return new StatusCodeResult(500);
            }
        }

        /// <summary>
        /// Updates the user's address and/or location
        /// </summary>
        /// <param name="user">the user to update</param>
        /// <returns>200 Ok if the update is successful, 400 Bad Request
        /// if the user id, location or address are invalid, or 500
        /// Internal Server Error if a database error occurs.</returns>
        [HttpPut]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Put([FromBody]ServiceHub.User.Library.Models.User user)
        {
            try
            {
                if (user == null)
                {
                    return BadRequest("Invalid user: object was null");
                }
                else
                {
                    var id = user.UserId;
                    if(user.UserId == Guid.Empty) { return BadRequest("Invalid User Id"); }
                    var contextUser = _userStorage.GetById(user.UserId);
                    if(contextUser == null) { return BadRequest("User not found"); }
                    var libraryUser = UserModelMapper.ContextToLibrary(contextUser);
                    if(libraryUser == null) { return new StatusCodeResult(500); }

                    if(user.Location != null) { libraryUser.Location = user.Location; }
                    libraryUser.Address = user.Address;
                    contextUser = UserModelMapper.LibraryToContext(libraryUser);
                    if(contextUser == null) { return BadRequest("Invalid update of location or address."); }
                    _userStorage.Update(contextUser);
                    return await Task.Run(() => Ok());
                }
            }
            catch
            {
                return new StatusCodeResult(500);
            }
       }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="user"> A User model to be provided from an external source
        /// via JSON.  If the model is a valid model, it will be cast to a db-ready model
        /// and stored in the database. </param>
        /// <returns> If the user is accepted, it will return a 202, Accepted code.
        /// Otherwise, it will return a 400, client-error code. </returns>
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(202)]
        public async Task<IActionResult> Post([FromBody] User.Library.Models.User user)
        {
            if(user == null) { return BadRequest("Invalid user: User is null"); }
            var contextUser = UserModelMapper.LibraryToContext(user);
            if(contextUser == null) { return BadRequest("Invalid user: Validation failed"); }
            _userStorage.Insert(contextUser);
            return await Task.Run(() => Accepted());
        }
    }
}
