namespace ArtSolutions.MUN.Core.AccountModels
{
    public class AccountServiceNoteModel
    {
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public int AccountServiceID { get; set; }
        public string Note { get; set; }
        public string CreatedBy { get; set; }
        public System.Guid CreatedUserID { get; set; }
        public System.DateTime CreatedDate { get; set; }
    }
}
