using System.Drawing;
using ZXing.QrCode;
using ZXing;
using ZXing.Windows.Compatibility;
using System.Runtime.Versioning;

namespace BarcodeCreator
{
    [SupportedOSPlatform("windows")]
    public class LabelImage
    {
        string _fileName;
        string _supCode;
        string _prodCode;
        string _labelNumber;
        string _lotNumber;
        string _quantity;
        string _date;
        string _companySenderName;
        string _companyReceiverName;
        string _senderAddress;
        string _reg;
        string _senderStreet;
        string _senderCity;
        string _receiverAddress;
        string _receiverStreet;
        string _receiverCity;
        string _tradeName;

        public Bitmap LabelImageData { get; set; }

        public LabelImage(
            string fileName,
            string supCode,
            string prodCode,
            string labelNumber,
            string lotNumber,
            string quantity,
            string date,
            string companySenderName,
            string companyReceiverName,
            string senderAddress,
            string reg,
            string senderStreet,
            string senderCity,
            string receiverAddress,
            string receiverStreet,
            string receiverCity,
            string tradeName
        )
        {
            _fileName = fileName;
            _supCode = supCode;
            _prodCode = prodCode;
            _labelNumber = labelNumber;
            _lotNumber = lotNumber;
            _quantity = quantity;
            _date = date;
            _companySenderName = companySenderName;
            _companyReceiverName = companyReceiverName;
            _senderAddress = senderAddress;
            _reg = reg;
            _senderStreet = senderStreet;
            _senderCity = senderCity;
            _receiverAddress = receiverAddress;
            _receiverStreet = receiverStreet;
            _receiverCity = receiverCity;
            _tradeName = tradeName;
        }

        public void CreateLabel()
        {
            //barcode 2D
            string data2d = _supCode + _prodCode + _labelNumber + _lotNumber + _quantity + _date;

            int imageWidth = 550;
            int imageHeight = 150;

            LabelImageData = new Bitmap(2000, 2000);
            LabelImageData.SetResolution(72, 72);
            LabelImageData.SetPixel(0, 0, Color.Black);

            Pen pen = new Pen(Color.Black, 4);

            Font generalCapTextFont = new Font("Microsoft Sans Serif", 120F);
            Font capTextFont = new Font("Microsoft Sans Serif", 62F);
            Font textFont = new Font("Microsoft Sans Serif", 36F);

            QrCodeEncodingOptions options = new()
            {
                Width = imageWidth,
                Height = imageHeight,
                PureBarcode = true,
            };

            QrCodeEncodingOptions dataMatrixOptions = new()
            {
                Width = 500,
                Height = 500,
            };

            Bitmap barcode2DBitmap = CreateBarcode(dataMatrixOptions, BarcodeFormat.DATA_MATRIX, data2d);

            List<Bitmap> images = new List<Bitmap>()
            {
                CreateBarcode(options, BarcodeFormat.CODE_128, _supCode),
                CreateBarcode(options, BarcodeFormat.CODE_128, _prodCode),
                CreateBarcode(options, BarcodeFormat.CODE_128, _labelNumber),
                CreateBarcode(options, BarcodeFormat.CODE_128, _lotNumber),
                CreateBarcode(options, BarcodeFormat.CODE_128, _quantity),
                CreateBarcode(options, BarcodeFormat.CODE_128, _date),
            };

            Graphics graphic = Graphics.FromImage(LabelImageData);
            graphic.Clear(Color.White);

            int startX = 60;
            int startY = 60;
            int endX = 1940;
            int endY = 1940;

            //Drawing lines for label image box
            DrawLabelLine(graphic, pen, startX, startY, endX, startY);
            DrawLabelLine(graphic, pen, startX, startY, startX, endY);
            DrawLabelLine(graphic, pen, endX, startY, endX, endY);
            DrawLabelLine(graphic, pen, endX, endY, startX, endY);

            //Drawing first vertical line
            DrawLabelLine(graphic, pen, 1000, startY, 1000, 440);

            //Drawing first horizontal line
            DrawLabelLine(graphic, pen, startX, 440, endX, 440);

            //Drawing second horizontal line - Drawing line
            DrawLabelLine(graphic, pen, startX, 940, endX, 940);

            //Drawing third horizontal line
            DrawLabelLine(graphic, pen, startX, 1240, endX, 1240);

            //Drawing vertical line for 1D barcodes
            DrawLabelLine(graphic, pen, 1000, 1240, 1000, endY);

            //Drawing fourth horizontal line 
            DrawLabelLine(graphic, pen, startX, 1473, endX, 1473);

            //Drawing fifth horizontal line
            DrawLabelLine(graphic, pen, startX, 1706, endX, 1706);

            //Address string
            graphic.DrawString("SHIP FROM", capTextFont, Brushes.Black, 80, 80);
            graphic.DrawString(_companySenderName, textFont, Brushes.Black, 80, 160);
            graphic.DrawString(_senderAddress, textFont, Brushes.Black, 80, 200);
            graphic.DrawString(_reg, textFont, Brushes.Black, 80, 240);
            graphic.DrawString(_senderStreet, textFont, Brushes.Black, 80, 280);
            graphic.DrawString(_senderCity, textFont, Brushes.Black, 80, 320);

            graphic.DrawString("PLANT/DOCK", capTextFont, Brushes.Black, 1020, 80);
            graphic.DrawString(_companyReceiverName, textFont, Brushes.Black, 1020, 160);
            graphic.DrawString(_receiverAddress, textFont, Brushes.Black, 1020, 200);
            graphic.DrawString(_receiverStreet, textFont, Brushes.Black, 1020, 240);
            graphic.DrawString(_receiverCity, textFont, Brushes.Black, 1020, 280);

            //Trade name string
            graphic.DrawString("TRADE NAME", textFont, Brushes.Black, 80, 1175);
            graphic.DrawString(_tradeName, generalCapTextFont, Brushes.Black, 800, 1075);

            //First 1D codes string
            graphic.DrawString("PRODUCT CODE (P)", textFont, Brushes.Black, 80, 1260);
            graphic.DrawString(_prodCode, textFont, Brushes.Black, 80, 1410);

            graphic.DrawString("LABEL NUMBER (S)", textFont, Brushes.Black, 1020, 1260);
            graphic.DrawString(_labelNumber, textFont, Brushes.Black, 1020, 1410);

            //Second 1D codes string
            graphic.DrawString("SUPPLIER CODE (V)", textFont, Brushes.Black, 80, 1493);
            graphic.DrawString(_supCode, textFont, Brushes.Black, 80, 1643);

            graphic.DrawString("LOT NUMBER (H)", textFont, Brushes.Black, 1020, 1493);
            graphic.DrawString(_lotNumber, textFont, Brushes.Black, 1020, 1643);

            //Third 1D codes string
            graphic.DrawString("DATE OF FABRICATION (P) DD.MM.YY", textFont, Brushes.Black, 80, 1726);
            graphic.DrawString(_date, textFont, Brushes.Black, 80, 1876);

            graphic.DrawString("QUANTITY (Q)", textFont, Brushes.Black, 1020, 1726);
            graphic.DrawString(_quantity, textFont, Brushes.Black, 1020, 1876);

            int startBarcodeWidth = 575;
            int startBarcodeHeight1 = 1350;
            int startBarcodeHeight2 = 1580;
            int startBarcodeHeight3 = 1810;

            graphic.DrawImage(images[0], startBarcodeWidth, startBarcodeHeight1);
            graphic.DrawImage(images[1], startBarcodeWidth + 950, startBarcodeHeight1);

            graphic.DrawImage(images[2], startBarcodeWidth, startBarcodeHeight2);
            graphic.DrawImage(images[3], startBarcodeWidth + 950, startBarcodeHeight2);

            graphic.DrawImage(images[2], startBarcodeWidth, startBarcodeHeight3);
            graphic.DrawImage(images[3], startBarcodeWidth + 950, startBarcodeHeight3);

            graphic.DrawImage(barcode2DBitmap, 800, 485);
            graphic.DrawString(data2d, textFont, Brushes.Black, 535, 870);

            LabelImageData.Save(_fileName);
        }

        public Bitmap CreateBarcode(QrCodeEncodingOptions options, BarcodeFormat format, string data)
        {
            BarcodeWriter writer = new()
            {
                Format = format,
                Options = options,
            };

            return writer.Write(data);
        }
    
        public void DrawLabelLine(Graphics graphic, Pen pen, int x1, int y1, int x2, int y2)
        {
            Point p1 = new Point(x1, y1);
            Point p2 = new Point(x2, y2);

            graphic.DrawLine(pen, p1, p2);
        }
    }
}
