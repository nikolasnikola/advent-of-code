using System.Text.Json;

namespace AdventOfCode
{
    public class Day13Comparer : IComparer<string>
    {
        public int Compare(string? x, string? y)
        {
            var items1 = JsonSerializer.Deserialize<JsonElement>(x);
            var items2 = JsonSerializer.Deserialize<JsonElement>(y);

            var compared = CompareArrays(items1, items2);

            return compared.HasValue ? compared.Value ? -1 : 1 : 0;
        }

        bool? CompareArrays(JsonElement items1, JsonElement items2)
        {
            for (int i = 0; i < Math.Max(items1.GetArrayLength(), items2.GetArrayLength()); i++)
            {
                if (items1.GetArrayLength() == i)
                {
                    return true;
                }

                if (items2.GetArrayLength() == i)
                {
                    return false;
                }

                JsonElement currentItem1 = items1[i];
                JsonElement currentItem2 = items2[i];

                if (IsInteger(currentItem1) && IsInteger(currentItem2))
                {
                    if (currentItem1.GetInt32() > currentItem2.GetInt32())
                    {
                        return false;
                    }
                    if (currentItem1.GetInt32() < currentItem2.GetInt32())
                    {
                        return true;
                    }
                }
                if (isArray(currentItem1) && isArray(currentItem2))
                {
                    var compared = CompareArrays(currentItem1, currentItem2);
                    if (compared.HasValue) return compared.Value;
                }
                if (isArray(currentItem1) && IsInteger(currentItem2))
                {
                    var compared = CompareArrays(currentItem1, JsonSerializer.SerializeToElement(new int[] { currentItem2.GetInt32() }));
                    if (compared.HasValue) return compared.Value;
                }
                if (IsInteger(currentItem1) && isArray(currentItem2))
                {
                    var compared = CompareArrays(JsonSerializer.SerializeToElement(new int[] { currentItem1.GetInt32() }), currentItem2);
                    if (compared.HasValue) return compared.Value;
                }
            }

            return null;
        }

        bool IsInteger(JsonElement item)
        {
            return item.ValueKind == JsonValueKind.Number;
        }

        bool isArray(JsonElement item)
        {
            return item.ValueKind == JsonValueKind.Array;
        }
    }
}
