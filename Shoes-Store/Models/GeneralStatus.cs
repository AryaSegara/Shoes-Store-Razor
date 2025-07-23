namespace Shoes_Store.Models
{
    public class GeneralStatus
    {
        public enum GeneralStatusData
        {
            Published,// semua bisa lihat
            Unpublished,//admin aja yang lihat
            delete//cuman ad didata base
        }
    }
}
