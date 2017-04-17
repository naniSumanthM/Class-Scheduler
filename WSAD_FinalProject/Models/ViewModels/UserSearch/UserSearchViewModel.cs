using System.Runtime.Serialization;

namespace WSAD_FinalProject.Models.ViewModels.UserSearch
{
    [DataContract]
    public class UserSearchViewModel
    {
        public UserSearchViewModel(Data.User userDTO)
        {
            UserId = userDTO.UserId;
            UserFirstName = userDTO.UserFirstName;
            UserLastName = userDTO.UserLastName;
            UserEmailAddress = userDTO.UserEmailAddress;
            UserCompany = userDTO.UserCompany;
        }

        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public string UserFirstName { get; set; }
        [DataMember]
        public string UserLastName { get; set; }
        [DataMember]
        public string UserCompany { get; set; }
        [DataMember]
        public string UserEmailAddress { get; set; }
    }
}