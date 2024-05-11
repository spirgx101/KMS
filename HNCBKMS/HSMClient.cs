/***************************************************************************************************************/
/* HSM Client                                                           建立日期：2014.11.30   建立者：Vincent */
/*=============================================================================================================*/
/* V1.0.0   2015.03.06    新增 SoftNet Luna EFT function                                  2015.03.10   Vincent */
/* V1.1.0   2015.04.27    修改 SoftNet Luna EFT function                                  2015.04.27   Vincent */
/* V1.2.0   2015.05.06    修改 計算CVV值代入資料為Padding '0'                             2015.05.06   Vincent */
/***************************************************************************************************************/


using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace HSMClient
{
    class HSM
    {
        protected TcpClient Client;
        private const int reciveLens = 1024;
        private Byte[] reciveData = new Byte[reciveLens];
        private Boolean connect = false;
        private static string response = "";
        
        public HSM(TcpClient client)
        {
            this.Client = client;
        }

        public void initOps()
        {
            connect = true;
            Client.GetStream().BeginRead(reciveData, 0, reciveLens, new AsyncCallback(StreamReceive), null);
        }

        public Boolean isConnect()
        {
            return connect;
        }

        public void TermClient()
        {
            try
            {
                connect = false;
                Client.GetStream().Close();
                Client.Close();
            }
            catch(Exception ex) {}           

        }

        private void StreamReceive(IAsyncResult ar)
        {
            int ByteCount = 0;

            try
            {
                lock (Client.GetStream())
                {
                    ByteCount = Client.GetStream().EndRead(ar);
                }

                if (ByteCount < 1)
                {
                    connect = false;
                    Client.Close();
                    return;
                }

                //response += System.Text.ASCIIEncoding.GetEncoding(System.Globalization.CultureInfo.CurrentCulture.TextInfo.ANSICodePage).GetString(reciveData,2,ByteCount);
                if(ByteCount > 0)
                    response += BitConverter.ToString(reciveData, 0, ByteCount).Replace("-", "");
                //response += Encoding.Default.GetString(reciveData, 2, ByteCount);

                lock (Client.GetStream())
                {
                    Client.GetStream().BeginRead(reciveData, 0, reciveLens, new AsyncCallback(StreamReceive), null);
                }

            }
            catch (Exception ex)
            {
                connect = false;
                Client.Close();
                Console.Write("Exception: " + ex.ToString());
            }
        }

        public String sendHexString(String hexString)
        {
            Byte[] buffer = new Byte[hexString.Length / 2];
            for (int i = 0; i < hexString.Length / 2; i++)
                buffer[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);            
            
            lock (Client.GetStream())
            {
                Client.GetStream().BeginWrite(buffer, 0, buffer.Length, null, null);
            }
            return getResponse();
        }

        public String send(String command)
        {
            Byte[] buffer = Encoding.ASCII.GetBytes("  " + command);
            //Console.Write("Send: [" + command + "]\n");
            buffer[0] = Convert.ToByte((int)command.Length / 256);
            buffer[1] = Convert.ToByte(command.Length % 256);


            lock (Client.GetStream())
            {
                Client.GetStream().BeginWrite(buffer, 0, buffer.Length, null, null);
            }

            return getResponse();
        }

        public String send(Byte[] command)
        {
            lock (Client.GetStream())
            {
                Client.GetStream().BeginWrite(command, 0, command.Length, null, null);
            }

            return getResponse();
        }

        public String getResponse()
        {
            response = "";
            while ((response == "") && (connect))
            {
                System.Threading.Thread.Sleep(5);
            }
            return response;
        }
    }

    class ThalesHSM : HSM
    {
        public enum DesMode { ECB = 0, CBC = 1 };
        public enum KeyPair { KP30 = 0, KP14 = 1, KP16 = 2, KP18 = 3, KP20 = 4, KP22 = 5, KP28 = 6, KP38 = 7 };
        public enum KeyLength { Single = 0, Double = 1 };  
        private const String header = "1234";

        public ThalesHSM(TcpClient client) : base(client)
        {
        }

        public String DES_Encrypt(DesMode mode, KeyLength length, int keyVariant, KeyPair pair, String key, String data, String IV = "")
        {
            String command = String.Empty;
            String commandCode = "YY";
            String msg = String.Empty;
            int commandLength = 0;

            if(mode == DesMode.CBC && IV == "")
                throw new Exception("CBC mode need IV value.");

            if((length == KeyLength.Single && key.Length != 16) || 
               (length == KeyLength.Double && key.Length != 32))
                throw new Exception("Key length error.");

            if ((data.Length / 2) % 8 != 0)
                throw new Exception("Data length must in multiples of 8 bytes.");

            commandLength = 10 + key.Length + IV.Length / 2 + 3 + data.Length / 2;

            command = header + commandCode + ((int)mode).ToString() + ((int)length).ToString() 
                                        + keyVariant.ToString() + ((int)pair).ToString() + key;

            command = Convert.ToString(commandLength, 16).PadLeft(4, '0')
                                        + BitConverter.ToString(Encoding.ASCII.GetBytes(command)).Replace("-", "");
            command += IV;
            command += BitConverter.ToString(Encoding.ASCII.GetBytes((data.Length / 2).ToString("000"))).Replace("-", "");
            command += data;


            msg = send(SoapHexBinary.Parse(command).Value);

            if (msg.Substring(16, 4) != "3030")
                ReturnErrorMsg(msg.Substring(16, 4));

            return msg.Substring(20);
        }

        public String DES_Decrypt(DesMode mode, KeyLength length, int keyVariant, KeyPair pair, String key, String data, String IV = "")
        {
            String command = String.Empty;
            String commandCode = "ZC";
            String msg = String.Empty;
            int commandLength = 0;

            if (mode == DesMode.CBC && IV == "")
                throw new Exception("CBC mode need IV value.");

            if ((length == KeyLength.Single && key.Length != 16) ||
               (length == KeyLength.Double && key.Length != 32))
                throw new Exception("Key length error.");

            if ((data.Length / 2) % 8 != 0)
                throw new Exception("Data length must in multiples of 8 bytes.");

            commandLength = 10 + key.Length + IV.Length / 2 + 3 + data.Length / 2;

            command = header + commandCode + ((int)mode).ToString() + ((int)length).ToString()
                                        + keyVariant.ToString() + ((int)pair).ToString() + key;

            command = Convert.ToString(commandLength, 16).PadLeft(4, '0')
                                        + BitConverter.ToString(Encoding.ASCII.GetBytes(command)).Replace("-", "");
            command += IV;
            command += BitConverter.ToString(Encoding.ASCII.GetBytes((data.Length / 2).ToString("000"))).Replace("-", "");
            command += data;

            msg = send(SoapHexBinary.Parse(command).Value);

            if (msg.Substring(16, 4) != "3030")
                ReturnErrorMsg(msg.Substring(16, 4));

            return msg.Substring(20);

        }

        private void ReturnErrorMsg(String errorCode)
        {
            switch (errorCode)
            {
                case "3032":
                    throw new Exception("Error 02 : Invalid Encryption mode");
                case "3033":
                    throw new Exception("Error 03 : Invalid Encryption Key Length");
                case "3034":
                    throw new Exception("Error 04 : Invalid Encryption pair");
                case "3036":
                    throw new Exception("Error 06 : Invalid Key Variant");
                case "3130":
                    throw new Exception("Error 10 : Encryption Key parity error");
                case "3132":
                    throw new Exception("Error 12 : No keys loaded in user storage");
                case "3133":
                    throw new Exception("Error 13 : LMK error, report to supervisor");
                case "3135":
                    throw new Exception("Error 15 : Invalid input data");
                case "3231":
                    throw new Exception("Error 21 : Invalid user storage index");
                case "3830":
                    throw new Exception("Error 80 : Length of Clear Text Data not equal to Data Length or length is not divisible by 8");
            }
        }
    }

    class LunaEFT : HSM
    {
        public enum PIN_Block_Formats : int
        {

            ANSI = 1, Docutel2 = 2, PIN_Pad = 3, Docutel = 8, ZKA = 9,
            ISO0 = 10, ISO1 = 11, ISO2 = 12, ISO3 = 13
        };

        public enum HSM_Key_Specifier_Formats
        {
            Short_BCD = 0,
            Short_Binary = 1,
            Long_BCD = 2,
            Long_Binary = 3
        };

        public enum Chiper_Mode
        {
            ECB = 0,
            CBC = 1
        };

        private const String Header = "01010000";
        private const String Function_Modifier = "00";

        public LunaEFT(TcpClient client) : base(client)
        {
        }

        public String LunaEFT_EE0E04_PIN_GENERATE(int PIN_LEN, PIN_Block_Formats PFo, String PAN, 
                                                    HSM_Key_Specifier_Formats PPK_Format, int PPK_Index)
        {
            String cmd = String.Empty;
            String funcCode = "EE0E04";
            String ANB = PAN.Substring(4);
            String msg = String.Empty;

            cmd = funcCode + Function_Modifier + PIN_LEN.ToString("D2") + PFo.ToString("D").PadLeft(2,'0') + 
                        ANB + PPK_Format.ToString("D").PadLeft(2,'0') + PPK_Index.ToString("D4");
            cmd = Header + (cmd.Length / 2).ToString("X4") + cmd;

            msg = sendHexString(cmd);

            if (msg.Substring(18, 2) != "00") 
                ReturnErrorMsg(msg.Substring(18, 2));

            return msg.Substring(msg.Length - 16);
        }

        public String LunaEFT_EE0802_CVV_GENERATE(HSM_Key_Specifier_Formats Key_Format, int CVK_Index, 
                                                        String PAN, String ExpireDate, String ServiceCode)
        {
            String cmd = String.Empty;
            String funcCode = "EE0802";
            String msg = String.Empty;

            cmd = funcCode + Function_Modifier + Key_Format.ToString("D").PadLeft(2, '0') + 
                            CVK_Index.ToString("D4") + (PAN+ExpireDate+ServiceCode).PadRight(32,'0');            

            cmd = Header + (cmd.Length / 2).ToString("X4") + cmd;

            msg = sendHexString(cmd);

            if (msg.Substring(18, 2) != "00")
                ReturnErrorMsg(msg.Substring(18, 2));

            return msg.Substring(msg.Length - 4, 3);
        }

        public String LunaEFT_EE0803_CVV_VERIFY(HSM_Key_Specifier_Formats CVK_Format, int CVK_Index,
                                                        String PAN, String ExpireDate, String ServiceCode, int CVV)
        {
            String cmd = String.Empty;
            String funcCode = "EE0803";
            String msg = String.Empty;

            cmd = funcCode + Function_Modifier + CVK_Format.ToString("D").PadLeft(2, '0') + CVK_Index.ToString("D4") 
                       + (PAN + ExpireDate + ServiceCode).PadRight(32, '0') + CVV.ToString().PadRight(4, 'F');

            cmd = Header + (cmd.Length / 2).ToString("X4") + cmd;

            msg = sendHexString(cmd);

            if (msg.Substring(18, 2) != "00")
                ReturnErrorMsg(msg.Substring(18, 2));

            return msg.Substring(msg.Length - 2);
        }

        public String LunaEFT_EE0607_PVV_CALC(String ePIN, HSM_Key_Specifier_Formats PPK_Format, int PPK_Index, PIN_Block_Formats PFo,
                         String PAN, String PVKI, HSM_Key_Specifier_Formats PVVK_Format, int PVVK_Index)
        {
            String cmd = String.Empty;
            String funcCode = "EE0607";
            String msg = String.Empty;
            String ANB = PAN.Substring(4);
            String TSP12 = ANB.Substring(0, 11) + PVKI;

            cmd = funcCode + Function_Modifier + ePIN + PPK_Format.ToString("D").PadLeft(2, '0') + PPK_Index.ToString("D4") + 
                PFo.ToString("D").PadLeft(2,'0') + ANB + PVVK_Format.ToString("D").PadLeft(2, '0') + PVVK_Index.ToString("D4") + TSP12;

            cmd = Header + (cmd.Length / 2).ToString("X4") + cmd;

            msg = sendHexString(cmd);

            if (msg.Substring(18, 2) != "00")
                ReturnErrorMsg(msg.Substring(18, 2));

            return msg.Substring(msg.Length - 4);
        }

        public String LunaEFT_EE0605_PVV_VER(String ePIN, HSM_Key_Specifier_Formats PPK_Format, int PPK_Index, PIN_Block_Formats PFo,
                         String PAN, String PVKI, HSM_Key_Specifier_Formats PVVK_Format, int PVVK_Index, int PVV)
        {
            String cmd = String.Empty;
            String funcCode = "EE0605";
            String msg = String.Empty;
            String ANB = PAN.Substring(4);
            String TSP12 = ANB.Substring(0, 11) + "1";

            cmd = funcCode + Function_Modifier + ePIN + PPK_Format.ToString("D").PadLeft(2, '0') + PPK_Index.ToString("D4") +
                PFo.ToString("D").PadLeft(2, '0') + ANB + PVVK_Format.ToString("D").PadLeft(2, '0') + PVVK_Index.ToString("D4") + TSP12 + PVV.ToString("D4");

            cmd = Header + (cmd.Length / 2).ToString("X4") + cmd;

            msg = sendHexString(cmd);

            if (msg.Substring(18, 2) != "00")
                ReturnErrorMsg(msg.Substring(18, 2));

            return msg.Substring(msg.Length - 2);
        }

        public String LunaEFT_EE0800_ENCIPHER(HSM_Key_Specifier_Formats DPK_Format, int DPK_Index, 
            String HexData, Chiper_Mode Des_Mode , String ICV="0000000000000000")
        {
            String cmd = String.Empty;
            String funcCode = "EE0800";
            String msg = String.Empty;

            cmd = funcCode + Function_Modifier + DPK_Format.ToString("D").PadLeft(2, '0') + DPK_Index.ToString("D4") +
                        Des_Mode.ToString("D").PadLeft(2, '0') + ICV + (HexData.Length / 2).ToString("X2") + HexData;        
            
            cmd = Header + (cmd.Length / 2).ToString("X4") + cmd;

            msg = sendHexString(cmd);

            if (msg.Substring(18, 2) != "00")
                ReturnErrorMsg(msg.Substring(18, 2));

            return msg.Substring(msg.Length - HexData.Length);
        }

        public String LunaEFT_EE0801_DECIPHER(HSM_Key_Specifier_Formats DPK_Format, int DPK_Index,
            String HexData, Chiper_Mode Des_Mode, String ICV = "0000000000000000")
        {
            String cmd = String.Empty;
            String funcCode = "EE0801";
            String msg = String.Empty;

            cmd = funcCode + Function_Modifier + DPK_Format.ToString("D").PadLeft(2, '0') + DPK_Index.ToString("D4") +
                        Des_Mode.ToString("D").PadLeft(2, '0') + ICV + (HexData.Length / 2).ToString("X2") + HexData;

            cmd = Header + (cmd.Length / 2).ToString("X4") + cmd;

            msg = sendHexString(cmd);

            if (msg.Substring(18, 2) != "00")
                ReturnErrorMsg(msg.Substring(18, 2));

            return msg.Substring(msg.Length - HexData.Length);
        }

        private void ReturnErrorMsg(String errorCode)
        {
            switch (errorCode)
            {
                case "01":
                    throw new Exception("Error 01 : DES Fault (system disabled)");
                case "02":
                    throw new Exception("Error 02 : Illegal Function Code. Either, PIN mailing, or Log archiving is not enabled on console.");
                case "03":
                    throw new Exception("Error 03 : Incorrect message length");
                case "04":
                    throw new Exception("Error 04 : Invalid data in message: Character not in range (0-9, A-F)");
                case "05":
                    throw new Exception("Error 05 : Invalid key index: Index not defined, key with this Index not stored or incorrect key length");
                case "06":
                    throw new Exception("Error 06 : Invalid PIN format specifier: only AS/ANSI = 1 & PIN/PAD = 3 specified");
                case "07":
                    throw new Exception("Error 07 : PIN format error: PIN does not comply with the AS2805.3 1985 specification, is in an invalid PIN/PAD format, or is in an invalid Docutel format");
                case "08":
                    throw new Exception("Error 08 : Verification failure");
                case "09":
                    throw new Exception("Error 09 : Contents of key memory destroyed: e.g. the Luna EFT was tampered or all Keys deleted");
                case "0A":
                    throw new Exception("Error 0A : Uninitiated key accessed. Key or decimalization table (DT) is not stored in the Luna EFT.");
                case "0B":
                    throw new Exception("Error 0B : Checklength Error. Customer PIN length is less than the minimum PVK length or less than Checklen in function.");
                case "0C":
                    throw new Exception("Error 0C : Inconsistent Request Fields: inconsistent field size.");
                case "0F":
                    throw new Exception("Error 0F : Invalid VISA Index. Invalid VISA PIN verification key indicator.");
                case "10":
                    throw new Exception("Error 10 : Internal Error");
                case "11":
                    throw new Exception("Error 11 : Errlog file does not exist");
                case "12":
                    throw new Exception("Error 12 : Errlog internal error");
                case "13":
                    throw new Exception("Error 13 : Errlog request length invalid");
                case "14":
                    throw new Exception("Error 14 : Errlog file number invalid");
                case "15":
                    throw new Exception("Error 15 : Errlog index number invalid");
                case "16":
                    throw new Exception("Error 16 : Errlog date time invalid");
                case "17":
                    throw new Exception("Error 17 : Errlog before/after flag invalid");
                case "19":
                    throw new Exception("Error 19 : Unsupported key type");
                case "1A":
                    throw new Exception("Error 1A : Duplicate key or record");
                case "20":
                    throw new Exception("Error 20 : Invalid key specifier length");
                case "21":
                    throw new Exception("Error 21 : Unsupported key specifier");
                case "22":
                    throw new Exception("Error 22 : Invalid key specifier content");
                case "23":
                    throw new Exception("Error 23 : Invalid key specifier format");
                case "24":
                    throw new Exception("Error 24 : Invalid Function Modifier. Invalid=00");
                case "25":
                    throw new Exception("Error 25 : Invalid key attributes");
                case "27":
                    throw new Exception("Error 27 : Hash process failed");
                case "28":
                    throw new Exception("Error 28 : Invalid Key Type");
                case "29":
                    throw new Exception("Error 29 : Unsupported Triple Des Index");
                case "30":
                    throw new Exception("Error 30 : Invalid administrator signature");
                case "32":
                    throw new Exception("Error 32 : No administration session");
                case "33":
                    throw new Exception("Error 33 : Invalid file type");
                case "34":
                    throw new Exception("Error 34 : Invalid signature");
                case "35":
                    throw new Exception("Error 35 : KKL disabled");
                case "36":
                    throw new Exception("Error 36 : No PIN pad");
                case "37":
                    throw new Exception("Error 37 : Pin pad timeout");
                case "39":
                    throw new Exception("Error 39 : Public key pair not available");
                case "3A":
                    throw new Exception("Error 3A : Public key pair generating");
                case "3B":
                    throw new Exception("Error 3B : RSA cipher error");
                case "40":
                    throw new Exception("Error 40 : Unsupported HSM stored SEED key");
                case "50":
                    throw new Exception("Error 50 : Invalid Variant Scheme or Invalid SDF");
                case "51":
                    throw new Exception("Error 51 : Invalid hash indicator");
                case "52":
                    throw new Exception("Error 52 : Invalid public key algorithm");
                case "53":
                    throw new Exception("Error 53 : Public key pair incompatible");
                case "54":
                    throw new Exception("Error 54 : RSA key length error");
                case "60":
                    throw new Exception("Error 60 : Software already Loaded");
                case "61":
                    throw new Exception("Error 61 : Software being loaded from CD ROM");
                case "62":
                    throw new Exception("Error 62 : Software data segment too large");
                case "63":
                    throw new Exception("Error 63 : Invalid offset value");
                case "64":
                    throw new Exception("Error 64 : Software loading not initiated");
                case "65":
                    throw new Exception("Error 65 : Unsupported file id");
                case "66":
                    throw new Exception("Error 66 : Unsupported control id");
                case "67":
                    throw new Exception("Error 67 : Software image is being verified");
                case "70":
                    throw new Exception("Error 70 : Invalid PIN Block flag");
                case "71":
                    throw new Exception("Error 71 : Invalid PIN Block random padding");
                case "72":
                    throw new Exception("Error 72 : Invalid PIN Block delimiter");
                case "73":
                    throw new Exception("Error 73 : Invalid PIN Block RB");
                case "74":
                    throw new Exception("Error 74 : Invalid PIN Block. Random number invalid");
                case "75":
                    throw new Exception("Error 75 : Invalid PIN Block RA");
                case "76":
                    throw new Exception("Error 76 : Invalid PIN Block PIN");
                case "77":
                    throw new Exception("Error 77 : Invalid PIN Block PIN length");
                case "78":
                    throw new Exception("Error 78 : PIN Block format disabled or requested reformatting not allowed");
                case "79":
                    throw new Exception("Error 79 : Validation data check failed");
                case "7F":
                    throw new Exception("Error 7F : Invalid Print Token");
                case "80":
                    throw new Exception("Error 80 : OAEP Decode Error");
                case "81":
                    throw new Exception("Error 81 : OAEP Invalid Header Byte");
                case "82":
                    throw new Exception("Error 82 : OAEP Invalid PIN Block");
                case "83":
                    throw new Exception("Error 83 : OAEP Invalid Random Number");
                case "90":
                    throw new Exception("Error 90 : General Printer Error");
                case "F0":
                    throw new Exception("Error F0 : Zero length PIN");
                case "91":
                    throw new Exception("Error 91 : Invalid Key Block version Id");
                case "92":
                    throw new Exception("Error 92 : Key Block Key authentication failure");
                case "93":
                    throw new Exception("Error 93 : Invalid Key Usage");
                case "94":
                    throw new Exception("Error 94 : Invalid Algorithms");
                case "95":
                    throw new Exception("Error 95 : Invalid Mode of use");
                case "96":
                    throw new Exception("Error 96 : Invalid Version number");
                case "97":
                    throw new Exception("Error 97 : Invalid Export Flag");
                case "98":
                    throw new Exception("Error 98 : Invalid Key length");
                case "99":
                    throw new Exception("Error 99 : Invalid Reserve Field");
                case "9A":
                    throw new Exception("Error 9A : Invalid Number of optional block");
                case "9B":
                    throw new Exception("Error 9B : Invalid Optional block header");
                case "9C":
                    throw new Exception("Error 9C : Repeated Optional block");
                case "9D":
                    throw new Exception("Error 9D : Invalid Key Block");
                case "9E":
                    throw new Exception("Error 9E : Invalid Padding Indicator");
                case "9F":
                    throw new Exception("Error 9F : Key Translation not permitted");
                case "A0":
                    throw new Exception("Error A0 : PIN brute force attack detected");
            }

        }

    }
}
