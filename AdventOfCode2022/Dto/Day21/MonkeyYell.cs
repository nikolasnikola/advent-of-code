namespace AdventOfCode2022.Dto.Day21
{
    public class MonkeyYell
    {
        public MonkeyYell(string name)
        {
            MonkeyName = name;
        }

        public string MonkeyName { get; set; }

        public long? Number { get; set; }

        public string OperationMonkey1 { get; set; }

        public string OperationMonkey2 { get; set; }

        public char Operation { get; set; }

        public void CalculateOperation(long monkey1number, long monkey2number) => Number = Operation switch
        {
            '+' => monkey1number + monkey2number,
            '-' => monkey1number - monkey2number,
            '*' => monkey1number * monkey2number,
            _ => monkey1number / monkey2number,
        };
    }
}
