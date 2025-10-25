namespace BagStore.Web.AppConfig.Interface
{
    public interface IEnumMapper
    {
        TEnum MapToEnum<TEnum>(string value) where TEnum : struct, Enum;
        string MapToString<TEnum>(TEnum enumValue) where TEnum : struct, Enum;
    }
}
