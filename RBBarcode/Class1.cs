using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenCode128;

using ZXing;
using ZXing.Common;
using ZXing.QrCode;

namespace RBBarcode
{
    public static class PostBarcode
    {
        public static Bitmap PrintQRCode(string Content)
        {

            QRCodeWriter qrEncode = new QRCodeWriter(); //создание QR кода

            Dictionary<EncodeHintType, object> hints = new Dictionary<EncodeHintType, object>();    //для колекции поведений
            hints.Add(EncodeHintType.CHARACTER_SET, "utf-8");   //добавление в коллекцию кодировки utf-8
            BitMatrix qrMatrix = qrEncode.encode(   //создание матрицы QR
                Content,                 //кодируемая строка
                BarcodeFormat.QR_CODE,  //формат кода, т.к. используется QRCodeWriter применяется QR_CODE
                100,                    //ширина
                100,                    //высота
                hints);                 //применение колекции поведений

            BarcodeWriter qrWrite = new BarcodeWriter();    //класс для кодирования QR в растровом файле
            Bitmap qrImage = qrWrite.Write(qrMatrix);   //создание изображения
            //qrImage.Save("1.bmp", System.Drawing.Imaging.ImageFormat.Bmp);//сохранение изображения

            return qrImage;
        }
        public static Bitmap PrintCode128(string Content)
        {
            //int i = int.Parse(Content);
            Image myimg = Code128Rendering.MakeBarcodeImage(Content, 2, true);
            return (Bitmap)myimg;
        }

        public static Bitmap Print2of5Interleaved(string Content)
        {
            //string Content = "12345678";
            //string CheckSum = CalcCheckSum(Content);
            // MessageBox.Show(CheckSum);
            string startcode = "1010";
            string stopcode = "1101";
            int startX = 0;
            int startY = 14;
            int endY = startY + 55;
            int curX;
            int sectionIndex = 0;
            int pairIndex = 0;
            int barIndex = 0;
            int spaceIndex = 0;

            Graphics g;
            Bitmap bmp = new Bitmap(110, 90);

            g = Graphics.FromImage(bmp);
            //g.DrawRectangle(Pens.White, 0, 0, 110, 90);
            g.Clear(Color.White);
            curX = startX;
            //Content = Content;
            if ((Content.Length % 2) != 0)
            {
                //odd number, fill in a leading zero
                Content = "0" + Content;
            }
            //draw the start marker
            foreach (char digit in startcode)
            {
                if (digit == '1')
                {
                    g.DrawLine(Pens.Black, curX, startY, curX, endY);
                    curX += 1;
                }
                else
                {
                    curX += 1;
                }
            }
            //draw the content
            for (int i = 0; i < Content.Length; i += 2)
            {
                string pair = Content.Substring(i, 2);
                string barPattern = Get2of5Pattern(pair.Substring(0, 1));
                string spacePattern = Get2of5Pattern(pair.Substring(1, 1));
                barIndex = 0;
                spaceIndex = 0;
                sectionIndex = 0;
                while (sectionIndex < 10)
                {
                    if ((sectionIndex % 2) == 0)
                    {
                        //bar 0,2,4,6,8 positions
                        pairIndex = 0;
                        if (barPattern.Substring(barIndex, 1) == "W")
                        {
                            //draw wide bar
                            while (pairIndex < 2)
                            {
                                g.DrawLine(Pens.Black, curX + pairIndex, startY, curX + pairIndex, endY);
                                pairIndex++;
                            }
                            curX = curX + 2;
                        }
                        else
                        {
                            //draw narrow bar
                            g.DrawLine(Pens.Black, curX + pairIndex, startY, curX + pairIndex, endY);
                            curX = curX + 1;
                        }
                        barIndex++;
                    }
                    else
                    {
                        //space 1,3,5,7,8 positions
                        if (spacePattern.Substring(spaceIndex, 1) == "W")
                        {
                            //simulate drawing a wide white space
                            curX = curX + 2;
                        }
                        else
                        {
                            //simulate drawing a narrow white space
                            curX = curX + 1;
                        }
                        spaceIndex++;
                    }
                    sectionIndex += 1;
                }
            }
            //draw the stop marker
            foreach (char digit in stopcode)
            {
                if (digit == '1')
                {
                    g.DrawLine(Pens.Black, curX, startY, curX, endY);
                    curX += 1;
                }
                else
                {
                    curX += 1;
                }
            }
            bmp = new Bitmap(bmp, 220, 90);
            g = Graphics.FromImage(bmp);
            float fSize = 14;//8 * g.DpiY/72 ;
            Font f1 = new Font(FontFamily.GenericSansSerif, fSize / 4 * 3, FontStyle.Bold, GraphicsUnit.Pixel);
            Font f2 = new Font(FontFamily.GenericSansSerif, fSize, FontStyle.Regular, GraphicsUnit.Pixel);
            Font f3 = new Font(FontFamily.GenericSansSerif, fSize, FontStyle.Bold, GraphicsUnit.Pixel);
            //Brush b = Brushes.Black;
            SolidBrush b = new SolidBrush(Color.Black);
            //добавляем Почта России
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            g.DrawString("ПОЧТА РОССИИ", f1, b, 0, 0);
            //Добавляем Код разбитый

            g.DrawString("  " + Content.Substring(0, 6) + "    " + Content.Substring(6, 2) + "    ", f2, b, 0, endY + 2);
            g.DrawString(Content.Substring(8, 5), f3, b, 110, endY + 2);
            g.DrawString("  " + Content.Substring(13, 1), f2, b, 170, endY + 2);
            return bmp;
        }
        /* чексумма не ужна почте
public string CalcCheckSum(string CheckNum)
        {
            int i;
            int j;
            int checkval = 0;
            j = 3;
            i = CheckNum.Length - 1;
            while (i > 0)
            {
                checkval += Convert.ToInt32(CheckNum.Substring(i, 1)) * j;
                j = j ^ 2;
                i -= 1;
            }
            checkval = (10 - (checkval % 10)) % 10;
            return checkval.ToString();
        }*/

        public static string Get2of5Pattern(string letter)
        {
            string tmpPattern = "";
            switch (letter)
            {
                case "0":
                    tmpPattern = "NNWWN";
                    break;
                case "1":
                    tmpPattern = "WNNNW";
                    break;
                case "2":
                    tmpPattern = "NWNNW";
                    break;
                case "3":
                    tmpPattern = "WWNNN";
                    break;
                case "4":
                    tmpPattern = "NNWNW";
                    break;
                case "5":
                    tmpPattern = "WNWNN";
                    break;
                case "6":
                    tmpPattern = "NWWNN";
                    break;
                case "7":
                    tmpPattern = "NNNWW";
                    break;
                case "8":
                    tmpPattern = "WNNWN";
                    break;
                case "9":
                    tmpPattern = "NWNWN";
                    break;
            }
            return tmpPattern;
        }

        public static Bitmap PrintPostcode(string postcode){
            int dpm = 10; //dot per millimiter
            Bitmap bmp = new Bitmap(61 * dpm, 15 * dpm);
            Graphics g;
            g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            for (int i=0; i < 7;i++) {
                g.FillRectangle(Brushes.Black,i*9* dpm,0,7*dpm,2*dpm); //Верхушки цифр
            }
            g.FillRectangle(Brushes.Black, 0, 3*dpm, 7 * dpm, 1 * dpm); //ключ
            for (int i = 1; i < 7; i++)
            {
                //маска цифры
                for (int w=0; w<6; w++)
                    for (int h = 0; h < 11; h++)
                    {
                        if (w % 5 == 0 || h % 5 == 0||(h+w)%5==0)
                            g.FillEllipse(Brushes.Black,(w+i*9+ (float)0.875) *dpm,(h+4- (float)0.125) *dpm, dpm * (float)0.25, dpm * (float)0.25);
                    }
                //Цифра
                if (postcode.Length > i - 1)
                {
                    char p = postcode[i - 1];
                    Pen pen = new Pen(Color.Black, (float)0.5 * dpm);
                    Point p1 = new Point((i * 9 + 1) * dpm, (4) * dpm);
                    Point p2 = new Point((6 + i * 9) * dpm, (4) * dpm);
                    Point p3 = new Point((i * 9 + 1) * dpm, (9) * dpm);
                    Point p4 = new Point((6 + i * 9) * dpm, (9) * dpm);
                    Point p5 = new Point((i * 9 + 1) * dpm, (14) * dpm);
                    Point p6 = new Point((6 + i * 9) * dpm, (14) * dpm);
                    //a
                    if (new char[]{ '0', '2','3','5','7','8','9' }.Contains(p))
                        g.DrawLine(pen, p1, p2);
                    //b
                    if (new char[] { '0', '1', '2', '4', '8', '9' }.Contains(p))
                        g.DrawLine(pen, p2, p4);
                    //c
                    if (new char[] { '0', '1', '4', '5', '6', '8' }.Contains(p))
                        g.DrawLine(pen, p4, p6);
                    //d
                    if (new char[] { '0', '2', '5', '6', '8' }.Contains(p))
                        g.DrawLine(pen, p5, p6);
                    //e
                    if (new char[] { '0', '6', '7','8' }.Contains(p))
                        g.DrawLine(pen, p5, p3);
                    //f
                    if (new char[] { '0', '4', '5', '8', '9' }.Contains(p))
                        g.DrawLine(pen, p3, p1);
                    //g
                    if (new char[] { '3', '4', '5', '6', '8', '9' }.Contains(p))
                        g.DrawLine(pen, p3, p4);
                    //h
                    if (new char[] { '1', '3', '6', '7' }.Contains(p))
                        g.DrawLine(pen, p2, p3);
                    //i
                    if (new char[] { '2', '3', '9' }.Contains(p))
                        g.DrawLine(pen, p4, p5);
                }
            }
                return bmp;
        }
    }
}
