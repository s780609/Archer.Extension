// See https://aka.ms/new-console-template for more information
using Archer.Extension;
using Archer.Extension.DatabaseHelper;
using Archer.Extension.SecurityHelper;
using Archer.Extension.JwtHelper;
using System.Data;
using Microsoft.Extensions.Configuration;
using Archer.Extension.Models;
using System.Drawing;
using Archer.Extension.Images;

Console.WriteLine("Archer.Extension.Testing Start...");

IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

var issuer = config.GetValue<string>("JwtSettings:Issuer");
var aaa = config.GetValue<string>("UBOL_API");

// read images from D:\暫時\IMG_0123.jpg into base64Images

// read D:\暫時\IMG_0123.jpg
string base64Image = string.Empty;
using (FileStream fs = new FileStream(@"D:\暫時\IMG_0123.jpg", FileMode.Open, FileAccess.Read))
{
    byte[] buffer = new byte[fs.Length];
    fs.Read(buffer, 0, buffer.Length);

    base64Image = Convert.ToBase64String(buffer);
}

WatermarkHelper watermarkHelper = new WatermarkHelper();

string[] watermarkImages = watermarkHelper.AddWatermarkToImages(new string[] { base64Image },
    "僅限聯邦銀行業務使用",
    new Font("微軟雅黑", 50, FontStyle.Bold, GraphicsUnit.Pixel),
    Color.FromArgb(127, 0, 153, 153),
    WatermarkPosition.MiddleCenter);

// save watermarkImages to D:\暫時\IMG_0123_watermarked.jpg
for (int i = 0; i < watermarkImages.Length; i++)
{
    byte[] buffer = Convert.FromBase64String(watermarkImages[i]);

    using (FileStream fs = new FileStream($@"D:\暫時\IMG_0123_watermarked_{i}.jpg", FileMode.Create, FileAccess.Write))
    {
        fs.Write(buffer, 0, buffer.Length);
    }
}

//string base64Image = "your_base64_image_string";
Color watermarkColor = Color.FromArgb(127, 0, 153, 153); // 半透明白色
int threshold = 100; // 顏色相似度閾值

string removedResult = watermarkHelper.RemoveWatermark(watermarkImages[0], watermarkColor, threshold);

byte[] buffer2 = Convert.FromBase64String(removedResult);

using (FileStream fs2 = new FileStream($@"D:\暫時\IMG_0123_watermarked_removedResult.jpg", FileMode.Create, FileAccess.Write))
{
    fs2.Write(buffer2, 0, buffer2.Length);
}

Console.WriteLine("END");
Console.ReadLine();

JwtHelper jwtHelper = new JwtHelper(config);

TokenModel tokenModel = new TokenModel();
tokenModel.EmployeeNo = "totalwar";
tokenModel.EmployeeName = "WhosYourDaddy";
tokenModel.Roles = new string[] { "1" };
tokenModel.NotValidBefore = DateTime.Now;
tokenModel.IssuedAt = DateTime.Now.AddHours(1);
tokenModel.ExpirationTime = DateTime.Now.AddHours(1);

string token = jwtHelper.GenerateToken(tokenModel);

bool a = jwtHelper.ValidateToken(token);

const string keyConn = "AAA";
const string ivConn = "BBB";
const string keyData = "CCC";
const string ivData = "DDD";
SecurityHelper securityHelper = new SecurityHelper(keyConn, ivConn, keyData, ivData);

DatabaseHelper databaseHelper = new DatabaseHelper(securityHelper);

IDbConnection dbConnection = databaseHelper.CreateIDbConnection("EEE");

Loan loan = new Loan();

var propsname = loan.GetPropsName();
var propsvalue = loan.GetPropsValue();

Console.ReadLine();

public class Loan
{
    public string LoanID { get; set; } = null;

    public string LoanType { get; set; } = null;

    public string ApplicationDate { get; set; } = null;

    public string NationalId { get; set; } = null;

    public string SeqNo { get; set; } = null;

    public DateTime? ApplicationDateTime { get; set; } = null;

    public string BirthYear { get; set; } = null;

    public string BirthMonth { get; set; } = null;

    public string BirthDay { get; set; } = null;

    public string CellPhone { get; set; } = null;

    public string Name { get; set; } = null;

    public string MaritalStatus { get; set; } = null;

    public string Education { get; set; } = null;

    public bool? IsEstate { get; set; } = null;

    public string ContactPhoneArea { get; set; } = null;

    public string ContactPhoneNum { get; set; } = null;

    public string ContactPhoneExt { get; set; } = null;

    public string Email { get; set; } = null;

    public bool? IsOtherCitizenship { get; set; } = null;

    public string OtherCountry { get; set; } = null;

    public string OtherCity { get; set; } = null;

    public bool? IsOtherTaxpayer { get; set; } = null;

    public string CompanyName { get; set; } = null;

    public string TaxIdNum { get; set; } = null;

    public string CompanyPhoneArea { get; set; } = null;

    public string CompanyPhoneNum { get; set; } = null;

    public string CompanyPhoneExt { get; set; } = null;

    public string JobPosition { get; set; } = null;

    public string JobTitle { get; set; } = null;

    public string Seniority { get; set; } = null;

    public int? AnnualIncome { get; set; } = null;

    public int? LoanAmount { get; set; } = null;

    public int? LoanPeriod { get; set; } = null;

    public string LoanPurpose { get; set; } = null;

    public string ImportBank { get; set; } = null;

    public string ImportBranch { get; set; } = null;

    public string ImportAccount { get; set; } = null;

    public bool? IsGuaranteed { get; set; } = null;

    public string PromotionId { get; set; } = null;

    public string Unit { get; set; } = null;

    public string IdentifyMethod { get; set; } = null;

    public string RemoteIP { get; set; } = null;

    public string Status { get; set; } = null;

    public string ReplyCaseType { get; set; } = null;

    public bool? IsDigital { get; set; } = null;

    public string RiskLevel { get; set; } = null;

    public string RiskLevelTime { get; set; } = null;

    public string PromotionName { get; set; } = null;

    public bool? IsStakeholder { get; set; } = null;

    public string AreaUsed { get; set; } = null;

    public bool? IsExceedRelativeLimit { get; set; } = null;

    public bool? IsRule72 { get; set; } = null;

    public string PurposeType { get; set; } = null;

    public string MonthlyIncome { get; set; } = null;

    public string CreditDescription { get; set; } = null;

    public string Instruction { get; set; } = null;

    public string UsingWay { get; set; } = null;

    public string ProjectCode { get; set; } = null;

    public string GroupCode { get; set; } = null;

    public string OtherOpinions { get; set; } = null;

    public string Permission { get; set; } = null;

    public string Note { get; set; } = null;

    public string NoteObject { get; set; } = null;

    public DateTime? NoteTime { get; set; } = null;

    public string Note2 { get; set; } = null;

    public string NoteObject2 { get; set; } = null;

    public DateTime? NoteTime2 { get; set; } = null;

    public string Manager { get; set; } = null;

    public DateTime? ManageTime { get; set; } = null;

    public string ReviewSupervisor { get; set; } = null;

    public DateTime? ReviewTime { get; set; } = null;

    public string ApproveSupervisor { get; set; } = null;

    public DateTime? ApproveTime { get; set; } = null;

    public string WithdrawSupervisor { get; set; } = null;

    public string WithdrawReason { get; set; } = null;

    public string TransferToBranch { get; set; } = null;

    public string TransferReason { get; set; } = null;

    public string Transfer { get; set; } = null;

    public bool? IsNoteMatch { get; set; } = null;

    public DateTime? ConditionConfirmTime { get; set; } = null;

    public DateTime? ModifyManagerTime { get; set; } = null;

    public string Supervisor3 { get; set; } = null;

    public DateTime? SupervisorTime3 { get; set; } = null;

    public string Supervisor4 { get; set; } = null;

    public DateTime? SupervisorTime4 { get; set; } = null;

    public string ModifyReason { get; set; } = null;

    public DateTime? ConditionModifyTime { get; set; } = null;

    public DateTime? SignUpTime { get; set; } = null;

    public string Grantor { get; set; } = null;

    public DateTime? GrantTime { get; set; } = null;

    public string DebitAccountType { get; set; } = null;

    public string DebitAccount { get; set; } = null;

    public string BusinessRelationship { get; set; } = null;

    public string CertifiedDoc { get; set; } = null;

    public string BusinessPurpose { get; set; } = null;

    public bool? IsOBU { get; set; } = null;

    public string NaturalPerson { get; set; } = null;

    public DateTime? ModifySupervisorTime { get; set; } = null;

    public bool? IsModified { get; set; } = null;

    public string ApplyDigitalAccountState { get; set; } = null;

    public string StatusReason { get; set; } = null;

    public DateTime? ExpiredTime { get; set; } = null;

    public string SignUpIP { get; set; } = null;

    public string FeeDiscount { get; set; } = null;

    public string Nationality { get; set; } = null;

    public string BirthPlace { get; set; } = null;

    public string UploadIP { get; set; } = null;

    public DateTime? UploadTime { get; set; } = null;

    public int? IncomeRatio { get; set; } = null;

    public string IncomeRatioInfo { get; set; } = null;

    public string UbotApplicationIpInfo { get; set; } = null;

    public int? ReplenishmentNoticeCount { get; set; } = null;

    public DateTime? ReplenishmentNoticeTime { get; set; } = null;

    public string StatusReasonInfo { get; set; } = null;

    public byte? Department { get; set; } = null;

    public byte? CollateralType { get; set; } = null;

    public string OverviewBusinessType { get; set; } = null;

    public string KycJobId { get; set; } = null;

    public string KycJobDescription { get; set; } = null;

    public string ModifyManager { get; set; } = null;

    public string ModifySupervisor { get; set; } = null;

    public bool? IsApp { get; set; }
}