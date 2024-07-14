namespace GaragesStructure.Utils;

public class RandomNumberGenerator
{
    public static long GenerateRandom16DigitNumber()
    {
        Random random = new Random();
        long min = 1000000000; 
        long max = 9999999999; 

        long sixteenDigitNumber = (long)(random.NextDouble() * (max - min) + min);
        return sixteenDigitNumber;
    }
}