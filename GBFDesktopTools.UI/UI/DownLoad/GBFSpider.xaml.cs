using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Data;
using GBFDesktopTools.Model;
using GBFDesktopTools.Model.abstractModel;
using GBFDesktopTools.Model.ToolAndHelper;

namespace GBFDesktopTools.View
{
    /// <summary>
    /// GBFSpider.xaml 的交互逻辑
    /// </summary>
    public partial class GBFSpider : Window
    {
        //下载条件列表
        public List<SpiderCondition> DownList = new List<SpiderCondition>();
        public Navigationer<GBFSpider, pgSelectCondition> Ng = null;
        //下载状态
        public DownLoadMessage Dlm = new DownLoadMessage("Welcome GBFSpiderProgram", "Click the Buttons to Use the Program");
        public bool IsSelectCondition = false;
        
        //下载对象
        //技能列表
        public ObjectResult<WeaponSkill> SkillList = new ObjectResult<WeaponSkill>();
        //武器列表
        public ObjectResult<Weapon> WeaponList = new ObjectResult<Weapon>();

        #region ToolsDelegate

        delegate List<string> SplitStr(string Target, short splitType = 1, string CustomStr = null, string[] CustomArray = null,StringSplitOptions IsHaveEmpty = StringSplitOptions.None);
        SplitStr SplitString = ToolsAndHelper.SplitString;

        #endregion

        public GBFSpider()
        {
            InitializeComponent();
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            DownLoadMessage.ItemsSource = Dlm.CustomMessage;
            this.PBSpiderProgress.DataContext = Dlm;  
            this.Loaded += new RoutedEventHandler(GBFSpider_Loaded);
        }

        #region Event

        private void GBFSpider_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= GBFSpider_Loaded;
            Dlm.ProgressBarValue = 0;
        }

        private void SpiderRun_Click(object sender, RoutedEventArgs e)
        {
            Button SpiderButton = sender as Button;
            string SwitchCondition = SpiderButton.Tag.ToString();

            switch (SwitchCondition)
            {
                case "SelectionCondition":
                    Ng = new Navigationer<GBFSpider, pgSelectCondition>(this, new pgSelectCondition(this));
                    Ng.Title = "下载条件选择";
                    Ng.SetWinSize(true, 350, 470);
                    Ng.ShowPg();
                    Ng = null;
                    break;
                case "RunDownLoad":
                    if (IsSelectCondition == false) 
                    { 
                        MessageBox.Show("请选择下载条件"); 
                        return; 
                    }
                    //String projectName = Assembly.GetExecutingAssembly().GetName().Name.ToString();string A = "https://gbf.huijiwiki.com/wiki/%E8%A7%92%E8%89%B2%E6%90%9C%E7%B4%A2%E5%99%A8?rarity=3";
                    //GetHTMLResponse(A);
                    Dlm.AddCustomMessage("正在从本地加载数据，请稍等");

                    var skillObj = LoadFromLocalSkill();
                    if (!skillObj.hasError)
                    {
                        SkillList = skillObj;
                    }
                    else
                    {
                        MessageBox.Show(skillObj.ErrorMsg);
                        return;
                    }

                    var weaponObj = LoadFromLocalWeapon();
                    if (!weaponObj.hasError)
                    {
                        WeaponList = weaponObj;
                    }
                    else
                    {
                        MessageBox.Show(weaponObj.ErrorMsg);
                        return;
                    }

                    Dlm.ProgressBarValue = 100;
                    Dlm.AddCustomMessage("加载完成!");
                    break;
                case "Back":


                    break;
            }

        }

        #endregion

        #region WebMethod

        /// <summary>
        /// 获取HTML响应 并读取内容转化为string返回
        /// </summary>
        /// <param name="Url">目标网址</param>
        /// <returns>返回页面内容(string)</returns>
        public string GetHTMLResponse(string Url)
        {
            WebRequest WRq = WebRequest.Create(Url);
            String Result = String.Empty;
            try
            {
                WebResponse Response = WRq.GetResponse();
                StreamReader Sr = new StreamReader(Response.GetResponseStream(),Encoding.GetEncoding("utf-8"));
                Result = Sr.ReadToEnd();
                FileStream fs = new FileStream(@"E:\Temp.txt", FileMode.Create, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(Result);
                Sr.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }

        //public void DownLoadRun()
        //{   
        //    //获取目标信息的网址
        //    Dictionary<string, string> TargetUrlDic = new Dictionary<string, string>();
        //    TargetUrlDic.Add("RoleR", "https://gbf.huijiwiki.com/wiki/%E8%A7%92%E8%89%B2%E6%90%9C%E7%B4%A2%E5%99%A8?rarity=1");
        //    TargetUrlDic.Add("RoleSR", "https://gbf.huijiwiki.com/wiki/%E8%A7%92%E8%89%B2%E6%90%9C%E7%B4%A2%E5%99%A8?rarity=2");
        //    TargetUrlDic.Add("RoleSSR", "https://gbf.huijiwiki.com/wiki/%E8%A7%92%E8%89%B2%E6%90%9C%E7%B4%A2%E5%99%A8?rarity=3");

        //    TargetUrlDic.Add("WeaponN", "https://gbf.huijiwiki.com/wiki/%E6%AD%A6%E5%99%A8%E6%90%9C%E7%B4%A2%E5%99%A8?rarity=1");
        //    TargetUrlDic.Add("WeaponR", "https://gbf.huijiwiki.com/wiki/%E6%AD%A6%E5%99%A8%E6%90%9C%E7%B4%A2%E5%99%A8?rarity=2");
        //    TargetUrlDic.Add("WeaponSR", "https://gbf.huijiwiki.com/wiki/%E6%AD%A6%E5%99%A8%E6%90%9C%E7%B4%A2%E5%99%A8?rarity=3");
        //    TargetUrlDic.Add("WeaponSSR", "https://gbf.huijiwiki.com/wiki/%E6%AD%A6%E5%99%A8%E6%90%9C%E7%B4%A2%E5%99%A8?rarity=4");

        //    TargetUrlDic.Add("SummonGemN", "https://gbf.huijiwiki.com/wiki/%E5%8F%AC%E5%94%A4%E7%9F%B3%E6%90%9C%E7%B4%A2%E5%99%A8?rarity=1");
        //    TargetUrlDic.Add("SummonGemR", "https://gbf.huijiwiki.com/wiki/%E5%8F%AC%E5%94%A4%E7%9F%B3%E6%90%9C%E7%B4%A2%E5%99%A8?rarity=2");
        //    TargetUrlDic.Add("SummonGemSR", "https://gbf.huijiwiki.com/wiki/%E5%8F%AC%E5%94%A4%E7%9F%B3%E6%90%9C%E7%B4%A2%E5%99%A8?rarity=3");
        //    TargetUrlDic.Add("SummonGemSSR", "https://gbf.huijiwiki.com/wiki/%E5%8F%AC%E5%94%A4%E7%9F%B3%E6%90%9C%E7%B4%A2%E5%99%A8?rarity=4");

        //    foreach (var Downl in DownList)
        //    {
        //        int Amount;
        //        string ErrorAddress = Downl.DownLoadAddress + "/ErrorFile/";

        //        try
        //        {
        //            //目标类型
        //            string WRType = Downl.ConditionTargetType.ToString();
        //            //稀有度
        //            string Rarity = Downl.ConditionRarity.ToString();
        //            //操作类型 
        //            string Exc = Downl.ConditionExcType.ToString();

        //            #region 提取总列表的网页
        //            //保存文件的路径
        //            string address = Downl.DownLoadAddress + "All" + WRType + ".html";
        //            //如果文件保存失败 生存的错误文件
        //            ErrorAddress += "Error.html";



        //            GetResponse(address, Url, ErrorAddress);

        //            if (File.Exists(address) == false)
        //            {
        //                Exception ex = new Exception("ERROR：Target link is not responding！");
        //                throw ex;
        //            }
        //            Console.Clear();
        //            Console.WriteLine("全列表获取成功");
        //            Console.WriteLine("正在获取所有网页");
        //            #endregion

        //            #region 获取所有的网页
        //            reg = "/wiki/Char/.+?\"";
        //            string[] target = MatchTarget(address, reg, 1);
        //            Amount = target.Length;
        //            if (isUpdata == false)
        //            {
        //                for (int i = 0; i < Amount; i++)
        //                {
        //                    address = "E:/GBFSpiderTest/" + WRType + "/" + Rarity + "/" + WRType + i + ".html";
        //                    double Percentage = i / (double)Amount;
        //                    Console.Clear();
        //                    Console.WriteLine("全列表获取成功");
        //                    Console.WriteLine("正在获取所有网页");
        //                    Console.WriteLine("正在获取第" + (i + 1) + "条" + "     " + Percentage.ToString("0.###%"));
        //                    if (File.Exists(address) == false)
        //                    {
        //                        Url = target[i];
        //                        ErrorAddress = "E:/GBFSpiderTest/" + WRType + "/" + Rarity + "/" + WRType + i + ".html";
        //                        GetResponse(address, Url, ErrorAddress);
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                for (int i = 0; i < Amount; i++)
        //                {
        //                    address = "E:/GBFSpiderTest/" + WRType + "/" + Rarity + "/" + WRType + i + ".html";
        //                    double Percentage = i / (double)Amount;
        //                    Console.Clear();
        //                    Console.WriteLine("全列表获取成功");
        //                    Console.WriteLine("正在获取所有网页");
        //                    Console.WriteLine("正在获取第" + (i + 1) + "条" + "     " + Percentage.ToString("0.###%"));
        //                    Url = target[i];
        //                    ErrorAddress = "E:/GBFSpiderTest/" + WRType + "/" + Rarity + "/" + WRType + i + ".html";
        //                    GetResponse(address, Url, ErrorAddress);
        //                }
        //            }
        //            Console.WriteLine("全部" + Rarity + "'" + WRType + "获取成功");
        //            if (File.Exists("E:/GBFSpiderTest/" + WRType + "/" + Rarity + "/All" + WRType + ".html"))
        //            {
        //                File.Delete("E:/GBFSpiderTest/" + WRType + "/" + Rarity + "/All" + WRType + ".html");
        //            }

        //            #endregion

        //            #region 获取所有网页中想要的信息
        //            bool Isdelete = false;
        //            address = "E:/GBFSpiderTest/" + WRType + "/" + Rarity;
        //            DirectoryInfo di = new DirectoryInfo(address);
        //            FileInfo[] fi = di.GetFiles();
        //            Amount = fi.Length;
        //            Console.WriteLine("开始获取每个网页的信息");

        //            List<GetWebMessage> GWMList = new List<GetWebMessage>();
        //            List<Weapon> WpList = new List<Weapon>();

        //            for (var i = 0; i < Amount; i++)
        //            {
        //                double B = i / (double)Amount;
        //                Console.Clear();
        //                Console.WriteLine("开始获取每个网页的信息\n获取第" + (i + 1) + "条中      " + B.ToString("0.###%") + "\n");
        //                GetWebMessage GWM = new GetWebMessage();
        //                Weapon Wp = new Weapon();

        //                address = "E:/GBFSpiderTest/" + WRType + "/" + Rarity + "/" + WRType + i + ".html";

        //                reg = "Char/(\\d)+";
        //                GWM.FnGBF_RoleID = MatchTarget(address, reg, 3)[0];

        //                GWM.Rarity = Rarity;
        //                Console.WriteLine("获取ID以及稀有度成功");

        //                reg = "item-name\">.+(</div>){2}<div class=\"cha";
        //                GWM.RoleName = MatchTarget(address, reg, 4)[0];
        //                Console.WriteLine("姓名获取成功");

        //                reg = "evo-name jp\">\\[.+</s";
        //                GWM.FsGBF_NickName = MatchTarget(address, reg, 5)[0];
        //                Console.WriteLine("获取称号成功");

        //                string Description = string.Empty;
        //                reg = "角色档案</span></h2>.+<h2 class=\"hide-pc\">";
        //                Description = MatchTarget(address, reg, 6)[0];
        //                char[] separator = "&".ToCharArray();
        //                List<string> SplitList = Description.Split(separator).ToList();
        //                if (SplitList.Count >= 6)
        //                {
        //                    GWM.Age = SplitList[0];
        //                    GWM.Height = SplitList[1];
        //                    GWM.VoiceActors = SplitList[2];
        //                    GWM.Interest = SplitList[3];
        //                    GWM.Hobbies = SplitList[4];
        //                    GWM.NotGoodAt = SplitList[5];
        //                }
        //                else
        //                {
        //                    GWM.Description = SplitList[0];
        //                }
        //                Console.WriteLine("获取档案成功");

        //                reg = "解锁角色的.{2}";
        //                GWM.FsGBF_WeaponUrl = MatchTarget(address, reg, 2)[0];
        //                GWM.FnGBF_WeaponID = MatchTarget(address, reg, 2)[1];
        //                Console.WriteLine("获取角色解锁条件成功");

        //                reg = "https:\\/\\/huiji-public.huijistatic.com\\/gbf\\/uploads\\/(\\w)+\\/(\\w)+\\/(\\w)+02\\.png";
        //                GWM.ImageUrl = FuncDownLoadImage(reg, i, WRType, Rarity, ImageType);
        //                Console.WriteLine("图片链接获取成功");
        //                GWMList.Add(GWM);
        //                if (Isdelete == true) File.Delete(address);
        //            }

        //            #endregion
        //            Console.Clear();
        //            Console.WriteLine("所有信息获取完成，按任意键保存文件到本地");
        //            Console.ReadKey();
        //            xmlDoc(GWMList);
        //            Console.WriteLine("保存完毕");

        //        }

        //        catch (Exception ex)
        //        {

        //        }

        //    }
        //}

        #endregion

        #region 下载图片

        //public static string FuncDownLoadImage(string reg, int Index, string WRType, string Rarity, string ImageType)
        //{
        //    try
        //    {
        //        Console.WriteLine("下载" + ImageType + "中");
        //        bool Result;
        //        string address = "E:/GBFSpiderTest/" + WRType + "/" + Rarity + "/" + WRType + Index + ".html";
        //        string[] target = MatchTarget(address, reg, 0);
        //        if (target.Length == 0)
        //        {
        //            Exception ex = new Exception("E:/GBFSpiderTest/" + ImageType + "/" + Rarity + "/" + "UrlNotFound" + Index + ".txt");
        //            throw ex;
        //        }
        //        string ImageUrl = target[0];
        //        address = "E:/GBFSpiderTest/" + ImageType + "/" + Rarity + "/" + ImageType + Index + ".png";
        //        Result = DownLoadImage(ImageUrl, address, -1);
        //        if (Result == false)
        //        {
        //            File.Delete(address);
        //            Exception ex = new Exception("E:/GBFSpiderTest/" + ImageType + "/" + Rarity + "/" + "Error" + Index + ".txt");
        //            Console.WriteLine(ImageType + "下载失败");
        //            throw ex;
        //        }
        //        else
        //        {
        //            Console.WriteLine(ImageType + "下载成功");
        //            return address;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        File.Create(ex.Message.ToString());
        //        return ex.Message.ToString();
        //    }
        //}

        /// <summary>
        /// 下载图片核心功能
        /// </summary>
        /// <param name="Url">图片Http地址</param>
        /// <param name="Address">保存路径</param>
        /// <param name="timeOut">Request最大请求时间，-1为无限</param>
        /// <returns></returns>
        public static bool DownLoadImage(string Url, string Address, int timeOut)
        {
            bool Result = false;
            WebResponse WResp = null;
            Stream st = null;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                WResp = request.GetResponse();
                st = WResp.GetResponseStream();
                if (!WResp.ContentType.ToLower().StartsWith("text/"))
                    Result = SaveBinaryFile(WResp, Address);
            }
            finally
            {
                if (st != null)
                {
                    st.Close();
                }
                if (WResp != null)
                {
                    WResp.Close();
                }
            }
            return Result;
        }

        private static bool SaveBinaryFile(WebResponse WResp, string Address)
        {
            bool value = false;
            byte[] buffer = new byte[1024];
            Stream outstream = null;
            Stream instream = null;
            try
            {
                if (File.Exists(Address))
                {
                    File.Delete(Address);
                }
                outstream = System.IO.File.Create(Address);
                instream = WResp.GetResponseStream();
                int i;
                do
                {
                    i = instream.Read(buffer, 0, buffer.Length);
                    if (i > 0) outstream.Write(buffer, 0, i);
                } while (i > 0);
                value = true;
            }
            finally
            {
                if (outstream != null)
                {
                    outstream.Close();
                }
                if (instream != null)
                {
                    instream.Close();
                }
            }
            return value;
        }

        #endregion

        #region SpiderMethod

        //#region 角色

        //static void Main(string[] args)
        //{
        //    bool isUpdata = false;
        //    string reg;
        //    string address;
        //    string ErrorAddress;
        //    string Url;
        //    string WRType;
        //    string ImageType;
        //    string Rarity;
        //    int Amount;
        //    Console.Title = "SpiderOfGbfWiki";
        //    try
        //    {
        //        WRType = "Role";
        //        Rarity = "SSR";
        //        ImageType = "RoleImage";
        //        #region 提取总列表的网页
        //        address = "E:/GBFSpiderTest/" + WRType + "/" + Rarity + "/All" + WRType + ".html";
        //        ErrorAddress = "E:/GBFSpiderTest/" + WRType + "/" + Rarity + "/Error.html";
        //        Url = "https://gbf.huijiwiki.com/wiki/SSR%E4%BA%BA%E7%89%A9";
        //        GetResponse(address, Url, ErrorAddress);

        //        if (File.Exists(address) == false)
        //        {
        //            Exception ex = new Exception("ERROR：Target link is not responding！");
        //            throw ex;
        //        }
        //        Console.Clear();
        //        Console.WriteLine("全列表获取成功");
        //        Console.WriteLine("正在获取所有网页");
        //        #endregion

        //        #region 获取所有的网页
        //        reg = "/wiki/Char/.+?\"";
        //        string[] target = MatchTarget(address, reg, 1);
        //        Amount = target.Length;
        //        if (isUpdata == false)
        //        {
        //            for (int i = 0; i < Amount; i++)
        //            {
        //                address = "E:/GBFSpiderTest/" + WRType + "/" + Rarity + "/" + WRType + i + ".html";
        //                double Percentage = i / (double)Amount;
        //                Console.Clear();
        //                Console.WriteLine("全列表获取成功");
        //                Console.WriteLine("正在获取所有网页");
        //                Console.WriteLine("正在获取第" + (i + 1) + "条" + "     " + Percentage.ToString("0.###%"));
        //                if (File.Exists(address) == false)
        //                {
        //                    Url = target[i];
        //                    ErrorAddress = "E:/GBFSpiderTest/" + WRType + "/" + Rarity + "/" + WRType + i + ".html";
        //                    GetResponse(address, Url, ErrorAddress);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            for (int i = 0; i < Amount; i++)
        //            {
        //                address = "E:/GBFSpiderTest/" + WRType + "/" + Rarity + "/" + WRType + i + ".html";
        //                double Percentage = i / (double)Amount;
        //                Console.Clear();
        //                Console.WriteLine("全列表获取成功");
        //                Console.WriteLine("正在获取所有网页");
        //                Console.WriteLine("正在获取第" + (i + 1) + "条" + "     " + Percentage.ToString("0.###%"));
        //                Url = target[i];
        //                ErrorAddress = "E:/GBFSpiderTest/" + WRType + "/" + Rarity + "/" + WRType + i + ".html";
        //                GetResponse(address, Url, ErrorAddress);
        //            }
        //        }
        //        Console.WriteLine("全部" + Rarity + "'" + WRType + "获取成功");
        //        if (File.Exists("E:/GBFSpiderTest/" + WRType + "/" + Rarity + "/All" + WRType + ".html"))
        //        {
        //            File.Delete("E:/GBFSpiderTest/" + WRType + "/" + Rarity + "/All" + WRType + ".html");
        //        }

        //        #endregion

        //        #region 获取所有网页中想要的信息
        //        bool Isdelete = false;
        //        address = "E:/GBFSpiderTest/" + WRType + "/" + Rarity;
        //        DirectoryInfo di = new DirectoryInfo(address);
        //        FileInfo[] fi = di.GetFiles();
        //        Amount = fi.Length;
        //        Console.WriteLine("开始获取每个网页的信息");

        //        List<GetWebMessage> GWMList = new List<GetWebMessage>();
        //        List<Weapon> WpList = new List<Weapon>();

        //        for (var i = 0; i < Amount; i++)
        //        {
        //            double B = i / (double)Amount;
        //            Console.Clear();
        //            Console.WriteLine("开始获取每个网页的信息\n获取第" + (i + 1) + "条中      " + B.ToString("0.###%") + "\n");
        //            GetWebMessage GWM = new GetWebMessage();
        //            Weapon Wp = new Weapon();

        //            address = "E:/GBFSpiderTest/" + WRType + "/" + Rarity + "/" + WRType + i + ".html";

        //            reg = "Char/(\\d)+";
        //            GWM.FnGBF_RoleID = MatchTarget(address, reg, 3)[0];

        //            GWM.Rarity = Rarity;
        //            Console.WriteLine("获取ID以及稀有度成功");

        //            reg = "item-name\">.+(</div>){2}<div class=\"cha";
        //            GWM.RoleName = MatchTarget(address, reg, 4)[0];
        //            Console.WriteLine("姓名获取成功");

        //            reg = "evo-name jp\">\\[.+</s";
        //            GWM.FsGBF_NickName = MatchTarget(address, reg, 5)[0];
        //            Console.WriteLine("获取称号成功");

        //            string Description = string.Empty;
        //            reg = "角色档案</span></h2>.+<h2 class=\"hide-pc\">";
        //            Description = MatchTarget(address, reg, 6)[0];
        //            char[] separator = "&".ToCharArray();
        //            List<string> SplitList = Description.Split(separator).ToList();
        //            if (SplitList.Count >= 6)
        //            {
        //                GWM.Age = SplitList[0];
        //                GWM.Height = SplitList[1];
        //                GWM.VoiceActors = SplitList[2];
        //                GWM.Interest = SplitList[3];
        //                GWM.Hobbies = SplitList[4];
        //                GWM.NotGoodAt = SplitList[5];
        //            }
        //            else
        //            {
        //                GWM.Description = SplitList[0];
        //            }
        //            Console.WriteLine("获取档案成功");

        //            reg = "解锁角色的.{2}";
        //            GWM.FsGBF_WeaponUrl = MatchTarget(address, reg, 2)[0];
        //            GWM.FnGBF_WeaponID = MatchTarget(address, reg, 2)[1];
        //            Console.WriteLine("获取角色解锁条件成功");

        //            reg = "https:\\/\\/huiji-public.huijistatic.com\\/gbf\\/uploads\\/(\\w)+\\/(\\w)+\\/(\\w)+02\\.png";
        //            GWM.ImageUrl = FuncDownLoadImage(reg, i, WRType, Rarity, ImageType);
        //            Console.WriteLine("图片链接获取成功");
        //            GWMList.Add(GWM);
        //            if (Isdelete == true) File.Delete(address);
        //        }

        //        #endregion
        //        Console.Clear();
        //        Console.WriteLine("所有信息获取完成，按任意键保存文件到本地");
        //        Console.ReadKey();
        //        xmlDoc(GWMList);
        //        Console.WriteLine("保存完毕");

        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message.ToString() + "   按任意键退出。");
        //        Console.ReadKey();
        //    }


        //    //正则匹配 角色档案区域 角色档案</span></h2>.+<h2 class="hide-pc">
        //    //仔细划分 
        //    //年龄 年龄.+>(\d)+岁
        //    //身高 身高.+>(\d)+cm
        //    //声优 ([\u4e00-\u9fa5])+</a>
        //    //兴趣 兴趣.{27}[\u4e00-\u9fa5]+
        //    //喜好 喜好.{27}[\u4e00-\u9fa5]+
        //    //不擅长 不擅长</div><div class="content">[\u4e00-\u9fa5]+

        //    //正则匹配 RoleIndex  PageName":\s"Char/.+",\s"wgT
        //    //正则匹配 NikeName  evo-name jp">\[.+</s
        //    //正则匹配 RoleName  item-name">.+(</div>){2}<div class="char
        //    //正则匹配 description  \sjp">.+<
        //    //正则匹配 ImageUrl  https:\/\/huiji-public.huijistatic.com\/gbf\/uploads\/(\w)+\/(\w)+\/(\w)+02\.png  匹配出来后删除所有空格
        //    //正则匹配 unlockWeaponorItemUrl 先匹配 解锁角色的.{2}  判断是物品还是武器 武器直接匹配 /wiki/Weapon/(\d)+  物品则匹配 href="/wiki/Item/(\d)+"\sc

        //    //WRType = "Role";
        //    //ImageType = "RoleImage";
        //    //Rarity = "SSR";
        //    //reg = "https:\\/\\/huiji-public.huijistatic.com\\/gbf\\/uploads\\/(\\w)+\\/(\\w)+\\/(\\w)+02\\.png\"";

        //    Console.ReadKey();
        //}

        //#endregion

        //#region Weapon
        
        //static void Main(string[] args)
        //{

        //}
        //*/
        //#endregion

        //#region 提取

        //public static void GetResponse(string Address, string Url, string ErrorAddress, string EncodingFormat = "utf-8")
        //{
        //    WebRequest request = WebRequest.Create(Url);
        //    bool IsNormal = true;

        //    WebResponse responseA;
        //    try
        //    {
        //        responseA = request.GetResponse();
        //    }
        //    catch (WebException ex)
        //    {
        //        responseA = (HttpWebResponse)ex.Response;
        //        IsNormal = false;
        //    }
        //    StreamReader sr = new StreamReader(responseA.GetResponseStream(), Encoding.GetEncoding(EncodingFormat));

        //    string A = sr.ReadToEnd();
        //    try
        //    {
        //        if (IsNormal == false)
        //        {
        //            Address = ErrorAddress;
        //        }
        //        FileStream fs = new FileStream(Address, FileMode.Create, FileAccess.Write);
        //        StreamWriter sw = new StreamWriter(fs);
        //        sw.Write(A);
        //        sw.Close();
        //        fs.Close();
        //    }
        //    catch (Exception ex) { throw ex; }
        //}

        //#endregion

        //#region 匹配
        ///// <summary>
        ///// 正则匹配
        ///// </summary>
        ///// <param name="address">Path</param>
        ///// <param name="reg">正则表达式</param>
        ///// <param name="ExcType">匹配类型</param>
        ///// <returns></returns>
        //public static string[] MatchTarget(string address, string reg, int ExcType)
        //{
        //    MatchCollection matches;
        //    string C;
        //    string[] Str;
        //    using (StreamReader reader = new StreamReader(address, Encoding.GetEncoding("utf-8")))
        //    {
        //        C = reader.ReadToEnd();
        //        matches = Regex.Matches(C, reg);
        //    }
        //    if (matches.Count == 0 && ExcType == 5)
        //    {
        //        reg = "evo-name\\sjp\">\\[.+\\]";
        //        matches = Regex.Matches(C, reg);
        //        Str = ConversionArray(matches);
        //        Str[0] = Str[0].Remove(0, 14);
        //        Str[0] = Str[0].Remove(Str[0].Length - 1, 1);
        //        return Str;
        //    }
        //    else if (matches.Count == 0 && ExcType == 6)
        //    {
        //        Str = new string[1] { "无角色档案" };
        //        return Str;
        //    }
        //    else if (matches.Count == 0)
        //    {
        //        Str = new string[1];
        //    }
        //    else
        //    {
        //        Str = ConversionArray(matches);
        //    }

        //    for (int i = 0; i < Str.Length; i++)
        //    {
        //        string B = string.Empty;
        //        switch (ExcType)
        //        {
        //            case 0:
        //                {
        //                    return Str;
        //                }

        //            case 1:
        //                {
        //                    B = "https://gbf.huijiwiki.com";
        //                    B += Str[i].Remove(Str[i].Length - 1, 1);
        //                    break;
        //                }

        //            case 2:
        //                {
        //                    if (Str[0] == null)
        //                    {
        //                        Str = new string[2] { "解锁角色的条件：无", "000000" };
        //                        return Str;
        //                    }
        //                    B = Str[i].Remove(0, 5);
        //                    if (B == "武器")
        //                    {
        //                        reg = "/wiki/Weapon/(\\d)+";
        //                        Str[0] = Regex.Match(C, reg).ToString();
        //                        List<string> Lstr = Str.ToList<string>();
        //                        Lstr.Add(Str[0].Remove(0, 13));
        //                        Str = Lstr.ToArray();
        //                        Str[0] = "https://gbf.huijiwiki.com" + Str[0];
        //                        return Str;
        //                    }
        //                    else if (B == "物品")
        //                    {
        //                        reg = "href=\"/wiki/Item/(\\d)+\"\\sc";
        //                        Str[0] = Regex.Match(C, reg).ToString();
        //                        Str[0] = Str[0].Remove(0, 6);
        //                        Str[0] = Str[0].Remove(Str[0].Length - 3, 3);
        //                        List<string> Lstr = Str.ToList<string>();
        //                        Lstr.Add(Str[0].Remove(0, 12));
        //                        Str = Lstr.ToArray();
        //                        Str[0] = "https://gbf.huijiwiki.com" + Str[0];
        //                        return Str;
        //                    }
        //                    break;
        //                }

        //            case 3:
        //                {
        //                    Str[i] = Str[i].Remove(0, 5);
        //                    return Str;
        //                }
        //            case 4:
        //                {
        //                    Str[i] = Str[i].Remove(0, 11);
        //                    Str[i] = Str[i].Remove(Str[i].Length - 27, 27);
        //                    return Str;
        //                }
        //            case 5:
        //                {
        //                    Str[i] = Str[i].Remove(0, 28);
        //                    int index = Str[0].IndexOf(">");
        //                    Str[i] = Str[i].Remove(0, index + 1);
        //                    Str[i] = Str[i].Remove(Str[i].Length - 3, 3);
        //                    return Str;
        //                }
        //            case 6:
        //                {
        //                    string InformationFile = string.Empty;
        //                    string Temp = string.Empty;
        //                    if (Str.Length == 1)
        //                    {
        //                        Program pr = new Program();
        //                        Str[0] = pr.RexRoleFile(Str[0]);
        //                        return Str;
        //                    }
        //                    else
        //                    {
        //                        Program pr = new Program();
        //                        string Te = string.Empty;
        //                        for (var a = 0; a < Str.Length; a++)
        //                        {
        //                            Te += pr.RexRoleFile(Str[a]);
        //                        }
        //                    }
        //                    return Str;
        //                }
        //        }
        //        Str[i] = B;
        //    }
        //    return Str;
        //}

        //public string RexRoleFile(string Target)
        //{
        //    string InformationFile = string.Empty;
        //    string reg = string.Empty;
        //    string Temp = string.Empty;

        //    //reg = "me\">([\u4e00-\u9fa5])+";
        //    //Temp = Regex.Match(Target, reg).ToString();
        //    //Temp = Temp.Remove(0, 4);
        //    //InformationFile += ("<" + Temp + ">");

        //    reg = "年龄.+>(\\d)+岁";
        //    Temp = Regex.Match(Target, reg).ToString();
        //    if (Temp == string.Empty)
        //    {
        //        Temp = "年龄：不明";
        //    }
        //    else
        //    {
        //        Temp = "年龄：" + Temp.Remove(0, 29);
        //    }

        //    InformationFile += (Temp + "&");

        //    reg = "身高.+>(\\d)+cm";
        //    Temp = Regex.Match(Target, reg).ToString();
        //    if (Temp == string.Empty) Temp = "身高:不明";
        //    else
        //    {
        //        Temp = "身高:" + Temp.Remove(0, 29);
        //    }
        //    InformationFile += (Temp + "&");

        //    reg = "([\u4e00-\u9fa5])+</a>";
        //    Temp = Regex.Match(Target, reg).ToString();
        //    if (Temp == string.Empty)
        //    {
        //        Temp = Regex.Match(Target, "([\u4e00-\u9fa5])+\">").ToString();
        //        if (Temp != string.Empty) Temp = Temp.Remove(Temp.Length - 2, 2);
        //        else
        //        {
        //            Temp = Regex.Match(Target, "([\u4e00-\u9fa5])+（").ToString();
        //            if (Temp == string.Empty) Temp = Regex.Match(Target, "？？？").ToString();
        //            else
        //            {
        //                Temp = Temp.Remove(Temp.Length - 1, 1);
        //            }
        //            Temp = "声优:不明";
        //        }
        //    }
        //    else
        //    {
        //        Temp = "声优:" + Temp.Remove(Temp.Length - 4, 4);
        //    }
        //    InformationFile += (Temp + "&");

        //    reg = "兴趣.{27}[\u4e00-\u9fa5]+";
        //    Temp = Regex.Match(Target, reg).ToString();
        //    if (Temp == string.Empty)
        //    {
        //        Temp = "兴趣：不明";
        //    }
        //    else
        //    {
        //        Temp = "兴趣：" + Temp.Remove(0, 29);
        //    }
        //    InformationFile += (Temp + "&");

        //    reg = "喜好.{27}[\u4e00-\u9fa5]+";
        //    Temp = Regex.Match(Target, reg).ToString();
        //    if (Temp == string.Empty)
        //    {
        //        Temp = "喜好：不明";
        //    }
        //    else
        //    {
        //        Temp = "喜好：" + Temp.Remove(0, 29);
        //    }
        //    InformationFile += (Temp + "&");

        //    reg = "不擅长</div><div class=\"content\">[\u4e00-\u9fa5]+";
        //    Temp = Regex.Match(Target, reg).ToString();
        //    if (Temp == string.Empty)
        //    {
        //        Temp = "不擅长：不明";
        //    }
        //    else
        //    {
        //        Temp = "不擅长：" + Temp.Remove(0, 30);
        //    }
        //    InformationFile += (Temp + "&");

        //    return InformationFile;
        //}

        //#endregion

        //#region 转换string为stringArray并移除数组中重复数据

        //public static string[] ConversionArray(MatchCollection matches)
        //{
        //    List<string> Str = new List<string>();
        //    for (int i = 0; i < matches.Count; i++)
        //    {
        //        Str.Add(matches[i].ToString());
        //    }
        //    string[] AStr = Str.ToArray();
        //    AStr = DelRepeatData(AStr);
        //    return AStr;
        //}

        ///// <summary>
        ///// 移除数组中重复数据
        ///// </summary>
        ///// <param name="array">需要除重的数组</param>
        ///// <returns>不重复数组</returns>
        //public static string[] DelRepeatData(string[] array)
        //{
        //    return array.GroupBy(p => p).Select(p => p.Key).ToArray();
        //}

        //#endregion



        //#region XML

        //public static void xmlDoc(List<GetWebMessage> GWMList)
        //{
        //    #region XML

        //    //在内存中构建Dom对象
        //    XmlDocument xmlDoc = new XmlDocument();

        //    //文档说明
        //    XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "yes");
        //    xmlDoc.AppendChild(xmlDeclaration);

        //    //根元素
        //    XmlElement rootElement = xmlDoc.CreateElement("RoleMessage");
        //    xmlDoc.AppendChild(rootElement);

        //    foreach (var item in GWMList)
        //    {
        //        //子元素Role
        //        XmlElement childElement = xmlDoc.CreateElement("Role");
        //        childElement.SetAttribute("RoleID", item.FnGBF_RoleID);
        //        rootElement.AppendChild(childElement);

        //        //Role扩展(普通信息)
        //        XmlElement GeneralInformation = xmlDoc.CreateElement("GeneralInformation");
        //        childElement.AppendChild(GeneralInformation);

        //        #region GeneralInformation扩展元素
        //        //角色ID
        //        XmlElement FnGBF_RoleID = xmlDoc.CreateElement("FnGBF_RoleID");
        //        FnGBF_RoleID.InnerText = item.FnGBF_RoleID.ToString();
        //        GeneralInformation.AppendChild(FnGBF_RoleID);
        //        //稀有度
        //        XmlElement Rarity = xmlDoc.CreateElement("Rarity");
        //        Rarity.InnerText = item.Rarity;
        //        GeneralInformation.AppendChild(Rarity);
        //        //别称
        //        XmlElement FsGBF_NickName = xmlDoc.CreateElement("FsGBF_NickName");
        //        FsGBF_NickName.InnerText = item.FsGBF_NickName;
        //        GeneralInformation.AppendChild(FsGBF_NickName);
        //        //角色名称
        //        XmlElement RoleName = xmlDoc.CreateElement("RoleName");
        //        RoleName.InnerText = item.RoleName;
        //        GeneralInformation.AppendChild(RoleName);
        //        if (item.Age != null)
        //        {
        //            //年龄
        //            XmlElement Age = xmlDoc.CreateElement("Age");
        //            Age.InnerText = item.Age;
        //            GeneralInformation.AppendChild(Age);
        //            if (item.Age.Length > 8)
        //            {
        //                item.IsMultipleRoles = true;
        //            }
        //        }
        //        if (item.Height != null)
        //        {
        //            //身高
        //            XmlElement Height = xmlDoc.CreateElement("Height");
        //            Height.InnerText = item.Height;
        //            GeneralInformation.AppendChild(Height);
        //            if (item.Height.Length > 8)
        //            {
        //                item.IsMultipleRoles = true;
        //            }
        //        }
        //        if (item.VoiceActors != null)
        //        {
        //            //Cv
        //            XmlElement VoiceActors = xmlDoc.CreateElement("VoiceActors");
        //            VoiceActors.InnerText = item.VoiceActors;
        //            GeneralInformation.AppendChild(VoiceActors);
        //        }
        //        if (item.Interest != null)
        //        {
        //            //兴趣
        //            XmlElement Interest = xmlDoc.CreateElement("Interest");
        //            Interest.InnerText = item.Interest;
        //            GeneralInformation.AppendChild(Interest);
        //        }
        //        if (item.Hobbies != null)
        //        {
        //            //爱好
        //            XmlElement Hobbies = xmlDoc.CreateElement("Hobbies");
        //            Hobbies.InnerText = item.Hobbies;
        //            GeneralInformation.AppendChild(Hobbies);
        //        }
        //        if (item.NotGoodAt != null)
        //        {
        //            //不擅长
        //            XmlElement NotGoodAt = xmlDoc.CreateElement("NotGoodAt");
        //            NotGoodAt.InnerText = item.NotGoodAt;
        //            GeneralInformation.AppendChild(NotGoodAt);
        //        }
        //        //角色描述
        //        XmlElement Description = xmlDoc.CreateElement("Description");
        //        Description.InnerText = item.Description;
        //        GeneralInformation.AppendChild(Description);
        //        //武器链接
        //        XmlElement FsGBF_WeaponUrl = xmlDoc.CreateElement("FsGBF_WeaponUrl");
        //        FsGBF_WeaponUrl.InnerText = item.FsGBF_WeaponUrl;
        //        GeneralInformation.AppendChild(FsGBF_WeaponUrl);
        //        //武器ID
        //        XmlElement FnGBF_WeaponID = xmlDoc.CreateElement("FnGBF_WeaponID");
        //        FnGBF_WeaponID.InnerText = item.FnGBF_WeaponID;
        //        GeneralInformation.AppendChild(FnGBF_WeaponID);
        //        //角色图片链接
        //        XmlElement ImageUrl = xmlDoc.CreateElement("ImageUrl");
        //        ImageUrl.InnerText = item.ImageUrl;
        //        GeneralInformation.AppendChild(ImageUrl);

        //        if (item.IsMultipleRoles == true)
        //        {
        //            XmlElement SpecialInformation = xmlDoc.CreateElement("SpecialInformation");
        //            childElement.AppendChild(SpecialInformation);

        //            XmlElement AnotherRole = xmlDoc.CreateElement("AnotherRoleName");
        //            AnotherRole.InnerText = string.Empty;
        //            SpecialInformation.AppendChild(AnotherRole);

        //            XmlElement Age = xmlDoc.CreateElement("Age");
        //            Age.InnerText = "年龄：";
        //            SpecialInformation.AppendChild(Age);

        //            XmlElement Height = xmlDoc.CreateElement("Height");
        //            Height.InnerText = "身高：";
        //            SpecialInformation.AppendChild(Height);

        //            XmlElement VoiceActors = xmlDoc.CreateElement("VoiceActors");
        //            VoiceActors.InnerText = "声优：";
        //            SpecialInformation.AppendChild(VoiceActors);

        //            XmlElement Interest = xmlDoc.CreateElement("Interest");
        //            Interest.InnerText = "兴趣：";
        //            SpecialInformation.AppendChild(Interest);

        //            XmlElement Hobbies = xmlDoc.CreateElement("Hobbies");
        //            Hobbies.InnerText = "喜好：";
        //            SpecialInformation.AppendChild(Hobbies);

        //            XmlElement NotGoodAt = xmlDoc.CreateElement("NotGoodAt");
        //            NotGoodAt.InnerText = "不擅长：";
        //            SpecialInformation.AppendChild(NotGoodAt);
        //        }

        //        #endregion

        //    }

        //    xmlDoc.Save(@"E:\RoleMessage.xml");

        //    #endregion
        //}

        //#endregion

        #region Weapon

        /// <summary>
        /// 设置武器图片路径(更新图片后调用)
        /// </summary>
        /// <param name="ID">武器ID</param>
        /// <param name="RowIndex">武器在Excel中的行数索引</param>
        /// <param name="Rarity">稀有度</param>
        /// <param name="sheet">导入目标源Excel</param>
        public void FilePath(long ID, int RowIndex, string Rarity, Aspose.Cells.Worksheet sheet)
        {
            DirectoryInfo directory = new DirectoryInfo(@"E:\WeaponPanelSimulator\WeaponPanelSimulator\bin\Debug\Resources\Image\Weapon\" + Rarity + @"\");
            directory.Create();
            //输出稀有度目录下的所有文件与文件夹,如果目标文件夹为空 return
            List<FileSystemInfo> files = directory.GetFileSystemInfos().ToList();
            if (files == null || files.Count == 0) { return; }
            //如果没有找到目标的资源文件夹则return
            var f = files.Where(x => x.Name.Remove(x.Name.IndexOf("_")) == ID.ToString()).FirstOrDefault();
            if (f == null) { return; }

            string pathOfFile = f.FullName;
            string fileName = files.Where(x => x.Name.Remove(x.Name.IndexOf("_")) == ID.ToString()).FirstOrDefault().Name;
            if (pathOfFile == null) { return; }
            DirectoryInfo dirt = new DirectoryInfo(pathOfFile);
            var fileList = dirt.GetFiles().ToList();
            if (fileList.Exists(x => x.Name.Remove(x.Name.IndexOf("_")) == "b"))
            {
                sheet.Cells[RowIndex, 32].PutValue(@"Resources\Image\Weapon\" + Rarity + @"\" + fileName + @"\b_" + ID + ".jpg");
            }
            if (fileList.Exists(x => x.Name.Remove(x.Name.IndexOf("_")) == "cjs"))
            {
                sheet.Cells[RowIndex, 33].PutValue(@"Resources\Image\Weapon\" + Rarity + @"\" + fileName + @"\cjs_" + ID + ".jpg");
            }
            if (fileList.Exists(x => x.Name.Remove(x.Name.IndexOf("_")) == "ls"))
            {
                sheet.Cells[RowIndex, 34].PutValue(@"Resources\Image\Weapon\" + Rarity + @"\" + fileName + @"\ls_" + ID + ".jpg");
            }
            if (fileList.Exists(x => x.Name.Remove(x.Name.IndexOf("_")) == "m"))
            {
                sheet.Cells[RowIndex, 35].PutValue(@"Resources\Image\Weapon\" + Rarity + @"\" + fileName + @"\m_" + ID + ".jpg");
            }
            if (fileList.Exists(x => x.Name.Remove(x.Name.IndexOf("_")) == "s"))
            {
                sheet.Cells[RowIndex, 36].PutValue(@"Resources\Image\Weapon\" + Rarity + @"\" + fileName + @"\s_" + ID + ".jpg");
            }
        }

        /// <summary>
        /// 设置武器技能
        /// </summary>
        /// <param name="SkillStr">Excel中的技能短字符串</param>
        /// <param name="Weapon">被设置技能的武器</param>
        public void SetSkillNew(String SkillStr, Weapon Weapon)
        {
            if (SkillStr == "") return;
            //不确定的技能名单
            List<string> ineptitude = ToolsAndHelper.GetSpecialSkillList();
            try
            {
                //先用英文逗号分割技能字符串 ，提取其中的多个技能
                var SkillArrray = SplitString(SkillStr, 2,IsHaveEmpty:StringSplitOptions.RemoveEmptyEntries);
                //遍历技能列表获取技能名称
                foreach (var item in SkillArrray)
                {
                    //技能实体
                    WeaponSkill Skill = null;
                    //技能主名称
                    string SkillMainName = "";
                    //技能副名称
                    //技能属性
                    string Element = "";
                    //技能后缀
                    string SkillSuffix = "";
                    //后缀索引
                    short SkillSuffixIndex = 0;

                    //判断是否为特殊技能 如果是则直接设置特殊技能
                    if (ineptitude.Exists(x => x == item))
                    {
                        Skill = SkillList.ObjStrDic[item].FirstOrDefault().Clone() as WeaponSkill;
                        //设置是否启用技能警告
                        Weapon.SkillWarning = true;
                        Weapon.WeaponSkill.Add(Skill);
                        continue;
                    }
                    
                    var TempStr = SplitString(item,3);
                    //取出技能主名称
                    SkillMainName = TempStr.FirstOrDefault();

                    //当索引为1的字符串 不为数字（属性）时 ，寻找技能副名称，且索引为2的字符串为属性
                    if (!ToolsAndHelper.StringContentType(TempStr[1],2))
                    {
                        var ExtraNameResult = ToolsAndHelper.FoundSkillExtraName(SkillMainName, TempStr[1], SkillList.ObjStrDic);
                        
                        //如果返回值为空则说明字典集中不存在副名称为SkillExtraName的技能
                        if (ExtraNameResult == "")
                        {
                            Skill = SkillList.ObjStrDic["errorSkill"].FirstOrDefault();
                            Weapon.WeaponSkill.Add(Skill);
                            Weapon.SkillWarning = true;
                            continue;
                        }

                        //将返回值(主名称+副名称) 赋值给主名称
                        SkillMainName = ExtraNameResult;
                        Element = TempStr[2];
                        //技能后缀的索引
                        SkillSuffixIndex = 3;
                    }
                    else
                    {
                        //否则索引为1的字符串为元素
                        //赋值属性，设置技能后缀索引
                        Element = TempStr[1];
                        SkillSuffixIndex = 2;
                    }
                    //赋值后缀
                    if (TempStr.Count >= 1 + SkillSuffixIndex)
                    {
                        SkillSuffix = TempStr[SkillSuffixIndex];
                        if (TempStr.Count > 1 + SkillSuffixIndex)
                        {
                            SkillSuffix += TempStr[1 + SkillSuffixIndex];
                        }
                    }
                    //验证后缀
                    SkillSuffix = ToolsAndHelper.FoundSkillSuffix(SkillMainName, SkillSuffix, SkillList.ObjStrDic);
                    
                    //如果技能字典集中有此技能
                    if (SkillList.ObjStrDic.Keys.ToList().Exists(x => x == SkillMainName))
                    {
                        //搜索并设置武器的技能
                        Skill = SkillList.ObjStrDic[SkillMainName].FirstOrDefault(x => x.Extra_Name == SkillSuffix);
                        if (Skill == null)
                        {
                            Skill = new WeaponSkill()
                            {
                                Main_Name = (WeaponSkill.SkillTypeEnum)Enum.Parse(typeof(WeaponSkill.SkillTypeEnum), SkillMainName),
                                Extra_Name = SkillSuffix,
                                Main_Description = "Excel中未读取到此后缀的技能，请添加",
                                IsCalculation = false,
                                TheReason = "Excel中未读取到此后缀的技能，请添加"
                            };
                            SkillList.ObjStrDic[SkillMainName].Add(Skill);
                            Weapon.SkillWarning = true;
                        }
                        Weapon.WeaponSkill.Add(Skill);
                    }
                    else
                    {
                        throw new Exception("特殊技能 请添加信息");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("设置武器\"" + (Weapon.FsName_CHS == "" || Weapon.FsName_CHS == null ? Weapon.FnWeapon_ID.ToString() : Weapon.FsName_CHS) + "\"的武器技能时时发生错误:" + ex.Message);
            }
        }

        /// <summary>
        /// 读取Excel表格并向内存中写入技能数据
        /// </summary>
        /// RowCount，ColumnCount : 读取的行数和列数 维护时请删除Excel内的空白行 以提高性能
        /// <returns></returns>
        public ObjectResult<WeaponSkill> LoadFromLocalSkill()
        {
            var ObjResult = new ObjectResult<WeaponSkill>();
            List<WeaponSkill> SkillList = new List<WeaponSkill>();
            Aspose.Cells.Workbook wk = null;
            try
            {
                var asm = System.Reflection.Assembly.GetExecutingAssembly();
                string resoureeName = asm.GetName().Name + ".Resources.Excel.WeaponSkill.xls";
                using (System.IO.Stream stream = asm.GetManifestResourceStream(resoureeName))
                {
                    wk = new Aspose.Cells.Workbook(stream);
                }
                var sheet = wk.Worksheets["WeaponSkill"];

                //读取的行数和列数 维护时请根据Excel内的行数列数设置  以提高性能
                int RowCount = sheet.Cells.Rows.Count;
                int ColumnCount = 50;

                DataTable dt = sheet.Cells.ExportDataTable(0, 0, RowCount, ColumnCount);
                for (var i = 2; i < dt.Rows.Count; i++)
                {
                    WeaponSkill skill = new WeaponSkill();
                    skill.Skill_ID = Convert.ToInt32(dt.Rows[i][0]);
                    skill.Main_Name = (WeaponSkill.SkillTypeEnum)Enum.Parse(typeof(WeaponSkill.SkillTypeEnum), dt.Rows[i][1].ToString());
                    skill.Extra_Name = dt.Rows[i][2].ToString() == string.Empty ? "" : dt.Rows[i][2].ToString();
                    skill.Main_Description = dt.Rows[i][3].ToString() == string.Empty ? "" : dt.Rows[i][3].ToString();
                    skill.Main_Description = dt.Rows[i][4].ToString() == string.Empty ? "" : dt.Rows[i][4].ToString();
                    skill.Main_Tag = dt.Rows[i][5].ToString() == string.Empty ? "" : dt.Rows[i][5].ToString();
                    skill.Extra_Tag = dt.Rows[i][6].ToString() == string.Empty ? "" : dt.Rows[i][6].ToString();
                    skill.IsCalculation = dt.Rows[i][7].ToString() == "1" ? true : false;
                    skill.IsSpecialCalculation = dt.Rows[i][8].ToString() == "1" ? true : false;
                    skill.IsSpecial = dt.Rows[i][9].ToString() == "1" ? true : false;
                    skill.TheReason = dt.Rows[i][10].ToString() == string.Empty ? "" : dt.Rows[i][10].ToString();
                    //设置技能目标
                    if (dt.Rows[i][11].ToString() != string.Empty)
                    {
                        skill.SetSkillTarget(dt.Rows[i][11].ToString());
                    }
                    //设置计算方式
                    if (dt.Rows[i][12].ToString() != string.Empty)
                    {
                        skill.SetFormulaModeEnum(dt.Rows[i][12].ToString());
                    }
                    skill.Extra_Comment = dt.Rows[i][13].ToString() == string.Empty ? "" : dt.Rows[i][13].ToString();
                    //设置持续时间类型
                    skill.DurationType = dt.Rows[i][14].ToString() == string.Empty ? WeaponSkill.DurationEnum.NoHave : (WeaponSkill.DurationEnum)Enum.Parse(typeof(WeaponSkill.DurationEnum), dt.Rows[i][14].ToString());
                    skill.DurationValue = dt.Rows[i][15].ToString() == string.Empty ? (short)-1 : Convert.ToInt16(dt.Rows[i][15].ToString());
                    skill.SummonType = dt.Rows[i][16].ToString() == string.Empty ? WeaponSkill.SummonEnum.Normal : (WeaponSkill.SummonEnum)Enum.Parse(typeof(WeaponSkill.SummonEnum), dt.Rows[i][16].ToString());
                    //设置技能生效条件
                    skill.SetConditionType(dt.Rows[i][17].ToString());
                    skill.MaxValue = dt.Rows[i][18].ToString() == String.Empty ? 0 : Convert.ToDouble(dt.Rows[i][18].ToString());
                    skill.BaseLimit = dt.Rows[i][19].ToString() == String.Empty ? -1.0 : Convert.ToDouble(dt.Rows[i][19].ToString());
                    var StrList = new List<string>();
                    for (var index = 19; index < 40; index++)
                    {
                        StrList.Add(dt.Rows[i][index].ToString() == String.Empty ? "" : dt.Rows[i][index].ToString());
                    }
                    skill.SetSkillValue(StrList);
                    SkillList.Add(skill);
                }
                SkillList.Add(new WeaponSkill()
                {   
                    Main_Name = WeaponSkill.SkillTypeEnum.errorSkill,
                    Main_Description = "错误技能",
                    Skill_ID = -999,
                    TheReason = "技能字典集中无此技能，请维护技能字典集",
                    IsCalculation = false
                });
                ObjResult.ObjList = SkillList;
                ObjResult = new WeaponSkill().GetWeaponSkillList(ObjResult);
            }
            catch (Exception ex)
            {
                return ObjResult.SetError("从Excel读取技能数据失败:" + ex.Message);
            }
            return ObjResult;
        }
        
        /// <summary>
        /// 从本地Excel加载Weapon数据
        /// </summary>
        /// <returns></returns>
        public ObjectResult<Weapon> LoadFromLocalWeapon(bool IsUpdateImage = false)
        {
            ObjectResult<Weapon> resultObj = new ObjectResult<Weapon>();
            List<Weapon> WeaponList = new List<Weapon>();
            Aspose.Cells.Workbook wk = null;
            try
            {
                var asm = System.Reflection.Assembly.GetExecutingAssembly();
                string resoureeName = asm.GetName().Name + ".Resources.Excel.武器列表.xls";
                using (System.IO.Stream stream = asm.GetManifestResourceStream(resoureeName))
                {
                    wk = new Aspose.Cells.Workbook(stream);
                }
                var sheet = wk.Worksheets["武器列表"];
                int RowCount = sheet.Cells.Rows.Count;
                int ColumnCount = 80;
                DataTable dt = sheet.Cells.ExportDataTable(0, 0, RowCount, ColumnCount);
                for (var i = 2; i < dt.Rows.Count; i++)
                {
                    Weapon wp = new Weapon();
                    wp.FnWeapon_ID = dt.Rows[i][0].ToString() == string.Empty ? -99 : Convert.ToInt64(dt.Rows[i][0]);
                    wp.FsName_JP = dt.Rows[i][1].ToString() == string.Empty ? "" : dt.Rows[i][1].ToString();
                    wp.FsTitle_JP = dt.Rows[i][2].ToString() == string.Empty ? "" : dt.Rows[i][2].ToString();
                    //wp.FsSeries_Name = dt.Rows[i][3].ToString() == string.Empty ? "" : dt.Rows[i][3].ToString();
                    wp.FsName_EN = dt.Rows[i][4].ToString() == string.Empty ? "" : dt.Rows[i][4].ToString();
                    wp.FsName_CHS = dt.Rows[i][5].ToString() == string.Empty ? "" : dt.Rows[i][5].ToString();
                    wp.FsGBF_Nickname = dt.Rows[i][6].ToString() == string.Empty ? new List<string>() : SplitString(dt.Rows[i][7].ToString());
                    wp.FsSearch_Nickname = dt.Rows[i][8].ToString() == string.Empty ? new List<string>() : SplitString(dt.Rows[i][8].ToString());
                    wp.FeGBF_Element = dt.Rows[i][9].ToString() == string.Empty ? Weapon.GBFElementCHSEnum.无 : (Weapon.GBFElementCHSEnum)Enum.Parse(typeof(Weapon.GBFElementCHSEnum), dt.Rows[i][9].ToString());
                    //更新图片路径
                    if (IsUpdateImage)
                    {
                        FilePath(wp.FnWeapon_ID, i, wp.FeGBF_Rarity.ToString(), sheet);
                    }
                    wp.FeWeapon_Kind = dt.Rows[i][10].ToString() == string.Empty ? Weapon.WeaponKind.无效类型 : (Weapon.WeaponKind)Enum.Parse(typeof(Weapon.WeaponKind), dt.Rows[i][10].ToString());
                    //wp.FeGBF_Rarity = dt.Rows[i][11].ToString() == string.Empty ? Weapon.GBFRarityEnum.未知 : (Weapon.GBFRarityEnum)Enum.Parse(typeof(Weapon.GBFRarityEnum), dt.Rows[i][11].ToString());
                    ////wp.FsGBF_Category = dt.Rows[i][12].ToString() == string.Empty ? new List<string>() : SplitString(dt.Rows[i][12].ToString());
                    ////wp.FsGBF_Tag = dt.Rows[i][13].ToString() == string.Empty ? new List<string>() : SplitString(dt.Rows[i][13].ToString());
                    //wp.FdGBF_ReleaseDate = dt.Rows[i][14].ToString() == string.Empty ? Convert.ToDateTime(null) : Convert.ToDateTime(dt.Rows[i][14].ToString().Remove(0, 2));
                    //wp.FdGBF_Star4 = dt.Rows[i][15].ToString() == string.Empty ? Convert.ToDateTime(null) : Convert.ToDateTime(dt.Rows[i][15].ToString().Remove(0, 2));
                    //wp.FdGBF_Star5 = dt.Rows[i][16].ToString() == string.Empty ? Convert.ToDateTime(null) : Convert.ToDateTime(dt.Rows[i][16].ToString().Remove(0, 2));
                    //wp.FdGBF_LastDate = dt.Rows[i][17].ToString() == string.Empty ? Convert.ToDateTime(null) : Convert.ToDateTime(dt.Rows[i][17].ToString().Remove(0, 2));
                    //wp.FnGBF_UnlockChar = dt.Rows[i][18].ToString() == string.Empty ? 0 : Convert.ToInt64(dt.Rows[i][18]);
                    //wp.FsGBF_LinkGamewith = dt.Rows[i][19].ToString() == string.Empty ? "" : dt.Rows[i][19].ToString();
                    //wp.FsGBF_LinkEnwiki = dt.Rows[i][20].ToString() == string.Empty ? "" : dt.Rows[i][20].ToString();
                    //wp.FnGBF_UserLevel = dt.Rows[i][21].ToString() == string.Empty ? (short)0 : Convert.ToInt16(dt.Rows[i][21]);
                    //wp.FnGBF_UnlockChar = dt.Rows[i][22].ToString() == string.Empty ? 0 : Convert.ToInt32(dt.Rows[i][22]);
                    //wp.FnWeapon_MaxAttack = dt.Rows[i][23].ToString() == string.Empty ? 0 : Convert.ToInt32(dt.Rows[i][23]);
                    //wp.FnWeaponEvoFourHp = dt.Rows[i][23].ToString() == string.Empty ? 0 : Convert.ToInt32(dt.Rows[i][24]);
                    //wp.FnWeaponEvoFourAttack = dt.Rows[i][25].ToString() == string.Empty ? 0 : Convert.ToInt32(dt.Rows[i][25]);
                    //wp.FnWeaponEvoFiveHp = dt.Rows[i][26].ToString() == string.Empty ? 0 : Convert.ToInt32(dt.Rows[i][26]);
                    //wp.FnWeaponEvoFiveAttack = dt.Rows[i][27].ToString() == string.Empty ? 0 : Convert.ToInt32(dt.Rows[i][27]);
                    //wp.FnGBF_BaseEvo = dt.Rows[i][28].ToString() == string.Empty ? (short)0 : Convert.ToInt16(dt.Rows[i][28]);
                    //wp.FnGBF_MaxEvo = dt.Rows[i][29].ToString() == string.Empty ? (short)0 : Convert.ToInt16(dt.Rows[i][29]);
                    //wp.FbGbfIsArchaic = dt.Rows[i][30].ToString() == string.Empty ? false : Convert.ToBoolean(dt.Rows[i][30]);
                    //wp.FsWeaponSkillName = dt.Rows[i][31].ToString() == string.Empty ? "" : dt.Rows[i][31].ToString();
                    //wp.WeaponImgUrlB = dt.Rows[i][32].ToString() == string.Empty ? "" : dt.Rows[i][32].ToString();
                    //wp.WeaponImgUrlCjs = dt.Rows[i][33].ToString() == string.Empty ? "" : dt.Rows[i][33].ToString();
                    //wp.WeaponImgUrlLs = dt.Rows[i][34].ToString() == string.Empty ? "" : dt.Rows[i][34].ToString();
                    //wp.WeaponImgUrlM = dt.Rows[i][35].ToString() == string.Empty ? "" : dt.Rows[i][35].ToString();
                    //wp.WeaponImgUrlS = dt.Rows[i][36].ToString() == string.Empty ? "" : dt.Rows[i][36].ToString();
                    //SetSkillNew(wp.FsWeaponSkillName, wp);
                    WeaponList.Add(wp);
                }
                resultObj.ObjList = WeaponList;
            }
            catch (Exception ex)
            {
                return resultObj.SetError("从Excel读取武器数据失败:" + ex.Message);
            }
            return resultObj;
        }

        

        #endregion

        #endregion
    }
}
