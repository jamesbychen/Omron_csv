using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Omron_csv
{
    class PublicClass
    {
        //設定資料夾路徑
        public string pathMachineCsv { get; set; } = "C:\\CSV\\Machine";
        public string pathOpertorCsv { get; set; } = "C:\\CSV\\Operator";
        public string pathOutput { get; set; } = "C:\\CSV\\output";
        public string MachineName { get; set; }
        public string BoardID { get; set; }
        public string ProcessTime { get; set; }
        public string InspectionID { get; set; }
        public string ComponentID { get; set; }
        public string apiUrl { get; set; }
        //從CSV檔案名稱解析出變數
        public CsvFileName GetParameterFromCsv(string filename)
        {
            CsvFileName ins = new CsvFileName();
            string[] paras = filename.Split('_');//8 parameters
            ins.MachineName = paras[0];
            ins.BatchName = paras[1];
            ins.BoardID = paras[2];
            ins.ProcessName = paras[3];
            ins.BoardSide = paras[4];
            ins.ProcessTime = paras[5];
            return ins;
        }

        //檢查資料夾中的檔案
        public void checkCSV()
        {
            //迴圈處理每一個CSV檔案
        }

        //從CSV檔案讀取資料轉換成DATATABLE
        public DataTable Csv2Datatable(string filePath)
        {
            FileStream fs = new FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            StreamReader sr = new StreamReader(fs, Encoding.UTF8);
            DataTable dt = new DataTable();
            //將原有CSV讀取出來

            //記錄每次讀取的一行記錄
            string strLine = "";
            //記錄每行記錄中的各字段內容
            string[] aryLine = null;
            string[] tableHead = null;
            //標示列數
            int columnCount = 0;
            //標示是否是讀取的第一行
            bool IsFirst = true;
            //逐行讀取CSV中的數據
            while ((strLine = sr.ReadLine()) != null)
            {
                if (IsFirst == true)
                {
                    //標題列
                    tableHead = strLine.Split(',');
                    IsFirst = false;
                    columnCount = tableHead.Length;
                    //創建列
                    DataColumn dc;
                    for (int i = 0; i < columnCount; i++)
                    {
                        //原有欄位
                        tableHead[i] = tableHead[i].Replace("\"", "");
                        if (dt.Columns[tableHead[i]] == null)
                        {
                            dc = new DataColumn(tableHead[i]);
                        }
                        else
                        {
                            dc = new DataColumn(tableHead[i] + "_");
                        }
                        dt.Columns.Add(dc);

                    }
                    //新建欄位1:Block Barcode
                    dc = new DataColumn("Block Barcode");
                    dt.Columns.Add(dc);
                    //新建欄位2:NG Component Image Path
                    dc = new DataColumn("NG Component Image Path");
                    dt.Columns.Add(dc);
                    //
                }
                else
                {
                    aryLine = strLine.Split(',');
                    DataRow dr = dt.NewRow();
                    for (int j = 0; j < columnCount; j++)
                    {
                        dr[j] = aryLine[j].Replace("\"", "");//去掉雙引號
                    }
                    //底下為新增欄位
                    string[] fullfilename = filePath.Split('.');
                    string filename = fullfilename[0];//將副檔名分離
                    CsvFileName iFn = GetParameterFromCsv(filename);//透過檔案名稱取得變數
                    //Block Barcode
                    string blockbarcode = "barcode123456789";//測試值
                    //blockbarcode = API_getBlockBarcode(iFn);
                    dr[columnCount] = blockbarcode;
                    //NG Component Image
                    string Componentimagepath = "c:/picture/abc.jpg";//測試值
                    //Componentimagepath = API_getCompomentPic(iFn);
                    dr[columnCount + 1] = Componentimagepath;
                    dt.Rows.Add(dr);
                }
            }
            #region 排序（已註解）
            //if (aryLine != null && aryLine.Length > 0)
            //{
            //    dt.DefaultView.Sort = tableHead[2] + " " + "DESC";
            //}
            #endregion
            sr.Close();
            fs.Close();
            return dt;
        }
        //將DTATABLE匯出到新的CSV檔案
        public void Datatable2Csv(DataTable oDt, string output)
        {
            string data = "";
            StreamWriter wr = new StreamWriter(output, false, System.Text.Encoding.UTF8);
            //欄位名稱
            foreach (DataColumn column in oDt.Columns)
            {
                data += column.ColumnName + ",";
            }
            data += "\n";
            wr.Write(data);
            data = "";
            //欄位內容
            foreach (DataRow row in oDt.Rows)
            {
                foreach (DataColumn column in oDt.Columns)
                {
                    data += row[column].ToString().Trim() + ",";
                }
                data += "\n";
                wr.Write(data);
                data = "";
            }
            data += "\n";

            wr.Dispose();
            wr.Close();
        }

        //
        public XmlNodeList getOutputXMLNodeList(string XML)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(XML);
            //check return code
            XmlNode recordCount = doc["resultDataM"]["recordCount"];
            XmlNode returnCode = doc["resultDataM"]["returnCode"];
            XmlNodeList outputList = null;
            if (!recordCount.InnerText.Equals("0") && returnCode.InnerText.Equals("0"))
            {
                outputList = doc["resultDataM"]["output"].ChildNodes;
            }
            return outputList;
        }
        //
        public List<Inspection> getInspectionID(string XML)
        {
            XmlNodeList outputList = getOutputXMLNodeList(XML);
            //doc["resultDataM"]["output"]["data"].ChildNodes.Item(0).Attributes.GetNamedItem("value").Value
            List<Inspection> output = new List<Inspection>();
            if (outputList != null)
            {
                foreach (XmlNode dataList in outputList)//get [data] node
                {
                    Inspection item = new Inspection();
                    foreach (XmlNode param in dataList)//get [param] node
                    {
                        if (param.Attributes.GetNamedItem("name").Value == "inspection_id")
                        {
                            item.InspectionID = param.Attributes.GetNamedItem("value").Value;
                        }
                        if (param.Attributes.GetNamedItem("name").Value == "system_machine_name")
                        {
                            item.MachineName = param.Attributes.GetNamedItem("value").Value;
                        }
                        if (param.Attributes.GetNamedItem("name").Value == "inspection_end_time")
                        {
                            item.inspection_end_time = param.Attributes.GetNamedItem("value").Value;
                        }
                        if (param.Attributes.GetNamedItem("name").Value == "confirm_date")
                        {
                            //item.confrim_date = param.Attributes.GetNamedItem("value").Value;
                        }
                    }
                    output.Add(item);
                }
            }
            return output;
        }
        //從輸出的XML中取得Component ID
        public List<Inspection> getComponentID(string XML)
        {
            List<Inspection> output = new List<Inspection>();
            if (XML.Length > 0)
            {
                XmlNodeList outputList = getOutputXMLNodeList(XML);
                //doc["resultDataM"]["output"]["data"].ChildNodes.Item(0).Attributes.GetNamedItem("value").Value
                if (outputList != null)
                {
                    foreach (XmlNode dataList in outputList)//get [data] node
                    {
                        Inspection item = new Inspection();
                        item.MachineName = MachineName;
                        item.InspectionID = InspectionID;
                        foreach (XmlNode param in dataList)//get [param] node
                        {
                            if (param.Attributes.GetNamedItem("name").Value == "segment_no")
                            {
                                item.SegmentNo = param.Attributes.GetNamedItem("value").Value;
                            }
                            if (param.Attributes.GetNamedItem("name").Value == "segment_code")
                            {
                                item.SegmentCode = param.Attributes.GetNamedItem("value").Value;
                            }
                            if (param.Attributes.GetNamedItem("name").Value == "circuit_number")
                            {
                                item.CircuitNumber = param.Attributes.GetNamedItem("value").Value;
                            }
                            if (param.Attributes.GetNamedItem("name").Value == "component_id")
                            {
                                item.ComponentID = param.Attributes.GetNamedItem("value").Value;
                            }
                            //因為傳回的COMPONENT中，是否良品都包含其中，需加入檢查結果來判斷
                            if (param.Attributes.GetNamedItem("name").Value == "last_result")
                            {
                                item.LastResult = param.Attributes.GetNamedItem("value").Value;
                            }
                        }
                        //不正常資料(last_result=3)
                        //20171105  暫時不篩選
                        if (!item.LastResult.Equals("3") || true)
                        {
                            output.Add(item);
                        }
                    }
                }
            }
            else
            {
                //不良品
                output.Add(new Inspection
                {
                    MachineName = MachineName,
                    InspectionID = InspectionID,
                    ComponentID = "99",
                    SegmentCode = "04123|6789",
                    SegmentNo = "1",
                    CircuitNumber = "NEW001",
                    LastResult = "1",
                });
                //正常品
                output.Add(new Inspection
                {
                    MachineName = MachineName,
                    InspectionID = InspectionID,
                    ComponentID = "88",
                    SegmentCode = "04123|6789",
                    SegmentNo = "2",
                    CircuitNumber = "NEW002",
                    LastResult = "0",
                });
            }
            return output;
        }
        //
        public List<Inspection> getImagePath(string XML)
        {
            XmlNodeList outputList = getOutputXMLNodeList(XML);
            //doc["resultDataM"]["output"]["data"].ChildNodes.Item(0).Attributes.GetNamedItem("value").Value
            List<Inspection> output = new List<Inspection>();
            if (outputList != null)
            {
                foreach (XmlNode dataList in outputList)//get [data] node
                {
                    Inspection item = new Inspection();
                    item.MachineName = MachineName;
                    item.InspectionID = InspectionID;
                    item.ComponentID = ComponentID;
                    foreach (XmlNode param in dataList)//get [param] node
                    {
                        //
                        if (param.Attributes.GetNamedItem("name").Value == "inspection_image_path")
                        {
                            item.InspectionImagePath = param.Attributes.GetNamedItem("value").Value;
                        }
                    }
                    output.Add(item);
                }
            }
            return output;
        }
        //
        public string CallApi(string API)
        {
            HttpClient hc = new HttpClient();
            WebClient wc = new WebClient();
            string output = "";
            string xmlStr = bindXML(API);
            wc.Encoding = System.Text.Encoding.UTF8;
            var parameters = new StringContent(xmlStr, Encoding.UTF8, "text/xml");
            //new method with WebClient
            wc.QueryString.Add("param", xmlStr);
            string wcResponse = "";
            wcResponse = wc.DownloadString(apiUrl);
            output = wcResponse;
            wc.Dispose();
            return output;
        }

        //get XML by API
        //*set input param first
        public string bindXML(string APINO)
        {
            //帶入time直接回傳該筆inspection
            string xmlStr = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>"
                           + "<parameterUM>"
                           + "<params>"
                           + "<param name=\"inspection_end_time_from\" value=\"\" />"
                           + "<param name=\"inspection_end_time_to\" value=\"\" />"
                           + "<param name=\"system_machine_name\" value=\"" + MachineName + "\" />"
                           + "<param name=\"board_id\" value=\"" + BoardID + "\" />"
                           + "<param name=\"board_side\" value=\"0\" />"
                           + "<param name=\"lang_type\" value=\"0\" />"
                           + "</params>"
                           + "</parameterUM>";
            switch (APINO)
            {
                case "4-3"://get componet ID
                    xmlStr = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>"
                           + "<parameterUM>"
                           + "<params>"
                           + "<param name=\"system_machine_name\" value=\"" + MachineName + "\" />"
                           + "<param name=\"inspection_id\" value=\"" + InspectionID + "\" />"
                           + "<param name=\"lang_type\" value=\"0\" />"
                           + "</params>"
                           + "</parameterUM>";
                    break;
                case "4-12"://get inspection image path
                    xmlStr = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>"
                           + "<parameterUM>"
                           + "<params>"
                           + "<param name=\"system_machine_name\" value=\"" + MachineName + "\" />"
                           + "<param name=\"inspection_id\" value=\"" + InspectionID.PadLeft(10, '0') + "\" />"
                           + "<param name=\"Component_id\" value=\"" + ComponentID.PadLeft(5, '0') + "\" />"
                           + "</params>"
                           + "</parameterUM>";
                    //xmlStr = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>"
                    //           + "<parameterUM>"
                    //           + "<params>"
                    //           + "<param name=\"system_machine_name\" value=\"" + MachineName + "\" />"
                    //           + "<param name=\"inspection_id\" value=\"" + InspectionID + "\" />"
                    //           + "<param name=\"Component_id\" value=\"" + ComponentID + "\" />"
                    //           + "<param name=\"lang_type\" value=\"0\" />"
                    //           + "</params>"
                    //           + "</parameterUM>";
                    break;
                default:

                    break;
            }

            return xmlStr;
        }


        #region test string for XML
        public string testXML = @"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes""?>
<resultDataM>
    <output>
        <data>
            <param value = ""1"" name=""component_id""/>
            <param value = ""1"" name=""segment_id""/>
            <param value = ""1"" name=""segment_no""/>
            <param value = ""G23M-00303T"" name=""segment_code""/>
            <param value = ""C1"" name=""circuit_number""/>
            <param value = ""72"" name=""component_number_id""/>
            <param value = ""1"" name=""component_number_revision""/>
            <param value = ""NEWCOMPNO.80"" name=""component_number_name""/>
            <param value = ""2"" name=""component_type_code""/>
            <param value = ""Chip Capacitor"" name=""component_type_name""/>
            <param value = ""32800.0"" name=""pos_x""/>
            <param value = ""21713.0"" name=""pos_y""/>
            <param value = ""440.0"" name=""size_x""/>
            <param value = ""480.0"" name=""size_y""/>
            <param value = ""360"" name=""component_angle""/>
            <param value = ""0"" name=""individual_flag""/>
            <param value = ""4"" name=""window_count""/>
            <param value = ""0"" name=""inspection_result_code""/>
            <param value = ""OK"" name=""inspection_result_name""/>
            <param value = """" name=""confirm_result_code""/>
            <param value = """" name=""confirm_result_name""/>
            <param value = ""0"" name=""user_confirm_flag""/>
            <param value = ""0"" name=""last_result""/>
        </data>
        <data>
            <param value = ""2"" name=""component_id""/>
            <param value = ""1"" name=""segment_id""/>
            <param value = ""1"" name=""segment_no""/>
            <param value = ""G23M-00303T"" name=""segment_code""/>
            <param value = ""C2"" name=""circuit_number""/>
            <param value = ""72"" name=""component_number_id""/>
            <param value = ""1"" name=""component_number_revision""/>
            <param value = ""NEWCOMPNO.80"" name=""component_number_name""/>
            <param value = ""2"" name=""component_type_code""/>
            <param value = ""Chip Capacitor"" name=""component_type_name""/>
            <param value = ""41050.0"" name=""pos_x""/>
            <param value = ""28961.0"" name=""pos_y""/>
            <param value = ""440.0"" name=""size_x""/>
            <param value = ""480.0"" name=""size_y""/>
            <param value = ""0"" name=""component_angle""/>
            <param value = ""0"" name=""individual_flag""/>
            <param value = ""4"" name=""window_count""/>
            <param value = ""0"" name=""inspection_result_code""/>
            <param value = ""OK"" name=""inspection_result_name""/>
            <param value = """" name=""confirm_result_code""/>
            <param value = """" name=""confirm_result_name""/>
            <param value = ""0"" name=""user_confirm_flag""/>
            <param value = ""0"" name=""last_result""/>
        </data>
        <data>
            <param value = ""3"" name=""component_id""/>
            <param value = ""1"" name=""segment_id""/>
            <param value = ""1"" name=""segment_no""/>
            <param value = ""G23M-00303T"" name=""segment_code""/>
            <param value = ""C3"" name=""circuit_number""/>
            <param value = ""72"" name=""component_number_id""/>
            <param value = ""1"" name=""component_number_revision""/>
            <param value = ""NEWCOMPNO.80"" name=""component_number_name""/>
            <param value = ""2"" name=""component_type_code""/>
            <param value = ""Chip Capacitor"" name=""component_type_name""/>
            <param value = ""15049.0"" name=""pos_x""/>
            <param value = ""56061.0"" name=""pos_y""/>
            <param value = ""440.0"" name=""size_x""/>
            <param value = ""480.0"" name=""size_y""/>
            <param value = ""0"" name=""component_angle""/>
            <param value = ""0"" name=""individual_flag""/>
            <param value = ""4"" name=""window_count""/>
            <param value = ""0"" name=""inspection_result_code""/>
            <param value = ""OK"" name=""inspection_result_name""/>
            <param value = """" name=""confirm_result_code""/>
            <param value = """" name=""confirm_result_name""/>
            <param value = ""0"" name=""user_confirm_flag""/>
            <param value = ""0"" name=""last_result""/>
        </data>
        <data>
            <param value = ""4"" name=""component_id""/>
            <param value = ""1"" name=""segment_id""/>
            <param value = ""1"" name=""segment_no""/>
            <param value = ""G23M-00303T"" name=""segment_code""/>
            <param value = ""C4"" name=""circuit_number""/>
            <param value = ""72"" name=""component_number_id""/>
            <param value = ""1"" name=""component_number_revision""/>
            <param value = ""NEWCOMPNO.80"" name=""component_number_name""/>
            <param value = ""2"" name=""component_type_code""/>
            <param value = ""Chip Capacitor"" name=""component_type_name""/>
            <param value = ""54955.0"" name=""pos_x""/>
            <param value = ""58419.0"" name=""pos_y""/>
            <param value = ""440.0"" name=""size_x""/>
            <param value = ""480.0"" name=""size_y""/>
            <param value = ""0"" name=""component_angle""/>
            <param value = ""0"" name=""individual_flag""/>
            <param value = ""4"" name=""window_count""/>
            <param value = ""0"" name=""inspection_result_code""/>
            <param value = ""OK"" name=""inspection_result_name""/>
            <param value = """" name=""confirm_result_code""/>
            <param value = """" name=""confirm_result_name""/>
            <param value = ""0"" name=""user_confirm_flag""/>
            <param value = ""0"" name=""last_result""/>
        </data>
        <data>
            <param value = ""5"" name=""component_id""/>
            <param value = ""1"" name=""segment_id""/>
            <param value = ""1"" name=""segment_no""/>
            <param value = ""G23M-00303T"" name=""segment_code""/>
            <param value = ""C5"" name=""circuit_number""/>
            <param value = ""72"" name=""component_number_id""/>
            <param value = ""1"" name=""component_number_revision""/>
            <param value = ""NEWCOMPNO.80"" name=""component_number_name""/>
            <param value = ""2"" name=""component_type_code""/>
            <param value = ""Chip Capacitor"" name=""component_type_name""/>
            <param value = ""106445.0"" name=""pos_x""/>
            <param value = ""43559.0"" name=""pos_y""/>
            <param value = ""440.0"" name=""size_x""/>
            <param value = ""480.0"" name=""size_y""/>
            <param value = ""0"" name=""component_angle""/>
            <param value = ""0"" name=""individual_flag""/>
            <param value = ""4"" name=""window_count""/>
            <param value = ""0"" name=""inspection_result_code""/>
            <param value = ""OK"" name=""inspection_result_name""/>
            <param value = """" name=""confirm_result_code""/>
            <param value = """" name=""confirm_result_name""/>
            <param value = ""0"" name=""user_confirm_flag""/>
            <param value = ""0"" name=""last_result""/>
        </data>
        <data>
            <param value = ""6"" name=""component_id""/>
            <param value = ""2"" name=""segment_id""/>
            <param value = ""3"" name=""segment_no""/>
            <param value = ""G23M-00303T"" name=""segment_code""/>
            <param value = ""C1"" name=""circuit_number""/>
            <param value = ""72"" name=""component_number_id""/>
            <param value = ""1"" name=""component_number_revision""/>
            <param value = ""NEWCOMPNO.80"" name=""component_number_name""/>
            <param value = ""2"" name=""component_type_code""/>
            <param value = ""Chip Capacitor"" name=""component_type_name""/>
            <param value = ""159584.0"" name=""pos_x""/>
            <param value = ""47332.0"" name=""pos_y""/>
            <param value = ""440.0"" name=""size_x""/>
            <param value = ""480.0"" name=""size_y""/>
            <param value = ""180"" name=""component_angle""/>
            <param value = ""0"" name=""individual_flag""/>
            <param value = ""4"" name=""window_count""/>
            <param value = ""0"" name=""inspection_result_code""/>
            <param value = ""OK"" name=""inspection_result_name""/>
            <param value = """" name=""confirm_result_code""/>
            <param value = """" name=""confirm_result_name""/>
            <param value = ""0"" name=""user_confirm_flag""/>
            <param value = ""0"" name=""last_result""/>
        </data>
        <data>
            <param value = ""7"" name=""component_id""/>
            <param value = ""2"" name=""segment_id""/>
            <param value = ""3"" name=""segment_no""/>
            <param value = ""G23M-00303T"" name=""segment_code""/>
            <param value = ""C2"" name=""circuit_number""/>
            <param value = ""72"" name=""component_number_id""/>
            <param value = ""1"" name=""component_number_revision""/>
            <param value = ""NEWCOMPNO.80"" name=""component_number_name""/>
            <param value = ""2"" name=""component_type_code""/>
            <param value = ""Chip Capacitor"" name=""component_type_name""/>
            <param value = ""151352.0"" name=""pos_x""/>
            <param value = ""40074.0"" name=""pos_y""/>
            <param value = ""440.0"" name=""size_x""/>
            <param value = ""480.0"" name=""size_y""/>
            <param value = ""180"" name=""component_angle""/>
            <param value = ""0"" name=""individual_flag""/>
            <param value = ""4"" name=""window_count""/>
            <param value = ""0"" name=""inspection_result_code""/>
            <param value = ""OK"" name=""inspection_result_name""/>
            <param value = """" name=""confirm_result_code""/>
            <param value = """" name=""confirm_result_name""/>
            <param value = ""0"" name=""user_confirm_flag""/>
            <param value = ""0"" name=""last_result""/>
        </data>
        <data>
            <param value = ""8"" name=""component_id""/>
            <param value = ""2"" name=""segment_id""/>
            <param value = ""3"" name=""segment_no""/>
            <param value = ""G23M-00303T"" name=""segment_code""/>
            <param value = ""C3"" name=""circuit_number""/>
            <param value = ""72"" name=""component_number_id""/>
            <param value = ""1"" name=""component_number_revision""/>
            <param value = ""NEWCOMPNO.80"" name=""component_number_name""/>
            <param value = ""2"" name=""component_type_code""/>
            <param value = ""Chip Capacitor"" name=""component_type_name""/>
            <param value = ""177334.0"" name=""pos_x""/>
            <param value = ""12984.0"" name=""pos_y""/>
            <param value = ""440.0"" name=""size_x""/>
            <param value = ""480.0"" name=""size_y""/>
            <param value = ""180"" name=""component_angle""/>
            <param value = ""0"" name=""individual_flag""/>
            <param value = ""4"" name=""window_count""/>
            <param value = ""0"" name=""inspection_result_code""/>
            <param value = ""OK"" name=""inspection_result_name""/>
            <param value = """" name=""confirm_result_code""/>
            <param value = """" name=""confirm_result_name""/>
            <param value = ""0"" name=""user_confirm_flag""/>
            <param value = ""0"" name=""last_result""/>
        </data>
        <data>
            <param value = ""9"" name=""component_id""/>
            <param value = ""2"" name=""segment_id""/>
            <param value = ""3"" name=""segment_no""/>
            <param value = ""G23M-00303T"" name=""segment_code""/>
            <param value = ""C4"" name=""circuit_number""/>
            <param value = ""72"" name=""component_number_id""/>
            <param value = ""1"" name=""component_number_revision""/>
            <param value = ""NEWCOMPNO.80"" name=""component_number_name""/>
            <param value = ""2"" name=""component_type_code""/>
            <param value = ""Chip Capacitor"" name=""component_type_name""/>
            <param value = ""137449.0"" name=""pos_x""/>
            <param value = ""10627.0"" name=""pos_y""/>
            <param value = ""440.0"" name=""size_x""/>
            <param value = ""480.0"" name=""size_y""/>
            <param value = ""180"" name=""component_angle""/>
            <param value = ""0"" name=""individual_flag""/>
            <param value = ""4"" name=""window_count""/>
            <param value = ""0"" name=""inspection_result_code""/>
            <param value = ""OK"" name=""inspection_result_name""/>
            <param value = """" name=""confirm_result_code""/>
            <param value = """" name=""confirm_result_name""/>
            <param value = ""0"" name=""user_confirm_flag""/>
            <param value = ""0"" name=""last_result""/>
        </data>
        <data>
            <param value = ""10"" name=""component_id""/>
            <param value = ""2"" name=""segment_id""/>
            <param value = ""3"" name=""segment_no""/>
            <param value = ""G23M-00303T"" name=""segment_code""/>
            <param value = ""C5"" name=""circuit_number""/>
            <param value = ""72"" name=""component_number_id""/>
            <param value = ""1"" name=""component_number_revision""/>
            <param value = ""NEWCOMPNO.80"" name=""component_number_name""/>
            <param value = ""2"" name=""component_type_code""/>
            <param value = ""Chip Capacitor"" name=""component_type_name""/>
            <param value = ""85949.0"" name=""pos_x""/>
            <param value = ""25435.0"" name=""pos_y""/>
            <param value = ""440.0"" name=""size_x""/>
            <param value = ""480.0"" name=""size_y""/>
            <param value = ""180"" name=""component_angle""/>
            <param value = ""0"" name=""individual_flag""/>
            <param value = ""4"" name=""window_count""/>
            <param value = ""0"" name=""inspection_result_code""/>
            <param value = ""OK"" name=""inspection_result_name""/>
            <param value = """" name=""confirm_result_code""/>
            <param value = """" name=""confirm_result_name""/>
            <param value = ""0"" name=""user_confirm_flag""/>
            <param value = ""0"" name=""last_result""/>
        </data>
        <data>
            <param value = ""11"" name=""component_id""/>
            <param value = ""3"" name=""segment_id""/>
            <param value = ""4"" name=""segment_no""/>
            <param value = ""04123|6789"" name=""segment_code""/>
            <param value = ""C1"" name=""circuit_number""/>
            <param value = ""72"" name=""component_number_id""/>
            <param value = ""1"" name=""component_number_revision""/>
            <param value = ""NEWCOMPNO.80"" name=""component_number_name""/>
            <param value = ""2"" name=""component_type_code""/>
            <param value = ""Chip Capacitor"" name=""component_type_name""/>
            <param value = ""159604.0"" name=""pos_x""/>
            <param value = ""103664.0"" name=""pos_y""/>
            <param value = ""440.0"" name=""size_x""/>
            <param value = ""480.0"" name=""size_y""/>
            <param value = ""180"" name=""component_angle""/>
            <param value = ""0"" name=""individual_flag""/>
            <param value = ""4"" name=""window_count""/>
            <param value = ""0"" name=""inspection_result_code""/>
            <param value = ""OK"" name=""inspection_result_name""/>
            <param value = """" name=""confirm_result_code""/>
            <param value = """" name=""confirm_result_name""/>
            <param value = ""0"" name=""user_confirm_flag""/>
            <param value = ""0"" name=""last_result""/>
        </data>
        <data>
            <param value = ""12"" name=""component_id""/>
            <param value = ""3"" name=""segment_id""/>
            <param value = ""4"" name=""segment_no""/>
            <param value = ""04123|6789"" name=""segment_code""/>
            <param value = ""C2"" name=""circuit_number""/>
            <param value = ""72"" name=""component_number_id""/>
            <param value = ""1"" name=""component_number_revision""/>
            <param value = ""NEWCOMPNO.80"" name=""component_number_name""/>
            <param value = ""2"" name=""component_type_code""/>
            <param value = ""Chip Capacitor"" name=""component_type_name""/>
            <param value = ""151343.0"" name=""pos_x""/>
            <param value = ""96455.0"" name=""pos_y""/>
            <param value = ""440.0"" name=""size_x""/>
            <param value = ""480.0"" name=""size_y""/>
            <param value = ""180"" name=""component_angle""/>
            <param value = ""0"" name=""individual_flag""/>
            <param value = ""4"" name=""window_count""/>
            <param value = ""0"" name=""inspection_result_code""/>
            <param value = ""OK"" name=""inspection_result_name""/>
            <param value = """" name=""confirm_result_code""/>
            <param value = """" name=""confirm_result_name""/>
            <param value = ""0"" name=""user_confirm_flag""/>
            <param value = ""0"" name=""last_result""/>
        </data>
        <data>
            <param value = ""13"" name=""component_id""/>
            <param value = ""3"" name=""segment_id""/>
            <param value = ""4"" name=""segment_no""/>
            <param value = ""04123|6789"" name=""segment_code""/>
            <param value = ""C3"" name=""circuit_number""/>
            <param value = ""72"" name=""component_number_id""/>
            <param value = ""1"" name=""component_number_revision""/>
            <param value = ""NEWCOMPNO.80"" name=""component_number_name""/>
            <param value = ""2"" name=""component_type_code""/>
            <param value = ""Chip Capacitor"" name=""component_type_name""/>
            <param value = ""177373.0"" name=""pos_x""/>
            <param value = ""69305.0"" name=""pos_y""/>
            <param value = ""440.0"" name=""size_x""/>
            <param value = ""480.0"" name=""size_y""/>
            <param value = ""180"" name=""component_angle""/>
            <param value = ""0"" name=""individual_flag""/>
            <param value = ""4"" name=""window_count""/>
            <param value = ""0"" name=""inspection_result_code""/>
            <param value = ""OK"" name=""inspection_result_name""/>
            <param value = """" name=""confirm_result_code""/>
            <param value = """" name=""confirm_result_name""/>
            <param value = ""0"" name=""user_confirm_flag""/>
            <param value = ""0"" name=""last_result""/>
        </data>
        <data>
            <param value = ""14"" name=""component_id""/>
            <param value = ""3"" name=""segment_id""/>
            <param value = ""4"" name=""segment_no""/>
            <param value = ""04123|6789"" name=""segment_code""/>
            <param value = ""C4"" name=""circuit_number""/>
            <param value = ""72"" name=""component_number_id""/>
            <param value = ""1"" name=""component_number_revision""/>
            <param value = ""NEWCOMPNO.80"" name=""component_number_name""/>
            <param value = ""2"" name=""component_type_code""/>
            <param value = ""Chip Capacitor"" name=""component_type_name""/>
            <param value = ""137438.0"" name=""pos_x""/>
            <param value = ""66939.0"" name=""pos_y""/>
            <param value = ""440.0"" name=""size_x""/>
            <param value = ""480.0"" name=""size_y""/>
            <param value = ""180"" name=""component_angle""/>
            <param value = ""0"" name=""individual_flag""/>
            <param value = ""4"" name=""window_count""/>
            <param value = ""0"" name=""inspection_result_code""/>
            <param value = ""OK"" name=""inspection_result_name""/>
            <param value = """" name=""confirm_result_code""/>
            <param value = """" name=""confirm_result_name""/>
            <param value = ""0"" name=""user_confirm_flag""/>
            <param value = ""0"" name=""last_result""/>
        </data>
        <data>
            <param value = ""15"" name=""component_id""/>
            <param value = ""3"" name=""segment_id""/>
            <param value = ""4"" name=""segment_no""/>
            <param value = ""04123|6789"" name=""segment_code""/>
            <param value = ""C5"" name=""circuit_number""/>
            <param value = ""72"" name=""component_number_id""/>
            <param value = ""1"" name=""component_number_revision""/>
            <param value = ""NEWCOMPNO.80"" name=""component_number_name""/>
            <param value = ""2"" name=""component_type_code""/>
            <param value = ""Chip Capacitor"" name=""component_type_name""/>
            <param value = ""85969.0"" name=""pos_x""/>
            <param value = ""81805.0"" name=""pos_y""/>
            <param value = ""440.0"" name=""size_x""/>
            <param value = ""480.0"" name=""size_y""/>
            <param value = ""180"" name=""component_angle""/>
            <param value = ""0"" name=""individual_flag""/>
            <param value = ""4"" name=""window_count""/>
            <param value = ""0"" name=""inspection_result_code""/>
            <param value = ""OK"" name=""inspection_result_name""/>
            <param value = """" name=""confirm_result_code""/>
            <param value = """" name=""confirm_result_name""/>
            <param value = ""0"" name=""user_confirm_flag""/>
            <param value = ""0"" name=""last_result""/>
        </data>
        <data>
            <param value = ""16"" name=""component_id""/>
            <param value = ""4"" name=""segment_id""/>
            <param value = ""2"" name=""segment_no""/>
            <param value = ""04123|6789"" name=""segment_code""/>
            <param value = ""C1"" name=""circuit_number""/>
            <param value = ""72"" name=""component_number_id""/>
            <param value = ""1"" name=""component_number_revision""/>
            <param value = ""NEWCOMPNO.80"" name=""component_number_name""/>
            <param value = ""2"" name=""component_type_code""/>
            <param value = ""Chip Capacitor"" name=""component_type_name""/>
            <param value = ""32829.0"" name=""pos_x""/>
            <param value = ""78014.0"" name=""pos_y""/>
            <param value = ""440.0"" name=""size_x""/>
            <param value = ""480.0"" name=""size_y""/>
            <param value = ""360"" name=""component_angle""/>
            <param value = ""0"" name=""individual_flag""/>
            <param value = ""4"" name=""window_count""/>
            <param value = ""0"" name=""inspection_result_code""/>
            <param value = ""OK"" name=""inspection_result_name""/>
            <param value = """" name=""confirm_result_code""/>
            <param value = """" name=""confirm_result_name""/>
            <param value = ""0"" name=""user_confirm_flag""/>
            <param value = ""0"" name=""last_result""/>
        </data>
        <data>
            <param value = ""17"" name=""component_id""/>
            <param value = ""4"" name=""segment_id""/>
            <param value = ""2"" name=""segment_no""/>
            <param value = ""04123|6789"" name=""segment_code""/>
            <param value = ""C2"" name=""circuit_number""/>
            <param value = ""72"" name=""component_number_id""/>
            <param value = ""1"" name=""component_number_revision""/>
            <param value = ""NEWCOMPNO.80"" name=""component_number_name""/>
            <param value = ""2"" name=""component_type_code""/>
            <param value = ""Chip Capacitor"" name=""component_type_name""/>
            <param value = ""41060.0"" name=""pos_x""/>
            <param value = ""85313.0"" name=""pos_y""/>
            <param value = ""440.0"" name=""size_x""/>
            <param value = ""480.0"" name=""size_y""/>
            <param value = ""360"" name=""component_angle""/>
            <param value = ""0"" name=""individual_flag""/>
            <param value = ""4"" name=""window_count""/>
            <param value = ""0"" name=""inspection_result_code""/>
            <param value = ""OK"" name=""inspection_result_name""/>
            <param value = """" name=""confirm_result_code""/>
            <param value = """" name=""confirm_result_name""/>
            <param value = ""0"" name=""user_confirm_flag""/>
            <param value = ""0"" name=""last_result""/>
        </data>
        <data>
            <param value = ""18"" name=""component_id""/>
            <param value = ""4"" name=""segment_id""/>
            <param value = ""2"" name=""segment_no""/>
            <param value = ""04123|6789"" name=""segment_code""/>
            <param value = ""C3"" name=""circuit_number""/>
            <param value = ""72"" name=""component_number_id""/>
            <param value = ""1"" name=""component_number_revision""/>
            <param value = ""NEWCOMPNO.80"" name=""component_number_name""/>
            <param value = ""2"" name=""component_type_code""/>
            <param value = ""Chip Capacitor"" name=""component_type_name""/>
            <param value = ""15058.0"" name=""pos_x""/>
            <param value = ""112402.0"" name=""pos_y""/>
            <param value = ""440.0"" name=""size_x""/>
            <param value = ""480.0"" name=""size_y""/>
            <param value = ""0"" name=""component_angle""/>
            <param value = ""0"" name=""individual_flag""/>
            <param value = ""4"" name=""window_count""/>
            <param value = ""0"" name=""inspection_result_code""/>
            <param value = ""OK"" name=""inspection_result_name""/>
            <param value = """" name=""confirm_result_code""/>
            <param value = """" name=""confirm_result_name""/>
            <param value = ""0"" name=""user_confirm_flag""/>
            <param value = ""0"" name=""last_result""/>
        </data>
        <data>
            <param value = ""19"" name=""component_id""/>
            <param value = ""4"" name=""segment_id""/>
            <param value = ""2"" name=""segment_no""/>
            <param value = ""04123|6789"" name=""segment_code""/>
            <param value = ""C4"" name=""circuit_number""/>
            <param value = ""72"" name=""component_number_id""/>
            <param value = ""1"" name=""component_number_revision""/>
            <param value = ""NEWCOMPNO.80"" name=""component_number_name""/>
            <param value = ""2"" name=""component_type_code""/>
            <param value = ""Chip Capacitor"" name=""component_type_name""/>
            <param value = ""54994.0"" name=""pos_x""/>
            <param value = ""114769.0"" name=""pos_y""/>
            <param value = ""440.0"" name=""size_x""/>
            <param value = ""480.0"" name=""size_y""/>
            <param value = ""0"" name=""component_angle""/>
            <param value = ""0"" name=""individual_flag""/>
            <param value = ""4"" name=""window_count""/>
            <param value = ""0"" name=""inspection_result_code""/>
            <param value = ""OK"" name=""inspection_result_name""/>
            <param value = """" name=""confirm_result_code""/>
            <param value = """" name=""confirm_result_name""/>
            <param value = ""0"" name=""user_confirm_flag""/>
            <param value = ""0"" name=""last_result""/>
        </data>
        <data>
            <param value = ""20"" name=""component_id""/>
            <param value = ""4"" name=""segment_id""/>
            <param value = ""2"" name=""segment_no""/>
            <param value = ""04123|6789"" name=""segment_code""/>
            <param value = ""C5"" name=""circuit_number""/>
            <param value = ""72"" name=""component_number_id""/>
            <param value = ""1"" name=""component_number_revision""/>
            <param value = ""NEWCOMPNO.80"" name=""component_number_name""/>
            <param value = ""2"" name=""component_type_code""/>
            <param value = ""Chip Capacitor"" name=""component_type_name""/>
            <param value = ""106454.0"" name=""pos_x""/>
            <param value = ""99902.0"" name=""pos_y""/>
            <param value = ""440.0"" name=""size_x""/>
            <param value = ""480.0"" name=""size_y""/>
            <param value = ""0"" name=""component_angle""/>
            <param value = ""0"" name=""individual_flag""/>
            <param value = ""4"" name=""window_count""/>
            <param value = ""0"" name=""inspection_result_code""/>
            <param value = ""OK"" name=""inspection_result_name""/>
            <param value = """" name=""confirm_result_code""/>
            <param value = """" name=""confirm_result_name""/>
            <param value = ""0"" name=""user_confirm_flag""/>
            <param value = ""0"" name=""last_result""/>
        </data>
        <data>
            <param value = ""900001"" name=""component_id""/>
            <param value = ""0"" name=""segment_id""/>
            <param value = ""0"" name=""segment_no""/>
            <param value = """" name=""segment_code""/>
            <param value = ""Visual Field"" name=""circuit_number""/>
            <param value = ""-1"" name=""component_number_id""/>
            <param value = ""-1"" name=""component_number_revision""/>
            <param value = ""Foreign Material Detection (Entire PCB)"" name=""component_number_name""/>
            <param value = ""-1"" name=""component_type_code""/>
            <param value = ""Foreign Material Detection (Entire PCB)"" name=""component_type_name""/>
            <param value = ""159040.0"" name=""pos_x""/>
            <param value = ""119015.0"" name=""pos_y""/>
            <param value = ""40960.0"" name=""size_x""/>
            <param value = ""30720.0"" name=""size_y""/>
            <param value = ""0"" name=""component_angle""/>
            <param value = ""0"" name=""individual_flag""/>
            <param value = ""1"" name=""window_count""/>
            <param value = ""1"" name=""inspection_result_code""/>
            <param value = ""Except Inspection"" name=""inspection_result_name""/>
            <param value = """" name=""confirm_result_code""/>
            <param value = """" name=""confirm_result_name""/>
            <param value = ""0"" name=""user_confirm_flag""/>
            <param value = ""3"" name=""last_result""/>
        </data>
        <data>
            <param value = ""900002"" name=""component_id""/>
            <param value = ""0"" name=""segment_id""/>
            <param value = ""0"" name=""segment_no""/>
            <param value = """" name=""segment_code""/>
            <param value = ""Visual Field"" name=""circuit_number""/>
            <param value = ""-1"" name=""component_number_id""/>
            <param value = ""-1"" name=""component_number_revision""/>
            <param value = ""Foreign Material Detection (Entire PCB)"" name=""component_number_name""/>
            <param value = ""-1"" name=""component_type_code""/>
            <param value = ""Foreign Material Detection (Entire PCB)"" name=""component_type_name""/>
            <param value = ""-10000.0"" name=""pos_x""/>
            <param value = ""34483.0"" name=""pos_y""/>
            <param value = ""40960.0"" name=""size_x""/>
            <param value = ""30720.0"" name=""size_y""/>
            <param value = ""0"" name=""component_angle""/>
            <param value = ""0"" name=""individual_flag""/>
            <param value = ""1"" name=""window_count""/>
            <param value = ""1"" name=""inspection_result_code""/>
            <param value = ""Except Inspection"" name=""inspection_result_name""/>
            <param value = """" name=""confirm_result_code""/>
            <param value = """" name=""confirm_result_name""/>
            <param value = ""0"" name=""user_confirm_flag""/>
            <param value = ""3"" name=""last_result""/>
        </data>
        <data>
            <param value = ""900003"" name=""component_id""/>
            <param value = ""0"" name=""segment_id""/>
            <param value = ""0"" name=""segment_no""/>
            <param value = """" name=""segment_code""/>
            <param value = ""Visual Field"" name=""circuit_number""/>
            <param value = ""-1"" name=""component_number_id""/>
            <param value = ""-1"" name=""component_number_revision""/>
            <param value = ""Foreign Material Detection (Entire PCB)"" name=""component_number_name""/>
            <param value = ""-1"" name=""component_type_code""/>
            <param value = ""Foreign Material Detection (Entire PCB)"" name=""component_type_name""/>
            <param value = ""12550.0"" name=""pos_x""/>
            <param value = ""36772.0"" name=""pos_y""/>
            <param value = ""40960.0"" name=""size_x""/>
            <param value = ""30720.0"" name=""size_y""/>
            <param value = ""0"" name=""component_angle""/>
            <param value = ""0"" name=""individual_flag""/>
            <param value = ""1"" name=""window_count""/>
            <param value = ""1"" name=""inspection_result_code""/>
            <param value = ""Except Inspection"" name=""inspection_result_name""/>
            <param value = """" name=""confirm_result_code""/>
            <param value = """" name=""confirm_result_name""/>
            <param value = ""0"" name=""user_confirm_flag""/>
            <param value = ""3"" name=""last_result""/>
        </data>
        <data>
            <param value = ""900004"" name=""component_id""/>
            <param value = ""0"" name=""segment_id""/>
            <param value = ""0"" name=""segment_no""/>
            <param value = """" name=""segment_code""/>
            <param value = ""Visual Field"" name=""circuit_number""/>
            <param value = ""-1"" name=""component_number_id""/>
            <param value = ""-1"" name=""component_number_revision""/>
            <param value = ""Foreign Material Detection (Entire PCB)"" name=""component_number_name""/>
            <param value = ""-1"" name=""component_type_code""/>
            <param value = ""Foreign Material Detection (Entire PCB)"" name=""component_type_name""/>
            <param value = ""1958.0"" name=""pos_x""/>
            <param value = ""57570.0"" name=""pos_y""/>
            <param value = ""40960.0"" name=""size_x""/>
            <param value = ""30720.0"" name=""size_y""/>
            <param value = ""0"" name=""component_angle""/>
            <param value = ""0"" name=""individual_flag""/>
            <param value = ""1"" name=""window_count""/>
            <param value = ""1"" name=""inspection_result_code""/>
            <param value = ""Except Inspection"" name=""inspection_result_name""/>
            <param value = """" name=""confirm_result_code""/>
            <param value = """" name=""confirm_result_name""/>
            <param value = ""0"" name=""user_confirm_flag""/>
            <param value = ""3"" name=""last_result""/>
        </data>
        <data>
            <param value = ""900005"" name=""component_id""/>
            <param value = ""0"" name=""segment_id""/>
            <param value = ""0"" name=""segment_no""/>
            <param value = """" name=""segment_code""/>
            <param value = ""Visual Field"" name=""circuit_number""/>
            <param value = ""-1"" name=""component_number_id""/>
            <param value = ""-1"" name=""component_number_revision""/>
            <param value = ""Foreign Material Detection (Entire PCB)"" name=""component_number_name""/>
            <param value = ""-1"" name=""component_type_code""/>
            <param value = ""Foreign Material Detection (Entire PCB)"" name=""component_type_name""/>
            <param value = ""23625.0"" name=""pos_x""/>
            <param value = ""83263.0"" name=""pos_y""/>
            <param value = ""40960.0"" name=""size_x""/>
            <param value = ""30720.0"" name=""size_y""/>
            <param value = ""0"" name=""component_angle""/>
            <param value = ""0"" name=""individual_flag""/>
            <param value = ""1"" name=""window_count""/>
            <param value = ""1"" name=""inspection_result_code""/>
            <param value = ""Except Inspection"" name=""inspection_result_name""/>
            <param value = """" name=""confirm_result_code""/>
            <param value = """" name=""confirm_result_name""/>
            <param value = ""0"" name=""user_confirm_flag""/>
            <param value = ""3"" name=""last_result""/>
        </data>
        <data>
            <param value = ""900006"" name=""component_id""/>
            <param value = ""0"" name=""segment_id""/>
            <param value = ""0"" name=""segment_no""/>
            <param value = """" name=""segment_code""/>
            <param value = ""Visual Field"" name=""circuit_number""/>
            <param value = ""-1"" name=""component_number_id""/>
            <param value = ""-1"" name=""component_number_revision""/>
            <param value = ""Foreign Material Detection (Entire PCB)"" name=""component_number_name""/>
            <param value = ""-1"" name=""component_type_code""/>
            <param value = ""Foreign Material Detection (Entire PCB)"" name=""component_type_name""/>
            <param value = ""65263.0"" name=""pos_x""/>
            <param value = ""40985.0"" name=""pos_y""/>
            <param value = ""40960.0"" name=""size_x""/>
            <param value = ""30720.0"" name=""size_y""/>
            <param value = ""0"" name=""component_angle""/>
            <param value = ""0"" name=""individual_flag""/>
            <param value = ""1"" name=""window_count""/>
            <param value = ""1"" name=""inspection_result_code""/>
            <param value = ""Except Inspection"" name=""inspection_result_name""/>
            <param value = """" name=""confirm_result_code""/>
            <param value = """" name=""confirm_result_name""/>
            <param value = ""0"" name=""user_confirm_flag""/>
            <param value = ""3"" name=""last_result""/>
        </data>
        <data>
            <param value = ""900007"" name=""component_id""/>
            <param value = ""0"" name=""segment_id""/>
            <param value = ""0"" name=""segment_no""/>
            <param value = """" name=""segment_code""/>
            <param value = ""Visual Field"" name=""circuit_number""/>
            <param value = ""-1"" name=""component_number_id""/>
            <param value = ""-1"" name=""component_number_revision""/>
            <param value = ""Foreign Material Detection (Entire PCB)"" name=""component_number_name""/>
            <param value = ""-1"" name=""component_type_code""/>
            <param value = ""Foreign Material Detection (Entire PCB)"" name=""component_type_name""/>
            <param value = ""116753.0"" name=""pos_x""/>
            <param value = ""29720.0"" name=""pos_y""/>
            <param value = ""40960.0"" name=""size_x""/>
            <param value = ""30720.0"" name=""size_y""/>
            <param value = ""0"" name=""component_angle""/>
            <param value = ""0"" name=""individual_flag""/>
            <param value = ""1"" name=""window_count""/>
            <param value = ""1"" name=""inspection_result_code""/>
            <param value = ""Except Inspection"" name=""inspection_result_name""/>
            <param value = """" name=""confirm_result_code""/>
            <param value = """" name=""confirm_result_name""/>
            <param value = ""0"" name=""user_confirm_flag""/>
            <param value = ""3"" name=""last_result""/>
        </data>
        <data>
            <param value = ""900008"" name=""component_id""/>
            <param value = ""0"" name=""segment_id""/>
            <param value = ""0"" name=""segment_no""/>
            <param value = """" name=""segment_code""/>
            <param value = ""Visual Field"" name=""circuit_number""/>
            <param value = ""-1"" name=""component_number_id""/>
            <param value = ""-1"" name=""component_number_revision""/>
            <param value = ""Foreign Material Detection (Entire PCB)"" name=""component_number_name""/>
            <param value = ""-1"" name=""component_type_code""/>
            <param value = ""Foreign Material Detection (Entire PCB)"" name=""component_type_name""/>
            <param value = ""143663.0"" name=""pos_x""/>
            <param value = ""42064.0"" name=""pos_y""/>
            <param value = ""40960.0"" name=""size_x""/>
            <param value = ""30720.0"" name=""size_y""/>
            <param value = ""0"" name=""component_angle""/>
            <param value = ""0"" name=""individual_flag""/>
            <param value = ""1"" name=""window_count""/>
            <param value = ""1"" name=""inspection_result_code""/>
            <param value = ""Except Inspection"" name=""inspection_result_name""/>
            <param value = """" name=""confirm_result_code""/>
            <param value = """" name=""confirm_result_name""/>
            <param value = ""0"" name=""user_confirm_flag""/>
            <param value = ""3"" name=""last_result""/>
        </data>
        <data>
            <param value = ""900009"" name=""component_id""/>
            <param value = ""0"" name=""segment_id""/>
            <param value = ""0"" name=""segment_no""/>
            <param value = """" name=""segment_code""/>
            <param value = ""Visual Field"" name=""circuit_number""/>
            <param value = ""-1"" name=""component_number_id""/>
            <param value = ""-1"" name=""component_number_revision""/>
            <param value = ""Foreign Material Detection (Entire PCB)"" name=""component_number_name""/>
            <param value = ""-1"" name=""component_type_code""/>
            <param value = ""Foreign Material Detection (Entire PCB)"" name=""component_type_name""/>
            <param value = ""147793.0"" name=""pos_x""/>
            <param value = ""73859.0"" name=""pos_y""/>
            <param value = ""40960.0"" name=""size_x""/>
            <param value = ""30720.0"" name=""size_y""/>
            <param value = ""0"" name=""component_angle""/>
            <param value = ""0"" name=""individual_flag""/>
            <param value = ""1"" name=""window_count""/>
            <param value = ""1"" name=""inspection_result_code""/>
            <param value = ""Except Inspection"" name=""inspection_result_name""/>
            <param value = """" name=""confirm_result_code""/>
            <param value = """" name=""confirm_result_name""/>
            <param value = ""0"" name=""user_confirm_flag""/>
            <param value = ""3"" name=""last_result""/>
        </data>
        <data>
            <param value = ""900010"" name=""component_id""/>
            <param value = ""0"" name=""segment_id""/>
            <param value = ""0"" name=""segment_no""/>
            <param value = """" name=""segment_code""/>
            <param value = ""Visual Field"" name=""circuit_number""/>
            <param value = ""-1"" name=""component_number_id""/>
            <param value = ""-1"" name=""component_number_revision""/>
            <param value = ""Foreign Material Detection (Entire PCB)"" name=""component_number_name""/>
            <param value = ""-1"" name=""component_type_code""/>
            <param value = ""Foreign Material Detection (Entire PCB)"" name=""component_type_name""/>
            <param value = ""134773.0"" name=""pos_x""/>
            <param value = ""115580.0"" name=""pos_y""/>
            <param value = ""40960.0"" name=""size_x""/>
            <param value = ""30720.0"" name=""size_y""/>
            <param value = ""0"" name=""component_angle""/>
            <param value = ""0"" name=""individual_flag""/>
            <param value = ""1"" name=""window_count""/>
            <param value = ""1"" name=""inspection_result_code""/>
            <param value = ""Except Inspection"" name=""inspection_result_name""/>
            <param value = """" name=""confirm_result_code""/>
            <param value = """" name=""confirm_result_name""/>
            <param value = ""0"" name=""user_confirm_flag""/>
            <param value = ""3"" name=""last_result""/>
        </data>
        <data>
            <param value = ""900011"" name=""component_id""/>
            <param value = ""0"" name=""segment_id""/>
            <param value = ""0"" name=""segment_no""/>
            <param value = """" name=""segment_code""/>
            <param value = ""Visual Field"" name=""circuit_number""/>
            <param value = ""-1"" name=""component_number_id""/>
            <param value = ""-1"" name=""component_number_revision""/>
            <param value = ""Foreign Material Detection (Entire PCB)"" name=""component_number_name""/>
            <param value = ""-1"" name=""component_type_code""/>
            <param value = ""Foreign Material Detection (Entire PCB)"" name=""component_type_name""/>
            <param value = ""101471.0"" name=""pos_x""/>
            <param value = ""70589.0"" name=""pos_y""/>
            <param value = ""40960.0"" name=""size_x""/>
            <param value = ""30720.0"" name=""size_y""/>
            <param value = ""0"" name=""component_angle""/>
            <param value = ""0"" name=""individual_flag""/>
            <param value = ""1"" name=""window_count""/>
            <param value = ""1"" name=""inspection_result_code""/>
            <param value = ""Except Inspection"" name=""inspection_result_name""/>
            <param value = """" name=""confirm_result_code""/>
            <param value = """" name=""confirm_result_name""/>
            <param value = ""0"" name=""user_confirm_flag""/>
            <param value = ""3"" name=""last_result""/>
        </data>
        <data>
            <param value = ""900012"" name=""component_id""/>
            <param value = ""0"" name=""segment_id""/>
            <param value = ""0"" name=""segment_no""/>
            <param value = """" name=""segment_code""/>
            <param value = ""Visual Field"" name=""circuit_number""/>
            <param value = ""-1"" name=""component_number_id""/>
            <param value = ""-1"" name=""component_number_revision""/>
            <param value = ""Foreign Material Detection (Entire PCB)"" name=""component_number_name""/>
            <param value = ""-1"" name=""component_type_code""/>
            <param value = ""Foreign Material Detection (Entire PCB)"" name=""component_type_name""/>
            <param value = ""75726.0"" name=""pos_x""/>
            <param value = ""106139.0"" name=""pos_y""/>
            <param value = ""40960.0"" name=""size_x""/>
            <param value = ""30720.0"" name=""size_y""/>
            <param value = ""0"" name=""component_angle""/>
            <param value = ""0"" name=""individual_flag""/>
            <param value = ""1"" name=""window_count""/>
            <param value = ""1"" name=""inspection_result_code""/>
            <param value = ""Except Inspection"" name=""inspection_result_name""/>
            <param value = """" name=""confirm_result_code""/>
            <param value = """" name=""confirm_result_name""/>
            <param value = ""0"" name=""user_confirm_flag""/>
            <param value = ""3"" name=""last_result""/>
        </data>
        <data>
            <param value = ""900013"" name=""component_id""/>
            <param value = ""0"" name=""segment_id""/>
            <param value = ""0"" name=""segment_no""/>
            <param value = """" name=""segment_code""/>
            <param value = ""Visual Field"" name=""circuit_number""/>
            <param value = ""-1"" name=""component_number_id""/>
            <param value = ""-1"" name=""component_number_revision""/>
            <param value = ""Foreign Material Detection (Entire PCB)"" name=""component_number_name""/>
            <param value = ""-1"" name=""component_type_code""/>
            <param value = ""Foreign Material Detection (Entire PCB)"" name=""component_type_name""/>
            <param value = ""52787.0"" name=""pos_x""/>
            <param value = ""126000.0"" name=""pos_y""/>
            <param value = ""40960.0"" name=""size_x""/>
            <param value = ""30720.0"" name=""size_y""/>
            <param value = ""0"" name=""component_angle""/>
            <param value = ""0"" name=""individual_flag""/>
            <param value = ""1"" name=""window_count""/>
            <param value = ""1"" name=""inspection_result_code""/>
            <param value = ""Except Inspection"" name=""inspection_result_name""/>
            <param value = """" name=""confirm_result_code""/>
            <param value = """" name=""confirm_result_name""/>
            <param value = ""0"" name=""user_confirm_flag""/>
            <param value = ""3"" name=""last_result""/>
        </data>
        <data>
            <param value = ""900014"" name=""component_id""/>
            <param value = ""0"" name=""segment_id""/>
            <param value = ""0"" name=""segment_no""/>
            <param value = """" name=""segment_code""/>
            <param value = ""Visual Field"" name=""circuit_number""/>
            <param value = ""-1"" name=""component_number_id""/>
            <param value = ""-1"" name=""component_number_revision""/>
            <param value = ""Foreign Material Detection (Entire PCB)"" name=""component_number_name""/>
            <param value = ""-1"" name=""component_type_code""/>
            <param value = ""Foreign Material Detection (Entire PCB)"" name=""component_type_name""/>
            <param value = ""7790.0"" name=""pos_x""/>
            <param value = ""113872.0"" name=""pos_y""/>
            <param value = ""40960.0"" name=""size_x""/>
            <param value = ""30720.0"" name=""size_y""/>
            <param value = ""0"" name=""component_angle""/>
            <param value = ""0"" name=""individual_flag""/>
            <param value = ""1"" name=""window_count""/>
            <param value = ""1"" name=""inspection_result_code""/>
            <param value = ""Except Inspection"" name=""inspection_result_name""/>
            <param value = """" name=""confirm_result_code""/>
            <param value = """" name=""confirm_result_name""/>
            <param value = ""0"" name=""user_confirm_flag""/>
            <param value = ""3"" name=""last_result""/>
        </data>
    </output>
    <recordCount>34</recordCount>
    <returnCode>0</returnCode>
</resultDataM>";
        public string testXML_0count = @"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes""?>
                            <resultDataM>
                                <recordCount>0</recordCount>
                                <returnCode>1</returnCode>
                            </resultDataM>";
        #endregion
    }


}

class CsvFileName
{
    public string MachineName { get; set; }//from CSV file name
    public string BatchName { get; set; }//from CSV file name
    public string BoardID { get; set; }//from CSV file name
    public string ProcessName { get; set; }//from CSV file name
    public string BoardSide { get; set; }//from CSV file name
    public string ProcessTime { get; set; }//from CSV file name
}
class Inspection
{
    public string MachineName { get; set; }//from CSV file name
    public string InspectionID { get; set; }//get from API 4-1
    public string confirm_date { get; set; }
    public string inspection_end_time { get; set; }
    public string SegmentNo { get; set; }//get from API 4-3(ComponentBlockName)
    public string SegmentCode { get; set; }//get from API 4-3(block_barcode) 
    public string CircuitNumber { get; set; }//get from API 4-3(PartsName)
    public string ComponentID { get; set; }//get from API 4-3
    public string LastResult { get; set; }//get from API 4-3
    public string ComponentNo { get; set; }//get from CSV Table
    public string InspectionImagePath { get; set; }//get from API 4-12
}

