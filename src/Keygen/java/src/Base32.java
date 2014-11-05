/**
 * Created by beeven on 11/4/14.
 */

public class Base32
{
    private static final String base32Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";

    public Base32()
    {
    }

    public static String encode(byte[] bytesToEncode)
    {
        StringBuilder result = new StringBuilder((bytesToEncode.length*8-1)/5+1);
        int buffer = bytesToEncode[0];
        int next = 1;
        int bitsLeft = 8;
        while(bitsLeft > 0 || next < bytesToEncode.length) {
            if(bitsLeft < 5) {
                if(next < bytesToEncode.length) {
                    buffer <<= 8;
                    buffer |= bytesToEncode[next++] & 0xff;
                    bitsLeft += 8;
                } else {
                    int pad = 5 - bitsLeft;
                    buffer <<= pad;
                    bitsLeft += pad;
                }
            }
            int index = 0x1f & (buffer >> (bitsLeft - 5));
            bitsLeft -= 5;
            result.append(base32Chars.charAt(index));
        }
        //result.append('\000');
        return result.toString();
    }

    public static byte[] decode(String stringToDecode)
    {
        int buffer = 0;
        int bitsLeft = 0;
        int tmp = 0;
        int count = 0;
        byte[] result = new byte[(stringToDecode.length() * 5 +7) / 8];
        for(int index = 0; index < stringToDecode.length(); index++) {
            char ch = stringToDecode.charAt(index);
            if(ch == '=') break;
            if(ch == ' ' || ch == '\t' || ch == '\r' || ch == '\n' || ch == '-') {
                continue;
            }

            buffer <<= 5;
            if(ch == '0') ch = 'O';
            else if (ch == '1') ch = 'L';
            else if (ch == '8') ch = 'B';

            if((ch >= 'A' && ch <= 'Z') || (ch >= 'a' && ch <= 'z'))
                tmp = (ch & 0x1f) - 1;
            else if (ch >= '2' && ch <= '7')
                tmp -= '2' - 26;
            else
                throw new ArithmeticException();
            buffer |= tmp;
            bitsLeft += 5;
            if(bitsLeft >= 8) {
                result[count++] =(byte) (buffer >> (bitsLeft - 8));
                bitsLeft -= 8;
            }
        }
        return result;
    }

    public static void main(String[] args)
    {
        String encoded = "MFRGGZDF";
        System.out.println(" Original: " + encoded);
        byte[] arrayOfByte = decode(encoded);
        System.out.print("      Hex: ");
        for (int i = 0; i < arrayOfByte.length; i++)
        {
            int j = arrayOfByte[i];
            if (j < 0)
                j += 256;
            System.out.print(Integer.toHexString(j + 256).substring(1));
        }
        System.out.println();
        System.out.println("Reencoded: " + encode(arrayOfByte));
    }
}